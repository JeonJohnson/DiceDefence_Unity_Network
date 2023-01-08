Shader "JGH/BlurEffect"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _BlurAmount("Blur Amount", Range(0, 1)) = 0.0
    }
        SubShader
    {
        Tags { "Queue" = "Transparent" }
        Cull Back
        ZTest Always

        GrabPass { }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 position : POSITION;
                float4 screenPos : TEXCOORD0;
            };

            half _BlurAmount;
            fixed4 _Color;
            sampler2D _GrabTexture : register(s0);

            v2f vert(appdata input)
            {
                v2f output;

                output.position = UnityObjectToClipPos(input.vertex);
                output.screenPos = output.position;

                return output;
            }

            half4 pixel;
            half2 uv;
            fixed i = 0;
            half iBlur;
            half4 frag(v2f input) : SV_Target
            {
                uv = input.screenPos.xy / input.screenPos.w;
                uv.x = (uv.x + 1) * .5;
                uv.y = 1.0 - (uv.y + 1) * .5;

                pixel = 0;

                float blurAmount = _BlurAmount * 0.0025;

                pixel += tex2D(_GrabTexture, half2(uv.x + 1.5 * blurAmount, uv.y + 0.5 * blurAmount));
                pixel += tex2D(_GrabTexture, half2(uv.x + 1.5 * blurAmount, uv.y - 0.5 * blurAmount));
                pixel += tex2D(_GrabTexture, half2(uv.x + 1.5 * blurAmount, uv.y - 1.5 * blurAmount));
                pixel += tex2D(_GrabTexture, half2(uv.x + 1.5 * blurAmount, uv.y - 2.5 * blurAmount));
                pixel += tex2D(_GrabTexture, half2(uv.x + 0.5 * blurAmount, uv.y + 2.5 * blurAmount));
                pixel += tex2D(_GrabTexture, half2(uv.x + 0.5 * blurAmount, uv.y + 1.5 * blurAmount));

                pixel += tex2D(_GrabTexture, half2(uv.x + 0.5 * blurAmount, uv.y + 0.5 *  blurAmount));
                pixel += tex2D(_GrabTexture, half2(uv.x + 0.5 * blurAmount, uv.y - 0.5 *  blurAmount));

                pixel += tex2D(_GrabTexture, half2(uv.x + 0.5 * blurAmount, uv.y - 1.5 *  blurAmount));
                pixel += tex2D(_GrabTexture, half2(uv.x + 0.5 * blurAmount, uv.y - 2.5 *  blurAmount));
                pixel += tex2D(_GrabTexture, half2(uv.x - 0.5 * blurAmount, uv.y + 2.5 *  blurAmount));
                pixel += tex2D(_GrabTexture, half2(uv.x - 0.5 * blurAmount, uv.y + 1.5 *  blurAmount));

                pixel += tex2D(_GrabTexture, half2(uv.x - 0.5 * blurAmount, uv.y + 0.5 *  blurAmount));
                pixel += tex2D(_GrabTexture, half2(uv.x - 0.5 * blurAmount, uv.y - 0.5 *  blurAmount));

                pixel += tex2D(_GrabTexture, half2(uv.x - 0.5 * blurAmount, uv.y - 1.5 *  blurAmount));
                pixel += tex2D(_GrabTexture, half2(uv.x - 0.5 * blurAmount, uv.y - 2.5 *  blurAmount));
                pixel += tex2D(_GrabTexture, half2(uv.x - 1.5 * blurAmount, uv.y + 2.5 *  blurAmount));
                pixel += tex2D(_GrabTexture, half2(uv.x - 1.5 * blurAmount, uv.y + 1.5 *  blurAmount));
                pixel += tex2D(_GrabTexture, half2(uv.x - 1.5 * blurAmount, uv.y + 0.5 *  blurAmount));
                pixel += tex2D(_GrabTexture, half2(uv.x - 1.5 * blurAmount, uv.y - 0.5 *  blurAmount));

                pixel += tex2D(_GrabTexture, half2(uv.x, uv.y));

                return (pixel / 20.0) * _Color;
            }
            ENDCG
        }
    }
}
