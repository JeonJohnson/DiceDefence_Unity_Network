Shader "JGH/GrayScaleImageShader"
{//�տ� ���� ������� �Ӹ��׸��� �ν����� â�� �ȳ��� 
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
    ////������ ("Inspectorâ�� �㰪', ���� Ȥ�� ��) = �ʱⰪ (����Ŭ�� ���ʿ�)
    //        _Vector3Offset("test", Vector) = (1,1,1,1)
    //        _FloatTest("floatTest", Range(0,3)) = 0
        _GrayScaleAmount("GrayScale Amount", Range(0, 1)) = 0
            //1�̸� ���
            //0�̸� ǮĮ��
    }
        SubShader
        {
            // No culling or depth
            Cull Off ZWrite Off ZTest Always
            //����Ʈ���μ����� �ĸ��ø��̶� z����Ʈ�� ���� z�׽�Ʈ�� �׻� �Ѿ���

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
                    //=>�ʷ���Ƽ
                    //texcol.rgb = lerp(texcol.rgb, dot(texcol.rgb, float3(0.29f, 0.59f, 0.12f)), _GrayScaleAmount);
                    //=>�̰� �ֵǴ°��� 
                    return texcol;

                }
                ENDCG
            }
        }
}
