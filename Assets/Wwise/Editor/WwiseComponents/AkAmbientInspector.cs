﻿#if UNITY_EDITOR
//////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2014 Audiokinetic Inc. / All Rights Reserved
//
//////////////////////////////////////////////////////////////////////

[UnityEditor.CanEditMultipleObjects]
[UnityEditor.CustomEditor(typeof(AkAmbient))]
public class AkAmbientInspector : AkEventInspector
{
	public enum AttenuationSphereOptions
	{
		Dont_Show,
		Current_Event_Only,
		All_Events
	}

	public static System.Collections.Generic.Dictionary<UnityEngine.Object, AttenuationSphereOptions> attSphereProperties =
		new System.Collections.Generic.Dictionary<UnityEngine.Object, AttenuationSphereOptions>();

	private int curPointIndex = -1;
	public AttenuationSphereOptions currentAttSphereOp;
	private bool hideDefaultHandle;

	private AkAmbient m_AkAmbient;
	private UnityEditor.SerializedProperty multiPositionType;

	private System.Collections.Generic.List<int> triggerList;

	private new void OnEnable()
	{
		base.OnEnable();

		m_AkAmbient = target as AkAmbient;

		multiPositionType = serializedObject.FindProperty("multiPositionTypeLabel");
		DefaultHandles.Hidden = hideDefaultHandle;

		if (!attSphereProperties.ContainsKey(target))
			attSphereProperties.Add(target, AttenuationSphereOptions.Dont_Show);

		currentAttSphereOp = attSphereProperties[target];

		AkWwiseXMLWatcher.Instance.XMLUpdated += PopulateMaxAttenuation;
	}

	private void OnDisable()
	{
		DefaultHandles.Hidden = false;

		AkWwiseXMLWatcher.Instance.XMLUpdated -= PopulateMaxAttenuation;
	}

	private void DoMyWindow(int windowID)
	{
		UnityEngine.GUILayout.Space(UnityEditor.EditorGUIUtility.standardVerticalSpacing);

		using (new UnityEngine.GUILayout.HorizontalScope())
		{
			if (UnityEngine.GUILayout.Button("Add Point"))
				m_AkAmbient.multiPositionArray.Add(m_AkAmbient.transform.InverseTransformPoint(m_AkAmbient.transform.position));

			if (curPointIndex >= 0 && UnityEngine.GUILayout.Button("Delete Point"))
			{
				m_AkAmbient.multiPositionArray.RemoveAt(curPointIndex);
				curPointIndex = m_AkAmbient.multiPositionArray.Count - 1;
			}
		}

		UnityEngine.GUILayout.Space(UnityEditor.EditorGUIUtility.standardVerticalSpacing);

		if (hideDefaultHandle)
		{
			if (UnityEngine.GUILayout.Button("Show Main Transform"))
			{
				hideDefaultHandle = false;
				DefaultHandles.Hidden = hideDefaultHandle;
			}
		}
		else if (UnityEngine.GUILayout.Button("Hide Main Transform"))
		{
			hideDefaultHandle = true;
			DefaultHandles.Hidden = hideDefaultHandle;
		}
	}

	public override void OnChildInspectorGUI()
	{
		//Save trigger mask to know when it changes
		triggerList = m_AkAmbient.triggerList;

		base.OnChildInspectorGUI();

		UnityEngine.GUILayout.Space(UnityEditor.EditorGUIUtility.standardVerticalSpacing);

		var type = m_AkAmbient.multiPositionTypeLabel;

		using (new UnityEditor.EditorGUILayout.VerticalScope("box"))
		{
			UnityEditor.EditorGUILayout.PropertyField(multiPositionType, new UnityEngine.GUIContent("Position Type: "));

			UnityEngine.GUILayout.Space(UnityEditor.EditorGUIUtility.standardVerticalSpacing);

			currentAttSphereOp = (AttenuationSphereOptions) UnityEditor.EditorGUILayout.EnumPopup("Show Attenuation Sphere: ", currentAttSphereOp);
			attSphereProperties[target] = currentAttSphereOp;
		}

		//Save multi-position type to know if it has changed
		var multiPosType = m_AkAmbient.multiPositionTypeLabel;

		if (m_AkAmbient.multiPositionTypeLabel == MultiPositionTypeLabel.MultiPosition_Mode)
			UpdateTriggers(multiPosType);

		if (UnityEngine.GUI.changed)
		{
			if (type != m_AkAmbient.multiPositionTypeLabel &&
			    m_AkAmbient.multiPositionTypeLabel != MultiPositionTypeLabel.Large_Mode)
				m_AkAmbient.multiPositionArray.Clear();
		}
	}

	private void UpdateTriggers(MultiPositionTypeLabel in_multiPosType)
	{
		//if we just switched to MultiPosition_Mode
		if (in_multiPosType != m_AkAmbient.multiPositionTypeLabel)
		{
			//Get all AkAmbients in the scene
			var akAmbients = FindObjectsOfType<AkAmbient>();

			//Find the first AkAmbient that is in multiPosition_Mode and that has the same event as the current AkAmbient
			for (var i = 0; i < akAmbients.Length; i++)
			{
				if (akAmbients[i] != m_AkAmbient &&
				    akAmbients[i].multiPositionTypeLabel == MultiPositionTypeLabel.MultiPosition_Mode &&
				    akAmbients[i].data.Id == m_AkAmbient.data.Id)
				{
					//if the current AkAmbient doesn't have the same trigger as the others, we ask the user which one he wants to keep
					if (!HasSameTriggers(akAmbients[i].triggerList))
					{
						if (UnityEditor.EditorUtility.DisplayDialog("AkAmbient Trigger Mismatch",
							"All ambients in multi-position mode with the same event must have the same triggers.\n" +
							"Which triggers would you like to keep?", "Current AkAmbient Triggers", "Other AkAmbients Triggers"))
							SetMultiPosTrigger(akAmbients);
						else
							m_AkAmbient.triggerList = akAmbients[i].triggerList;
					}

					break;
				}
			}
		}
		//if the trigger changed or there was an undo/redo operation, we update the triggers of all the AkAmbients in the same group as the current one
		else if (!HasSameTriggers(triggerList) || UnityEngine.Event.current.type == UnityEngine.EventType.ValidateCommand &&
		         UnityEngine.Event.current.commandName == "UndoRedoPerformed")
		{
			var akAmbients = FindObjectsOfType<AkAmbient>();
			SetMultiPosTrigger(akAmbients);
		}
	}

	private bool HasSameTriggers(System.Collections.Generic.List<int> other)
	{
		return other.Count == m_AkAmbient.triggerList.Count &&
		       System.Linq.Enumerable.Count(System.Linq.Enumerable.Except(m_AkAmbient.triggerList, other)) == 0;
	}

	private void SetMultiPosTrigger(AkAmbient[] akAmbients)
	{
		for (var i = 0; i < akAmbients.Length; i++)
		{
			if (akAmbients[i].multiPositionTypeLabel == MultiPositionTypeLabel.MultiPosition_Mode &&
			    akAmbients[i].data.Id == m_AkAmbient.data.Id)
				akAmbients[i].triggerList = m_AkAmbient.triggerList;
		}
	}

	private void OnSceneGUI()
	{
		RenderAttenuationSpheres();

		if (m_AkAmbient.multiPositionTypeLabel == MultiPositionTypeLabel.Simple_Mode)
			return;

		var someHashCode = GetHashCode();

		UnityEditor.Handles.matrix = m_AkAmbient.transform.localToWorldMatrix;

		for (var i = 0; i < m_AkAmbient.multiPositionArray.Count; i++)
		{
			var pos = m_AkAmbient.multiPositionArray[i];

			UnityEditor.Handles.Label(pos, "Point_" + i);

			var handleSize = UnityEditor.HandleUtility.GetHandleSize(pos);

			// Get the needed data before the handle
			var controlIDBeforeHandle = UnityEngine.GUIUtility.GetControlID(someHashCode, UnityEngine.FocusType.Passive);
			var isEventUsedBeforeHandle = UnityEngine.Event.current.type == UnityEngine.EventType.Used;

			UnityEditor.Handles.color = UnityEngine.Color.green;
#if UNITY_5_6_OR_NEWER
			UnityEditor.Handles.CapFunction capFunc = UnityEditor.Handles.SphereHandleCap;
#else
			UnityEditor.Handles.DrawCapFunction capFunc = UnityEditor.Handles.SphereCap;
#endif
			UnityEditor.Handles.ScaleValueHandle(0, pos, UnityEngine.Quaternion.identity, handleSize, capFunc, 0);

			if (curPointIndex == i)
				pos = UnityEditor.Handles.PositionHandle(pos, UnityEngine.Quaternion.identity);

			// Get the needed data after the handle
			var controlIDAfterHandle = UnityEngine.GUIUtility.GetControlID(someHashCode, UnityEngine.FocusType.Passive);
			var isEventUsedByHandle = !isEventUsedBeforeHandle && UnityEngine.Event.current.type == UnityEngine.EventType.Used;

			if (controlIDBeforeHandle < UnityEngine.GUIUtility.hotControl &&
			    UnityEngine.GUIUtility.hotControl < controlIDAfterHandle || isEventUsedByHandle)
				curPointIndex = i;

			m_AkAmbient.multiPositionArray[i] = pos;
		}

		if (m_AkAmbient.multiPositionTypeLabel == MultiPositionTypeLabel.Large_Mode)
		{
			UnityEditor.Handles.BeginGUI();

			var size = new UnityEngine.Rect(0, 0, 200, 70);
			float xPosition = UnityEngine.Screen.width / UnityEditor.EditorGUIUtility.pixelsPerPoint - size.width - 10;
			float yPosition = UnityEngine.Screen.height / UnityEditor.EditorGUIUtility.pixelsPerPoint - size.height - 50;

			UnityEngine.GUI.Window(0, new UnityEngine.Rect(xPosition, yPosition, size.width, size.height), DoMyWindow, "AkAmbient Tool Bar");

			UnityEditor.Handles.EndGUI();
		}
	}

	public void RenderAttenuationSpheres()
	{
		if (currentAttSphereOp == AttenuationSphereOptions.Dont_Show)
			return;

		if (currentAttSphereOp == AttenuationSphereOptions.Current_Event_Only)
		{
			// Get the max attenuation for the event (if available)
			var radius = AkWwiseProjectInfo.GetData().GetEventMaxAttenuation(m_AkAmbient.data.Id);

			if (m_AkAmbient.multiPositionTypeLabel == MultiPositionTypeLabel.Simple_Mode)
				DrawSphere(m_AkAmbient.gameObject.transform.position, radius);
			else if (m_AkAmbient.multiPositionTypeLabel == MultiPositionTypeLabel.Large_Mode)
			{
				UnityEditor.Handles.matrix = m_AkAmbient.transform.localToWorldMatrix;

				for (var i = 0; i < m_AkAmbient.multiPositionArray.Count; i++)
					DrawSphere(m_AkAmbient.multiPositionArray[i], radius);
			}
			else
			{
				var akAmbiants = FindObjectsOfType<AkAmbient>();

				for (var i = 0; i < akAmbiants.Length; i++)
				{
					if (akAmbiants[i].multiPositionTypeLabel == MultiPositionTypeLabel.MultiPosition_Mode &&
					    akAmbiants[i].data.Id == m_AkAmbient.data.Id)
						DrawSphere(akAmbiants[i].gameObject.transform.position, radius);
				}
			}
		}
		else
		{
			var akAmbiants = FindObjectsOfType<AkAmbient>();

			for (var i = 0; i < akAmbiants.Length; i++)
			{
				// Get the max attenuation for the event (if available)
				var radius = AkWwiseProjectInfo.GetData().GetEventMaxAttenuation(akAmbiants[i].data.Id);

				if (akAmbiants[i].multiPositionTypeLabel == MultiPositionTypeLabel.Large_Mode)
				{
					UnityEditor.Handles.matrix = akAmbiants[i].transform.localToWorldMatrix;

					for (var j = 0; j < akAmbiants[i].multiPositionArray.Count; j++)
						DrawSphere(akAmbiants[i].multiPositionArray[j], radius);

					UnityEditor.Handles.matrix = UnityEngine.Matrix4x4.identity;
				}
				else
					DrawSphere(akAmbiants[i].gameObject.transform.position, radius);
			}
		}
	}

    private static UnityEngine.Color SPHERE_COLOR = new UnityEngine.Color(1.0f, 0.0f, 0.0f, 0.1f);

    private void DrawSphere(UnityEngine.Vector3 in_position, float in_radius)
	{
		UnityEditor.Handles.color = SPHERE_COLOR;

		if (UnityEngine.Vector3.SqrMagnitude(
			    UnityEditor.SceneView.lastActiveSceneView.camera.transform.position - in_position) > in_radius * in_radius)
		{
#if UNITY_5_6_OR_NEWER
			UnityEditor.Handles.SphereHandleCap(0, in_position, UnityEngine.Quaternion.identity, in_radius * 2.0f,
				UnityEngine.EventType.Repaint);
#else
			UnityEditor.Handles.SphereCap(0, in_position, UnityEngine.Quaternion.identity, in_radius * 2.0f);
#endif
		}
		else
			DrawDiscs(UnityEngine.Vector3.up, UnityEngine.Vector3.down, 6, in_position, in_radius);
	}

	private void DrawDiscs(UnityEngine.Vector3 in_startNormal, UnityEngine.Vector3 in_endNormal, uint in_nbDiscs,
		UnityEngine.Vector3 in_position, float in_radius)
	{
		var f = 1.0f / in_nbDiscs;
		for (var i = 0; i < in_nbDiscs; i++)
		{
			UnityEditor.Handles.DrawWireDisc(in_position, UnityEngine.Vector3.Slerp(in_startNormal, in_endNormal, f * i),
				in_radius);
		}
	}

	public static void PopulateMaxAttenuation()
	{
		UnityEditor.SceneView.RepaintAll();
	}
}
#endif
