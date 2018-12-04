#if UNITY_EDITOR && UNITY_2017_1_OR_NEWER

//////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2017 Audiokinetic Inc. / All Rights Reserved
//
//////////////////////////////////////////////////////////////////////
[UnityEditor.CustomEditor(typeof(AkEventPlayable))]
public class AkEventPlayableInspector : UnityEditor.Editor
{
	private UnityEditor.SerializedProperty akEvent;
	private UnityEditor.SerializedProperty emitterObjectRef;
	private AkEventPlayable m_AkEventPlayable;
	private UnityEditor.SerializedProperty[] m_guidProperty;
	private UnityEditor.SerializedProperty[] m_IDProperty;

	private UnityEditor.SerializedProperty overrideTrackEmitterObject;
	private UnityEditor.SerializedProperty retriggerEvent;

	public void OnEnable()
	{
		m_AkEventPlayable = target as AkEventPlayable;
		akEvent = serializedObject.FindProperty("akEvent");
		overrideTrackEmitterObject = serializedObject.FindProperty("overrideTrackEmitterObject");
		emitterObjectRef = serializedObject.FindProperty("emitterObjectRef");
		retriggerEvent = serializedObject.FindProperty("retriggerEvent");

		m_IDProperty = new[] { akEvent.FindPropertyRelative("ID") };
		m_guidProperty = new[] { akEvent.FindPropertyRelative("valueGuid.Array") };
	}

	public override void OnInspectorGUI()
	{
		if (m_AkEventPlayable != null && m_AkEventPlayable.OwningClip != null)
			m_AkEventPlayable.OwningClip.displayName = name;
		serializedObject.Update();

		UnityEngine.GUILayout.Space(UnityEditor.EditorGUIUtility.standardVerticalSpacing);

		using (new UnityEditor.EditorGUILayout.VerticalScope("box"))
		{
			UnityEditor.EditorGUILayout.PropertyField(overrideTrackEmitterObject,
				new UnityEngine.GUIContent("Override Track Object: "));
			if (overrideTrackEmitterObject.boolValue)
				UnityEditor.EditorGUILayout.PropertyField(emitterObjectRef, new UnityEngine.GUIContent("Emitter Object Ref: "));
			UnityEditor.EditorGUILayout.PropertyField(retriggerEvent, new UnityEngine.GUIContent("Retrigger Event: "));
			UnityEditor.EditorGUILayout.PropertyField(akEvent, new UnityEngine.GUIContent("Event: "));
		}

		if (m_AkEventPlayable != null && m_AkEventPlayable.OwningClip != null)
		{
			var componentName = GetEventName(m_AkEventPlayable.akEvent.valueGuid);
			m_AkEventPlayable.OwningClip.displayName = componentName;
		}

		serializedObject.ApplyModifiedProperties();

		if (!m_AkEventPlayable.akEvent.IsValid())
		{
			new AkWwiseComponentPicker.PickerCreator
			{
				objectType = AkWwiseProjectData.WwiseObjectType.EVENT,
				guidProperty = m_guidProperty,
				idProperty = m_IDProperty,
				pickerPosition = AkUtilities.GetLastRectAbsolute(UnityEngine.GUILayoutUtility.GetLastRect()),
				serializedObject = akEvent.serializedObject
			};
		}
	}

	bool EqualGuids(byte[] first, byte[] second)
	{
		if (first.Length != second.Length)
			return false;

		for (var i = 0; i < first.Length; ++i)
			if (first[i] != second[i])
				return false;

		return true;
	}

	public string GetEventName(byte[] in_guid)
	{
		var list = AkWwiseProjectInfo.GetData().EventWwu;

		for (var i = 0; i < list.Count; i++)
		{
			var element = list[i].List.Find(x => EqualGuids(x.Guid, in_guid));
			if (element != null)
				return element.Name;
		}

		return string.Empty;
	}
}

#endif //#if UNITY_EDITOR && UNITY_2017_1_OR_NEWER