using UnityEngine;

public class BridgeFall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var joint = GetComponentsInParent<HingeJoint>();
            Destroy(joint[1]);
        }
    }
}
