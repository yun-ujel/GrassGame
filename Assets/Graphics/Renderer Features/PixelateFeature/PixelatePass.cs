using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PixelatePass : ScriptableRenderPass
{
    public PixelatePass(PixelatePassSettings settings, Material material)
    {
        this.settings = settings;
        renderPassEvent = settings.RenderPassEvent;

        this.material = material;
    }

    private PixelatePassSettings settings;
    private RTHandle cameraColorTarget;

    private int pixelScreenHeight;
    private int pixelScreenWidth;

    private Material material;

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
        using (new ProfilingScope(cmd, new ProfilingSampler("Pixelate Pass")))
        {
            pixelScreenHeight = renderingData.cameraData.camera.pixelHeight / settings.PixelScale;
            pixelScreenWidth = renderingData.cameraData.camera.pixelWidth / settings.PixelScale;

            material.SetVector("_BlockCount", new Vector2(pixelScreenWidth, pixelScreenHeight));
            material.SetVector("_BlockSize", new Vector2(1.0f / pixelScreenWidth, 1.0f / pixelScreenHeight));
            material.SetVector("_HalfBlockSize", new Vector2(0.5f / pixelScreenWidth, 0.5f / pixelScreenHeight));

            material.SetFloat("_DepthThreshold", settings.DepthThreshold);
            material.SetFloat("_NormalsThreshold", settings.NormalsThreshold);

            material.SetVector("_NormalEdgeBias", settings.NormalEdgeBias);

            material.SetFloat("_DepthEdgeStrength", settings.DepthEdgeStrength);
            material.SetFloat("_NormalEdgeStrength", settings.NormalEdgeStrength);

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
public class PixelatePassSettings
{
    [field: SerializeField, Range(1, 10)] public int PixelScale { get; set; } = 3;
    [field: SerializeField] public RenderPassEvent RenderPassEvent { get; set; } = RenderPassEvent.BeforeRenderingPostProcessing;

    [field: Header("Outline Settings"), SerializeField]
    public float DepthThreshold { get; set; }
    [field: SerializeField] public float NormalsThreshold { get; set; }
    [field: SerializeField, Space] public Vector3 NormalEdgeBias { get; set; }
    [field: SerializeField, Space] public float DepthEdgeStrength { get; set; }
    [field: SerializeField] public float NormalEdgeStrength { get; set; }
}
