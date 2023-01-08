Shader "JGH/DissolveEffect"
{
    Properties
    {
        _MainColor ("MainColor", Color) = (1,1,1,1)
        _MainTex ("MainTex (Albedo)", 2D) = "white" {}
        _EffectTex("EffectTex", 2D) = "white" {}
        //흔히 말하는 노이즈 텍스쳐 사용

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
        //HLSL에서의 부동 소수점 변수
        //float -> half -> fixed4 의 정확도 순서(고정밀 -> 저정밀)

        //float => 우리가 아는 32비트의 그 친구 (월드 공간 포지션, 텍스처 좌표 또는 삼각법이나 제곱/지수연산 같은 복합 함수를 수반하는 스칼라 계산에 사용)
        //half => 16비트( 짧은 벡터, 방향, 오브젝트 공간 포지션, 고다이나믹 레인지 컬러)
        //fixed4 => 11비트, -2~2 의 범위, 1/256의 정확도 (일반 컬러와 간단한 컬러 작업 수행.)

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
            //clip(x) : x의 한 원소가 0보다 작으면 현재 픽셀을 버린다. (픽셀쉐이더에서)
            //즉 현재 픽셀 세이더 함수 자체를 그냥 return 해버린다는거임.
            
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
