// Upgrade NOTE: replaced 'UNITY_INSTANCE_ID' with 'UNITY_VERTEX_INPUT_INSTANCE_ID'

Shader "Custom/DenoisePass"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Pass2Tex ("Texture", 2D) = "white" {}
		_eyeX ("_eyeX", float) = 0.5
		_eyeY ("_eyeY", float) = 0.5
		_iResolutionX ("_iResolutionX", float) = 500
		_iResolutionY ("_iResolutionY", float) = 500
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
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			v2f vert (appdata v)
			{
					v2f o;

					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_INITIALIZE_OUTPUT(v2f, o);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = v.uv;
					return o;
			}

			float4 gausBlur5(sampler2D myTexture, float2 pos, float2 iResolution) // perform gaussian blur
			{
				//this will be our RGBA sum
				float2 pixel_size = float2(1.0,1.0) / iResolution.xy;
				float4 sum = float4(0.0f,0.0f,0.0f,0.0f);
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-2,-2) * pixel_size) * 0.0165315806437010;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-2,-1) * pixel_size) * 0.0297018706890914;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-2, 0) * pixel_size) * 0.0361082918460354;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-2, 1) * pixel_size) * 0.0297018706890914;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-2, 2) * pixel_size) * 0.0165315806437010;

				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-1,-2) * pixel_size) * 0.0297018706890914;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-1,-1) * pixel_size) * 0.0533645960084072;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-1, 0) * pixel_size) * 0.0648748500418541;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-1, 1) * pixel_size) * 0.0533645960084072;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-1, 2) * pixel_size) * 0.0297018706890914;

				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 0,-2) * pixel_size) * 0.0361082918460354;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 0,-1) * pixel_size) * 0.0648748500418541;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 0, 0) * pixel_size) * 0.0788677603272776;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 0, 1) * pixel_size) * 0.0648748500418541;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 0, 2) * pixel_size) * 0.0361082918460354;

				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 1,-2) * pixel_size) * 0.0297018706890914;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 1,-1) * pixel_size) * 0.0533645960084072;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 1, 0) * pixel_size) * 0.0648748500418541;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 1, 1) * pixel_size) * 0.0533645960084072;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 1, 2) * pixel_size) * 0.0297018706890914;

				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 2,-2) * pixel_size) * 0.0165315806437010;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 2,-1) * pixel_size) * 0.0297018706890914;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 2, 0) * pixel_size) * 0.0361082918460354;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 2, 1) * pixel_size) * 0.0297018706890914;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 2, 2) * pixel_size) * 0.0165315806437010;
				return sum;
			}

			float4 gausBlur3(sampler2D myTexture, float2 pos, float2 iResolution) // perform gaussian blur
			{
				//this will be our RGBA sum
				float2 pixel_size = float2(1.0,1.0) / iResolution.xy;
				float4 sum = float4(0.0f,0.0f,0.0f,0.0f);
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-1,-1) * pixel_size) * 0.0751;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-1, 0) * pixel_size) * 0.1238;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-1, 1) * pixel_size) * 0.0751;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 0,-1) * pixel_size) * 0.1238;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 0, 0) * pixel_size) * 0.2042;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 0, 1) * pixel_size) * 0.1238;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 1,-1) * pixel_size) * 0.0751;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 1, 0) * pixel_size) * 0.1238;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 1, 1) * pixel_size) * 0.0751;
				return sum;
			}

			float4 gausBlur11(sampler2D myTexture, float2 pos, float2 iResolution){
				float2 pixel_size = float2(1.0,1.0) / iResolution.xy;
				float4 sum = float4(0.0f,0.0f,0.0f,0.0f);
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-5,-5) * pixel_size) * 0.007959;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-5,-4) * pixel_size) * 0.008049;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-5,-3) * pixel_size) * 0.008120;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-5,-2) * pixel_size) * 0.008171;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-5,-1) * pixel_size) * 0.008202;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-5, 0) * pixel_size) * 0.008212;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-5, 1) * pixel_size) * 0.008202;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-5, 2) * pixel_size) * 0.008171;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-5, 3) * pixel_size) * 0.008120;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-5, 4) * pixel_size) * 0.008049;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-5, 5) * pixel_size) * 0.007959;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-4,-5) * pixel_size) * 0.008049;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-4,-4) * pixel_size) * 0.008140;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-4,-3) * pixel_size) * 0.008212;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-4,-2) * pixel_size) * 0.008263;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-4,-1) * pixel_size) * 0.008295;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-4, 0) * pixel_size) * 0.008305;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-4, 1) * pixel_size) * 0.008295;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-4, 2) * pixel_size) * 0.008263;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-4, 3) * pixel_size) * 0.008212;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-4, 4) * pixel_size) * 0.008140;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-4, 5) * pixel_size) * 0.008049;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-3,-5) * pixel_size) * 0.008120;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-3,-4) * pixel_size) * 0.008212;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-3,-3) * pixel_size) * 0.008284;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-3,-2) * pixel_size) * 0.008336;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-3,-1) * pixel_size) * 0.008367;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-3, 0) * pixel_size) * 0.008378;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-3, 1) * pixel_size) * 0.008367;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-3, 2) * pixel_size) * 0.008336;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-3, 3) * pixel_size) * 0.008284;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-3, 4) * pixel_size) * 0.008212;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-3, 5) * pixel_size) * 0.008120;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-2,-5) * pixel_size) * 0.008171;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-2,-4) * pixel_size) * 0.008263;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-2,-3) * pixel_size) * 0.008336;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-2,-2) * pixel_size) * 0.008388;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-2,-1) * pixel_size) * 0.008420;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-2, 0) * pixel_size) * 0.008430;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-2, 1) * pixel_size) * 0.008420;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-2, 2) * pixel_size) * 0.008388;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-2, 3) * pixel_size) * 0.008336;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-2, 4) * pixel_size) * 0.008263;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-2, 5) * pixel_size) * 0.008171;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-1,-5) * pixel_size) * 0.008202;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-1,-4) * pixel_size) * 0.008295;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-1,-3) * pixel_size) * 0.008367;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-1,-2) * pixel_size) * 0.008420;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-1,-1) * pixel_size) * 0.008451;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-1, 0) * pixel_size) * 0.008462;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-1, 1) * pixel_size) * 0.008451;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-1, 2) * pixel_size) * 0.008420;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-1, 3) * pixel_size) * 0.008367;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-1, 4) * pixel_size) * 0.008295;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2(-1, 5) * pixel_size) * 0.008202;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 0,-5) * pixel_size) * 0.008212;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 0,-4) * pixel_size) * 0.008305;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 0,-3) * pixel_size) * 0.008378;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 0,-2) * pixel_size) * 0.008430;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 0,-1) * pixel_size) * 0.008462;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 0, 0) * pixel_size) * 0.008473;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 0, 1) * pixel_size) * 0.008462;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 0, 2) * pixel_size) * 0.008430;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 0, 3) * pixel_size) * 0.008378;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 0, 4) * pixel_size) * 0.008305;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 0, 5) * pixel_size) * 0.008212;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 1,-5) * pixel_size) * 0.008202;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 1,-4) * pixel_size) * 0.008295;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 1,-3) * pixel_size) * 0.008367;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 1,-2) * pixel_size) * 0.008420;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 1,-1) * pixel_size) * 0.008451;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 1, 0) * pixel_size) * 0.008462;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 1, 1) * pixel_size) * 0.008451;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 1, 2) * pixel_size) * 0.008420;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 1, 3) * pixel_size) * 0.008367;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 1, 4) * pixel_size) * 0.008295;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 1, 5) * pixel_size) * 0.008202;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 2,-5) * pixel_size) * 0.008171;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 2,-4) * pixel_size) * 0.008263;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 2,-3) * pixel_size) * 0.008336;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 2,-2) * pixel_size) * 0.008388;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 2,-1) * pixel_size) * 0.008420;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 2, 0) * pixel_size) * 0.008430;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 2, 1) * pixel_size) * 0.008420;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 2, 2) * pixel_size) * 0.008388;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 2, 3) * pixel_size) * 0.008336;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 2, 4) * pixel_size) * 0.008263;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 2, 5) * pixel_size) * 0.008171;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 3,-5) * pixel_size) * 0.008120;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 3,-4) * pixel_size) * 0.008212;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 3,-3) * pixel_size) * 0.008284;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 3,-2) * pixel_size) * 0.008336;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 3,-1) * pixel_size) * 0.008367;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 3, 0) * pixel_size) * 0.008378;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 3, 1) * pixel_size) * 0.008367;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 3, 2) * pixel_size) * 0.008336;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 3, 3) * pixel_size) * 0.008284;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 3, 4) * pixel_size) * 0.008212;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 3, 5) * pixel_size) * 0.008120;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 4,-5) * pixel_size) * 0.008049;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 4,-4) * pixel_size) * 0.008140;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 4,-3) * pixel_size) * 0.008212;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 4,-2) * pixel_size) * 0.008263;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 4,-1) * pixel_size) * 0.008295;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 4, 0) * pixel_size) * 0.008305;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 4, 1) * pixel_size) * 0.008295;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 4, 2) * pixel_size) * 0.008263;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 4, 3) * pixel_size) * 0.008212;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 4, 4) * pixel_size) * 0.008140;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 4, 5) * pixel_size) * 0.008049;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 5,-5) * pixel_size) * 0.007959;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 5,-4) * pixel_size) * 0.008049;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 5,-3) * pixel_size) * 0.008120;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 5,-2) * pixel_size) * 0.008171;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 5,-1) * pixel_size) * 0.008202;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 5, 0) * pixel_size) * 0.008212;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 5, 1) * pixel_size) * 0.008202;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 5, 2) * pixel_size) * 0.008171;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 5, 3) * pixel_size) * 0.008120;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 5, 4) * pixel_size) * 0.008049;
				sum += UNITY_SAMPLE_SCREENSPACE_TEXTURE(myTexture, pos + float2( 5, 5) * pixel_size) * 0.007959;
				return sum;
			}
			CBUFFER_START(UnityPerMaterial)
				UNITY_DECLARE_SCREENSPACE_TEXTURE(_MainTex);
				UNITY_DECLARE_SCREENSPACE_TEXTURE(_Pass2Tex);
				uniform float _iResolutionX;
				uniform float _iResolutionY;
				uniform float _eyeX;
				uniform float _eyeY;
				half4 _Pass2Tex_ST;
			CBUFFER_END
			fixed4 frag (v2f i) : SV_Target
			{
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				fixed4 col;
				float2 iResolution = float2(_iResolutionX, _iResolutionY);
				float dist = length(i.uv - float2(_eyeX, _eyeY));
				if (dist < 0.2)
					col = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_Pass2Tex, i.uv);
				else if (dist < 0.25)
					col = gausBlur3(_Pass2Tex, i.uv, iResolution);
				else if (dist < 0.35)
					col = gausBlur5(_Pass2Tex, i.uv, iResolution);
				else
					col = gausBlur11(_Pass2Tex, i.uv, iResolution);
				//if (dist < 0.2)
				//	col = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_MainTex, i.uv);
				//else if (dist < 0.25)
				//	col = gausBlur3(_MainTex, i.uv, iResolution);
				//else if (dist < 0.45)
				//	col = gausBlur5(_MainTex, i.uv, iResolution);
				//else
				//	col = gausBlur11(_MainTex, i.uv, iResolution);
				//col = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_MainTex, i.uv);
				return col;
			}
			ENDCG
		}
	}
}
