 Shader "Custom/FakeVolumetricLightShader" {
 Properties {
     _Fresnel("Fresnel", Range (0., 10.)) = 1.
     _AlphaOffset("Alpha Offset", Range(0., 1.)) = 1.
     _NoiseSpeed("Noise Speed", Range(0., 1.)) = .5
     _Ambient("Ambient", Range(0., 1.)) = .3
     _Intensity("Intensity", Range(0., 1.5)) = .2
     _Fade("Fade", Range(0., 10.)) = 1.
     _Wind("Wind", Range(0., 1.)) = .1
 }
 
 SubShader {
	 // set render type for transparency
	 // transparent will draw after all the opaque geometry drawn 
     Tags {"RenderType" = "Transparent" "Queue" = "Transparent"} 
     LOD 100 // set level of detail minimum
     
     ZWrite Off // we don't need depth buffer, we're gonana use transparency and blending mode
     Blend SrcAlpha One // blend mode - additive with transparency
     
     Pass {  
         CGPROGRAM
             #pragma vertex vert
             #pragma fragment frag

             #include "UnityCG.cginc"
             #include "classicNoise3d.cginc" // import noise functions
 
             struct appdata_t {
                 float4 vertex : POSITION;
                 float3 normal : NORMAL;
                 float2 uv : TEXCOORD0;
             };
 
             struct v2f {
	             float4 vertex : SV_POSITION;
                 float3 worldPos : TEXCOORD1;
                 half2 uv : TEXCOORD0;
                 float3 normal : NORMAL;
             };
 
             float _Fresnel;
             float _AlphaOffset;
             float _NoiseSpeed;
             float _Ambient;
             float _Intensity;
             float _Fade;
             float _Wind;
             
             v2f vert (appdata_t v){
				v2f o;

				// add noise to vertices 
				float noise = _Wind * cnoise(v.normal + _Time.y);
				float4 nv = float4(v.vertex.xyz + noise * v.normal, v.vertex.w);
				// move model's vertices to screen position 
				o.vertex = UnityObjectToClipPos(nv);	
				// get vertex's world position 
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz; 
				// get world mormal
				o.normal = UnityObjectToWorldNormal(v.normal);
				o.uv = v.uv;

				return o;
             }
             
             fixed4 frag (v2f i) : SV_Target{
				float nu = (i.uv.x < .5)? i.uv.x : (1. - i.uv.x);
				nu = pow(nu, 2.);
				float2 n_uv = float2(nu, i.uv.y);

				float n_a = cnoise(float3(n_uv * 5., 1.) + _Time.y * _NoiseSpeed * -1.) * _Intensity + _Ambient; 
				float n_b = cnoise(float3(n_uv * 10., 1.) + _Time.y * _NoiseSpeed * -1.) * .9; 
				float n_c = cnoise(float3(n_uv * 20., 1.) + _Time.y * _NoiseSpeed * -2.) * .9; 
				float n_d = pow(cnoise(float3(n_uv * 30., 1.) + _Time.y * _NoiseSpeed * -2.), 2.) * .9; 
				float noise = n_a + n_b + n_c + n_d;
				noise = (noise < 0.)? 0. : noise;
				float4 col = float4(noise, noise, noise, 1.);

                 // get vertices directions toward world camera
                 // *note that UnityWorldSpaceViewDir return vertices' direction (not cam's direction)
                 // - float3 WorldSpaceViewDir (float4 v)	
                 // - Returns world space direction (not normalized) from given object space vertex position 
                 // - towards the camera.
                 // - https://docs.unity3d.com/Manual/SL-BuiltinFunctions.html
				 half3 viewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));
				 // get raycast between vertice's dir and normal
				 // discard vertices facing opposite way with view direction 
				 // if the value is closer to 1 then that means the vertex is facing more towards the camera
                 float raycast = saturate(dot(viewDir, i.normal));
                 // make extreme distribution
                 float fresnel = pow(raycast, _Fresnel);

                 // fade out
				 float fade = saturate(pow(1. - i.uv.y, _Fade));

			     col.a *= fresnel * _AlphaOffset * fade;

                 return col;
             }
         ENDCG
		}
	}
}

