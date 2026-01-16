Shader "Custom/PE_ToneCorrection"
{
        Properties{
      saturation("彩度",range(0,1)) = 1
       contrast("コントラスト",range(0,2)) = 1
    }

  SubShader
  {
    // URP用であると記述
    Tags{ "RenderPipeline" = "UniversalPipeline" }
    Pass
    {
      // ポストエフェクトでは不要な機能を切る
      ZWrite Off 
      ZTest Always 
      Blend Off 
      Cull Off
      // CGではなくHLSLを使う
      HLSLPROGRAM
        #pragma vertex Vert
        #pragma fragment Frag
        #pragma editor_sync_compilation

        // 次ページへ
        // 前ページから

        // URP用のシェーダの機能群
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        // ポストエフェクト用の機能群
        #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

         half saturation;
           half contrast;

        // 前ページから
        // vertはBlit.hlslに書いてあるので不要
          half4 Frag(Varyings input):SV_Target
          {
            half4 output = 
              SAMPLE_TEXTURE2D(_BlitTexture, 
                sampler_LinearRepeat, input.texcoord);
            half grayscale = 
              0.2126 * output.r + 
              0.7152 * output.g + 
              0.0722 * output.b;
            half4 monochromeColor = 
              half4(grayscale, grayscale, grayscale,1);
            //return monochromeColor;
              half4 outputColor = 
            lerp(monochromeColor,output, saturation);
            //return outputColor;
              outputColor = 
            (outputColor - 0.5) * contrast + 0.5;
            return outputColor;
          }

      ENDHLSL
    }
  }
}


