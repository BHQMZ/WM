Shader "Game/Checkerboard"
{
    Properties
    {
        [NoScaleOffset] _CheckeredTex("Checkered", 2D) = "white" {}
        _Horizontal("Horizontal Count", int) = 10
        _Vertical("Vertical Count",int) = 10

        [NoScaleOffset] _PlayerCheckeredTex1("Player1", 2D) = "white" {}

        [NoScaleOffset] _PlayerCheckeredTex2("Player2", 2D) = "white" {}
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

            sampler2D _PlayerCheckeredTex1;
            float4 _PlayerCheckeredTex1_ST;

            float2 _Plyer1[49];

            sampler2D _PlayerCheckeredTex2;
            float4 _PlayerCheckeredTex2_ST;

            float2 _Plyer2[49];

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            bool isPlayer(float4 uv,float2 players[49]){
                int x = floor(uv.x) + 1;
                int y = floor(uv.y) + 1;
                int i = 0;
                float2 player = players[i];
                while(player.x > 0){
                    if(x == player.x && y == player.y){
                        return true;
                    }
                    player = players[++i];
                }
                return false;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float4 uv = i.uv;
                uv.x *= _Horizontal;
                uv.y *= _Vertical;
                if(isPlayer(uv,_Plyer1)){
                    return tex2D(_PlayerCheckeredTex1, uv);
                }else if(isPlayer(uv,_Plyer2)){
                    return tex2D(_PlayerCheckeredTex2, uv);
                }else{
                    return tex2D(_CheckeredTex, uv);
                }
            }
            ENDCG
        }
    }
}
