Shader "Test/ColorShader"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _BoxCount("BoxCount", int) = 10
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
			#pragma fragment frag

            fixed4 _Color;
            float _BoxCount;

         //   struct appdata
	        //{
		       // float4 vertex : POSITION;		//顶点
		       // float4 tangent : TANGENT;		//切线
		       // float3 normal : NORMAL;			//法线
		       // fixed4 color : COLOR;			//顶点色
	        //};

            struct appData{
                float4 vertex : POSITION;
                float2 uv : TEXCOORD;
            };

            struct v2f{
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD;
            };

            v2f vert(appData v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 checker(float2 uv){
                float2 repeatUV = uv * _BoxCount;
                //float2 c = floor(repeatUV) / 2;
                //float checker = frac(c.x + c.y) * 2;
                if(repeatUV.x>1&&repeatUV.y>1&&repeatUV.x<2&&repeatUV.y<2){
                    return _Color;
                }else{
                    float x = frac(repeatUV.x);
                    float y = frac(repeatUV.y);
                    if(x<=0.05||x>=0.95||y<=0.05||y>=0.95){
                        return 0;
                    }else{
                        return 1;
                    }
                }
            }
            
            fixed4 frag(v2f i) : SV_TARGET
            {
                //return checker(i.uv);
                return _Color;
            }

            ENDCG
        }
    }
}
