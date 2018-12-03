namespace AK.Wwise.Editor
{
	[UnityEditor.CustomPropertyDrawer(typeof(AuxBus))]
	public class AuxBusDrawer : BaseTypeDrawer
	{
		public override string UpdateIds(System.Guid[] in_guid)
		{
			var list = AkWwiseProjectInfo.GetData().AuxBusWwu;

			for (var i = 0; i < list.Count; i++)
			{
				var element = list[i].List.Find(x => new System.Guid(x.Guid).Equals(in_guid[0]));

				if (element != null)
				{
					m_IDProperty[0].intValue = element.ID;
					return element.Name;
				}
			}

			m_IDProperty[0].intValue = 0;
			return string.Empty;
		}

		public override void SetupSerializedProperties(UnityEditor.SerializedProperty property)
		{
			m_objectType = AkWwiseProjectData.WwiseObjectType.AUXBUS;
			m_typeName = "AuxBus";

			m_IDProperty = new[] { property.FindPropertyRelative("ID") };
			m_guidProperty = new[] { property.FindPropertyRelative("valueGuid.Array") };
		}
	}
}