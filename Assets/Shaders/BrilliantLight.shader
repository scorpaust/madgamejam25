Shader "Unlit/BrilliantLight"
{
   Properties
    {
        _MainColor ("Main Color", Color) = (1,1,1,1) // Cor principal da luz
        _EmissionIntensity ("Emission Intensity", Range(1,10)) = 5 // Intensidade do brilho
        _Radius ("Light Radius", Range(0.1,5)) = 1 // Raio da luz
        _Softness ("Edge Softness", Range(0,1)) = 0.5 // Suavidade nas bordas
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha One // Adição para brilho extremo
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _MainColor;
            float _EmissionIntensity;
            float _Radius;
            float _Softness;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 center = float2(0.5, 0.5);
                float dist = distance(i.uv, center) / _Radius;
                float glow = smoothstep(1, _Softness, dist);
                
                return _MainColor * _EmissionIntensity * glow;
            }
            ENDCG
        }
    }
}
