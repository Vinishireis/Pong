using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SimpleBlit : ScriptableRendererFeature
{
    [System.Serializable]
    private class Settings
    {
        public Material blitMaterial;
        public bool setUnscaledTime = false;
    }

    [SerializeField] private Settings settings = new Settings();

    private BlitPass blitPass;

    // Initializes this feature's resources.
    public override void Create()
    {
        blitPass = new BlitPass(settings);
    }

    // Injects one or multiple ScriptableRenderPass in the renderer.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        blitPass.Setup(renderer.cameraColorTargetHandle);
        renderer.EnqueuePass(blitPass);
    }

    private class BlitPass : ScriptableRenderPass
    {
        private Material blitMaterial;
        private bool setUnscaledTime;

        private RTHandle temporaryColorTexture;

        private RTHandle source;

        public BlitPass(Settings settings)
        {
            renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
            blitMaterial = settings.blitMaterial;
            setUnscaledTime = settings.setUnscaledTime;
        }

        public void Setup(RTHandle source)
        {
            this.source = source;
        }

        // Execute the pass. This is where custom rendering occurs. Specific details are left to the implementation.
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get();

            if (setUnscaledTime)
            {
                blitMaterial.SetFloat("_UnscaledTime", Time.unscaledTime);
            }

            // Allocate a temporary RTHandle for the blit
            RenderingUtils.ReAllocateIfNeeded(ref temporaryColorTexture, renderingData.cameraData.cameraTargetDescriptor, FilterMode.Bilinear, TextureWrapMode.Clamp, name: "_TemporaryColorTexture");

            Blit(cmd, source, temporaryColorTexture, blitMaterial);
            Blit(cmd, temporaryColorTexture, source);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        // Cleanup any allocated data that was created during the execution of the pass.
        public override void FrameCleanup(CommandBuffer cmd)
        {
            if (temporaryColorTexture != null)
            {
                RTHandles.Release(temporaryColorTexture);
            }
        }
    }
}
