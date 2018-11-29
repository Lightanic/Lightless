using UnityEngine;
using UnityEngine.UI;

// Used in Main Menu
// Makes a spotlight move to target a selected location
public class SpotlightTarget : MonoBehaviour {
    
    public Button[] buttons;   // associated target buttons
    public Transform[] associatedLoc;   // world-space location

    Transform target;
    float speed = 2.0f;    // angular rotation speed
	
	void Update ()
    {
        if (target != null)
        {
            Vector3 targetDir = target.position - transform.position;

            //float step = speed * Time.deltaTime;
            //Vector3 newDir = Vector3.RotateTowards(transform.right, targetDir, step, 0.0f);
            //transform.rotation = Quaternion.LookRotation(newDir);

            Quaternion rotation = Quaternion.LookRotation(targetDir, Vector3.up);

            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * speed);

        }
       
        // find the currenly selected button
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].GetComponent<ButtonHighlight>().isHighlighted == true)
            {
                // assign target of spotlight to the associated transform location to the button in the scene
                target = associatedLoc[i];
                return;
            }
        }
        if (target == null) return;

        
	}
}
