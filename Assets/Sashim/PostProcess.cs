using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcess : ScriptableRendererFeature
{
    [System.Serializable]
    public class FullScreenEffectSettings
    {
        public Material effectMaterial; // The material with your custom shader
    }

    public FullScreenEffectSettings settings = new FullScreenEffectSettings();

    class FullScreenEffectPass : ScriptableRenderPass
    {
        private Material effectMaterial;
        private RTHandle temporaryRT;

        public FullScreenEffectPass(Material material)
        {
            effectMaterial = material;
            renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;

        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            // Initialize the temporary RTHandle with the camera's target descriptor
            temporaryRT = RTHandles.Alloc(
                renderingData.cameraData.cameraTargetDescriptor.width,
                renderingData.cameraData.cameraTargetDescriptor.height,
                colorFormat: renderingData.cameraData.cameraTargetDescriptor.graphicsFormat,
                useDynamicScale: renderingData.cameraData.camera.allowDynamicResolution,
                name: "_TemporaryRT"
            );
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
          Debug.Log("Here");
            if (effectMaterial == null)
                return;

            CommandBuffer cmd = CommandBufferPool.Get("FullScreenEffect");

            // Get the source (camera color target) and temporary RTHandle
            var source = renderingData.cameraData.renderer.cameraColorTargetHandle;

            // Apply the effect to the temporary RTHandle and copy it back to the camera target
            Blitter.BlitCameraTexture(cmd, source, temporaryRT, effectMaterial, 0); // Pass 0 for default shader pass
            Blitter.BlitCameraTexture(cmd, temporaryRT, source);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            // Release the temporary RTHandle
            temporaryRT?.Release();
        }
    }

    private FullScreenEffectPass fullScreenEffectPass;

    public override void Create()
    {
      Debug.Log("create");
        if (settings.effectMaterial != null)
        {
            fullScreenEffectPass = new FullScreenEffectPass(settings.effectMaterial)
            {
                renderPassEvent = RenderPassEvent.AfterRenderingTransparents
            };
        }
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
          Debug.Log("passes added");
        if (fullScreenEffectPass != null && settings.effectMaterial != null)
        {
            renderer.EnqueuePass(fullScreenEffectPass);
        }
    }
}
