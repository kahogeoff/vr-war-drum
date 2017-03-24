Shader "RAIN/RAINShader"
{
	SubShader
	{
		// Default shader
		Pass
		{
			BindChannels { Bind "Color", color }
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off 
			Cull Off 
			Fog { Mode Off }
			Color(1, 1, 1, 1)
		}
		// Outline sphere shader (first pass)
		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			ZTest Off
			ZWrite Off
			Cull Front

			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
	
			float4 _colorSolid;
			float _colorAlpha;
			
			struct vert_out
			{
				float4 position : POSITION;
			};
			
			vert_out vert(appdata_base v)
			{
				vert_out tOut;
				tOut.position = mul(UNITY_MATRIX_MVP, v.vertex);
				
				return tOut;
			}
	
			float4 frag(vert_out f) : COLOR
			{
				return float4(_colorSolid.rgb, _colorAlpha);
			}
				
			ENDCG
		}
		// Outline sphere shader (second pass)
		Pass
		{
			Blend One Zero
			ZTest LEqual
			Cull Front

			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
	
			float4 _colorSolid;
			
			struct vert_out
			{
				float4 position : POSITION;
			};
			
			vert_out vert(appdata_base v)
			{
				vert_out tOut;
				tOut.position = mul(UNITY_MATRIX_MVP, v.vertex);
				
				return tOut;
			}
	
			float4 frag(vert_out f) : COLOR
			{
				return _colorSolid;
			}
				
			ENDCG
		}
		// Outline sphere shader (third pass)
		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			ZTest Off
			ZWrite Off

			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
	
			float4 _colorSolid;
			float _colorAlpha;
			
			struct vert_out
			{
				float4 position : POSITION;
			};
			
			vert_out vert(appdata_base v)
			{
				float4x4 tScale = float4x4(0.8,   0,   0,   0,
										     0, 0.8,   0,   0,
										     0,   0, 0.8,   0,
										     0,   0,   0,   1);
				vert_out tOut;
				tOut.position = mul(UNITY_MATRIX_MVP, mul(tScale, v.vertex));
				
				return tOut;
			}
	
			float4 frag(vert_out f) : COLOR
			{
				return float4(_colorSolid.rgb, _colorAlpha);
			}
				
			ENDCG
		}
		// Outline sphere shader (fourth pass)
		Pass
		{
			Blend One Zero
			ZTest LEqual

			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
	
			float4 _colorSolid;
			
			struct vert_out
			{
				float4 position : POSITION;
			};
			
			vert_out vert(appdata_base v)
			{
				float4x4 tScale = float4x4(0.8,   0,   0,   0,
										     0, 0.8,   0,   0,
										     0,   0, 0.8,   0,
										     0,   0,   0,   1);
				vert_out tOut;
				tOut.position = mul(UNITY_MATRIX_MVP, mul(tScale, v.vertex));
				
				return tOut;
			}
	
			float4 frag(vert_out f) : COLOR
			{
				return _colorSolid;
			}
				
			ENDCG
		}
		// Outline polygon shader
		Pass
		{
			Offset -2, -2

			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
	
			float _height;
			float4 _colorSolid;
			float4 _colorOutline;
			sampler2D _outlineTexture;
			
			struct vert_out
			{
				float4 position : POSITION;
				float4 texcoord : TEXCOORD0;
			};
			
			vert_out vert(appdata_base v)
			{
				vert_out tOut;
				tOut.position = mul(UNITY_MATRIX_MVP, v.vertex + float4(0, _height, 0, 0));
				tOut.texcoord = v.texcoord;
				
				return tOut;
			}
	
			float4 frag(vert_out f) : COLOR
			{
				return lerp(_colorOutline, _colorSolid, tex2D(_outlineTexture, f.texcoord.xy).x);
			}
				
			ENDCG
		}
		// Outline unculled shader
		Pass
		{
			Cull Off
			Offset -2, -2
			
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
	
			float _height;
			float4 _colorSolid;
			float4 _colorOutline;
			sampler2D _outlineTexture;
			
			struct vert_out
			{
				float4 position : POSITION;
				float4 texcoord : TEXCOORD0;
			};
			
			vert_out vert(appdata_base v)
			{
				vert_out tOut;
				tOut.position = mul(UNITY_MATRIX_MVP, v.vertex + float4(0, _height, 0, 0));
				tOut.texcoord = v.texcoord;
				
				return tOut;
			}
	
			float4 frag(vert_out f) : COLOR
			{
				return lerp(_colorOutline, _colorSolid, tex2D(_outlineTexture, f.texcoord.xy).x);
			}
				
			ENDCG
		}
		// Outline range shader
		Pass
		{
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			
			float4 _colorSolid;
			float4 _colorOutline;
			sampler2D _outlineTexture;

			float _angle;
			float _tiltAngle;
			
			struct vert_out
			{
				float4 position : POSITION;
				float4 texcoord : TEXCOORD0;
			};
			
			vert_out vert(appdata_base v)
			{
				float tBackAngle = 3.14159265 * 2 * (v.texcoord.y - 0.5);
				float tForwardAngle = 3.14159265 * 2 * (v.texcoord.y - 0.5) * _angle;
				
				float4x4 tBackRotation = float4x4(cos(-tBackAngle), 0, sin(-tBackAngle), 0,
												  0, 1, 0, 0,
												  -sin(-tBackAngle), 0, cos(-tBackAngle), 0,
												  0, 0, 0, 1);
				float4x4 tTiltRotation = float4x4(1, 0, 0, 0,
												  0, cos(_tiltAngle), -sin(_tiltAngle), 0,
												  0, sin(_tiltAngle), cos(_tiltAngle), 0,
												  0, 0, 0, 1);
				float4x4 tForwardRotation = float4x4(cos(tForwardAngle), 0, sin(tForwardAngle), 0,
													 0, 1, 0, 0,
													 -sin(tForwardAngle), 0, cos(tForwardAngle), 0,
													 0, 0, 0, 1);
				
				vert_out tOut;
				tOut.position = mul(UNITY_MATRIX_MVP, mul(tForwardRotation, mul(tTiltRotation, mul(tBackRotation, v.vertex))));
				tOut.texcoord = v.texcoord;
				
				return tOut;
			}
	
			float4 frag(vert_out f) : COLOR
			{
				return lerp(_colorOutline, _colorSolid, tex2D(_outlineTexture, float2(f.texcoord.x, 0.5)).x);
			}
				
			ENDCG
		}
		// Outline range line shader
		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off

			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			
			float4 _colorSolid;

			struct vert_out
			{
				float4 position : POSITION;
				float4 texcoord : TEXCOORD0;
			};
			
			vert_out vert(appdata_base v)
			{
				vert_out tOut;
				tOut.position = mul(UNITY_MATRIX_MVP, v.vertex);
				tOut.texcoord = v.texcoord;
				
				return tOut;
			}
	
			float4 frag(vert_out f) : COLOR
			{
				return float4(_colorSolid.xyz, saturate(f.texcoord.x * 1.25 - 0.25));
			}
				
			ENDCG
		}
		// Behavior node shader
		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			Cull Off
			Fog { Mode Off }

			CGPROGRAM
		 
			#pragma vertex vert
			#pragma fragment frag
		 	
			#include "UnityCG.cginc"
		 
			sampler2D _texture;
		 
			struct appdata
			{
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
				float4 color : COLOR;
			};
		 
			struct vert_out
			{
		 		float4 position : POSITION;
		 		float4 texcoord : TEXCOORD0;
		 		float4 color : COLOR;
			};
		 	
			vert_out vert(appdata v)
			{
		 		vert_out tOut;
		 		tOut.position = mul(UNITY_MATRIX_MVP, v.vertex);
		 		tOut.texcoord = v.texcoord;
		 		tOut.color = v.color;
		 		
		 		return tOut;
			}
		 
			float4 frag(vert_out f) : COLOR
			{
				return f.color * tex2D(_texture, f.texcoord.xy);
			}
		 	
			ENDCG
		}
	}
    FallBack "Diffuse"
}
