Shader "JGH/DissolveEffect"
{
    Properties
    {
        _MainColor ("MainColor", Color) = (1,1,1,1)
        _MainTex ("MainTex (Albedo)", 2D) = "white" {}
        _EffectTex("EffectTex", 2D) = "white" {}
        //���� ���ϴ� ������ �ؽ��� ���

        _Progress("Progress", Range(0,1)) = 0.25
        _EffectSize("Effect Size", Range(0,1)) = 0.05
        [HDR] _DissolveColor("Dissolve Color", Color) = (1,1,1,1)
        [Enum(UnityEngine.Rendering.CullMode)] _Culling("Culling", Int) = 2
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
        Cull [_Culling]

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _EffectTex;


        half _Progress;
        half _EffectSize;

        fixed4 _MainColor;
        fixed4 _DissolveColor;
        //HLSL������ �ε� �Ҽ��� ����
        //float -> half -> fixed4 �� ��Ȯ�� ����(������ -> ������)

        //float => �츮�� �ƴ� 32��Ʈ�� �� ģ�� (���� ���� ������, �ؽ�ó ��ǥ �Ǵ� �ﰢ���̳� ����/�������� ���� ���� �Լ��� �����ϴ� ��Į�� ��꿡 ���)
        //half => 16��Ʈ( ª�� ����, ����, ������Ʈ ���� ������, ����̳��� ������ �÷�)
        //fixed4 => 11��Ʈ, -2~2 �� ����, 1/256�� ��Ȯ�� (�Ϲ� �÷��� ������ �÷� �۾� ����.)

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_EffectTex;
        };


        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
            UNITY_INSTANCING_BUFFER_END(Props)

        fixed4 mainPixel;
        fixed4 effectPixel;
        half progress;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            mainPixel = tex2D(_MainTex, IN.uv_MainTex) * _MainColor;
            o.Albedo = mainPixel.rgb;

            effectPixel = tex2D(_EffectTex, IN.uv_EffectTex);

            if (effectPixel.r >= _Progress)
            {
                clip(1);
            }
            else
            {
                clip(-1);
            }
            //clip(x) : x�� �� ���Ұ� 0���� ������ ���� �ȼ��� ������. (�ȼ����̴�����)
            //�� ���� �ȼ� ���̴� �Լ� ��ü�� �׳� return �ع����ٴ°���.
            
            half temp = _Progress * (_EffectSize + 1.0);

            if (effectPixel.r >= temp)
            {
                o.Emission = 0;
            }
            else
            {
                o.Emission = _DissolveColor;
            }
            
        }
        ENDCG
    }
    FallBack "Diffuse"
}
