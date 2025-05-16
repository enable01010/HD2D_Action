using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーを追いかけるカメラ
/// </summary>
public class PlayerFollowedCamera : MonoBehaviour
{
    public Transform target;
    public float lerpSpeed = 1.0f;

    private Vector3 offset;

    private void Awake()
    {
        EventBus.Register<CameraAngleData>(SetData);
    }

    private void Update()
    {
        Vector3 targetPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.deltaTime);
    }

    private void OnDestroy()
    {
        EventBus.UnRegister<CameraAngleData>(SetData);
    }

    /// <summary>
    /// 角度情報を適用する処理
    /// </summary>
    /// <param name="data">カメラ用データ</param>
    public void SetData(CameraAngleData data)
    {
        offset = data.offset;
        transform.eulerAngles = data.angle;
    }
}

/// <summary>
/// カメラ関係のデータ集約クラス
/// </summary>
[System.Serializable]
public class CameraAngleData : IEvent
{
    public Vector3 offset;
    public Vector3 angle;
}