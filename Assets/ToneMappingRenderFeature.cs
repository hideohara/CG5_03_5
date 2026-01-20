using UnityEngine;
using UnityEngine.Rendering.Universal;
// URPにPostEffectRenderPassを渡すためのクラス
public class ToneMappingRenderFeature : ScriptableRendererFeature
{
    // ポストエフェクト計算用のマテリアル
    [SerializeField]
    private Material postEffectMaterial_;
    // URPに渡すRenderPass
    private ToneMappingRenderPass renderPass_;

    // このクラスがURPによって生成されたときに呼ばれる関数
    public override void Create()
    {
        renderPass_ = new
          ToneMappingRenderPass(postEffectMaterial_);
        // レンダリング完了後、他ポストエフェクトが適用される前
        renderPass_.renderPassEvent =
          RenderPassEvent.BeforeRenderingPostProcessing;
    }
    // 次ページへ
    // 前ページから

    // パスを追加する関数
    public override void AddRenderPasses(
      ScriptableRenderer rendererPass,
      ref RenderingData renderingData)
    {
        if (rendererPass != null)
        {
            rendererPass.EnqueuePass(renderPass_);
        }
    }
}

