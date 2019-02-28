#if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.
#if UNITY_EDITOR
/// <summary>
///     @brief This class is used to perform DragAndDrop operations from the AkWwisePicker to any GameObject.
///     We found out that DragAndDrop operations in Unity do not transfer components, but only scripts. This
///     prevented us to set the name and ID of our components before performing the drag and drop. To fix this,
///     the DragAndDrop operation always transfers a AkDragDropHelper component that gets instantiated on the
///     target GameObject. On its Awake() call, it will parse the DragAndDrop structure, which contains
///     all necessary information to instantiate the correct component, with the correct information
/// </summary>
[UnityEngine.ExecuteInEditMode]
public class AkDragDropHelper : UnityEngine.MonoBehaviour
{
	public static string DragDropIdentifier = "AKWwiseDDInfo";

	private void Awake()
	{
		UnityEngine.GUIUtility.hotControl = 0;

		var reference = UnityEditor.DragAndDrop.GetGenericData(DragDropIdentifier) as WwiseObjectReference;
		if (!reference)
			return;

		switch (reference.WwiseObjectType)
		{
			case WwiseObjectType.AuxBus:
				var akEnvironments = gameObject.GetComponents<AkEnvironment>();
				foreach (var environment in akEnvironments)
					if (environment.data.ObjectReference == reference)
						return;

				var AkEnvironment = UnityEditor.Undo.AddComponent<AkEnvironment>(gameObject);
				if (AkEnvironment != null)
					AkEnvironment.data.ObjectReference = reference;
				break;

			case WwiseObjectType.Event:
				var AkAmbient = UnityEditor.Undo.AddComponent<AkAmbient>(gameObject);
				if (AkAmbient != null)
					AkAmbient.data.ObjectReference = reference;
				break;

			case WwiseObjectType.Soundbank:
				var AkBank = UnityEditor.Undo.AddComponent<AkBank>(gameObject);
				if (AkBank != null)
					AkBank.data.ObjectReference = reference;
				break;

			case WwiseObjectType.State:
				var AkState = UnityEditor.Undo.AddComponent<AkState>(gameObject);
				if (AkState != null)
					AkState.data.ObjectReference = reference;
				break;

			case WwiseObjectType.Switch:
				var AkSwitch = UnityEditor.Undo.AddComponent<AkSwitch>(gameObject);
				if (AkSwitch != null)
					AkSwitch.data.ObjectReference = reference;
				break;
		}
	}

	private void Start()
	{
		// Don't forget to destroy the AkDragDropHelper when we're done!
		DestroyImmediate(this);
	}
}
#endif // UNITY_EDITOR
#endif // #if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.