Shader "Unlit/Unlit"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MaskTex1("Mask1", 2D) = "white" {}
        _MaskTex2("Mask2", 2D) = "white" {}
        _MaskTex3("Mask3", 2D) = "white" {}
        _MaskTex4("Mask4", 2D) = "white" {}
        [Toggle]_EnableAngry("Angry", Int) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        Zwrite off
        

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
                float4 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _MaskTex1;
            sampler2D _MaskTex2;
            sampler2D _MaskTex3;
            sampler2D _MaskTex4;
            float4 _MainTex_ST;
            float4 _MaskTex1_ST;
            float4 _MaskTex2_ST;
            float4 _MaskTex3_ST;
            float4 _MaskTex4_ST;
            int _EnableAngry;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv.xy = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv.zw = TRANSFORM_TEX(v.uv, _MaskTex1);
                o.uv1.xy = TRANSFORM_TEX(v.uv, _MaskTex2);
                o.uv1.zw = TRANSFORM_TEX(v.uv, _MaskTex3);
                o.uv2.xy = TRANSFORM_TEX(v.uv, _MaskTex4);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float4 col = tex2D(_MainTex, i.uv);
                half mask1 = tex2D(_MaskTex1, i.uv.zw).a;
                half mask2 = tex2D(_MaskTex2, i.uv1.xy).a;
                half mask3 = tex2D(_MaskTex3, i.uv1.zw).a;
                half mask4 = tex2D(_MaskTex4, i.uv2.xy).a;
                col.a *= mask1 - mask2;

                if (_EnableAngry)
                    col.a *= mask3 - mask4;
                return col;
            }
            ENDCG
        }
    }
}
