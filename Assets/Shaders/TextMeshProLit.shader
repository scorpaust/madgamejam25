Shader "Custom/TextMeshProLit"
{
    Properties
    {
        _FaceColor ("Face Color", Color) = (1,1,1,1)
        _Glossiness ("Glossiness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        struct Input
        {
            float2 uv_MainTex;
        };

        half4 _FaceColor;
        half _Glossiness;
        half _Metallic;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            o.Albedo = _FaceColor.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
        }
        ENDCG
    }
}
