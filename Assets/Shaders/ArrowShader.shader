Shader "Custom/ArrowFill"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FillAmount ("Fill Amount", Range(0,1)) = 0
        _TintColor ("Tint Color", Color) = (1, 0.3, 0.5, 1)
        _GlowColor ("Glow Color", Color) = (0, 1, 1, 1)
        _WhiteThreshold ("White Threshold", Range(0,1)) = 0.8
        _GlowIntensity ("Glow Intensity", Range(0, 5)) = 2.0
        _PulseSpeed ("Pulse Speed", Range(0, 10)) = 3.0
        _EdgeGlow ("Edge Glow Width", Range(0, 0.1)) = 0.05
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        LOD 100
        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _TintColor;
            float4 _GlowColor;
            float _FillAmount;
            float _WhiteThreshold;
            float _GlowIntensity;
            float _PulseSpeed;
            float _EdgeGlow;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Sample base texture for the glow
                fixed4 baseColor = tex2D(_MainTex, i.uv);
                
                float brightness = dot(baseColor.rgb, float3(0.299, 0.587, 0.114));
                float isWhite = step(_WhiteThreshold, brightness);

                float fillMask = step(1.0 - _FillAmount, i.uv.x);
                
                // Edge glow effect (glowing edge of the fill)
                float edgeDistance = abs(i.uv.x - (1.0 - _FillAmount));
                float edgeGlow = 1.0 - smoothstep(0.0, _EdgeGlow, edgeDistance);
                edgeGlow *= fillMask;
                
                // pulse effect when fully filled
                float pulseEffect = 0.5 + 0.5 * sin(_Time.y * _PulseSpeed);
                float fullFillBonus = step(0.99, _FillAmount) * pulseEffect * 0.5;
                
                float totalGlow = (edgeGlow + fullFillBonus) * _GlowIntensity;
                
                fixed4 finalColor = baseColor;
                
                float affectPixel = (1.0 - isWhite) * fillMask;
                
                finalColor.rgb = lerp(baseColor.rgb, baseColor.rgb * _TintColor.rgb, affectPixel);
                
                finalColor.rgb += _GlowColor.rgb * totalGlow * affectPixel;

                float saturationBoost = affectPixel * 0.3;
                float3 gray = dot(finalColor.rgb, float3(0.299, 0.587, 0.114));
                finalColor.rgb = lerp(finalColor.rgb, lerp(gray, finalColor.rgb, 1.0 + saturationBoost), affectPixel);
                
                return finalColor;
            }
            ENDCG
        }
    }
}