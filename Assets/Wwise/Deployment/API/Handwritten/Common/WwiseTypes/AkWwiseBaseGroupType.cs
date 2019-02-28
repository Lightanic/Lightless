#if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.

namespace AK.Wwise
{
	[System.Serializable]
	///@brief This type represents the base for all Wwise Types that also require a group GUID, such as State and Switch.
	public abstract class BaseGroupType : BaseType
	{
		public WwiseObjectReference GroupWwiseObjectReference
		{
			get
			{
				var reference = ObjectReference as WwiseGroupValueObjectReference;
				return reference ? reference.GroupObjectReference : null;
			}
		}

		public abstract WwiseObjectType WwiseObjectGroupType { get; }

		[System.Obsolete(AkSoundEngine.Deprecation_2018_1_2)]
		public int groupID
		{
			get { return (int)GroupId; }
		}

		public uint GroupId
		{
			get { return GroupWwiseObjectReference ? GroupWwiseObjectReference.Id : AkSoundEngine.AK_INVALID_UNIQUE_ID; }
		}

		public override bool IsValid()
		{
			return base.IsValid() && GroupWwiseObjectReference != null;
		}

#if UNITY_EDITOR
		public void SetupGroupReference(string name, System.Guid guid)
		{
			var reference = ObjectReference as WwiseGroupValueObjectReference;
			if (reference)
				reference.SetupGroupObjectReference(name, guid);
		}
#endif

		#region WwiseMigration
#if UNITY_EDITOR
		protected override bool CanMigrateData()
		{
			return base.CanMigrateData() && IsByteArrayValidGuid(groupGuid);
		}

		protected override void MigrateDataExtension()
		{
			var reference = ObjectReference as WwiseGroupValueObjectReference;
			if (reference)
				reference.SetupGroupObjectReferenceForMigration(groupGuid);
		}

		protected override void ClearData()
		{
			valueGuid = groupGuid = null;
		}
#endif

		[UnityEngine.HideInInspector]
		public byte[] groupGuid;
		#endregion
	}
}
#endif // #if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.