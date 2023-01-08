Shader "JGH/DiceShader"
{
    Properties
    {
        _MainTex("Dice Tex", 2D) = "white" {}
        _SubTex("Spot Tex ", 2D) = "white" {}
        _Color("Spot Color", Color) = (1,1,1,1)
        _Alpha("Alpha", Range(0, 1)) = 1
        //_Gray("Gray", bool) = FALSE

    [MaterialToggle] _Gray("Gray", Float) = 0
  //      _OutLine("OutLine", Range(0.0, 1)) = 0
		//_OutLineColor("OutLine Color", Color) = (0,0,0,1)
    }

    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }    


        blend SrcAlpha OneMinusSrcAlpha    
        zwrite off


        CGPROGRAM
       #pragma surface surf Lambert keepalpha   

      sampler2D _MainTex;
      sampler2D _SubTex;

      struct Input
      {
          float2 uv_MainTex;
          float2 uv_SubTex;
      };


        fixed4 _Color;
        float _Alpha;
        bool _Gray;

        void surf(Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            float4 sub = tex2D(_SubTex, IN.uv_SubTex) * _Color;


            o.Albedo = sub.a < 1.0 ? c.rgb : sub.rgb;

            if(_Gray)
            {   
                float4 grayScaleRGB = (o.Albedo.r * 0.29f) + (o.Albedo.g * 0.59f) + (o.Albedo.b * 0.12f);
                o.Albedo = grayScaleRGB;
            }
             
            //if (sub.a < 1)
            //{
            //    o.Albedo = c.rgb;
            //}
            //else
            //{
            //    o.Albedo = sub.rgb;
            //}

            o.Alpha = _Alpha;
        }
        ENDCG
    }
        FallBack "Diffuse"
}
