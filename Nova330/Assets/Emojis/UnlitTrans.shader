Shader "Unlit/Unlit"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MaskTex1("Mask1", 2D) = "white" {}
        _MaskTex2("Mask2", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 uv : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _MaskTex1;
            sampler2D _MaskTex2;
            float4 _MainTex_ST;
            float4 _MaskTex1_ST;
            float4 _MaskTex2_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv.xy = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv.zw = TRANSFORM_TEX(v.uv, _MaskTex1);
                o.uv1 = TRANSFORM_TEX(v.uv, _MaskTex2);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float4 col = tex2D(_MainTex, i.uv);
                half mask1 = tex2D(_MaskTex1, i.uv.zw).a;
                half mask2 = tex2D(_MaskTex2, i.uv1.xy).a;
                col.a *= mask1 - mask2;
                return col;
            }
            ENDCG
        }
    }
}
