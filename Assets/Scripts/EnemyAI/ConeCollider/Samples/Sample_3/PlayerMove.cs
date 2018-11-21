using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {

    private float mouseY;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        mouseY = Input.GetAxis("Mouse X");
        this.transform.localEulerAngles += new Vector3(0, mouseY, 0);
        var speed = 0.3f * Time.deltaTime * 60f;
        if(Input.GetKey(KeyCode.W))
        {
            this.transform.Translate(Vector3.forward * speed);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            this.transform.Translate(Vector3.back * speed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.Translate(Vector3.left * speed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            this.transform.Translate(Vector3.right * speed);
        }
    }
}
