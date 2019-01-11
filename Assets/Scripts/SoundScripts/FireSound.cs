using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSound : MonoBehaviour {

	// Use this for initialization
	void Start () {
        AkSoundEngine.PostEvent("FireStart", gameObject);
        StartCoroutine(KillSound());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnDestroy()
    {
        AkSoundEngine.PostEvent("FireStop", gameObject);
    }

    IEnumerator KillSound()
    {
        yield return new WaitForSeconds(14f);
        Destroy(gameObject);
    }
}
