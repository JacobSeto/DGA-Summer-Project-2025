Shader "Custom/MovingDotsLine"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Line Color", Color) = (0, 1, 1, 1)
        _DotSize ("Dot Size", Range(0.1, 2.0)) = 0.5
        _DotSpacing ("Dot Spacing", Range(0.1, 2.0)) = 0.8
        _Speed ("Movement Speed", Range(0, 10)) = 2.0
        _Brightness ("Brightness", Range(0, 3)) = 1.5
        _FadeStart ("Fade Start", Range(0, 1)) = 0.3
        _FadeEnd ("Fade End", Range(0, 1)) = 1.0
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

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _Color;
            float _DotSize;
            float _DotSpacing;
            float _Speed;
            float _Brightness;
            float _FadeStart;
            float _FadeEnd;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 tex = tex2D(_MainTex, i.uv);
                
                float lineProgress = i.uv.x;
                
                float movingOffset = -_Time.y * _Speed;
                float dotPattern = lineProgress * (1.0 / _DotSpacing) + movingOffset;
            
                float dot = sin(dotPattern * 6.28318530718) * 0.5 + 0.5; 
                dot = pow(dot, 1.0 / _DotSize);

                float fade = 1.0;
                if (lineProgress > _FadeStart)
                {
                    fade = 1.0 - smoothstep(_FadeStart, _FadeEnd, lineProgress);
                }
                
                float intensity = dot * fade * _Brightness;
                
                fixed4 finalColor = _Color * tex * i.color;
                finalColor.a *= intensity;
                
                return finalColor;
            }
            ENDCG
        }
    }
}