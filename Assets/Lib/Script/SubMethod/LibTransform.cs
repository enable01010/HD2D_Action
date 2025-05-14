using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LibTransform
{
    /// <summary>
    /// Chenge_X の拡張メソッド
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    static public Transform SetX_Position(this Transform transform, float value)
    {
        transform.position = LibVector.Set_X(transform.position, value);

        return transform;
    }

    /// <summary>
    /// Chenge_Y の拡張メソッド
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    static public Transform SetY_Position(this Transform transform, float value)
    {
        transform.position = LibVector.Set_Y(transform.position, value);

        return transform;
    }

    /// <summary>
    /// Chenge_Z の拡張メソッド
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    static public Transform SetZ_Position(this Transform transform, float value)
    {
        transform.position = LibVector.Set_Z(transform.position, value);

        return transform;
    }

    /// <summary>
    /// Add_X の拡張メソッド
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="x"></param>
    /// <returns></returns>
    static public Transform AddX_Position(this Transform transform, float value)
    {
        transform.position = LibVector.Add_X(transform.position, value);

        return transform;
    }

    /// <summary>
    /// Add_Y の拡張メソッド
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    static public Transform AddY_Position(this Transform transform, float value)
    {
        transform.position = LibVector.Add_Y(transform.position, value);

        return transform;
    }

    /// <summary>
    /// Add_Z の拡張メソッド
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    static public Transform AddZ_Position(this Transform transform, float value)
    {
        transform.position = LibVector.Add_Z(transform.position, value);

        return transform;
    }

    //指定のポイントまで時間管理で移動する
    static public bool MoveFocusTime(this Transform transform, Vector3 goalPos, ref float time)
    {
        return transform.MoveFocusTime(goalPos, ref time, out Vector3 moveDir);
    }

    static public bool MoveFocusTime(this Transform transform, Vector3 goalPos, ref float time, out Vector3 moveDir)
    {
        if (time <= Time.deltaTime)
        {
            time = 0;
            moveDir = goalPos - transform.position;
            transform.position = goalPos;
            return true;
        }

        float rate = Time.deltaTime / time;
        Vector3 startPos = transform.position;
        transform.position = Vector3.Lerp(transform.position, goalPos, rate);
        moveDir = transform.position - startPos;
        time -= Time.deltaTime;
        return false;
    }

    //指定のポイントまで速度管理で移動する
    static public bool MoveFocusSpeed(this Transform transform, Vector3 goalPos, float speed)
    {
        return transform.MoveFocusSpeed(goalPos, speed, out Vector3 moveDir);
    }

    static public bool MoveFocusSpeed(this Transform transform, Vector3 goalPos, float speed, out Vector3 moveDir)
    {
        Vector3 startPos = transform.position;
        transform.position = Vector3.MoveTowards(transform.position, goalPos, speed);
        moveDir = transform.position - startPos;

        return moveDir.magnitude < speed;
    }

    //指定のポイントまで時間管理で移動する
    static public bool MoveFocusTime(this RectTransform transform, Vector3 goalPos, ref float time)
    {
        return transform.MoveFocusTime(goalPos, ref time, out Vector3 moveDir);
    }

    static public bool MoveFocusTime(this RectTransform transform, Vector3 goalPos, ref float time, out Vector3 moveDir)
    {
        if (time <= Time.deltaTime)
        {
            time = 0;
            moveDir = goalPos - (Vector3)transform.anchoredPosition;
            transform.anchoredPosition = goalPos;
            return true;
        }

        float rate = Time.deltaTime / time;
        Vector3 startPos = transform.anchoredPosition;
        transform.anchoredPosition = Vector3.Lerp(transform.anchoredPosition, goalPos, rate);
        moveDir = (Vector3)transform.anchoredPosition - startPos;
        time -= Time.deltaTime;
        return false;
    }

    //指定のポイントまで速度管理で移動する
    static public bool MoveFocusSpeed(this RectTransform transform, Vector3 goalPos, float speed)
    {
        return transform.MoveFocusSpeed(goalPos, speed, out Vector3 moveDir);
    }

    static public bool MoveFocusSpeed(this RectTransform transform, Vector3 goalPos, float speed, out Vector3 moveDir)
    {
        Vector3 startPos = transform.anchoredPosition;
        transform.anchoredPosition = Vector3.MoveTowards(transform.anchoredPosition, goalPos, speed);
        moveDir = (Vector3)transform.anchoredPosition - startPos;

        return moveDir.magnitude < speed;
    }

    //指定の方向を一定速度で振り向く
    static public void RotFocusSpeed(this Transform transform, Quaternion targetRotation, float speed)
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, speed);
    }


    //Vecgtor3

    //Forwardベクトル（正面）に対して別のベクトルが前後どちらにあるか判定する(1が前-1が後）
    static public float VerticalElementOfForwardToDir(Vector3 forward, Vector3 dir)
    {
        float angle = Vector3.Angle(forward.normalized, dir.normalized);
        float vertical = LibMath.GetRangeToValue(angle, 0, 180) * 2 - 1;
        return vertical;
    }

    //Forwardベクトル（正面）に対して別のベクトルが左右どちらにあるか判定する(1が右-1が左）
    static public float HolizontalElementOfForwardToDir(Vector3 forward, Vector3 dir)
    {
        float holizontal = (Vector3.Cross(forward.normalized, dir.normalized).y < 0 ? -1 : 1);
        return Vector3.Cross(forward, dir).y;
    }

    //ベクトルを特定のY軸角度で回転させる（カメラの向きを考慮した入力方向の検知等）
    static public Vector3 RotationDirOfObjectFront(Transform obj, Vector3 dir)
    {
        Quaternion _targetRotation = Quaternion.AngleAxis(obj.eulerAngles.y, Vector3.up);
        Vector3 targetDirection = _targetRotation * dir.normalized;
        return targetDirection;
    }

    static public Vector3 RotationDirOfObjectFront(Transform obj, Vector2 dir)
    {
        return RotationDirOfObjectFront(obj, new Vector3(dir.x, 0, dir.y));
    }


    /// <summary>
    /// 子階層以下のすべてのオブジェクト一覧を取得する処理
    /// </summary>
    /// <param name="parent">自身</param>
    /// <param name="list">入力しなくてよしメモリ対策用変数</param>
    /// <returns></returns>
    public static List<Transform> GetAllChildren(this Transform parent, List<Transform> list = null)
    {
        if (list == null) list = new List<Transform>();

        list.Add(parent);
        foreach (Transform children in parent)
        {
            children.GetAllChildren(list);
        }

        return list;
    }
}
