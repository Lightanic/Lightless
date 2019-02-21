using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PlayerProperties : MonoBehaviour
{
    [Header("Number of narrative pickups")]
    [SerializeField]
    public int NumNarrativePickup;

    public static Dictionary<int, NarrativePickup> narrativePickups = new Dictionary<int, NarrativePickup>();

    [Header("Foliage Emission")]
    public LightComponent Lantern;
    public float FoliageFillTime = 2F;
    public float FoliageEmissionDistance = 20F;

    private float CurrentTime = 0F;
    private Vector3 previousLanternPos;
    private void Start()
    {
        if (Lantern == null)
            Lantern = GameObject.FindGameObjectWithTag("Lantern").GetComponent<LightComponent>();
        Shader.SetGlobalFloat("FoliageEmissionDistance", FoliageEmissionDistance);
        previousLanternPos = Lantern.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        var lanternPos = Vector3.Lerp(previousLanternPos, Lantern.transform.position, Time.deltaTime);
        var foliageFillAmount = CurrentTime / FoliageFillTime;
        Shader.SetGlobalFloat("FoliageFillAmount", foliageFillAmount * foliageFillAmount);
        Shader.SetGlobalVector("LanternPos", lanternPos);
        Shader.SetGlobalVector("PlayerPos", gameObject.transform.position);
        UpdateLightValue();
        previousLanternPos = lanternPos;
    }

    void UpdateLightValue()
    {
        if (Lantern.LightIsOn)
        {
            CurrentTime += Time.deltaTime;
            if (CurrentTime >= FoliageFillTime)
            {
                CurrentTime = FoliageFillTime;
            }
        }
        else
        {
            //CurrentTime = 0;
            CurrentTime -= Time.deltaTime * 1.5F;
            if (CurrentTime <= 0F)
            {
                CurrentTime = 0;
            }
        }
    }
}
