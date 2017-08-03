// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "CouchGames/Toon/LitTest" {
	Properties {
		_Color ("Main Color", Color) = (0.5,0.5,0.5,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Ramp ("Toon Ramp (RGB)", 2D) = "gray" {} 
		_Outline("_Outline", Range(0,0.1)) = 0
		_OutlineColor("Color", Color) = (1, 1, 1, 1)
	}


	SubShader {
		
			Pass{
				Tags { "RenderType" = "Opaque" }
				LOD 200
				Cull front

					CGPROGRAM

					#pragma vertex vert
					#pragma fragment frag
					#include "UnityCG.cginc"

							struct v2f {
							float4 pos : SV_POSITION;
						};

						float _Outline;
						float4 _OutlineColor;

						float4 vert(appdata_base v) : SV_POSITION
						{
						v2f o;
						o.pos = UnityObjectToClipPos(v.vertex);
						float3 normal = mul((float3x3) UNITY_MATRIX_MV, v.normal);
						normal.x *= UNITY_MATRIX_P[0][0];
						normal.y *= UNITY_MATRIX_P[1][1];
						o.pos.xy += normal.xy * _Outline;
						return o.pos;
						}

							half4 frag(v2f i) : COLOR{
							return _OutlineColor;
						}

					ENDCG
				}


				CGPROGRAM
				#pragma surface surf ToonRamp

				sampler2D _Ramp;

						// custom lighting function that uses a texture ramp based
						// on angle between light direction and normal
						#pragma lighting ToonRamp exclude_path:prepass
						inline half4 LightingToonRamp(SurfaceOutput s, half3 lightDir, half atten)
						{
							#ifndef USING_DIRECTIONAL_LIGHT
							lightDir = normalize(lightDir);
							#endif

							half d = dot(s.Normal, lightDir)*0.5 + 0.5;
							half3 ramp = tex2D(_Ramp, float2(d,d)).rgb;

							half4 c;
							c.rgb = s.Albedo * _LightColor0.rgb * ramp * (atten * 2);
							c.a = 0;
							return c;
						}


						sampler2D _MainTex;
						float4 _Color;

						struct Input {
							float2 uv_MainTex : TEXCOORD0;
						};

						void surf(Input IN, inout SurfaceOutput o) {
							half4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
							o.Albedo = c.rgb;
							o.Alpha = c.a;
						}
						ENDCG

					
	} 

	Fallback "Diffuse"
}
