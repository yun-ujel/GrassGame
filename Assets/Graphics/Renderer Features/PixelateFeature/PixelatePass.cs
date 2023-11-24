using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PixelatePass : ScriptableRenderPass
{
    public PixelatePass(PixelatePassSettings settings, Material material)
    {
        this.settings = settings;
        renderPassEvent = settings.RenderPassEvent;
    }

    private PixelatePassSettings settings;
    private RTHandle cameraColorTarget;

    private int pixelScreenHeight;
    private int pixelScreenWidth;

    private Material material;

    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        ConfigureTarget(cameraColorTarget);

        pixelScreenHeight = settings.ScreenHeight;
        pixelScreenWidth = settings.GetScreenWidth(renderingData.cameraData.camera.aspect);

        material.SetVector("_BlockCount", new Vector2(pixelScreenWidth, pixelScreenHeight));
        material.SetVector("_BlockSize", new Vector2(1.0f / pixelScreenWidth, 1.0f / pixelScreenHeight));
        material.SetVector("_HalfBlockSize", new Vector2(0.5f / pixelScreenWidth, 0.5f / pixelScreenHeight));

        RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;

        descriptor.width = pixelScreenWidth;
        descriptor.height = pixelScreenHeight;

        //d.GetTemporaryRT(Shader.PropertyToID(pixelHandle.name), descriptor, FilterMode.Point);
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (material == null)
        {
            return;
        }

        CommandBuffer cmd = CommandBufferPool.Get();
        using (new ProfilingScope(cmd, new ProfilingSampler("Pixelate Pass")))
        {
            Blitter.BlitCameraTexture(cmd, cameraColorTarget, cameraColorTarget, material, 0);
        }
        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }

    public void SetTarget(RTHandle target)
    {
        cameraColorTarget = target;
    }
}

[System.Serializable]
public class PixelatePassSettings
{
    [field: SerializeField] public RenderPassEvent RenderPassEvent { get; set; } = RenderPassEvent.BeforeRenderingPostProcessing;
    [field: SerializeField] public int ScreenHeight { get; set; } = 360;

    public int GetScreenWidth(float aspect)
    {
        return Mathf.CeilToInt(ScreenHeight * aspect);
    }
}
