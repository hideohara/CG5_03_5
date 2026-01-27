using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.RenderGraphModule.Util;
using UnityEngine.Rendering.Universal;

// ScriptableRenderPassを継承
public class PostEffectRenderPass : ScriptableRenderPass
{
    // ポストエフェクト用マテリアル
    private Material blurMaterial_ = null;
    // Blit用のパススルーマテリアル
    private Material passThroughMaterial_ = null;

    public PostEffectRenderPass(
      Material blurMaterial,
      Material passThroughmaterial
    )
    {
        // コンストラクタ引数からマテリアルを取得
        blurMaterial_ = blurMaterial;
        passThroughMaterial_ = passThroughmaterial;
    }


    // 次ページへ

    // 前ページから

    // RenderGraphへの描画設定や描画実行など一連の操作
    public override void RecordRenderGraph(
      RenderGraph renderGraph, ContextContainer frameData)
    {
        // どちらかのマテリアルがnullであれば
        if (
          blurMaterial_ == null ||
          passThroughMaterial_ == null
        )
        {
            // material_がnullならば従来通りの描画を行なう  
            base.RecordRenderGraph(renderGraph, frameData);
            return;
        }
        // このフレームの描画リソースを取得する。
        UniversalResourceData resourceData =
        frameData.Get<UniversalResourceData>();
        // 取得したResourceDataがBackBufferであれば仕様上読み込み
        // 不可なので早期リターン。
        if (resourceData.isActiveTargetBackBuffer)
        { return; }

        // 次ページへ

        // 前ページから

        // カメラ（描画予定）のテクスチャを取得
        TextureHandle cameraTexture =
          resourceData.activeColorTexture;

        // ポストエフェクトを適用したテクスチャを作るために
        // カメラの情報を取得する
        TextureDesc tempDesc =
          renderGraph.GetTextureDesc(cameraTexture);

        // 元サイズの一時テクスチャ。
        tempDesc.name = "_OrigTempTexture";
        // 深度値は使わない
        tempDesc.depthBufferBits = 0;
        // 元サイズの一時テクスチャ
        TextureHandle origTempTexture =
          renderGraph.CreateTexture(tempDesc);

        // 次ページへ
        // 前ページから

        // 縮小サイズの一時テクスチャ
        tempDesc.name = "_SmallTempTexure";
        int div = 2;
        tempDesc.width /= div;
        tempDesc.height /= div;
        TextureHandle smallTempTexture =
          renderGraph.CreateTexture(tempDesc);


        // 次ページへ
        // 前ページから

        // カメラテクスチャにmaterial_を適用し仮テクスチャに
        // 出力する設定を作成。小さくする際にブラーを適用
        RenderGraphUtils.BlitMaterialParameters
          downSampleBlitMaterialParameters =
            new RenderGraphUtils.BlitMaterialParameters(
              cameraTexture, smallTempTexture,
              blurMaterial_, 0
          );
        //その設定をURPに適用   
        renderGraph.AddBlitPass(
          downSampleBlitMaterialParameters,
          "DownSamplingBlitBlur"
        );

        // 次ページへ
        // 前ページから

        // PassThroughを使ってサイズを元に戻す
        RenderGraphUtils.BlitMaterialParameters
          upSampleBlitMaterialParameters =
            new RenderGraphUtils.BlitMaterialParameters(
              smallTempTexture, origTempTexture,
              passThroughMaterial_,
              0
          );
        // その設定をURPに適用
        renderGraph.AddBlitPass(
          upSampleBlitMaterialParameters,
          "UpSamplingBlitBlur"
        );
        // cameraTextureに戻す
        renderGraph.AddCopyPass(
          origTempTexture, cameraTexture,
          "CopyBlur"
        );
    }
}

