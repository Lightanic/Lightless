Shader "Foliage/Wavy"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
		_NoiseSpeedX("Noise Speed X", Range(0 , 100)) = 0.0
		_NoiseSpeedY("Noise Speed Y", Range(0 , 100)) = 0.0
		_NoiseSpeedZ("Noise Speed Z", Range(0 , 100)) = 1.0

		_NoiseFrequency("Noise Frequency", Range(0 , 1)) = 0.1
		_NoiseAmplitude("Noise Amplitude", Range(0 , 10)) = 2.0

		_NoiseAbs("Noise Abs", Range(0 , 1)) = 1.0

		[HDR] _ColourA("Color A", Color) = (0,0,0,0)
		[HDR] _ColourB("Color B", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM

#pragma vertex vert
#pragma multi_compile_fog

#include "UnityCG.cginc"
#include "SimplexNoise3D.hlsl"
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;


		struct appdata
		{
			float4 vertex : POSITION;
			float3 normal : NORMAL;
			fixed4 color : COLOR;
			float4 tc0 : TEXCOORD0;
			float4 texcoord1 : TEXCOORD1;
			float4 texcoord2 : TEXCOORD2;
		};

		struct v2f
		{
			//float4 tc0 : TEXCOORD0;
			//float4 tc1 : TEXCOORD1;
			//UNITY_FOG_COORDS(1)
			float4 position : SV_POSITION;
			//fixed4 color : COLOR;
		};


		struct Input
		{
			float4 vertex: SV_POSITION;
			//float2 uv_MainTex;
			float4 tc0 : TEXCOORD0;
			float4 tc1 : TEXCOORD1;
			fixed4 color : COLOR;
			UNITY_FOG_COORDS(2)
		};



		float4 _MainTex_ST;

		float _NoiseSpeedX;
		float _NoiseSpeedY;
		float _NoiseSpeedZ;

		float _NoiseFrequency;
		float _NoiseAmplitude;

		float _NoiseAbs;

		float4 _ColourA;
		float4 _ColourB;

		Input vert(inout appdata v)
		{
			Input o;

			float3 particleCenter = float3(v.tc0.zw, v.texcoord1.x);
			float offset = snoise((particleCenter + 1) * _NoiseFrequency);
			float offset1 = snoise((particleCenter + 2) * _NoiseFrequency);
			float offset2 = snoise((particleCenter + 3) * _NoiseFrequency);
			float3 noiseOffset = _Time.y * float3(_NoiseSpeedX + offset, _NoiseSpeedY + offset1, _NoiseSpeedZ + offset2);

			float noise = snoise((particleCenter + noiseOffset) * _NoiseFrequency);
			float noise1 = snoise((particleCenter + noiseOffset) * _NoiseFrequency);
			float noise2 = snoise((particleCenter + noiseOffset) * _NoiseFrequency);

			float noise01 = (noise + 1.0) / 2.0;
			float noiseRemap = lerp(noise, noise01, _NoiseAbs);
			float noiseRemap1 = lerp(noise1, noise01, _NoiseAbs);
			float noiseRemap2 = lerp(noise2, noise01, _NoiseAbs);

			float3 vertexOffset = float3(noiseRemap * _NoiseAmplitude, noiseRemap1 * _NoiseAmplitude, noiseRemap2 * _NoiseAmplitude);

			v.vertex.xyz += vertexOffset;
			o.vertex = UnityObjectToClipPos(v.vertex);

			// Initialize outgoing colour with the data recieved from the particle system stored in the colour vertex input.

			o.color = v.color;

			o.tc0.xy = TRANSFORM_TEX(v.tc0, _MainTex);

			// Initialize outgoing tex coord variables.
			o.tc0 = v.tc0;
			o.tc0.zw = v.tc0.zw;
			o.tc1 = v.texcoord1;

			UNITY_TRANSFER_FOG(o, o.vertex);
			return o;
		}

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input i, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, i.tc0) * _ColourA;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
