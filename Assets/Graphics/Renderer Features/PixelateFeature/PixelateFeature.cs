using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PixelateFeature : ScriptableRendererFeature
{
    [SerializeField] private Shader shader;
    [SerializeField] private PixelatePassSettings settings;
    private PixelatePass pixelatePass;

    private Material material;

    public override void Create()
    {
        material = CoreUtils.CreateEngineMaterial(shader);
        pixelatePass = new PixelatePass(settings, material);
    }

    protected override void Dispose(bool disposing)
    {
        CoreUtils.Destroy(material);
    }

    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        pixelatePass.ConfigureInput(ScriptableRenderPassInput.Color);
        pixelatePass.SetTarget(renderer.cameraColorTargetHandle);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(pixelatePass);
    }
}
