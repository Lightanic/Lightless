#if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.
//////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2012 Audiokinetic Inc. / All Rights Reserved
//
//////////////////////////////////////////////////////////////////////

public class AkPropagationPathInfoArray : AkBaseArray<AkPropagationPathInfo>
{
	public AkPropagationPathInfoArray(int count) : base(count)
	{
	}

	protected override int StructureSize
	{
		get { return AkSoundEnginePINVOKE.CSharp_AkPropagationPathInfo_GetSizeOf(); }
	}

	protected override AkPropagationPathInfo CreateNewReferenceFromIntPtr(System.IntPtr address)
	{
		return new AkPropagationPathInfo(address, false);
	}

	protected override void CloneIntoReferenceFromIntPtr(System.IntPtr address, AkPropagationPathInfo other)
	{
		AkSoundEnginePINVOKE.CSharp_AkPropagationPathInfo_Clone(address, AkPropagationPathInfo.getCPtr(other));
	}
}
#endif // #if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.