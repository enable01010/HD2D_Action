using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAngleFollower : MonoBehaviour
{
    private void Awake()
    {
        EventBus.Register<ObjectAngleData>(SetData);
    }

    private void OnDestroy()
    {
        EventBus.UnRegister<ObjectAngleData>(SetData);
    }

    /// <summary>
    /// 角度情報を適用する処理
    /// </summary>
    /// <param name="data">オブジェクト用データ</param>
    public void SetData(ObjectAngleData data)
    {
        transform.eulerAngles = data.angle;
    }
}

/// <summary>
/// オブジェクト用のデータ集約クラス
/// </summary>
public class ObjectAngleData:IEvent
{
    public Vector3 angle;
}