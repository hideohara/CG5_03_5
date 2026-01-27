
Shader "Custom/PE_PassThrough"
{
  SubShader
  {
    // URPの使用を宣言
    Tags { 
      "RenderPipeline" = "UniversalPipeline" 
    }
    Pass
    {
      HLSLPROGRAM

      #pragma vertex Vert
      #pragma fragment Frag
      // 同期コンパイルの強制
      #pragma editor_sync_compilation

// 次ページへ
// 前ページから

      // URPコアファイル
      #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
      // ポストエフェクトに必要なファイル
      #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

// 次ページへ
// 前ページから

      half4 Frag(Varyings IN) : SV_Target
      {
        half4 output = 
          SAMPLE_TEXTURE2D(
            _BlitTexture, 
            sampler_LinearRepeat,
            IN.texcoord
          );
        return output;
      }
      ENDHLSL
    }
  }
}

