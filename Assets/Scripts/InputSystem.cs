using Unity.Entities;
using UnityEngine;

public class InputSystem : ComponentSystem
{
    // Struct specifying all entites that use this move system
    private struct Group
    {
        readonly public int Length;
        public ComponentArray<InputComponent> inputComponents;
    }

    [Inject] private Group data;     // Inject entities with "Group" components

    protected override void OnUpdate()
    {
        var horizontal = Input.GetAxis("Horizontal");               // Get input from horizontal axis
        var vertical = Input.GetAxis("Vertical");                   // Get input from vertical axis
        for (int i = 0; i< data.Length; i++)                        // Move every entity having Group compononts
        {
            data.inputComponents[i].horizontal = horizontal;
            data.inputComponents[i].vertical = vertical;
        }
    }
}
