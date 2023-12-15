Shader "Screen/Sobel Outline" {
	SubShader {
		Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline"}
        LOD 100
        ZWrite Off Cull Off
		
		Pass {
			Name "ColorBlitPass"

			HLSLPROGRAM
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            // The Blit.hlsl file provides the vertex shader (Vert),
            // input structure (Attributes) and output strucutre (Varyings)
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

			#pragma vertex Vert
            #pragma fragment frag

			float _Leniency;

			float _DepthThreshold;
			float _DepthThickness;
			float _DepthStrength;

            TEXTURE2D(_CameraOpaqueTexture);
            TEXTURE2D(_CameraDepthTexture);
            TEXTURE2D(_CameraNormalsTexture);

			float4 _CameraDepthTexture_TexelSize;

			SamplerState sampler_point_clamp;

			static float2 sobelSamplePoints[9] = {
				float2(-1, 1), float2(0, 1), float2(1, 1),
				float2(-1, 0), float2(0, 0), float2(1, 0),
				float2(-1, -1), float2(0, -1), float2(1, -1)
			};

			static float sobelXMatrix[9] = {
				1, 0, -1,
				2, 0, -2,
				1, 0, -1
			};

			static float sobelYMatrix[9] = {
				1, 2, 1,
				0, 0, 0,
				-1, -2, -1
			};

			float GetOutline(float2 UV, float leniency, float depthThreshold, float depthThickness, float depthStrength) {
				float2 sobel = 0;

				[unroll] for (int i = 0; i < 9; i++) {
					float2 UVSamplePoint = sobelSamplePoints[i] / _CameraDepthTexture_TexelSize.xy;
					UVSamplePoint += UV;

					float depth = _CameraDepthTexture.Sample(sampler_point_clamp, UVSamplePoint);
					sobel += depth * float2(sobelXMatrix[i], sobelYMatrix[i]);
				}
				
				float sobelLength = length(sobel);
				float joe = smoothstep(0, depthThreshold, sobelLength);
				
				joe = pow(joe, depthThickness);
				
				return joe * depthStrength;
			}

			half4 frag (Varyings input) : SV_Target {
				float4 color = _CameraOpaqueTexture.Sample(sampler_point_clamp);
				return color;
			}

			ENDHLSL
		}
	}
}