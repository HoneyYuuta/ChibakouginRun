Shader "Unlit/ObstacleShader"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        _DissolveTex ("Dissolve Noise", 2D) = "white" {}
        _DissolveAmount ("Dissolve Amount", Range(0, 1)) = 0
        [HDR] _EdgeColor ("Edge Color", Color) = (0, 1, 0, 1)
        _EdgeWidth ("Edge Width", Range(0, 0.1)) = 0.02
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc" // URPのCore.hlslの代わりに使用

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            sampler2D _DissolveTex;
            float _DissolveAmount;
            float4 _EdgeColor;
            float _EdgeWidth;

            v2f vert (appdata v)
            {
                v2f o;
                // 根拠: UnityObjectToClipPosはBuilt-inでの座標変換標準関数
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                float dissolveValue = tex2D(_DissolveTex, i.uv).r;

                // 根拠: clipは0未満で描画破棄。仕組みはURPと同じ
                clip(dissolveValue - _DissolveAmount);

                // 境界線の計算
                float edgeLogic = step(dissolveValue - _EdgeWidth, _DissolveAmount);
                col.rgb += edgeLogic * _EdgeColor.rgb;

                return col;
            }
            ENDCG
        }
    }
}