Shader "Custom/CartoonShader"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Main Texture", 2D) = "white" {}
        [HDR]
        _AmbientColor("Ambient Color", Color) = (0.1,0.1,0.1,1)
        [HDR]
        _SpecularColor("Specular Color", Color) = (0.5,0.5,0.5,1)
        _Glossiness("Glossiness", Float) = 16
        [HDR]
        _RimColor("Rim Color", Color) = (0.5,0.5,0.5,1)
        _RimAmount("Rim Amount", Range(0, 1)) = 0.6
        _RimThreshold("Rim Threshold", Range(0, 1)) = 0.1
        _SpecularIntensity("Specular Intensity", Range(0, 1)) = 0.3
        _RimIntensity("Rim Intensity", Range(0, 1)) = 0.3
        _LightIntensity("Light Intensity", Range(0, 1)) = 0.7
    }
    SubShader
    {
        Pass
        {
            Tags
            {
                "LightMode" = "ForwardBase"
                "PassFlags" = "OnlyDirectional"
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase
            
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            struct appdata
            {
                float4 vertex : POSITION;                
                float4 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldNormal : NORMAL;
                float2 uv : TEXCOORD0;
                float3 viewDir : TEXCOORD1;    
                SHADOW_COORDS(2)
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);        
                o.viewDir = WorldSpaceViewDir(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                TRANSFER_SHADOW(o)
                return o;
            }

            float4 _Color;
            float4 _AmbientColor;

            float4 _SpecularColor;
            float _Glossiness;
            float _SpecularIntensity;

            float4 _RimColor;
            float _RimAmount;
            float _RimThreshold;
            float _RimIntensity;

            float _LightIntensity;

            float4 frag (v2f i) : SV_Target
            {
                float3 normal = normalize(i.worldNormal);
                float3 viewDir = normalize(i.viewDir);

                // Directional light
                float NdotL = dot(_WorldSpaceLightPos0, normal);
                float shadow = SHADOW_ATTENUATION(i);
                float lightIntensity = smoothstep(0, 0.01, NdotL * shadow);
                lightIntensity *= _LightIntensity; // Scale overall lighting
                float4 light = lightIntensity * _LightColor0;

                // Specular reflection
                float3 halfVector = normalize(_WorldSpaceLightPos0 + viewDir);
                float NdotH = dot(normal, halfVector);
                float specularIntensity = pow(NdotH * lightIntensity, _Glossiness);
                specularIntensity = smoothstep(0.005, 0.01, specularIntensity);
                float4 specular = specularIntensity * _SpecularColor * _SpecularIntensity;

                // Rim lighting
                float rimDot = 1 - dot(viewDir, normal);
                float rimIntensity = rimDot * pow(NdotL, _RimThreshold);
                rimIntensity = smoothstep(_RimAmount - 0.01, _RimAmount + 0.01, rimIntensity);
                float4 rim = rimIntensity * _RimColor * _RimIntensity;

                // Main texture
                float4 sample = tex2D(_MainTex, i.uv);

                return (light + _AmbientColor + specular + rim) * _Color * sample;
            }
            ENDCG
        }

        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
}