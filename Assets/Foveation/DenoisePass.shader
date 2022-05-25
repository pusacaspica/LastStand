Shader "Custom/DenoisePass"
{
	Properties
	{
		_DebugColor11 ("Debug Color Gaussian 11", Color) = (1, 0, 0, 1)
		_DebugColor5 ("Debug Color Gaussian 5", Color) = (0, 1, 0, 1)
		_DebugColor3 ("Debug Color Gaussian 3", Color) = (0, 0, 1, 1)
		_MainTex ("Texture", 2D) = "white" {}
		_Pass2Tex ("Texture", 2D) = "white" {}
		_eyeX ("_eyeX", float) = 0.5
		_eyeY ("_eyeY", float) = 0.5
		_iResolutionX ("_iResolutionX", float) = 500
		_iResolutionY ("_iResolutionY", float) = 500
		_denoiseRadius3 ("Denoise Radius Gaussian 3", float) = 0.2
		_denoiseRadius5 ("Denoise Radius Gaussian 5", float) = 0.25
		_denoiseRadius11 ("Denoise Radius Gaussian 11", float) = 0.35
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				//fixed4 color: COLOR0;
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				//fixed4 color: COLOR0;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				//o.color = v.color;
				o.uv = v.uv;
				return o;
			}

			float4 gausBlur5(sampler2D myTexture, float2 pos, float2 iResolution) // perform gaussian blur
			{
				//this will be our RGBA sum
				float2 pixel_size = float2(1.0,1.0) / iResolution.xy;
				float4 sum = float4(0.0f,0.0f,0.0f,0.0f);
				sum += tex2D(myTexture, pos + float2(-2,-2) * pixel_size) * 0.0165315806437010;
				sum += tex2D(myTexture, pos + float2(-2,-1) * pixel_size) * 0.0297018706890914;
				sum += tex2D(myTexture, pos + float2(-2, 0) * pixel_size) * 0.0361082918460354;
				sum += tex2D(myTexture, pos + float2(-2, 1) * pixel_size) * 0.0297018706890914;
				sum += tex2D(myTexture, pos + float2(-2, 2) * pixel_size) * 0.0165315806437010;

				sum += tex2D(myTexture, pos + float2(-1,-2) * pixel_size) * 0.0297018706890914;
				sum += tex2D(myTexture, pos + float2(-1,-1) * pixel_size) * 0.0533645960084072;
				sum += tex2D(myTexture, pos + float2(-1, 0) * pixel_size) * 0.0648748500418541;
				sum += tex2D(myTexture, pos + float2(-1, 1) * pixel_size) * 0.0533645960084072;
				sum += tex2D(myTexture, pos + float2(-1, 2) * pixel_size) * 0.0297018706890914;

				sum += tex2D(myTexture, pos + float2( 0,-2) * pixel_size) * 0.0361082918460354;
				sum += tex2D(myTexture, pos + float2( 0,-1) * pixel_size) * 0.0648748500418541;
				sum += tex2D(myTexture, pos + float2( 0, 0) * pixel_size) * 0.0788677603272776;
				sum += tex2D(myTexture, pos + float2( 0, 1) * pixel_size) * 0.0648748500418541;
				sum += tex2D(myTexture, pos + float2( 0, 2) * pixel_size) * 0.0361082918460354;

				sum += tex2D(myTexture, pos + float2( 1,-2) * pixel_size) * 0.0297018706890914;
				sum += tex2D(myTexture, pos + float2( 1,-1) * pixel_size) * 0.0533645960084072;
				sum += tex2D(myTexture, pos + float2( 1, 0) * pixel_size) * 0.0648748500418541;
				sum += tex2D(myTexture, pos + float2( 1, 1) * pixel_size) * 0.0533645960084072;
				sum += tex2D(myTexture, pos + float2( 1, 2) * pixel_size) * 0.0297018706890914;

				sum += tex2D(myTexture, pos + float2( 2,-2) * pixel_size) * 0.0165315806437010;
				sum += tex2D(myTexture, pos + float2( 2,-1) * pixel_size) * 0.0297018706890914;
				sum += tex2D(myTexture, pos + float2( 2, 0) * pixel_size) * 0.0361082918460354;
				sum += tex2D(myTexture, pos + float2( 2, 1) * pixel_size) * 0.0297018706890914;
				sum += tex2D(myTexture, pos + float2( 2, 2) * pixel_size) * 0.0165315806437010;
				return sum;
			}

			float4 gausBlur3(sampler2D myTexture, float2 pos, float2 iResolution) // perform gaussian blur
			{
				//this will be our RGBA sum
				float2 pixel_size = float2(1.0,1.0) / iResolution.xy;
				float4 sum = float4(0.0f,0.0f,0.0f,0.0f);
				sum += tex2D(myTexture, pos + float2(-1,-1) * pixel_size) * 0.0751;
				sum += tex2D(myTexture, pos + float2(-1, 0) * pixel_size) * 0.1238;
				sum += tex2D(myTexture, pos + float2(-1, 1) * pixel_size) * 0.0751;
				sum += tex2D(myTexture, pos + float2( 0,-1) * pixel_size) * 0.1238;
				sum += tex2D(myTexture, pos + float2( 0, 0) * pixel_size) * 0.2042;
				sum += tex2D(myTexture, pos + float2( 0, 1) * pixel_size) * 0.1238;
				sum += tex2D(myTexture, pos + float2( 1,-1) * pixel_size) * 0.0751;
				sum += tex2D(myTexture, pos + float2( 1, 0) * pixel_size) * 0.1238;
				sum += tex2D(myTexture, pos + float2( 1, 1) * pixel_size) * 0.0751;
				return sum;
			}

			float4 gausBlur11(sampler2D myTexture, float2 pos, float2 iResolution){
				float2 pixel_size = float2(1.0,1.0) / iResolution.xy;
				float4 sum = float4(0.0f,0.0f,0.0f,0.0f);
				sum += tex2D(myTexture, pos + float2(-5,-5) * pixel_size) * 0.007959;
				sum += tex2D(myTexture, pos + float2(-5,-4) * pixel_size) * 0.008049;
				sum += tex2D(myTexture, pos + float2(-5,-3) * pixel_size) * 0.008120;
				sum += tex2D(myTexture, pos + float2(-5,-2) * pixel_size) * 0.008171;
				sum += tex2D(myTexture, pos + float2(-5,-1) * pixel_size) * 0.008202;
				sum += tex2D(myTexture, pos + float2(-5, 0) * pixel_size) * 0.008212;
				sum += tex2D(myTexture, pos + float2(-5, 1) * pixel_size) * 0.008202;
				sum += tex2D(myTexture, pos + float2(-5, 2) * pixel_size) * 0.008171;
				sum += tex2D(myTexture, pos + float2(-5, 3) * pixel_size) * 0.008120;
				sum += tex2D(myTexture, pos + float2(-5, 4) * pixel_size) * 0.008049;
				sum += tex2D(myTexture, pos + float2(-5, 5) * pixel_size) * 0.007959;
				sum += tex2D(myTexture, pos + float2(-4,-5) * pixel_size) * 0.008049;
				sum += tex2D(myTexture, pos + float2(-4,-4) * pixel_size) * 0.008140;
				sum += tex2D(myTexture, pos + float2(-4,-3) * pixel_size) * 0.008212;
				sum += tex2D(myTexture, pos + float2(-4,-2) * pixel_size) * 0.008263;
				sum += tex2D(myTexture, pos + float2(-4,-1) * pixel_size) * 0.008295;
				sum += tex2D(myTexture, pos + float2(-4, 0) * pixel_size) * 0.008305;
				sum += tex2D(myTexture, pos + float2(-4, 1) * pixel_size) * 0.008295;
				sum += tex2D(myTexture, pos + float2(-4, 2) * pixel_size) * 0.008263;
				sum += tex2D(myTexture, pos + float2(-4, 3) * pixel_size) * 0.008212;
				sum += tex2D(myTexture, pos + float2(-4, 4) * pixel_size) * 0.008140;
				sum += tex2D(myTexture, pos + float2(-4, 5) * pixel_size) * 0.008049;
				sum += tex2D(myTexture, pos + float2(-3,-5) * pixel_size) * 0.008120;
				sum += tex2D(myTexture, pos + float2(-3,-4) * pixel_size) * 0.008212;
				sum += tex2D(myTexture, pos + float2(-3,-3) * pixel_size) * 0.008284;
				sum += tex2D(myTexture, pos + float2(-3,-2) * pixel_size) * 0.008336;
				sum += tex2D(myTexture, pos + float2(-3,-1) * pixel_size) * 0.008367;
				sum += tex2D(myTexture, pos + float2(-3, 0) * pixel_size) * 0.008378;
				sum += tex2D(myTexture, pos + float2(-3, 1) * pixel_size) * 0.008367;
				sum += tex2D(myTexture, pos + float2(-3, 2) * pixel_size) * 0.008336;
				sum += tex2D(myTexture, pos + float2(-3, 3) * pixel_size) * 0.008284;
				sum += tex2D(myTexture, pos + float2(-3, 4) * pixel_size) * 0.008212;
				sum += tex2D(myTexture, pos + float2(-3, 5) * pixel_size) * 0.008120;
				sum += tex2D(myTexture, pos + float2(-2,-5) * pixel_size) * 0.008171;
				sum += tex2D(myTexture, pos + float2(-2,-4) * pixel_size) * 0.008263;
				sum += tex2D(myTexture, pos + float2(-2,-3) * pixel_size) * 0.008336;
				sum += tex2D(myTexture, pos + float2(-2,-2) * pixel_size) * 0.008388;
				sum += tex2D(myTexture, pos + float2(-2,-1) * pixel_size) * 0.008420;
				sum += tex2D(myTexture, pos + float2(-2, 0) * pixel_size) * 0.008430;
				sum += tex2D(myTexture, pos + float2(-2, 1) * pixel_size) * 0.008420;
				sum += tex2D(myTexture, pos + float2(-2, 2) * pixel_size) * 0.008388;
				sum += tex2D(myTexture, pos + float2(-2, 3) * pixel_size) * 0.008336;
				sum += tex2D(myTexture, pos + float2(-2, 4) * pixel_size) * 0.008263;
				sum += tex2D(myTexture, pos + float2(-2, 5) * pixel_size) * 0.008171;
				sum += tex2D(myTexture, pos + float2(-1,-5) * pixel_size) * 0.008202;
				sum += tex2D(myTexture, pos + float2(-1,-4) * pixel_size) * 0.008295;
				sum += tex2D(myTexture, pos + float2(-1,-3) * pixel_size) * 0.008367;
				sum += tex2D(myTexture, pos + float2(-1,-2) * pixel_size) * 0.008420;
				sum += tex2D(myTexture, pos + float2(-1,-1) * pixel_size) * 0.008451;
				sum += tex2D(myTexture, pos + float2(-1, 0) * pixel_size) * 0.008462;
				sum += tex2D(myTexture, pos + float2(-1, 1) * pixel_size) * 0.008451;
				sum += tex2D(myTexture, pos + float2(-1, 2) * pixel_size) * 0.008420;
				sum += tex2D(myTexture, pos + float2(-1, 3) * pixel_size) * 0.008367;
				sum += tex2D(myTexture, pos + float2(-1, 4) * pixel_size) * 0.008295;
				sum += tex2D(myTexture, pos + float2(-1, 5) * pixel_size) * 0.008202;
				sum += tex2D(myTexture, pos + float2( 0,-5) * pixel_size) * 0.008212;
				sum += tex2D(myTexture, pos + float2( 0,-4) * pixel_size) * 0.008305;
				sum += tex2D(myTexture, pos + float2( 0,-3) * pixel_size) * 0.008378;
				sum += tex2D(myTexture, pos + float2( 0,-2) * pixel_size) * 0.008430;
				sum += tex2D(myTexture, pos + float2( 0,-1) * pixel_size) * 0.008462;
				sum += tex2D(myTexture, pos + float2( 0, 0) * pixel_size) * 0.008473;
				sum += tex2D(myTexture, pos + float2( 0, 1) * pixel_size) * 0.008462;
				sum += tex2D(myTexture, pos + float2( 0, 2) * pixel_size) * 0.008430;
				sum += tex2D(myTexture, pos + float2( 0, 3) * pixel_size) * 0.008378;
				sum += tex2D(myTexture, pos + float2( 0, 4) * pixel_size) * 0.008305;
				sum += tex2D(myTexture, pos + float2( 0, 5) * pixel_size) * 0.008212;
				sum += tex2D(myTexture, pos + float2( 1,-5) * pixel_size) * 0.008202;
				sum += tex2D(myTexture, pos + float2( 1,-4) * pixel_size) * 0.008295;
				sum += tex2D(myTexture, pos + float2( 1,-3) * pixel_size) * 0.008367;
				sum += tex2D(myTexture, pos + float2( 1,-2) * pixel_size) * 0.008420;
				sum += tex2D(myTexture, pos + float2( 1,-1) * pixel_size) * 0.008451;
				sum += tex2D(myTexture, pos + float2( 1, 0) * pixel_size) * 0.008462;
				sum += tex2D(myTexture, pos + float2( 1, 1) * pixel_size) * 0.008451;
				sum += tex2D(myTexture, pos + float2( 1, 2) * pixel_size) * 0.008420;
				sum += tex2D(myTexture, pos + float2( 1, 3) * pixel_size) * 0.008367;
				sum += tex2D(myTexture, pos + float2( 1, 4) * pixel_size) * 0.008295;
				sum += tex2D(myTexture, pos + float2( 1, 5) * pixel_size) * 0.008202;
				sum += tex2D(myTexture, pos + float2( 2,-5) * pixel_size) * 0.008171;
				sum += tex2D(myTexture, pos + float2( 2,-4) * pixel_size) * 0.008263;
				sum += tex2D(myTexture, pos + float2( 2,-3) * pixel_size) * 0.008336;
				sum += tex2D(myTexture, pos + float2( 2,-2) * pixel_size) * 0.008388;
				sum += tex2D(myTexture, pos + float2( 2,-1) * pixel_size) * 0.008420;
				sum += tex2D(myTexture, pos + float2( 2, 0) * pixel_size) * 0.008430;
				sum += tex2D(myTexture, pos + float2( 2, 1) * pixel_size) * 0.008420;
				sum += tex2D(myTexture, pos + float2( 2, 2) * pixel_size) * 0.008388;
				sum += tex2D(myTexture, pos + float2( 2, 3) * pixel_size) * 0.008336;
				sum += tex2D(myTexture, pos + float2( 2, 4) * pixel_size) * 0.008263;
				sum += tex2D(myTexture, pos + float2( 2, 5) * pixel_size) * 0.008171;
				sum += tex2D(myTexture, pos + float2( 3,-5) * pixel_size) * 0.008120;
				sum += tex2D(myTexture, pos + float2( 3,-4) * pixel_size) * 0.008212;
				sum += tex2D(myTexture, pos + float2( 3,-3) * pixel_size) * 0.008284;
				sum += tex2D(myTexture, pos + float2( 3,-2) * pixel_size) * 0.008336;
				sum += tex2D(myTexture, pos + float2( 3,-1) * pixel_size) * 0.008367;
				sum += tex2D(myTexture, pos + float2( 3, 0) * pixel_size) * 0.008378;
				sum += tex2D(myTexture, pos + float2( 3, 1) * pixel_size) * 0.008367;
				sum += tex2D(myTexture, pos + float2( 3, 2) * pixel_size) * 0.008336;
				sum += tex2D(myTexture, pos + float2( 3, 3) * pixel_size) * 0.008284;
				sum += tex2D(myTexture, pos + float2( 3, 4) * pixel_size) * 0.008212;
				sum += tex2D(myTexture, pos + float2( 3, 5) * pixel_size) * 0.008120;
				sum += tex2D(myTexture, pos + float2( 4,-5) * pixel_size) * 0.008049;
				sum += tex2D(myTexture, pos + float2( 4,-4) * pixel_size) * 0.008140;
				sum += tex2D(myTexture, pos + float2( 4,-3) * pixel_size) * 0.008212;
				sum += tex2D(myTexture, pos + float2( 4,-2) * pixel_size) * 0.008263;
				sum += tex2D(myTexture, pos + float2( 4,-1) * pixel_size) * 0.008295;
				sum += tex2D(myTexture, pos + float2( 4, 0) * pixel_size) * 0.008305;
				sum += tex2D(myTexture, pos + float2( 4, 1) * pixel_size) * 0.008295;
				sum += tex2D(myTexture, pos + float2( 4, 2) * pixel_size) * 0.008263;
				sum += tex2D(myTexture, pos + float2( 4, 3) * pixel_size) * 0.008212;
				sum += tex2D(myTexture, pos + float2( 4, 4) * pixel_size) * 0.008140;
				sum += tex2D(myTexture, pos + float2( 4, 5) * pixel_size) * 0.008049;
				sum += tex2D(myTexture, pos + float2( 5,-5) * pixel_size) * 0.007959;
				sum += tex2D(myTexture, pos + float2( 5,-4) * pixel_size) * 0.008049;
				sum += tex2D(myTexture, pos + float2( 5,-3) * pixel_size) * 0.008120;
				sum += tex2D(myTexture, pos + float2( 5,-2) * pixel_size) * 0.008171;
				sum += tex2D(myTexture, pos + float2( 5,-1) * pixel_size) * 0.008202;
				sum += tex2D(myTexture, pos + float2( 5, 0) * pixel_size) * 0.008212;
				sum += tex2D(myTexture, pos + float2( 5, 1) * pixel_size) * 0.008202;
				sum += tex2D(myTexture, pos + float2( 5, 2) * pixel_size) * 0.008171;
				sum += tex2D(myTexture, pos + float2( 5, 3) * pixel_size) * 0.008120;
				sum += tex2D(myTexture, pos + float2( 5, 4) * pixel_size) * 0.008049;
				sum += tex2D(myTexture, pos + float2( 5, 5) * pixel_size) * 0.007959;
				return sum;
			}

			sampler2D _MainTex;
			sampler2D _Pass2Tex;
			uniform float _iResolutionX;
			uniform float _iResolutionY;
			uniform float _eyeX;
			uniform float _eyeY;
			uniform float _fx;
			uniform float _fy;
			uniform float _denoiseRadius3;
			uniform float _denoiseRadius5;
			uniform float _denoiseRadius11;
			float4 _DebugColor11; 
			float4 _DebugColor5; 
			float4 _DebugColor3; 

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col;
				float2 iResolution = float2(_iResolutionX, _iResolutionY);
				float dist = length(i.uv - float2(_eyeX, _eyeY));

				float maxDxPos = 1.0 - _eyeX;
				float maxDyPos = 1.0 - _eyeY;
				float maxDxNeg = _eyeX;
				float maxDyNeg = _eyeY;

				float norDxPos = _fx * maxDxPos / (_fx + maxDxPos);
				float norDyPos = _fy * maxDyPos / (_fy + maxDyPos);
				float norDxNeg = _fx * maxDxNeg / (_fx + maxDxNeg);
				float norDyNeg = _fy * maxDyNeg / (_fy + maxDyNeg);

				float2 tc = (i.uv - float2(_eyeX, _eyeY)); //i.uv.x > cursorPos.x : [0,maxDxPos] i.uv.x < cursorPos.x : [-maxDxNeg, 0]

				float x = tc.x > 0 ? tc.x / maxDxPos : tc.x / maxDxNeg;//[0,1], [-1,0]
				float y = tc.y > 0 ? tc.y / maxDyPos : tc.y / maxDyNeg; 

				if (tc.x >= 0) {
					x = x * norDxPos; //[0,norDxPos]
					x = _fx * x / (_fx - x); //[0, 1]
					x = x + _eyeX;
				}
				else {
					x = x * norDxNeg;
					x = _fx * x / (_fx + x);
					x = x + _eyeX;
				}

				if (tc.y >= 0) {
					y = y * norDyPos;
					y = _fy * y / (_fy - y);
					y = y + _eyeY;
				}
				else {
					y = y * norDyNeg;
					y = _fy * y / (_fy + y);
					y = y + _eyeY;
				}

				float4 pq = (float4(x, y, 1, 1)); //0,1 --> 0-1

				if (dist < _denoiseRadius3){
					col = tex2D(_Pass2Tex, i.uv);
					//col = tex2D(_Pass2Tex, pq);
				}
				else if (dist < _denoiseRadius5){
					col = gausBlur3(_Pass2Tex, i.uv, iResolution) * _DebugColor3;
					//col = tex2D(_Pass2Tex, pq);
				}
				else if (dist < _denoiseRadius11){
					col = gausBlur5(_Pass2Tex, i.uv, iResolution)* _DebugColor5;
					//col = tex2D(_Pass2Tex, pq);
				}
				else{
					col = gausBlur11(_Pass2Tex, i.uv, iResolution)* _DebugColor11;
					//col = tex2D(_Pass2Tex, pq);
				}
				//if (dist < 0.2)
				//	col = tex2D(_MainTex, i.uv);
				//else if (dist < 0.25)
				//	col = gausBlur3(_MainTex, i.uv, iResolution);
				//else if (dist < 0.45)
				//	col = gausBlur5(_MainTex, i.uv, iResolution);
				//else
				//	col = gausBlur11(_MainTex, i.uv, iResolution);
				//col = tex2D(_MainTex, i.uv);
				return col;
			}
			ENDCG
		}
	}
}
