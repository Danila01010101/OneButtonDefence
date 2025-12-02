Shader "LemonSpawn/LazyFog"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Scale ("Scale", Range(0,5)) = 1
        _Intensity ("Intensity", Range(0,1)) = 0.5
        _Alpha ("Alpha", Range(0,2.5)) = 0.75
        _AlphaSub ("AlphaSub", Range(0,1)) = 0.0
        _Pow ("Pow", Range(0,4)) = 1.0
    }

    SubShader
    {
        Tags { "Queue"="Transparent+101" "IgnoreProjector"="True" "RenderType"="Transparent" }
        LOD 400

        Cull Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM

            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _Color;
            float _Scale;
            float _Intensity;
            float _Alpha;
            float _AlphaSub;
            float _Pow;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float4 color : COLOR;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.color = v.color;
                return o;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                float3 worldPos = IN.worldPos;

                float3 viewDir = normalize(_WorldSpaceCameraPos - worldPos);
                float dist = clamp(pow(length(0.1 * (viewDir)), 1.0), 0, 1);

                float4 c = tex2D(_MainTex, IN.uv * _Scale);

                float xx = pow(c.r * _Intensity, _Pow);

                c.rgb = xx * _Color.rgb;

                c.a = c.r;
                c.a *= IN.color.a - 2.5 * length(IN.uv - float2(0.5, 0.5));
                c.a *= _Alpha;
                c.a -= _AlphaSub;
                c.a = clamp(c.a, 0, 1);

                return c;
            }

            ENDCG
        }
    }
}