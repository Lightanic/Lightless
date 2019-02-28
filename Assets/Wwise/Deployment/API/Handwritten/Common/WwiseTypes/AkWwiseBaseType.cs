#if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.

namespace AK.Wwise
{
	[System.Serializable]
	///@brief This type represents the base for all Wwise Types that require a GUID.
	public abstract class BaseType : UnityEngine.ISerializationCallbackReceiver
	{
		public abstract WwiseObjectReference ObjectReference { get; set; }

		public abstract WwiseObjectType WwiseObjectType { get; }

		public virtual string Name
		{
			get { return IsValid() ? ObjectReference.DisplayName : string.Empty; }
		}

		[System.Obsolete(AkSoundEngine.Deprecation_2018_1_2)]
		public int ID
		{
			get { return (int)Id; }
		}

		public uint Id
		{
			get { return IsValid() ? ObjectReference.Id : AkSoundEngine.AK_INVALID_UNIQUE_ID; }
		}

		public virtual bool IsValid()
		{
			return ObjectReference != null;
		}

		public bool Validate()
		{
			if (IsValid())
				return true;

			UnityEngine.Debug.LogWarning("Wwise ID has not been resolved. Consider picking a new " + GetType().Name + ".");
			return false;
		}

		protected void Verify(AKRESULT result)
		{
#if UNITY_EDITOR
			if (result != AKRESULT.AK_Success && AkSoundEngine.IsInitialized())
				UnityEngine.Debug.LogWarning("Unsuccessful call made on " + GetType().Name + ".");
#endif
		}

		public override string ToString()
		{
			return IsValid() ? ObjectReference.ObjectName : ("Empty " + GetType().Name);
		}

#if UNITY_EDITOR
		public void SetupReference(string name, System.Guid guid)
		{
			ObjectReference = WwiseObjectReference.FindOrCreateWwiseObject(WwiseObjectType, name, guid);
		}
#endif

		#region WwiseMigration

		void UnityEngine.ISerializationCallbackReceiver.OnBeforeSerialize() { }

		void UnityEngine.ISerializationCallbackReceiver.OnAfterDeserialize()
		{
#if UNITY_EDITOR
			if (CanMigrateData())
				WwiseObjectReference.migrate += MigrateData;
			else
				ClearData();
#endif
		}

#if UNITY_EDITOR
		protected virtual bool CanMigrateData()
		{
			return !IsValid() && IsByteArrayValidGuid(valueGuid);
		}

		public void MigrateData()
		{
			ObjectReference = WwiseObjectReference.GetWwiseObjectForMigration(WwiseObjectType, valueGuid);

			MigrateDataExtension();

			if (IsValid())
				UnityEngine.Debug.Log("WwiseUnity: Converted " + Name + " in " + GetType().FullName);

			ClearData();
		}

		protected virtual void MigrateDataExtension() {}

		protected virtual void ClearData()
		{
			valueGuid = null;
		}
#endif

		[UnityEngine.HideInInspector]
		public byte[] valueGuid;

		public static bool IsByteArrayValidGuid(byte[] byteArray)
		{
			if (byteArray == null)
				return false;

			try
			{
				var guid = new System.Guid(byteArray);
				return !guid.Equals(System.Guid.Empty);
			}
			catch
			{
				return false;
			}
		}
		#endregion
	}
}
#endif // #if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.