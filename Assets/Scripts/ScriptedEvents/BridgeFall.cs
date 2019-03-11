using UnityEngine;

public class BridgeFall : MonoBehaviour
{
    bool triggered = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            triggered = true;
            var joint = GetComponentsInParent<HingeJoint>();
            Destroy(joint[1]);
        }
    }
}
