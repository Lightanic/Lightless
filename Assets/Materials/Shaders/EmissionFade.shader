// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Foliage/EmissionFade"
{
    Properties
    {
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_EmissionMap("Emission Map", 2D) = "black" {}
		[HDR]_EmissionColor("Emission Color", Color) = (0,0,0)
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_BumpMap("Bumpmap", 2D) = "bump" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:vert 
        #pragma target 5.0
		#include "UnityCG.cginc"
		struct Input
		{
			float2 uv_MainTex;
			float2 uv_BumpMap;
			float3 worldPos;
		};

		struct v2f
		{
			float4 position : SV_POSITION;
			float3 worldPos: TEXCOORD0;
		};

		v2f vert(inout appdata_full v)
		{
			v2f o;
			o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
			o.position = UnityObjectToClipPos(v.vertex);
			return o;
		}

        sampler2D _MainTex;
		sampler2D _SecondaryTex;
		sampler2D _EmissionMap;
		sampler2D _BumpMap;


		float3 _EmissionColor;
        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

		uniform float FoliageEmissionDistance = 5.f;
		uniform float FoliageFillAmount = 0;
		uniform float4 LanternPos; //Lantern position injected through PlayerProperties.cs script

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
			float distance = length(LanternPos - IN.worldPos);
			float eVal = (FoliageEmissionDistance - distance) / FoliageEmissionDistance;
			eVal = saturate(eVal);
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
			o.Emission = (tex2D(_EmissionMap, IN.uv_MainTex) * _EmissionColor).rgb * (eVal * eVal) * FoliageFillAmount;

        }
        ENDCG
    }
    FallBack "Diffuse"
}
