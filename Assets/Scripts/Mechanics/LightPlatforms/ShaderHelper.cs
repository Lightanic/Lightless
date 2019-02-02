using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Mechanics.LightPlatforms
{
    public static class ShaderHelper
    {
        public static void ApplyHitTexCoord(RaycastHit hit)
        {
            Renderer rend = hit.transform.GetComponent<Renderer>();
            MeshCollider meshCollider = hit.collider as MeshCollider;
            rend.GetComponent<Shader>();
            if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null || meshCollider == null)
                return;

            Texture2D tex = rend.material.mainTexture as Texture2D;
            Vector2 pixelUV = hit.textureCoord;
            rend.material.SetVector("_HitTexCoord", new Vector4(pixelUV.x, pixelUV.y, 0F, 0F));
            //Shader.SetGlobalVector("_HitTexCoord", new Vector4(pixelUV.x, pixelUV.y, 0F, 0F));
        }

        public static void SetFillValue(Material mat, float value)
        {
            mat.SetFloat("_FillValue", value);
        }
    }
}
