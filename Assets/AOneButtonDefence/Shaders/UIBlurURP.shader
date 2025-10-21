Shader "Custom/UIBlurURP"
{
    Properties
    {
        _BlurSize("Blur Size (Radius)", Range(0,10)) = 2
        _BlurStrength("Blur Strength", Range(0,2)) = 1
        _TintColor("Tint Color", Color) = (1,1,1,0.5)
    }

    SubShader
    {
        Tags
        { 
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _CameraOpaqueTexture;
            float4 _CameraOpaqueTexture_TexelSize;
            float _BlurSize;
            float _BlurStrength;
            fixed4 _TintColor;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float2 texel = _CameraOpaqueTexture_TexelSize.xy;
                float radius = _BlurSize * 1.5;

                fixed4 col = 0;
                float weightSum = 0;

                const float kernel[9] = {
                    0.05, 0.1, 0.05,
                    0.1,  0.4, 0.1,
                    0.05, 0.1, 0.05
                };

                int idx = 0;
                for (int x=-1; x<=1; x++)
                {
                    for (int y=-1; y<=1; y++)
                    {
                        float2 offset = float2(x, y) * texel * radius;
                        col += tex2D(_CameraOpaqueTexture, uv + offset) * kernel[idx];
                        weightSum += kernel[idx];
                        idx++;
                    }
                }

                col /= weightSum;

                // смешиваем с оригиналом по BlurStrength
                fixed4 blurred = lerp(tex2D(_CameraOpaqueTexture, uv), col, _BlurStrength);

                blurred *= _TintColor;

                return blurred;
            }
            ENDCG
        }
    }
}