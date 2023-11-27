Shader "Screen/Pixelate"
{
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline"}
        LOD 100
        ZWrite Off Cull Off
        Pass
        {
            Name "ColorBlitPass"

            HLSLPROGRAM
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            // The Blit.hlsl file provides the vertex shader (Vert),
            // input structure (Attributes) and output strucutre (Varyings)
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            #pragma vertex Vert
            #pragma fragment frag

            TEXTURE2D(_CameraOpaqueTexture);
            TEXTURE2D(_CameraDepthTexture);
            TEXTURE2D(_CameraNormalsTexture);

            SamplerState sampler_point_clamp;

            float2 _BlockCount;
            float2 _BlockSize;
            float2 _HalfBlockSize;

            half4 frag (Varyings input) : SV_Target
            {
                float2 blockPos = floor(input.texcoord * _BlockCount);
                float2 blockCenter = blockPos * _BlockSize + _HalfBlockSize;

                half4 color = _CameraOpaqueTexture.Sample(sampler_point_clamp, blockCenter);
                return color;
            }
            ENDHLSL
        }
    }
}
