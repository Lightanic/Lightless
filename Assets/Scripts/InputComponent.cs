using UnityEngine;
using XInputDotNetPure;
using System.Collections.Generic;

public class InputComponent : MonoBehaviour {
    public enum index { One = 1, Two = 2};
    public index PlayerNumber;

    public float Horizontal;
    public float Vertical;
    private int GamepadIndex;
    public XGamepad Gamepad;    // = new XGamepad(index);

    private void Start()
    {
        GamepadIndex = (int)PlayerNumber;
        Gamepad = new XGamepad(GamepadIndex);
    }

    /// <summary>
    /// Stores states of a single gamepad button
    /// </summary>
    public struct xButton
    {
        public ButtonState prev_state;
        public ButtonState state;
    }

    /// <summary>
    /// Stores state of a single gamepad trigger
    /// </summary>
    public struct TriggerState
    {
        public float prev_value;
        public float current_value;
    }

    /// <summary>
    /// Rumble (vibration) event
    /// </summary>
    class xRumble
    {
        public float timer;    // Rumble timer
        public float fadeTime; // Fade-out (in seconds)
        public Vector2 power;  // Rumble 'power'

        // Decrease timer
        public void Update()
        {
            this.timer -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Xbox Gamepad class
    /// </summary>
    public class XGamepad
    {
        public GamePadState State;      // Current gamepad state
        private GamePadState prev_state; // Previous gamepad state

        private int gamepadIndex;           // Index
        private PlayerIndex playerIndex;    // Player index
        private List<xRumble> rumbleEvents; // Stores rumble events

        // Button input map
        private Dictionary<string, xButton> inputMap;

        // States for all buttons/inputs supported
        private xButton A, B, X, Y; // Action (face) buttons
        private xButton DPad_Up, DPad_Down, DPad_Left, DPad_Right;

        private xButton Guide;       // Xbox logo button
        private xButton Back, Start;
        private xButton L3, R3;      // Thumbstick buttons
        private xButton LB, RB;      // Bumper buttons
        private TriggerState LT, RT; // Trigger states

        public bool EnableRumble; // Toggle rumble on/off

        // Constructor
        public XGamepad(int index)
        {
            // Set gamepad index
            gamepadIndex = index - 1;
            playerIndex = (PlayerIndex)gamepadIndex;

            EnableRumble = true;

            // Create rumble container and input map
            rumbleEvents = new List<xRumble>();
            inputMap = new Dictionary<string, xButton>();
        }

        /// <summary>
        /// Update input map
        /// </summary>
        void UpdateInputMap()
        {
            inputMap["A"] = A;
            inputMap["B"] = B;
            inputMap["X"] = X;
            inputMap["Y"] = Y;

            inputMap["DPad_Up"] = DPad_Up;
            inputMap["DPad_Down"] = DPad_Down;
            inputMap["DPad_Left"] = DPad_Left;
            inputMap["DPad_Right"] = DPad_Right;

            inputMap["Guide"] = Guide;
            inputMap["Back"] = Back;
            inputMap["Start"] = Start;

            inputMap["L3"] = L3;
            inputMap["R3"] = R3;
            inputMap["LB"] = LB;
            inputMap["RB"] = RB;
        }

        /// <summary>
        /// Update gamepad state
        /// </summary>
        public void Update()
        {
            // Get current state
            State = GamePad.GetState(playerIndex);

            // Check gamepad is connected
            if (State.IsConnected)
            {
                A.state = State.Buttons.A;
                B.state = State.Buttons.B;
                X.state = State.Buttons.X;
                Y.state = State.Buttons.Y;

                DPad_Up.state = State.DPad.Up;
                DPad_Down.state = State.DPad.Down;
                DPad_Left.state = State.DPad.Left;
                DPad_Right.state = State.DPad.Right;

                Guide.state = State.Buttons.Guide;
                Back.state = State.Buttons.Back;
                Start.state = State.Buttons.Start;
                L3.state = State.Buttons.LeftStick;
                R3.state = State.Buttons.RightStick;
                LB.state = State.Buttons.LeftShoulder;
                RB.state = State.Buttons.RightShoulder;

                LT.current_value = State.Triggers.Left;
                RT.current_value = State.Triggers.Right;

                UpdateInputMap(); // Update inputMap dictionary
                HandleRumble();   // Update rumble events
            }
        }

        /// <summary>
        /// Refresh previous gamepad state
        /// </summary>
        public void Refresh()
        {
            // This 'saves' the current state for next update
            prev_state = State;

            // Check gamepad is connected
            if (State.IsConnected)
            {
                A.prev_state = prev_state.Buttons.A;
                B.prev_state = prev_state.Buttons.B;
                X.prev_state = prev_state.Buttons.X;
                Y.prev_state = prev_state.Buttons.Y;

                DPad_Up.prev_state = prev_state.DPad.Up;
                DPad_Down.prev_state = prev_state.DPad.Down;
                DPad_Left.prev_state = prev_state.DPad.Left;
                DPad_Right.prev_state = prev_state.DPad.Right;

                Guide.prev_state = prev_state.Buttons.Guide;
                Back.prev_state = prev_state.Buttons.Back;
                Start.prev_state = prev_state.Buttons.Start;
                L3.prev_state = prev_state.Buttons.LeftStick;
                R3.prev_state = prev_state.Buttons.RightStick;
                LB.prev_state = prev_state.Buttons.LeftShoulder;
                RB.prev_state = prev_state.Buttons.RightShoulder;

                // Read previous trigger values into trigger states
                LT.prev_value = prev_state.Triggers.Left;
                RT.prev_value = prev_state.Triggers.Right;

                UpdateInputMap(); // Update button map
            }
        }

        /// <summary>
        /// Return numeric gamepad index
        /// </summary>
        public int Index { get { return gamepadIndex; } }

        /// <summary>
        /// Check if the gamepad is connected
        /// </summary>
        public bool IsConnected { get { return State.IsConnected; } }

        /// <summary>
        /// Update and apply rumble event(s)
        /// </summary>
        void HandleRumble()
        {
            // Ignore if there are no events
            if (rumbleEvents.Count > 0)
            {
                Vector2 currentPower = new Vector2(0, 0);

                for (int i = 0; i < rumbleEvents.Count; ++i)
                {
                    // Update current event
                    rumbleEvents[i].Update();

                    if (rumbleEvents[i].timer > 0)
                    {
                        // Calculate current power
                        float timeLeft = Mathf.Clamp(rumbleEvents[i].timer / rumbleEvents[i].fadeTime, 0f, 1f);
                        currentPower = new Vector2(Mathf.Max(rumbleEvents[i].power.x * timeLeft, currentPower.x),
                                                   Mathf.Max(rumbleEvents[i].power.y * timeLeft, currentPower.y));

                        GamePad.SetVibration(playerIndex, currentPower.x, currentPower.y);
                    }
                    else
                    {
                        // Cancel out any phantom vibration
                        GamePad.SetVibration(playerIndex, 0.0f, 0.0f);

                        // Remove expired event
                        rumbleEvents.Remove(rumbleEvents[i]);
                    }
                }
            }
        }

        /// <summary>
        /// Add a rumble event to the gamepad
        /// (vibration is set during 'HandleRumble' function)
        /// </summary>
        /// <param name="timer"></param>
        /// <param name="power"></param>
        /// <param name="fadeTime"></param>
        public void AddRumble(float timer, Vector2 power, float fadeTime = 0f)
        {
            xRumble rumble = new xRumble();

            rumble.timer = timer;
            rumble.power = power;
            rumble.fadeTime = fadeTime;

            // Add rumble event to container
            rumbleEvents.Add(rumble);
        }

        /// <summary>
        /// Return axes of left thumbstick
        /// </summary>
        /// <returns></returns>
        public GamePadThumbSticks.StickValue GetStick_L()
        {
            return State.ThumbSticks.Left;
        }

        /// <summary>
        /// Return axes of right thumbstick
        /// </summary>
        /// <returns></returns>
        public GamePadThumbSticks.StickValue GetStick_R()
        {
            return State.ThumbSticks.Right;
        }

        /// <summary>
        /// Return axis of left trigger
        /// </summary>
        public float GetTriggerLeft { get { return State.Triggers.Left; } }

        /// <summary>
        /// Return axis of right trigger
        /// </summary>
        public float GetTriggerRight { get { return State.Triggers.Right; } }

        /// <summary>
        /// Check if left trigger was tapped - on CURRENT frame
        /// </summary>
        /// <returns></returns>
        public bool GetTriggerTapLeft()
        {
            return (LT.prev_value == 0f && LT.current_value >= 0.1f) ? true : false;
        }

        /// <summary>
        /// Check if right trigger was tapped - on CURRENT frame
        /// </summary>
        /// <returns></returns>
        public bool GetTriggerTapRight()
        {
            return (RT.prev_value == 0f && RT.current_value >= 0.1f) ? true : false;
        }

        /// <summary>
        /// Return button state
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public bool GetButton(string button)
        {
            if (!inputMap.ContainsKey(button)) return false;
            return inputMap[button].state == ButtonState.Pressed ? true : false;
        }

        /// <summary>
        /// Return button state - on CURRENT frame
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public bool GetButtonDown(string button)
        {
            if (!inputMap.ContainsKey(button)) return false;
            return (inputMap[button].prev_state == ButtonState.Released &&
                    inputMap[button].state == ButtonState.Pressed) ? true : false;
        }
    }

    /// <summary>
    /// Remap input controls
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool Control(string key)
    {
        bool statusGamepad = false;
        bool statusKb = false;
        switch(key)
        {
            case "Interact":
                statusGamepad = this.Gamepad.GetButtonDown("X");
                statusKb = Input.GetKeyDown(KeyCode.E);
                break;
            case "DropItem":
                statusGamepad = Gamepad.GetButtonDown("B");
                statusKb = Input.GetKeyDown(KeyCode.G);
                break;
            case "InventoryNext":
                statusGamepad = Gamepad.GetButtonDown("DPad_Right") || Gamepad.GetButtonDown("DPad_Left");
                statusKb = Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow);
                var d = Input.GetAxis("Mouse ScrollWheel");
                if (d > 0 || d < 0) statusKb = true;
                break;
            case "Dodge":
                statusGamepad = this.Gamepad.GetButtonDown("A");
                statusKb = Input.GetKeyDown(KeyCode.Space);
                break;
            case "Sprint":
                statusGamepad = this.Gamepad.GetTriggerRight != 0 ;
                statusKb = Input.GetKey(KeyCode.LeftShift);
                break;
            case "LeftLightToggle":
                statusGamepad = this.Gamepad.GetButtonDown("LB");
                statusKb = Input.GetMouseButtonDown(0);
                break;
            case "RightLightToggle":
                statusGamepad = this.Gamepad.GetButtonDown("RB");
                statusKb = Input.GetMouseButtonDown(1);
                break;
            case "LightFire":
                statusGamepad = this.Gamepad.GetButtonDown("Y");
                statusKb = Input.GetKeyDown(KeyCode.F);
                break;
            case "OilTrail":
                statusGamepad = this.Gamepad.GetButton("X");
                statusKb = Input.GetMouseButton(0);
                break;
            default:
                statusGamepad = false;
                statusKb = false;
                break;
            //case "DodgeDone":
            //    statusGamepad = !this.Gamepad.GetButton("A");
            //    statusKb = Input.GetKeyUp(KeyCode.Space);
            //    break;
        }

        return (statusGamepad||statusKb);
    }
}
