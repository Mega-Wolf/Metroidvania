Shader "Sprites/Test"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0

		_ReplacementColor("ReplacementColor", Color) = (0,1,1,1)

		[MaterialToggle] _UseReplacement("UseReplacement", float) = 0
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON
			#include "UnityCG.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
			};
			
			fixed4 _Color;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			sampler2D _MainTex;
			sampler2D _AlphaTex;
			float _AlphaSplitEnabled;

			fixed4 _ReplacementColor;
            bool _UseReplacement;

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);

#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
				if (_AlphaSplitEnabled)
					color.a = tex2D (_AlphaTex, uv).r;
#endif //UNITY_TEXTURE_ALPHASPLIT_ALLOWED

				return color;
			}

			float3 Hue(float H)
			{
    			float R = abs(H * 6 - 3) - 1;
    			float G = 2 - abs(H * 6 - 2);
    			float B = 2 - abs(H * 6 - 4);
    			return saturate(float3(R,G,B));
			}

			float3 HSVtoRGB(in float3 HSV)
			{
    			return ((Hue(HSV.x) - 1) * HSV.y + 1) * HSV.z;
			}

			float3 RGBtoHSV(in float3 RGB)
			{
    			float3 HSV = 0;
    			HSV.z = max(RGB.r, max(RGB.g, RGB.b));
    			float M = min(RGB.r, min(RGB.g, RGB.b));
    			float C = HSV.z - M;
    			if (C != 0)
    			{
        			HSV.y = C / HSV.z;
        			float3 Delta = (HSV.z - RGB) / C;
        			Delta.rgb -= Delta.brg;
        			Delta.rg += float2(2,4);
        			if (RGB.r >= HSV.z)
            			HSV.x = Delta.b;
        			else if (RGB.g >= HSV.z)
            			HSV.x = Delta.r;
        			else
            			HSV.x = Delta.g;
        			HSV.x = frac(HSV.x / 6);
    			}
    			return HSV;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;

				if (_UseReplacement == 1) {
					float3 hsv = RGBtoHSV(c.rgb);
					c.rgb = hsv.z * _ReplacementColor.rgb;
					c.a *= _ReplacementColor.a;
				}

				c.rgb *= c.a;
				return c;
			}

		ENDCG
		}
	}
}