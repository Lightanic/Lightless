using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
public class InputManager : MonoBehaviour {
    static public InputManager Instance = null;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
        //DontDestroyOnLoad(gameObject);
    }
    
    public bool IsGamePadActive = false;
    private bool gamepadConnected = false;
    float kbTimeStamp;
    float gamePadTimeStamp;
    InputComponent playerInputComponent;

    // Use this for initialization
    void Start () {
        playerInputComponent = GameObject.FindGameObjectWithTag("Player").GetComponent<InputComponent>();
    }
	
    /// <summary>
    /// During each frame check for currently active input device
    /// NOTE: Timestamp could overflow for exceedingly long playsessions. To be tested
    /// </summary>
	// Update is called once per frame
	void Update () {
     
        gamepadConnected = playerInputComponent.Gamepad.IsConnected;   // Check if gamepad in connected this frame
		if(Input.anyKeyDown)                                                // Poll all keyboard inputs
        {
            //Debug.Log("Using KB");
            kbTimeStamp = Time.realtimeSinceStartup;                        // Record last keyboard use
        }
        if(gamepadConnected)
        {
            //Debug.Log("Gamepad connected");
            var gamepad = playerInputComponent.Gamepad;                  // Get current gamepad state
            if (GamePadActive(gamepad.State))
            {
                gamePadTimeStamp = Time.realtimeSinceStartup;               // Record last gamepad use
            }
        }
        else if(!gamepadConnected)
        {
            for (int i = 0; i < 4; i++)
            {
                var state = GamePad.GetState((PlayerIndex)i);
                if (state.IsConnected)
                {
                    playerInputComponent.Gamepad.playerIndex = (PlayerIndex)i;
                    break;
                }
            }
        }

        //Debug.Log("KB Time" + kbTimeStamp + "GP Time" + gamePadTimeStamp);
        if (gamePadTimeStamp >= kbTimeStamp)                                // Check which input was used mast recently
        {
            IsGamePadActive = true;
        }
        else
            IsGamePadActive = false;
	}

    /// <summary>
    ///  Poll all inputs from the gamepad to see if the gamepad is currently in use
    /// </summary>
    /// <param name="state"></param>
    /// <returns> Retruns true is the gamepad is currently active </returns>
    bool GamePadActive(GamePadState state)
    {
        if(state.Buttons.A == ButtonState.Pressed                   ||
                state.Buttons.B == ButtonState.Pressed              ||
                state.Buttons.X == ButtonState.Pressed              ||
                state.Buttons.Y == ButtonState.Pressed              ||
                state.Buttons.Back == ButtonState.Pressed           ||
                state.Buttons.Start == ButtonState.Pressed          ||
                state.Buttons.LeftShoulder == ButtonState.Pressed   ||
                state.Buttons.RightShoulder == ButtonState.Pressed  ||
                state.Buttons.LeftStick == ButtonState.Pressed      ||
                state.Buttons.RightStick == ButtonState.Pressed     ||
                state.DPad.Down == ButtonState.Pressed              ||
                state.DPad.Up == ButtonState.Pressed                ||
                state.DPad.Left == ButtonState.Pressed              ||
                state.DPad.Right == ButtonState.Pressed             ||
                state.Triggers.Left != 0                            || 
                state.Triggers.Right != 0
                )
        {
            return true; 
        }
        if(state.ThumbSticks.Right.X != 0 || state.ThumbSticks.Right.Y != 0 ||
            state.ThumbSticks.Left.X != 0 || state.ThumbSticks.Left.Y != 0)
            return true;

        return false;
        
    }
}
