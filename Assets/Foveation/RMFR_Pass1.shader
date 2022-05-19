// Upgrade NOTE: replaced 'UNITY_INSTANCE_ID' with 'UNITY_VERTEX_INPUT_INSTANCE_ID'

Shader "Custom/RMFR_Pass1"
{
	Properties
	{
		_DebugColor ("Debug Color", Color) = (1, 0, 0, 0.25)
		_MainTex("Texture", 2D) = "white" {}
		_eyeX("_eyeX", float) = 0.5
		_eyeY("_eyeY", float) = 0.5
		_scaleRatio("_scaleRatio", float) = 2.0
		_fx("_fx", float) = 1.0
		_fy("_fy", float) = 1.0
		_iApplyLogMap1("_iApplyRFRMap1", int) = 1
	}
		SubShader
		{
			// No culling or depth
			Cull Off ZWrite Off ZTest Always

			Tags { "RenderPipeline" = "UniversalRenderPipeline" }

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					float4 vertex : SV_POSITION;
				};

				v2f vert(appdata v)
				{
					v2f o;

					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_INITIALIZE_OUTPUT(v2f, o);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = v.uv;
					return o;
				}

				CBUFFER_START(UnityPerMaterial)
					UNITY_DECLARE_SCREENSPACE_TEXTURE(_MainTex);
					uniform float _iResolutionX;
					uniform float _iResolutionY;
					uniform float _eyeX;
					uniform float _eyeY;
					uniform float _scaleRatio;
					uniform float _fx;
					uniform float _fy;
					uniform int _iApplyRFRMap1;
					float4 _DebugColor;
					half4 _MainTex_ST;
				CBUFFER_END


				fixed4 frag(v2f i) : SV_Target
				{
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

					if (_iApplyRFRMap1 < 0.5)
						return UNITY_SAMPLE_SCREENSPACE_TEXTURE(_MainTex, i.uv);

				float2 cursorPos = float2(_eyeX, _eyeY); //(0,1)

				float maxDxPos = 1.0 - cursorPos.x;
				float maxDyPos = 1.0 - cursorPos.y;
				float maxDxNeg = cursorPos.x;
				float maxDyNeg = cursorPos.y;

				float norDxPos = _fx * maxDxPos / (_fx + maxDxPos);
				float norDyPos = _fy * maxDyPos / (_fy + maxDyPos);
				float norDxNeg = _fx * maxDxNeg / (_fx + maxDxNeg);
				float norDyNeg = _fy * maxDyNeg / (_fy + maxDyNeg);

				float2 tc = (i.uv - cursorPos); //i.uv.x > cursorPos.x : [0,maxDxPos] i.uv.x < cursorPos.x : [-maxDxNeg, 0]

				float x = tc.x > 0 ? tc.x / maxDxPos : tc.x / maxDxNeg;//[0,1], [-1,0]
				float y = tc.y > 0 ? tc.y / maxDyPos : tc.y / maxDyNeg; 
				if (tc.x >= 0) {
					x = x * norDxPos; //[0,norDxPos]
					x = _fx * x / (_fx - x); //[0, 1]
					x = x + cursorPos.x;
				}
				else {
					x = x * norDxNeg;
					x = _fx * x / (_fx + x);
					x = x + cursorPos.x;
				}

				if (tc.y >= 0) {
					y = y * norDyPos;
					y = _fy * y / (_fy - y);
					y = y + cursorPos.y;
				}
				else {
					y = y * norDyNeg;
					y = _fy * y / (_fy + y);
					y = y + cursorPos.y;
				}

				float4 pq = (float4(x, y, 1, 1)); //0,1 --> 0-1
				fixed4 col = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_MainTex, pq);
				return col;
			}
			ENDCG
		}
	}
}
