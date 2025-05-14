using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// カメラ動作用の汎用クラス
/// </summary>
public static class LibCameraAction
{
    private static IEnumerator hitStopCoroutine = null;
    private static Vector3 shakeVector;

    /// <summary>
    /// カメラのヒットストップ処理（画面揺らす）
    /// </summary>
    /// <param name="shakeData">揺らすために必要なデータ(特別な状況を除きnullの予定）</param>
    public static void Shake(CameraShake shakeData = null)
    {
        // すでにヒットストップのカメラ演出が再生されている場合は前の処理の終了処理をした後、再始動させる
        if(hitStopCoroutine != null)
        {
            // 終了処理
            LibCoroutineRunner.StopCoroutine(hitStopCoroutine);
            hitStopCoroutine = null;
        }

        // コルーチンデータを保持する
        hitStopCoroutine = ShakeCoroutine(shakeData);

        // カメラ挙動のコルーチン始動
        LibCoroutineRunner.StartCoroutine(hitStopCoroutine);
    }

    private static IEnumerator ShakeCoroutine(CameraShake shakeData)
    {
        // シェイクデータはnullを許容するためない場合は固定値で初期化
        if(shakeData == null)
        {
            shakeData = new CameraShake
            {
                delay = 0f,
                duration = 0.3f,
                shakeStrength = new Vector3(0.1f, 0.1f, 0.1f),
                shakeCurve = AnimationCurve.Linear(0, 1, 1, 0)
            };
        }

        // URPのカメラ設定
        RenderPipelineManager.beginCameraRendering += MoveCamera;

        // カメラの移動方向のみ算出
        float time = shakeData.duration;
        while (time > 0)
        {
            yield return null;

            time -= Time.deltaTime;
            float delta = Mathf.Clamp01(LibMath.OneMinus(time / shakeData.duration));

            var randomVec = new Vector3(Random.value, Random.value, Random.value);
            var shakeVec = Vector3.Scale(randomVec, shakeData.shakeStrength) * (Random.value > 0.5f ? -1 : 1);
            shakeVector = shakeVec * shakeData.shakeCurve.Evaluate(delta);
        }

        // URPのカメラ設定削除
        RenderPipelineManager.beginCameraRendering -= MoveCamera;
    }

    private static void MoveCamera(ScriptableRenderContext context, Camera cam)
    {
        cam.transform.localPosition += cam.transform.rotation * shakeVector;
    }
}

[System.Serializable]
public class CameraShake
{
    public float delay = 0.0f;
    public float duration = 1.0f;
    public Vector3 shakeStrength = new Vector3(0.1f, 0.1f, 0.1f);
    public AnimationCurve shakeCurve = AnimationCurve.Linear(0, 1, 1, 0);
}