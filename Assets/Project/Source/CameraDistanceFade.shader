Shader "Custom/CameraDistanceFade" 
{
    Properties 
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _MaxDistance ("Max Visibility Distance", Float) = 10.0
        _FadeStart ("Fade Start Distance", Float) = 8.0
    }
    SubShader 
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha

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
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            fixed4 _Color;
            float4 _MainTex_ST;
            float _MaxDistance;
            float _FadeStart;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float dist = distance(_WorldSpaceCameraPos, i.worldPos);
                float visibility = 1.0 - smoothstep(_FadeStart, _MaxDistance, dist);
                
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                col.a *= visibility;
                return col;
            }
            ENDCG
        }
    }
}
