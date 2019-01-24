Shader "Custom/Grass"
{
	Properties
	{
		_Color("Main Color", Color) = (0.5,0.5,0.5,1)
		_MainTex("Texture", 2D) = "white" {}
		_Speed("MoveSpeed", Range(20,100)) = 25 // speed of the swaying
		_Rigidness("Rigidness", Range(1,50)) = 25 // lower makes it look more "liquid" higher makes it look rigid
		_SwayMax("Sway Max", Range(0, 0.5)) = .005 // how far the swaying goes
		_YOffset("Y offset", float) = 0.5// y offset, below this is no animation
		_PlayerPush("Player Push force",Range(0,10.0)) = 2.0
		_Scale("Scale",Range(1.0,3.0)) = 1.0
	}

	SubShader
	{
		Tags { "RenderType" = "Opaque" }// disable batching lets us keep object space
		LOD 200


	CGPROGRAM
	#pragma surface surf Standard fullforwardshadows vertex:vert
	#pragma target 3.0

	sampler2D _Ramp;

	sampler2D _MainTex;
	float4 _Color;

	float _Speed;
	float _SwayMax;
	float _YOffset;
	float _Rigidness;
	float _PlayerPush;
	float _Scale;

	uniform float4 PlayerPos;

	struct Input {
		float2 uv_MainTex : TEXCOORD0;
	};
	void vert(inout appdata_full v)//
	{
		float3 wpos = mul(unity_ObjectToWorld, v.vertex).xyz;// world position
		float x = sin(wpos.x / _Rigidness + (_Time.x * _Speed)) *(v.vertex.y - _YOffset) * 5;// x axis movements
		float z = sin(wpos.z / _Rigidness + (_Time.x * _Speed)) *(v.vertex.y - _YOffset) * 5;// z axis movements
		v.vertex.x += step(0,v.vertex.y - _YOffset) * x * _SwayMax;// apply the movement if the vertex's y above the YOffset
		v.vertex.z += step(0,v.vertex.y - _YOffset) * z * _SwayMax;

		float3 dirAway = wpos - float3(PlayerPos.x, PlayerPos.y, PlayerPos.z);
		float dist = length(dirAway);
		float scale = max(0,_Scale - min(_Scale, dist));
		dirAway = normalize(dirAway);
		v.vertex.x += step(0, v.vertex.y - _YOffset) * dirAway * _PlayerPush * scale;
		v.vertex.z += step(0, v.vertex.y - _YOffset) * dirAway * _PlayerPush * scale;
	}

	void surf(Input IN, inout SurfaceOutputStandard o)
	{
		// Albedo comes from a texture tinted by color
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		o.Albedo = c.rgb;
		// Metallic and smoothness come from slider variables
		o.Alpha = c.a;
	}
	ENDCG

	}

	Fallback "Diffuse"
}
