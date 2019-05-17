Shader "Unlit/Healthbar"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

        //TODO; global?
        _Full("Full", Color) = (1,1,1,1)
        _Empty("Empty", Color) = (1,1,1,1)
        _Losing("Losing", Color) = (1,1,1,1)
        _Filling("Filling", Color) = (1,1,1,1)
        _Background("Background", Color) = (1,1,1,1)

        [HideInInspector]
        _Real("Real", float) = 1

        [HideInInspector]
        _Show("Show", float) = 1
    }
    SubShader
    {
        Tags {
            "RenderType"="Overlay"
            "Queue" = "Geometry+1"
        }

        Pass
        {
            ZTest Off
            Lighting Off
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile_instancing

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
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            fixed4 _Full;
            fixed4 _Empty;
            fixed4 _Losing;
            fixed4 _Filling;
            fixed4 _Background;

            UNITY_INSTANCING_BUFFER_START(Props)
            UNITY_DEFINE_INSTANCED_PROP(float, _Real)
            UNITY_DEFINE_INSTANCED_PROP(float, _Show)
            UNITY_DEFINE_INSTANCED_PROP(int, _ShowType)
            UNITY_INSTANCING_BUFFER_END(Props)

            v2f vert (appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                
                // If you need instance data in the fragment shader, uncomment next line
                UNITY_TRANSFER_INSTANCE_ID(v, o);

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);
                float real = UNITY_ACCESS_INSTANCED_PROP(Props, _Real);
                float show = UNITY_ACCESS_INSTANCED_PROP(Props, _Show);

                // the color left to normal
                fixed4 col = lerp(_Empty, _Full, real);

                fixed4 finalColor;

                // // TODO; This is hardcoded
                // if (abs(0.5 - i.uv.x) > 0.4 || abs(0.5 - i.uv.y) > 0.4) {
                //     return fixed4(1,1,1,1);
                // }
                
                float x = i.uv.x;

                finalColor = lerp(_Empty, _Full, x);

                if (real < show) {
                    if (x <= real) {
                        finalColor = col;
                    } else if (x <= show) {
                        finalColor = _Losing;
                    } else {
                        //finalColor = _Background;
                        finalColor = col / 2.5;
                    }
                } else {
                    if (x <= show) {
                        finalColor = col;
                    } else if (x <= real) {
                        finalColor = _Filling;
                    } else {
                        //finalColor = _Background;
                        finalColor = col / 2.5;
                    }
                }

                return tex2D(_MainTex, i.uv) * finalColor;
            }
            ENDCG
        }
    }
}


///
/// Normal      (Real)      Damge       (Show)       Empty
/// Normal      (Show)      Heal        (Real)        Empty

