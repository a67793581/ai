Shader "Unlit/Unlit"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MaskTex1("Mask1", 2D) = "white" {}
        _MaskTex2("Mask2", 2D) = "white" {}
        _MaskTex3("Mask3", 2D) = "white" {}
        _MaskTex4("Mask4", 2D) = "white" {}
        _MaskTex5("Mask5", 2D) = "white" {}
        _MaskTex6("Mask6", 2D) = "white" {}
        _MaskTex7("Mask7", 2D) = "white" {}
        _MaskTex8("Mask8", 2D) = "white" {}
        _MaskTex9("Mask9", 2D) = "white" {}
        _SadTint("Sad Tint", Color) = (1,1,1,1)
        _SadSpeed("Sad Speed", Float) = 1.0
        _RotateSpeed("Rotate Speed", Float) = 0.0
        [Toggle]_EnableAngry("Angry", Int) = 0
        [Toggle]_EnableLove("Love", Int) = 0
        [Toggle]_EnableSleep("Sleep", Int) = 0
        [Toggle]_EnableSad("Sad", Int) = 0
        [Toggle]_EnableFaint("Faint", Int) = 0
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
                float4 uv2 : TEXCOORD2;
                float4 uv3 : TEXCOORD3;
                float4 uv4 : TEXCOORD4;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _MaskTex1;
            sampler2D _MaskTex2;
            sampler2D _MaskTex3;
            sampler2D _MaskTex4;
            sampler2D _MaskTex5;
            sampler2D _MaskTex6;
            sampler2D _MaskTex7;
            sampler2D _MaskTex8;
            sampler2D _MaskTex9;
            float4 _MainTex_ST;
            float4 _MaskTex1_ST;
            float4 _MaskTex2_ST;
            float4 _MaskTex3_ST;
            float4 _MaskTex4_ST;
            float4 _MaskTex5_ST;
            float4 _MaskTex6_ST;
            float4 _MaskTex7_ST;
            float4 _MaskTex8_ST;
            float4 _MaskTex9_ST;
            half4 _SadTint;
            float _SadSpeed;
            float _RotateSpeed;
            int _EnableAngry;
            int _EnableLove;
            int _EnableSleep;
            int _EnableSad;
            int _EnableFaint;


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv.xy = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv.zw = TRANSFORM_TEX(v.uv, _MaskTex1);
                o.uv1.xy = TRANSFORM_TEX(v.uv, _MaskTex2);
                o.uv1.zw = TRANSFORM_TEX(v.uv, _MaskTex3);
                o.uv2.xy = TRANSFORM_TEX(v.uv, _MaskTex4);
                o.uv2.zw = TRANSFORM_TEX(v.uv, _MaskTex5);
                o.uv3.xy = TRANSFORM_TEX(v.uv, _MaskTex6);
                o.uv3.zw = TRANSFORM_TEX(v.uv, _MaskTex7) + frac(float2(_SadSpeed * _Time.y, 0));
                o.uv4.xy = TRANSFORM_TEX(v.uv, _MaskTex8);
                
                float sinX = sin ( _RotateSpeed * _Time.y );
                float cosX = cos ( _RotateSpeed * _Time.y );
                float sinY = sin ( _RotateSpeed * _Time.y );
                float2x2 rotationMatrix = float2x2( cosX, -sinX, sinY, cosX);
                v.uv.xy = mul ( v.uv.xy - .5, rotationMatrix ) + .5;
                o.uv4.zw = TRANSFORM_TEX(v.uv, _MaskTex9);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float4 col = tex2D(_MainTex, i.uv);
                half mask1 = tex2D(_MaskTex1, i.uv.zw).a;
                half mask2 = tex2D(_MaskTex2, i.uv1.xy).a;
                half mask3 = tex2D(_MaskTex3, i.uv1.zw).a;
                half mask4 = tex2D(_MaskTex4, i.uv2.xy).a;
                half mask5 = tex2D(_MaskTex5, i.uv2.zw).a;
                half mask6 = tex2D(_MaskTex6, i.uv3.xy).a;
                half mask7 = tex2D(_MaskTex7, i.uv3.zw).a;
                half mask8 = tex2D(_MaskTex8, i.uv4.xy).a;
                half mask9 = tex2D(_MaskTex9, i.uv4.zw).a;
                col.a *= mask1 - mask2;

                if (_EnableAngry)
                    col.a *= mask3 - mask4;

                if (_EnableLove)
                    col.a *= mask5;

                if (_EnableSleep)
                    col.a *= mask6;
                    
                if (_EnableSad)
                {
                    col.rgb = lerp(col.rgb, _SadTint, mask7);
                    col.a *= (1 - mask8);
                }

                if (_EnableFaint)
                {
                    col.a *= mask9;
                }
                
                return col;
            }
            ENDCG
        }
    }
}
