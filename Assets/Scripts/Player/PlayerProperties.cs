using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PlayerProperties : MonoBehaviour
{
    [Header("Number of narrative pickups")]
    [SerializeField]
    public int NumNarrativePickup;
    
    public static Dictionary<int,NarrativePickup> narrativePickups = new Dictionary<int, NarrativePickup>();
    // Update is called once per frame
    void Update()
    {
        Shader.SetGlobalVector("PlayerPos", gameObject.transform.position);
    }
}
