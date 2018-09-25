using Unity.Entities;
using UnityEngine;
using XInputDotNetPure;

public class InputSystem : ComponentSystem
{
    // Struct specifying all entites that use this move system
    private struct Group
    {
        readonly public int Length;
        public ComponentArray<InputComponent> InputComponents;
    }

    [Inject] private Group data;     // Inject entities with "Group" components

    protected override void OnUpdate()
    {
        var horizontal = Input.GetAxis("Horizontal");               // Get input from horizontal axis
        var vertical = Input.GetAxis("Vertical");                   // Get input from vertical axis
        for (int i = 0; i< data.Length; i++)                        // Move every entity having Group compononts
        {
            data.InputComponents[i].Horizontal = horizontal;
            data.InputComponents[i].Vertical = vertical;
            data.InputComponents[i].Gamepad.Refresh();
            data.InputComponents[i].Gamepad.Update();
        }
    }
}
