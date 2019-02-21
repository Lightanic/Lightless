using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootprintComponent : MonoBehaviour
{
    public GameObject DecalPrefab;
    public List<GameObject> Instances;// = new Queue<GameObject>();
    public int MaxFootprintCount = 10;
    public float FootprintDistance = 2F;
}
