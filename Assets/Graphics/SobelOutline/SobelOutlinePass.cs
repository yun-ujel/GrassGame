using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SobelOutlinePass : ScriptableRenderPass
{
    public SobelOutlinePass(SobelOutlinePassSettings settings, Material material)
    {
        renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        this.settings = settings;
        this.material = material;
    }

    private SobelOutlinePassSettings settings;
    private Material material;
    private RTHandle cameraColorTarget;

    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        ConfigureTarget(cameraColorTarget);
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (material == null)
        {
            return;
        }

        CommandBuffer cmd = CommandBufferPool.Get();
        using (new ProfilingScope(cmd, new ProfilingSampler("Sobel Outline Pass")))
        {
            material.SetFloat("_Leniency", settings.Leniency);

            material.SetFloat("_DepthThreshold", settings.DepthThreshold);
            material.SetFloat("_DepthThickness", settings.DepthThickness);
            material.SetFloat("_DepthStrength", settings.DepthStrength);

            Blitter.BlitCameraTexture(cmd, cameraColorTarget, cameraColorTarget, material, 0);
        }

        context.ExecuteCommandBuffer(cmd);
        cmd.Clear();

        CommandBufferPool.Release(cmd);
    }

    public void SetTarget(RTHandle target)
    {
        cameraColorTarget = target;
    }
}

[System.Serializable]
public struct SobelOutlinePassSettings
{
    [field: SerializeField] public float Leniency { get; set; }

    [field: SerializeField] public float DepthThreshold { get; set; }
    [field: SerializeField] public float DepthThickness { get; set; }
    [field: SerializeField] public float DepthStrength { get; set; }

    public SobelOutlinePassSettings(float leniency, float depthThreshold, float depthThickness, float depthStrength)
    {
        Leniency = leniency;
        DepthThreshold = depthThreshold;
        DepthThickness = depthThickness;
        DepthStrength = depthStrength;
    }
}
