using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuFlashlight : MonoBehaviour
{
    [Header("Move Speed")]
    public float speed = 4.0f;    // angular rotation speed

    [SerializeField]
    public GameObject[] buttons;   // associated target buttons
    [SerializeField]
    public Transform[] associatedLoc;   // world-space location

    Transform target;
    private void Start()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            associatedLoc[i] = buttons[i].transform;
        }
    }
    void Update()
    {
        if (target != null)
        {
            Vector3 targetDir = target.position - transform.position;

            //float step = speed * Time.deltaTime;
            //Vector3 newDir = Vector3.RotateTowards(transform.right, targetDir, step, 0.0f);
            //transform.rotation = Quaternion.LookRotation(newDir);

            Quaternion rotation = Quaternion.LookRotation(targetDir, transform.up);

            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * speed);

        }

        // find the currenly selected button
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].GetComponent<Animator>().GetBool("selected") == true)
            {
                // assign target of spotlight to the associated transform location to the button in the scene
                target = associatedLoc[i];
                return;
            }
        }
        if (target == null) return;


    }
}
