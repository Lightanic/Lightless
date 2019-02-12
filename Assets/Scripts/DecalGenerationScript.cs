using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalGenerationScript : MonoBehaviour
{

    public GameObject DecalPrefab;

    private GameObject previousInstance = null;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Quaternion quaternion = Quaternion.Euler(90F, transform.eulerAngles.y, transform.eulerAngles.z);
        if (previousInstance == null)
        {
            previousInstance = Instantiate(DecalPrefab, transform.position, quaternion);
        }

        if (Vector3.Distance(previousInstance.transform.position, transform.position) > 1F)
        {
            previousInstance = Instantiate(DecalPrefab, transform.position, quaternion);
        }
    }
}
