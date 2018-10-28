Shader "Custom/NeonShader" {
	Properties {
		_PlatformColor("Platform Color", Color) = (1, 1, 1, 1)
		_NeonColor("Neon Color", Color) = (1, 1, 1, 1)
		_NeonPower("Neon Power", Range(1.0, 100.0)) = 3.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		struct Input
		{
			float4 color : Color;
		};

		float4 _PlatformColor;
		float4 _NeonColor;
		float _NeonPower;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			half neon = length(o.Normal);
			o.Albedo = lerp(_PlatformColor, _NeonColor.rgb, pow(neon, _NeonPower));
			o.Albedo *= 5.0;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
