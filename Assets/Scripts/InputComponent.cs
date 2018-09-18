using UnityEngine;
using XInputDotNetPure;

public class InputComponent : MonoBehaviour {
    public float Horizontal;
    public float Vertical;
    public GamePadState GamePadState;
    public PlayerIndex SinglePlayer = (PlayerIndex)0;
}
