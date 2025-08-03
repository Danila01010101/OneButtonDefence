Shader "UI/SpotlightMask"
{
    Properties
    {
        _Color ("Color", Color) = (0, 0, 0, 0.75)
        _HoleCenter ("Hole Center (Viewport)", Vector) = (0.5, 0.5, 0, 0)
        _HoleSize ("Hole Size", Vector) = (0.2, 0.2, 0, 0)
        _Softness ("Softness", Float) = 0.05
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Overlay+100" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _Color;
            float4 _HoleCenter;
            float4 _HoleSize;
            float _Softness;

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;

                // Вектор от центра "дырки"
                float2 delta = abs(uv - _HoleCenter.xy);

                // Относительный радиус отверстия
                float2 threshold = _HoleSize.xy * 0.5;

                // Гладкий край
                float2 edge = threshold + _Softness;

                float alphaX = smoothstep(edge.x, threshold.x, delta.x);
                float alphaY = smoothstep(edge.y, threshold.y, delta.y);
                float mask = max(alphaX, alphaY);

                return _Color * mask;
            }
            ENDCG
        }
    }
}