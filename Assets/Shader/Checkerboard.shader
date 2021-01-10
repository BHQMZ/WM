Shader "Game/Checkerboard"
{
    Properties
    {
        [NoScaleOffset] _CheckeredTex("Checkered", 2D) = "white" {}
        _Horizontal("Horizontal Count", int) = 10
        _Vertical("Vertical Count",int) = 10

        [NoScaleOffset] _PlayerCheckeredTex("Player", 2D) = "white" {}
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct appdata
            {
                float4 vertex : POSITION;
                float4 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 uv : TEXCOORD0;
            };

            sampler2D _CheckeredTex;
            float4 _CheckeredTex_ST;

            float _Horizontal;
            float _Vertical;

            float2 _Test;

            sampler2D _PlayerCheckeredTex;
            float4 _PlayerCheckeredTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float4 uv = i.uv;
                uv.x *= _Horizontal;
                uv.y *= _Vertical;
                //uv.w = 0.5;
                if(uv.x>_Test.x&&uv.y>_Test.y&&uv.x<(_Test.x+1)&&(uv.y<_Test.y+1)){
                    return tex2D(_PlayerCheckeredTex, uv);
                }else{
                    return tex2D(_CheckeredTex, uv);
                }
            }
            ENDCG
        }
    }
}
