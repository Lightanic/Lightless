#if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.
//////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2014 Audiokinetic Inc. / All Rights Reserved
//
//////////////////////////////////////////////////////////////////////

[UnityEngine.AddComponentMenu("Wwise/AkSwitch")]
/// @brief This will call \c AkSoundEngine.SetSwitch() whenever the selected Unity event is triggered.  For example this component could be set on a Unity collider to trigger when an object enters it.
/// \sa 
/// - <a href="https://www.audiokinetic.com/library/edge/?source=SDK&id=soundengine__switch.html" target="_blank">Integration Details - Switches</a> (Note: This is described in the Wwise SDK documentation.)
public class AkSwitch : AkUnityEventHandler, UnityEngine.ISerializationCallbackReceiver
{
	public AK.Wwise.Switch data = new AK.Wwise.Switch();

	public override void HandleEvent(UnityEngine.GameObject in_gameObject)
	{
		data.SetValue(useOtherObject && in_gameObject != null ? in_gameObject : gameObject);
	}

	#region WwiseMigration
	void UnityEngine.ISerializationCallbackReceiver.OnBeforeSerialize() { }

	void UnityEngine.ISerializationCallbackReceiver.OnAfterDeserialize()
	{
#if UNITY_EDITOR
		if (!data.IsValid() && AK.Wwise.BaseType.IsByteArrayValidGuid(valueGuid) && AK.Wwise.BaseType.IsByteArrayValidGuid(groupGuid))
		{
			data.valueGuid = valueGuid;
			data.groupGuid = groupGuid;
			WwiseObjectReference.migrate += data.MigrateData;
		}

		valueGuid = null;
		groupGuid = null;
#endif
	}

#pragma warning disable 0414 // private field assigned but not used.
	[UnityEngine.HideInInspector]
	[UnityEngine.SerializeField]
	private byte[] valueGuid;

	[UnityEngine.HideInInspector]
	[UnityEngine.SerializeField]
	private byte[] groupGuid;
#pragma warning restore 0414 // private field assigned but not used.

	#endregion
}
#endif // #if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.