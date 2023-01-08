Shader "JGH/GrayScaleImageShader"
{//앞에 히든 안지우면 머리테리얼 인스펙터 창에 안나옴 
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
    ////변수명 ("Inspector창에 뜰값', 범위 혹은 값) = 초기값 (세미클론 불필요)
    //        _Vector3Offset("test", Vector) = (1,1,1,1)
    //        _FloatTest("floatTest", Range(0,3)) = 0
        _GrayScaleAmount("GrayScale Amount", Range(0, 1)) = 0
            //1이면 흑백
            //0이면 풀칼라
    }
        SubShader
        {
            // No culling or depth
            Cull Off ZWrite Off ZTest Always
            //포스트프로세싱은 후면컬링이랑 z라이트는 끄고 z테스트는 항상 켜야함

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
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;

                    return o;
                }

                sampler2D _MainTex;

                float _GrayScaleAmount;

                float4 frag(v2f i) : SV_Target
                {
                    float4 texcol = tex2D(_MainTex, i.uv);



                    float4 grayScaleRGB = (texcol.r * 0.29f) + (texcol.g * 0.59f) + (texcol.b * 0.12f);
                    texcol.rgb = lerp(texcol.rgb, grayScaleRGB, _GrayScaleAmount);

                    //texcol.rgb = lerp(texcol.rgb, cross(texcol.rgb, float3(0.29f, 0.59f, 0.12f)), _GrayScaleAmount);
                    //=>초록파티
                    //texcol.rgb = lerp(texcol.rgb, dot(texcol.rgb, float3(0.29f, 0.59f, 0.12f)), _GrayScaleAmount);
                    //=>이건 왜되는거지 
                    return texcol;

                }
                ENDCG
            }
        }
}
