public class AkWwiseProjectData : UnityEngine.ScriptableObject
{
	public enum WwiseObjectType
	{
		// Insert Wwise icons description here
		NONE,
		AUXBUS,
		BUS,
		EVENT,
		FOLDER,
		PHYSICALFOLDER,
		PROJECT,
		SOUNDBANK,
		STATE,
		STATEGROUP,
		SWITCH,
		SWITCHGROUP,
		WORKUNIT,
		GAMEPARAMETER,
		TRIGGER,
		ACOUSTICTEXTURE
	}

	public bool autoPopulateEnabled = true;
	public string CurrentPluginConfig;

	[UnityEngine.SerializeField]
	private int m_lastPopulateTimePart2;

	[UnityEngine.SerializeField]
	private int m_lastPopulateTimePsrt1;

	//An IComparer that enables us to sort work units by their physical path 
	public static WorkUnit_CompareByPhysicalPath s_compareByPhysicalPath = new WorkUnit_CompareByPhysicalPath();

	//An IComparer that enables us to sort AkInformations by their physical name
	public static AkInformation_CompareByName s_compareAkInformationByName = new AkInformation_CompareByName();

	public System.Collections.Generic.List<AkInfoWorkUnit> AcousticTextureWwu =
		new System.Collections.Generic.List<AkInfoWorkUnit>();

	public System.Collections.Generic.List<AkInfoWorkUnit> AuxBusWwu =
		new System.Collections.Generic.List<AkInfoWorkUnit>();

	public System.Collections.Generic.List<AkInfoWorkUnit> BankWwu
		= new System.Collections.Generic.List<AkInfoWorkUnit>();

	public System.Collections.Generic.List<EventWorkUnit> EventWwu
		= new System.Collections.Generic.List<EventWorkUnit>();

	public System.Collections.Generic.List<string> ExpandedItems
		= new System.Collections.Generic.List<string>();

	public System.Collections.Generic.List<AkInfoWorkUnit> RtpcWwu
		= new System.Collections.Generic.List<AkInfoWorkUnit>();

	public System.Collections.Generic.List<GroupValWorkUnit> StateWwu =
		new System.Collections.Generic.List<GroupValWorkUnit>();

	public System.Collections.Generic.List<GroupValWorkUnit> SwitchWwu =
		new System.Collections.Generic.List<GroupValWorkUnit>();

	public System.Collections.Generic.List<AkInfoWorkUnit> TriggerWwu =
		new System.Collections.Generic.List<AkInfoWorkUnit>();

	public System.Collections.ArrayList GetWwuListByString(string in_wwuType)
	{
		if (string.Equals(in_wwuType, "Events", System.StringComparison.OrdinalIgnoreCase))
			return System.Collections.ArrayList.Adapter(EventWwu);
		if (string.Equals(in_wwuType, "States", System.StringComparison.OrdinalIgnoreCase))
			return System.Collections.ArrayList.Adapter(StateWwu);
		if (string.Equals(in_wwuType, "Switches", System.StringComparison.OrdinalIgnoreCase))
			return System.Collections.ArrayList.Adapter(SwitchWwu);
		if (string.Equals(in_wwuType, "Master-Mixer Hierarchy", System.StringComparison.OrdinalIgnoreCase))
			return System.Collections.ArrayList.Adapter(AuxBusWwu);
		if (string.Equals(in_wwuType, "SoundBanks", System.StringComparison.OrdinalIgnoreCase))
			return System.Collections.ArrayList.Adapter(BankWwu);
		if (string.Equals(in_wwuType, "Game Parameters", System.StringComparison.OrdinalIgnoreCase))
			return System.Collections.ArrayList.Adapter(RtpcWwu);
		if (string.Equals(in_wwuType, "Triggers", System.StringComparison.OrdinalIgnoreCase))
			return System.Collections.ArrayList.Adapter(TriggerWwu);
		if (string.Equals(in_wwuType, "Virtual Acoustics", System.StringComparison.OrdinalIgnoreCase))
			return System.Collections.ArrayList.Adapter(AcousticTextureWwu);

		return null;
	}

	public WorkUnit NewChildWorkUnit(string in_wwuType)
	{
		if (string.Equals(in_wwuType, "Events", System.StringComparison.OrdinalIgnoreCase))
			return new EventWorkUnit();
		if (string.Equals(in_wwuType, "States", System.StringComparison.OrdinalIgnoreCase) ||
		    string.Equals(in_wwuType, "Switches", System.StringComparison.OrdinalIgnoreCase))
			return new GroupValWorkUnit();
		if (string.Equals(in_wwuType, "Master-Mixer Hierarchy", System.StringComparison.OrdinalIgnoreCase) ||
		    string.Equals(in_wwuType, "SoundBanks", System.StringComparison.OrdinalIgnoreCase) ||
		    string.Equals(in_wwuType, "Game Parameters", System.StringComparison.OrdinalIgnoreCase) ||
		    string.Equals(in_wwuType, "Virtual Acoustics", System.StringComparison.OrdinalIgnoreCase) ||
		    string.Equals(in_wwuType, "Triggers", System.StringComparison.OrdinalIgnoreCase))
			return new AkInfoWorkUnit();

		return null;
	}

	public float GetEventMaxAttenuation(int in_eventID)
	{
		for (var i = 0; i < EventWwu.Count; i++)
		{
			for (var j = 0; j < EventWwu[i].List.Count; j++)
			{
				if (EventWwu[i].List[j].ID.Equals(in_eventID))
					return EventWwu[i].List[j].maxAttenuation;
			}
		}

		return 0.0f;
	}

	public void Reset()
	{
		EventWwu = new System.Collections.Generic.List<EventWorkUnit>();
		StateWwu = new System.Collections.Generic.List<GroupValWorkUnit>();
		SwitchWwu = new System.Collections.Generic.List<GroupValWorkUnit>();
		BankWwu = new System.Collections.Generic.List<AkInfoWorkUnit>();
		AuxBusWwu = new System.Collections.Generic.List<AkInfoWorkUnit>();
		RtpcWwu = new System.Collections.Generic.List<AkInfoWorkUnit>();
		TriggerWwu = new System.Collections.Generic.List<AkInfoWorkUnit>();
		AcousticTextureWwu = new System.Collections.Generic.List<AkInfoWorkUnit>();
	}

	[System.Serializable]
	public class ByteArrayWrapper
	{
		public byte[] bytes;

		public ByteArrayWrapper(byte[] byteArray)
		{
			bytes = byteArray;
		}
	}

	[System.Serializable]
	public class AkInformation
	{
		public byte[] Guid = null;
		public int ID;
		public string Name;
		public string Path;
		public System.Collections.Generic.List<PathElement> PathAndIcons = new System.Collections.Generic.List<PathElement>();
	}

	[System.Serializable]
	public class GroupValue : AkInformation
	{
		//Unity can't serialize a list of arrays. So we create a serializable wrapper class for our array 
		public System.Collections.Generic.List<ByteArrayWrapper> ValueGuids =
			new System.Collections.Generic.List<ByteArrayWrapper>();

		public System.Collections.Generic.List<PathElement> ValueIcons = new System.Collections.Generic.List<PathElement>();
		public System.Collections.Generic.List<int> valueIDs = new System.Collections.Generic.List<int>();
		public System.Collections.Generic.List<string> values = new System.Collections.Generic.List<string>();
	}

	[System.Serializable]
	public class Event : AkInformation
	{
		public float maxAttenuation;
		public float maxDuration;
		public float minDuration;
	}

	[System.Serializable]
	public class WorkUnit : System.IComparable
	{
		public string Guid;

		[UnityEngine.SerializeField] private int m_lastTimePart2;

		//DateTime Objects are not serializable, so we have to use its binary format (64 bit long).
		//But apparently long isn't serializable neither, so we split it into two int
		[UnityEngine.SerializeField] private int m_lastTimePsrt1;

		public string ParentPhysicalPath;

		public string PhysicalPath;

		public WorkUnit()
		{
		}

		public WorkUnit(string in_physicalPath)
		{
			PhysicalPath = in_physicalPath;
		}

		public int CompareTo(object other)
		{
			var otherWwu = other as WorkUnit;

			return PhysicalPath.CompareTo(otherWwu.PhysicalPath);
		}

		public void SetLastTime(System.DateTime in_time)
		{
			var timeBin = in_time.ToBinary();

			m_lastTimePsrt1 = (int) timeBin;
			m_lastTimePart2 = (int) (timeBin >> 32);
		}

		public System.DateTime GetLastTime()
		{
			var timeBin = (long) m_lastTimePart2;
			timeBin <<= 32;
			timeBin |= (uint) m_lastTimePsrt1;

			return System.DateTime.FromBinary(timeBin);
		}
	}

	public class WorkUnit_CompareByPhysicalPath : System.Collections.IComparer
	{
		int System.Collections.IComparer.Compare(object a, object b)
		{
			var wwuA = a as WorkUnit;
			var wwuB = b as WorkUnit;

			return wwuA.PhysicalPath.CompareTo(wwuB.PhysicalPath);
		}
	}

	public class AkInformation_CompareByName : System.Collections.IComparer
	{
		int System.Collections.IComparer.Compare(object a, object b)
		{
			var AkInfA = a as AkInformation;
			var AkInfB = b as AkInformation;

			return AkInfA.Name.CompareTo(AkInfB.Name);
		}
	}

	[System.Serializable]
	public class EventWorkUnit : WorkUnit
	{
		public System.Collections.Generic.List<Event> List = new System.Collections.Generic.List<Event>();
	}


	[System.Serializable]
	public class AkInfoWorkUnit : WorkUnit
	{
		public System.Collections.Generic.List<AkInformation> List = new System.Collections.Generic.List<AkInformation>();
	}

	[System.Serializable]
	public class GroupValWorkUnit : WorkUnit
	{
		public System.Collections.Generic.List<GroupValue> List = new System.Collections.Generic.List<GroupValue>();
	}

	[System.Serializable]
	public class PathElement
	{
		public string ElementName;
		public WwiseObjectType ObjectType;

		public PathElement(string Name, WwiseObjectType objType)
		{
			ElementName = Name;
			ObjectType = objType;
		}
	}
}