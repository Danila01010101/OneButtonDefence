Shader "Custom/SpotlightMaskPixels"
{
    Properties
    {
        _Color ("Overlay Color", Color) = (0, 0, 0, 0.7)
        _ScreenResolution ("Screen Resolution (X,Y)", Vector) = (1920, 1080, 0, 0)
        _RectMin ("Rect Min (X,Y)", Vector) = (400, 300, 0, 0)
        _RectMax ("Rect Max (X,Y)", Vector) = (1500, 800, 0, 0)
        _Feather ("Feather (X,Y)", Vector) = (80, 80, 0, 0) // в пикселях
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        LOD 100

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _Color;
            float4 _ScreenResolution;
            float4 _RectMin;
            float4 _RectMax;
            float4 _Feather;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 screenUV = i.uv * _ScreenResolution.xy;

                float2 fadeStart = _RectMin.xy - _Feather.xy;
                float2 fadeEnd = _RectMax.xy + _Feather.xy;

                float2 alphaH = smoothstep(fadeStart.x, _RectMin.x, screenUV.x) * (1.0 - smoothstep(_RectMax.x, fadeEnd.x, screenUV.x));
                float2 alphaV = smoothstep(fadeStart.y, _RectMin.y, screenUV.y) * (1.0 - smoothstep(_RectMax.y, fadeEnd.y, screenUV.y));

                float alpha = alphaH * alphaV;

                return _Color * (1.0 - alpha);
            }
            ENDCG
        }
    }
}