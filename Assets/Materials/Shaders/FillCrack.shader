Shader "Custom/FillCrack"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_EmissionMap("Emission Map", 2D) = "black" {}
		[HDR]_EmissionColor("Emission Color", Color) = (0,0,0)
		_SecondaryTex ("Secondary (RGB)", 2D) = "black" {}
		[HDR]_SecondaryEmissionColor("Secondary Emission Color", Color) = (0,0,0)
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 5.0

		sampler2D _MainTex;
		sampler2D _SecondaryTex;
		sampler2D _EmissionMap;
		float3 _EmissionColor;
		float3 _SecondaryEmissionColor;
		uniform float4 _HitTexCoord;
		uniform float _FillValue = 0.f;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

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
			float4 secondary = tex2D(_SecondaryTex, IN.uv_MainTex) * _Color;
			float2 uv = IN.uv_MainTex;
			float2 center = _HitTexCoord.xy;
			float r = abs(_FillValue) * 0.9f;
			float d = length(uv - center);
			float fill = (d <= r);

			fixed4 c2 = tex2D(_EmissionMap, IN.uv_MainTex) * fill;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = 1;
			o.Emission = c2.rgb * _EmissionColor * c2.a + secondary.rgb * _SecondaryEmissionColor * secondary.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
