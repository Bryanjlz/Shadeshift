Shader "Custom/BeamMask"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags
		{
			"Queue"="Background"
			"IgnoreProjector"="True"
			"RenderType"="Transparent"
			"CanUseSpriteAtlas"="True"
		}
		Cull Off
		Lighting Off
		ZWrite Off
		Blend Off
		ColorMask 0

		Stencil {
			Ref 1
			Comp Always
			Pass Replace
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag#pragma multi_compile_local _ PIXELSNAP_ON
			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
			#include "UnitySprites.cginc"

			// alpha below which a mask should discard a pixel, thereby preventing the stencil buffer from being marked with the Mask's presence
			fixed _Cutoff;

			struct appdata_masking
			{
				float4 vertex : POSITION;
				half2 texcoord : TEXCOORD0;
			};

			struct v2f_masking
			{
				float4 pos : SV_POSITION;
				half2 uv : TEXCOORD0;
				UNITY_VERTEX_OUTPUT_STEREO
			};

			v2f_masking vert(appdata_masking IN)
			{
				v2f_masking OUT;

				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

				OUT.pos = UnityObjectToClipPos(IN.vertex);
				OUT.uv = IN.texcoord;

				#ifdef PIXELSNAP_ON
				OUT.pos = UnityPixelSnap (OUT.pos);
				#endif

				return OUT;
			}


			fixed4 frag(v2f_masking IN) : SV_Target
			{
				fixed4 c = SampleSpriteTexture(IN.uv);
				// for masks: discard pixel if alpha falls below MaskingCutoff
				clip (c.a - _Cutoff);
				return _Color;
			}
			ENDCG
		}
	}
}
