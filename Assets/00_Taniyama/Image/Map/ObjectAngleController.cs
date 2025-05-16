using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAngleController : Singleton<ObjectAngleController>
{
    const float MINI_FLOAT = 0.1f;

    /// <summary>
    /// すべての角度を調整する
    /// </summary>
    public void SetAllAngle()
    {

    }

    #region Camera

    CameraAngleData cameraAngleData = new CameraAngleData();

    /// <summary>
    /// カメラの角度を上げる
    /// </summary>
    public static void CamAngleUp()
    {
        instance.cameraAngleData.angle.x -= MINI_FLOAT;
        EventBus.Raise(instance.cameraAngleData);
    }

    /// <summary>
    /// カメラの角度を下げる
    /// </summary>
    public static void CamAngleDown()
    {
        instance.cameraAngleData.angle.x += MINI_FLOAT;
        EventBus.Raise(instance.cameraAngleData);
    }

    /// <summary>
    /// カメラの角度を右にする
    /// </summary>
    public void CamAngleRight()
    {
        instance.cameraAngleData.angle.y += MINI_FLOAT;
        EventBus.Raise(instance.cameraAngleData);
    }

    /// <summary>
    /// カメラの角度を左にする
    /// </summary>
    public void CamAngleLeft()
    {
        instance.cameraAngleData.angle.y -= MINI_FLOAT;
        EventBus.Raise(instance.cameraAngleData);
    }

    /// <summary>
    /// カメラの位置を上げる
    /// </summary>
    public void CamPositionUp()
    {
        instance.cameraAngleData.offset.y += MINI_FLOAT;
        EventBus.Raise(instance.cameraAngleData);
    }

    /// <summary>
    /// カメラの位置を下げる
    /// </summary>
    public void CamPositionDown()
    {
        instance.cameraAngleData.offset.y -= MINI_FLOAT;
        EventBus.Raise(instance.cameraAngleData);
    }

    /// <summary>
    /// カメラの位置を左に
    /// </summary>
    public void CamPositionLeft()
    {
        instance.cameraAngleData.offset.x += MINI_FLOAT;
        EventBus.Raise(instance.cameraAngleData);
    }

    /// <summary>
    /// カメラの位置を右に
    /// </summary>
    public void CamPositionRight()
    {
        instance.cameraAngleData.offset.x -= MINI_FLOAT;
        EventBus.Raise(instance.cameraAngleData);
    }

    #endregion

    #region Object

    public ObjectAngleData objectAngleData = new ObjectAngleData();

    /// <summary>
    /// オブジェクトの角度を上げる処理
    /// </summary>
    public static void ObjectAngleUp()
    {
        instance.objectAngleData.angle.x -= MINI_FLOAT;
        EventBus.Raise(instance.objectAngleData);
    }

    /// <summary>
    /// オブジェクトの角度を下げる処理
    /// </summary>
    public static void ObjectAngleDown()
    {
        instance.objectAngleData.angle.x += MINI_FLOAT;
        EventBus.Raise(instance.objectAngleData);
    }

    #endregion
}
