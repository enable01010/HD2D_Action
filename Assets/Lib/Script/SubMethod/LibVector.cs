using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// よく使うVector関係の処理用クラス
/// </summary>
public static class LibVector
{
    /// <summary>
    /// TransoformのXの値のみを変更する処理
    /// </summary>
    /// <param name="vector">元のベクトル</param>
    /// <param name="value">変更後の値</param>
    /// <returns></returns>
    static public Vector3 Set_X(Vector3 vector, float value)
    {
       
        vector.x = value;

        return vector;
    }

    /// <summary>
    /// TransoformのYの値のみを変更する処理
    /// </summary>
    /// <param name="vector">元のベクトル</param>
    /// <param name="value">変更後の値</param>
    /// <returns></returns>
    static public Vector3 Set_Y(Vector3 vector, float value)
    {
        vector.y = value;

        return vector;
    }

    /// <summary>
    /// TransoformのZの値のみを変更する処理
    /// </summary>
    /// <param name="vector">元のベクトル</param>
    /// <param name="value">変更後の値</param>
    /// <returns></returns>
    static public Vector3 Set_Z(Vector3 vector, float value)
    {
        vector.z = value;

        return vector;
    }

    /// <summary>
    /// TransoformのXの値のみを加算する処理
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    static public Vector3 Add_X(Vector3 vector, float value)
    {
        Vector3 answer = Vector3.zero;

        answer = vector;
        answer.x += value;

        return answer;
    }

    /// <summary>
    /// TransoformのYの値のみを加算する処理
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    static public Vector3 Add_Y(Vector3 vector, float value)
    {
        Vector3 answer = Vector3.zero;

        answer = vector;
        answer.y += value;

        return answer;
    }

    /// <summary>
    /// TransoformのZの値のみを加算する処理
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    static public Vector3 Add_Z(Vector3 vector, float value)
    {
        Vector3 answer = Vector3.zero;

        answer = vector;
        answer.z += value;

        return answer;
    }


    /// <summary>
    /// 2次元ベクトルを角度（弧度法）に変更する処理
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    static public float ChangeVectorToRadian_2D(Vector2 vector)
    {
        float answer = 0;

        answer = Mathf.Atan2(vector.y, vector.x);

        return answer;
    }

    /// <summary>
    ///  2次元ベクトルを角度（弧度法）に変更する処理
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    static public float ChangeVectorToRadian_2D(Vector3 vector)
    {
        float answer = 0;

        answer = Mathf.Atan2(vector.y, vector.x);

        return answer;
    }

    /// <summary>
    /// 2次元ベクトルを角度（弧度法）に変更する処理
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    static public float ChangeVectorToRadian_2D(float x, float y)
    {
        float answer = 0;

        answer = Mathf.Atan2(y, x);

        return answer;
    }

    /// <summary>
    /// 2次元ベクトルを角度（度数法）に変更する処理
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    static public float ChangeVectorToDegree_2D(Vector2 vector)
    {
        float answer = 0;

        float raian = ChangeVectorToRadian_2D(vector);
        answer = raian * Mathf.Rad2Deg;

        return answer;
    }

    /// <summary>
    ///  2次元ベクトルを角度（度数法）に変更する処理
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    static public float ChangeVectorToDegree_2D(Vector3 vector)
    {
        float answer = 0;

        float raian = ChangeVectorToRadian_2D(vector);
        answer = raian * Mathf.Rad2Deg;

        return answer;
    }

    /// <summary>
    ///  2次元ベクトルを角度（度数法）に変更する処理
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    static public float ChangeVectorToDegree_2D(float x, float y)
    {
        float answer = 0;

        float raian = ChangeVectorToRadian_2D(x, y);
        answer = raian * Mathf.Rad2Deg;

        return answer;
    }

    /// <summary>
    /// 2次元ベクトルを角度 (Vector3)に変更する処理
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    static public Vector3 ChangeVectorToEulerAngle_2D(Vector2 vector)
    {
        Vector3 answer = Vector3.zero;

        float degree = ChangeVectorToDegree_2D(vector);
        answer = Set_Z(answer, degree);

        return answer;
    }


    /// <summary>
    /// 2次元ベクトルを角度 (Vector3)に変更する処理
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    static public Vector3 ChangeVectorToEulerAngle_2D(Vector3 vector)
    {
        Vector3 answer = Vector3.zero;

        float degree = ChangeVectorToDegree_2D(vector);
        answer = Set_Z(answer, degree);

        return answer;
    }

    /// <summary>
    /// 2次元ベクトルを角度 (Vector3)に変更する処理
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    static public Vector3 ChangeVectorToEulerAngle_2D(float x, float y)
    {
        Vector3 answer = Vector3.zero;

        float degree = ChangeVectorToDegree_2D(x, y);
        answer = Set_Z(answer, degree);

        return answer;
    }


    // 拡張メソッド

   

    static public Vector3 Only_X(this Vector3 vec)
    {
        vec.y = 0;
        vec.z = 0;
        return vec;
    }

    static public Vector3 Only_Y(this Vector3 vec)
    {
        vec.x = 0;
        vec.z = 0;
        return vec;
    }

    static public Vector3 Only_Z(this Vector3 vec)
    {
        vec.y = 0;
        vec.x = 0;
        return vec;
    }

    static public Vector3 Only_X(this float value)
    {
        return new Vector3(value, 0, 0);
    }

    static public Vector3 Only_Y(this float value)
    {
        return new Vector3(0, value, 0);
    }

    static public Vector3 Only_Z(this float value)
    {
        return new Vector3(0, 0, value);
    }

    static public float3 ChangeFloat3(this Vector3 vec)
    {
        return new float3(vec.x, vec.y, vec.z);
    }

    static public Vector3 ChangeVector3(this float3 value)
    {
        return new Vector3(value.x, value.y, value.z);
    }

    static public void GetRange(Vector3[] points, out Vector3 min, out Vector3 max)
    {
        min = Vector3.positiveInfinity;
        max = Vector3.negativeInfinity;

        foreach (Vector3 point in points)
        {
            min = SelectMinData(point, min);
            max = SelectMaxData(point, max);
        }
    }

    static public void GetRange(Vector3 firstPos, Vector3 secondPos, out Vector3 min, out Vector3 max)
    {
        Vector3[] points = new Vector3[2];
        points[0] = firstPos;
        points[1] = secondPos;
        GetRange(points, out min, out max);
    }

    static public Vector3 SelectMinData(Vector3 value1, Vector3 value2)
    {
        float x = Mathf.Min(value1.x, value2.x);
        float y = Mathf.Min(value1.y, value2.y);
        float z = Mathf.Min(value1.z, value2.z);
        return new Vector3(x, y, z);
    }

    static public Vector3 SelectMaxData(Vector3 value1, Vector3 value2)
    {
        float x = Mathf.Max(value1.x, value2.x);
        float y = Mathf.Max(value1.y, value2.y);
        float z = Mathf.Max(value1.z, value2.z);
        return new Vector3(x, y, z);
    }

    static public void AddSpaceToRange(float space, ref Vector3 min, ref Vector3 max)
    {
        min.x -= space;
        min.y -= space;
        min.z -= space;
        max.x += space;
        max.y += space;
        max.z += space;
    }
}
