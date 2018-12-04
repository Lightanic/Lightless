#if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.
public class AkSoundEngineController
{
	private static AkSoundEngineController ms_Instance;

	public static AkSoundEngineController Instance
	{
		get
		{
			if (ms_Instance == null)
				ms_Instance = new AkSoundEngineController();

			return ms_Instance;
		}
	}

	~AkSoundEngineController()
	{
		if (ms_Instance == this)
		{
#if UNITY_EDITOR
#if UNITY_2017_2_OR_NEWER
			UnityEditor.EditorApplication.pauseStateChanged -= OnPauseStateChanged;
#else
			UnityEditor.EditorApplication.playmodeStateChanged -= OnEditorPlaymodeStateChanged;
#endif
			UnityEditor.EditorApplication.update -= LateUpdate;
#endif
			ms_Instance = null;
		}

		// Do nothing. AkTerminator handles sound engine termination.
	}

	public static string GetDecodedBankFolder()
	{
		return "DecodedBanks";
	}

	public static string GetDecodedBankFullPath()
	{
#if UNITY_SWITCH && !UNITY_EDITOR
		// Calling Application.persistentDataPath crashes Switch
		return null;
#elif (UNITY_ANDROID || PLATFORM_LUMIN || UNITY_IOS) && !UNITY_EDITOR
		// This is for platforms that only have a specific file location for persistent data.
		return System.IO.Path.Combine(UnityEngine.Application.persistentDataPath, GetDecodedBankFolder());
#else
		return System.IO.Path.Combine(AkBasePathGetter.GetPlatformBasePath(), GetDecodedBankFolder());
#endif
	}

	public void LateUpdate()
	{
#if UNITY_EDITOR
		if (!IsSoundEngineLoaded)
			return;
#endif

		//Execute callbacks that occurred in last frame (not the current update)
		AkCallbackManager.PostCallbacks();
		AkBankManager.DoUnloadBanks();
		AkSoundEngine.RenderAudio();
	}

	public void Init(AkInitializer akInitializer)
	{
#if UNITY_EDITOR
		if (!WasInitializedInPlayMode(akInitializer))
			return;

		var arguments = System.Environment.GetCommandLineArgs();
		if (System.Array.IndexOf(arguments, "-nographics") >= 0 &&
		    System.Array.IndexOf(arguments, "-wwiseEnableWithNoGraphics") < 0)
			return;

		var isInitialized = false;
		try
		{
			isInitialized = AkSoundEngine.IsInitialized();
			IsSoundEngineLoaded = true;
		}
		catch (System.DllNotFoundException)
		{
			IsSoundEngineLoaded = false;
			UnityEngine.Debug.LogWarning("WwiseUnity: AkSoundEngine is not loaded.");
			return;
		}
#else
		bool isInitialized = AkSoundEngine.IsInitialized();
#endif

		AkLogger.Instance.Init();

		if (isInitialized)
		{
#if UNITY_EDITOR
			if (AkWwiseInitializationSettings.ResetSoundEngine(UnityEngine.Application.isPlaying || UnityEditor.BuildPipeline.isBuildingPlayer))
				UnityEditor.EditorApplication.update += LateUpdate;
#endif
			return;
		}

#if UNITY_EDITOR
		if (UnityEditor.BuildPipeline.isBuildingPlayer)
			return;
#endif

		if (!AkWwiseInitializationSettings.InitializeSoundEngine())
			return;

#if UNITY_EDITOR
#if UNITY_2017_2_OR_NEWER
		UnityEditor.EditorApplication.pauseStateChanged += OnPauseStateChanged;
#else
		UnityEditor.EditorApplication.playmodeStateChanged += OnEditorPlaymodeStateChanged;
#endif

		OnEnableEditorListener(akInitializer.gameObject);
		UnityEditor.EditorApplication.update += LateUpdate;
#endif
	}

	public void OnDisable()
	{
#if UNITY_EDITOR
		if (!IsSoundEngineLoaded)
			return;

		OnDisableEditorListener();
#endif
	}

	public void Terminate()
	{
#if UNITY_EDITOR
		ClearInitializeState();

		if (!IsSoundEngineLoaded)
			return;
#endif

		AkWwiseInitializationSettings.TerminateSoundEngine();
	}

	// In the Editor, the sound needs to keep playing when switching windows (remote debugging in Wwise, for example).
	// On iOS, application interruptions are handled in the sound engine already.
#if UNITY_EDITOR || UNITY_IOS
	public void OnApplicationPause(bool pauseStatus)
	{
	}

	public void OnApplicationFocus(bool focus)
	{
	}
#else
	public void OnApplicationPause(bool pauseStatus) 
	{
		ActivateAudio(!pauseStatus);
	}

	public void OnApplicationFocus(bool focus)
	{
		ActivateAudio(focus);
	}
#endif

#if UNITY_EDITOR
	public bool IsSoundEngineLoaded { get; set; }

	// Enable/Disable the audio when pressing play/pause in the editor.
#if UNITY_2017_2_OR_NEWER
	private static void OnPauseStateChanged(UnityEditor.PauseState pauseState)
	{
		ActivateAudio(pauseState != UnityEditor.PauseState.Paused);
	}
#else
	private static void OnEditorPlaymodeStateChanged()
	{
		ActivateAudio(!UnityEditor.EditorApplication.isPaused);
	}
#endif
#endif

#if UNITY_EDITOR || !UNITY_IOS
	private static void ActivateAudio(bool activate)
	{
		if (AkSoundEngine.IsInitialized())
		{
			if (activate)
				AkSoundEngine.WakeupFromSuspend();
			else
				AkSoundEngine.Suspend();

			AkSoundEngine.RenderAudio();
		}
	}
#endif

#if UNITY_EDITOR
	#region Editor Listener

	private UnityEngine.GameObject editorListenerGameObject;

	private void OnEnableEditorListener(UnityEngine.GameObject gameObject)
	{
		if (!UnityEngine.Application.isPlaying && AkSoundEngine.IsInitialized() && editorListenerGameObject == null)
		{
			editorListenerGameObject = gameObject;

			AkSoundEngine.RegisterGameObj(editorListenerGameObject, editorListenerGameObject.name);

			// Do not create AkGameObj component when adding this listener
			var id = AkSoundEngine.GetAkGameObjectID(editorListenerGameObject);
			AkSoundEnginePINVOKE.CSharp_AddDefaultListener(id);

			UnityEditor.EditorApplication.update += UpdateEditorListenerPosition;
		}
	}

	private void OnDisableEditorListener()
	{
		if (!UnityEngine.Application.isPlaying && AkSoundEngine.IsInitialized() && editorListenerGameObject != null)
		{
			UnityEditor.EditorApplication.update -= UpdateEditorListenerPosition;

			var id = AkSoundEngine.GetAkGameObjectID(editorListenerGameObject);
			AkSoundEnginePINVOKE.CSharp_RemoveDefaultListener(id);

			AkSoundEngine.UnregisterGameObj(editorListenerGameObject);
			editorListenerGameObject = null;
		}
	}

	private void UpdateEditorListenerPosition()
	{
		if (!UnityEngine.Application.isPlaying && AkSoundEngine.IsInitialized() &&
		    UnityEditor.SceneView.lastActiveSceneView != null && UnityEditor.SceneView.lastActiveSceneView.camera != null)
		{
			AkSoundEngine.SetObjectPosition(editorListenerGameObject,
				UnityEditor.SceneView.lastActiveSceneView.camera.transform);
		}
	}

	#endregion

	#region Initialize only once
	private readonly System.Collections.Generic.List<AkInitializer> AkInitializers = new System.Collections.Generic.List<AkInitializer>();

	private bool WasInitializedInPlayMode(AkInitializer akInitializer)
	{
		if (UnityEngine.Application.isPlaying)
		{
			if (AkInitializers.Contains(akInitializer))
				return false;

			AkInitializers.Add(akInitializer);
			return AkInitializers.Count == 1;
		}

		return true;
	}

	private void ClearInitializeState()
	{
		AkInitializers.Clear();
	}
	#endregion
#endif // UNITY_EDITOR

}
#endif // #if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.