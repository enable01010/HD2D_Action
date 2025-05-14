using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ray(Line)を出した際にDebug.Drawをするクラス
/// </summary>
public static class LibPhysics
{
    #region Raycast2D

    /// <summary>
    /// Physics2D.Raycast
    /// </summary>
    /// <param name="origin"> 原点 </param>
    /// <param name="direction"> 方向 </param>
    /// <param name="distance"> 距離 </param>
    /// <param name="layerMask"> 特定のレイヤーのコライダーのみを判別 (1の場合はDefaultのみ) </param>
    /// <param name="duration"> 消えるまでの時間(秒単位) (0の場合は1フレームのみ表示) </param>
    /// <returns></returns>
    static public RaycastHit2D Raycast2D(Vector2 origin, Vector2 direction, float distance, int layerMask = 1, float duration = 0.0f)
    {
        RaycastHit2D answer = new RaycastHit2D();

        answer = Physics2D.Raycast(origin, direction, distance, layerMask);

#if UNITY_EDITOR
        
        if (answer == true)
        {
            Debug.DrawRay(origin, direction.normalized * distance, Color.green, duration);
        }
        else
        {
            Debug.DrawRay(origin, direction.normalized * distance, Color.red, duration);
        }

#endif

        return answer;
    }



    /// <summary>
    /// Physics2D.Raycast (引数をRayに変更(自作))
    /// </summary>
    /// <param name="ray"> レイ </param>
    /// <param name="distance"> 距離 </param>
    /// <param name="layerMask"> 特定のレイヤーのコライダーのみを判別 (1の場合はDefaultのみ) </param>
    /// <param name="duration"> 消えるまでの時間(秒単位) (0の場合は1フレームのみ表示) </param>
    /// <returns></returns>
    static public RaycastHit2D Raycast2D(Ray ray, float distance, int layerMask = 1, float duration = 0.0f)
    {
        return Raycast2D(ray.origin, ray.direction, distance, layerMask, duration);
    }



    /// <summary>
    /// Physics2D.Linecast
    /// </summary>
    /// <param name="start"> 開始地点 </param>
    /// <param name="end"> 終了地点 </param>
    /// <param name="layerMask"> 特定のレイヤーのコライダーのみを判別 (1の場合はDefaultのみ) </param>
    /// <param name="duration"> 消えるまでの時間(秒単位) (0の場合は1フレームのみ表示) </param>
    /// <returns></returns>
    static public RaycastHit2D Linecast2D(Vector2 start, Vector2 end, int layerMask = 1, float duration = 0.0f)
    {
        RaycastHit2D answer = new RaycastHit2D();

        answer = Physics2D.Linecast(start, end, layerMask);

#if UNITY_EDITOR

        if (answer == true)
        {
            Debug.DrawLine(start, end, Color.green, duration);
        }
        else
        {
            Debug.DrawLine(start, end, Color.red, duration);
        }

#endif

        return answer;
    }



    /// <summary>
    /// Physics2D.CircleCast
    /// </summary>
    /// <param name="origin"> 円の中心 </param>
    /// <param name="radius"> 円の半径 </param>
    /// <param name="direction"> 円を伸ばす方向 </param>
    /// <param name="distance"> 円を伸ばす距離 </param>
    /// <param name="layerMask"> 特定のレイヤーのコライダーのみを判別 (1の場合はDefaultのみ) </param>
    /// <param name="duration"> 消えるまでの時間(秒単位) (0の場合は1フレームのみ表示) </param>
    /// <returns></returns>
    static public RaycastHit2D CircleCast2D(Vector2 origin, float radius, Vector2 direction, float distance, int layerMask = 1, float duration = 0.0f)
    {
        RaycastHit2D answer = new RaycastHit2D();

        answer = Physics2D.CircleCast(origin, radius, direction, distance, layerMask);

#if UNITY_EDITOR

        if (answer == true)
        {
            DrawCircleLine(origin, radius, direction, distance, duration, Color.green);
        }
        else
        {
            DrawCircleLine(origin, radius, direction, distance, duration, Color.red);
        }

#endif

        return answer;
    }



    #endregion
    
    #region Raycast3D

    /// <summary>
    /// Physics.Raycast
    /// </summary>
    /// <param name="origin"> 原点 </param>
    /// <param name="direction"> 方向 </param>
    /// <param name="distance"> 距離 </param>
    /// <param name="layerMask"> 特定のレイヤーのコライダーのみを判別 (1の場合はDefaultのみ) </param>
    /// <param name="duration"> 消えるまでの時間(秒単位) (0の場合は1フレームのみ表示) </param>
    /// <returns></returns>
    static public RaycastHit Raycast(Vector3 origin, Vector3 direction, float distance, int layerMask = 1, float duration = 0.0f)
    {
        RaycastHit answer;

        Physics.Raycast(origin, direction, out answer, distance, layerMask);

#if UNITY_EDITOR
        Debug.DrawRay(origin, direction.normalized * distance, Color.red, duration);
#endif

        return answer;
    }



    /// <summary>
    /// Physics.Raycast (引数をRayに変更)
    /// </summary>
    /// <param name="ray"> レイ </param>
    /// <param name="distance"> 距離 </param>
    /// <param name="layerMask"> 特定のレイヤーのコライダーのみを判別 (1の場合はDefaultのみ) </param>
    /// <param name="duration"> 消えるまでの時間(秒単位) (0の場合は1フレームのみ表示) </param>
    /// <returns></returns>
    static public RaycastHit Raycast(Ray ray, float distance, int layerMask = 1, float duration = 0.0f)
    {
        return Raycast(ray.origin, ray.direction, distance, layerMask, duration);
    }



    /// <summary>
    /// Physics.Linecast
    /// </summary>
    /// <param name="start"> 開始地点 </param>
    /// <param name="end"> 終了地点 </param>
    /// <param name="layerMask"> 特定のレイヤーのコライダーのみを判別 (1の場合はDefaultのみ) </param>
    /// <param name="duration"> 消えるまでの時間(秒単位) (0の場合は1フレームのみ表示) </param>
    /// <returns></returns>
    static public RaycastHit Linecast(Vector3 start, Vector3 end, int layerMask = 1, float duration = 0.0f)
    {
        RaycastHit answer;

        Physics.Linecast(start, end, out answer, layerMask);

#if UNITY_EDITOR
        Debug.DrawLine(start, end, Color.red, duration);
#endif

        return answer;
    }



    /// <summary>
    /// Physics.SphereCast
    /// </summary>
    /// <param name="origin"> 球の中心 </param>
    /// <param name="radius"> 球の半径 </param>
    /// <param name="direction"> 球を伸ばす方向 </param>
    /// <param name="distance"> 球を伸ばす距離 </param>
    /// <param name="layerMask"> 特定のレイヤーのコライダーのみを判別 (1の場合はDefaultのみ) </param>
    /// <param name="duration"> 消えるまでの時間(秒単位) (0の場合は1フレームのみ表示) </param>
    /// <returns></returns>
    static public RaycastHit SphereCast(Vector3 origin, float radius, Vector3 direction, float distance, int layerMask = 1, float duration = 0.0f)
    {
        RaycastHit answer;

        Physics.SphereCast(origin, radius, direction, out answer, distance, layerMask);

#if UNITY_EDITOR

        if (answer.transform != null)
        {
            DrawSphereLine(origin, radius, direction, distance, duration, Color.green);
        }
        else
        {
            DrawSphereLine(origin, radius, direction, distance, duration, Color.red);
        }
        
#endif

        return answer;
    }



    #endregion
    
    #region Draw

    // CircleCastのDraw
    static private void DrawCircleLine(Vector2 origin, float radius, Vector2 direction, float distance, float duration, Color drawColor)
    {
        // Rayの色
        Color color = drawColor;

        // originの円の十字
        Vector2 right = origin + new Vector2(radius, 0);
        Vector2 left = origin + new Vector2(-radius, 0);
        Vector2 up = origin + new Vector2(0, radius);
        Vector2 down = origin + new Vector2(0, -radius);

        Debug.DrawLine(right, left, color, duration);
        Debug.DrawLine(up, down, color, duration);


        // offsetの円の十字
        Vector2 offset = direction.normalized * distance;

        Debug.DrawLine(right + offset, left + offset, color, duration);
        Debug.DrawLine(up + offset, down + offset, color, duration);


        // 円の周
        const int VERTEX = 64;// 頂点の数
        float angle = 2 * Mathf.PI / VERTEX;

        for (int i = 0; i < VERTEX; i++)
        {
            // originの円の周
            Vector2 temp1 = origin + new Vector2(radius * Mathf.Cos(angle * i), radius * Mathf.Sin(angle * i));
            Vector2 temp2 = origin + new Vector2(radius * Mathf.Cos(angle * (i + 1)), radius * Mathf.Sin(angle * (i + 1)));

            Debug.DrawLine(temp1, temp2, color, duration);


            // offsetの円の周
            Vector2 temp3 = offset + temp1;
            Vector2 temp4 = offset + temp2;

            Debug.DrawLine(temp3, temp4, color, duration);
        }


        // startの円とoffsetの円をつなぐ
        Vector2 nor = Vector2.Perpendicular(offset).normalized;// 法線ベクトル(normalizedされていない)

        Debug.DrawLine(origin + nor * radius, offset + origin + nor * radius, color, duration);
        Debug.DrawLine(origin - nor * radius, offset + origin - nor * radius, color, duration);
    }

    // SphereCastのDraw
    static private void DrawSphereLine(Vector3 origin, float radius, Vector3 direction, float distance, float duration, Color drawColor)
    {
        Color color = drawColor;

        // offset
        Vector3 offset = direction.normalized * distance;


        // x = rcosφcosθ
        // y = rsinφ      縦軸
        // x = rcosφsinθ

        float theta;// θ：平面角(経度) 0 <= θ <= 360
        float phi;  // φ：仰角　(緯度) 0 <= φ <= 180


        // 球の周
        int VERTEX = 64;// 頂点の数
        int halfVertex = VERTEX / 2;
        float angle = 2 * Mathf.PI / halfVertex;

        int surfaceNum = 64;// 経線(縦の線)の数
        float angle2 = 2 * Mathf.PI / surfaceNum;

        for (int i = 0; i < surfaceNum; i++)
        {
            theta = angle2 * i;

            for (int j = 0; j < halfVertex; j++)
            {
                phi = angle * j;

                float phiNext = angle * (j + 1);

                // originの球の周
                Vector3 vertex1 = origin;
                Vector3 vertex2 = origin;

                vertex1.x += radius * Mathf.Cos(phi) * Mathf.Cos(theta);
                vertex1.y += radius * Mathf.Sin(phi);
                vertex1.z += radius * Mathf.Cos(phi) * Mathf.Sin(theta);

                vertex2.x += radius * Mathf.Cos(phiNext) * Mathf.Cos(theta);
                vertex2.y += radius * Mathf.Sin(phiNext);
                vertex2.z += radius * Mathf.Cos(phiNext) * Mathf.Sin(theta);

                Debug.DrawLine(vertex1, vertex2, color, duration);


                // offsetの球の周
                Vector3 vertex3 = offset + vertex1;
                Vector3 vertex4 = offset + vertex2;

                Debug.DrawLine(vertex3, vertex4, color, duration);
            }

        }

        // originの円とoffsetの円をつなぐ
        Debug.DrawLine(origin, origin + offset, color, duration);
    }

    #endregion

    #region IsHit(boolを返す拡張メソッド) staticクラスにしないとダメ？

    // RaycastHit2D
    static public bool IsHit(this RaycastHit2D raycastHit2D)
    {
        return raycastHit2D;
    }

    // RaycastHit
    static public bool IsHit(this RaycastHit raycastHit)
    {
        return raycastHit.transform != null;
    }

    // --------------------------試行錯誤--------------------------

    //static public implicit operator bool(RaycastHit raycastHit)
    //{
    //    bool answer;

    //    answer = raycastHit.transform != null;

    //    return answer;
    //}

    //static public bool operator ==(RaycastHit raycastHit, bool temp)
    //{
    //    bool answer;

    //    // 一致してたらtrue
    //    if((raycastHit.transform != null) == temp)
    //    {
    //        answer = true;
    //    }
    //    else
    //    {
    //        answer = false;
    //    }

    //    return answer;
    //}

    //static public bool operator !=(RaycastHit raycastHit, bool temp)
    //{
    //    bool answer;

    //    // 一致してなかったらtrue
    //    if ((raycastHit.transform != null) != temp)
    //    {
    //        answer = true;
    //    }
    //    else
    //    {
    //        answer = false;
    //    }

    //    return answer;
    //}

    //static public bool operator true(RaycastHit raycastHit)
    //{
    //    return raycastHit.transform != null;
    //}

    //static public bool operator false(RaycastHit raycastHit)
    //{
    //    return raycastHit.transform == null;
    //}

    #endregion

    #region Builderパターン

    // Builderパターン
    //static public Raycast2DBuilder BuildStart()
    //{
    //    return new Raycast2DBuilder();
    //}

    //public class Raycast2DBuilder
    //{
    //    Vector2 origin = Vector2.zero;
    //    Vector2 direction = Vector2.zero;
    //    float distance = 0.0f;
    //    int layerMask = 1;
    //    float duration = 0.0f;

    //    public Raycast2DBuilder SetOrigin(Vector2 origin) 
    //    { 
    //        this.origin = origin;
    //        return this;
    //    } 
    //    public Raycast2DBuilder SetDirection(Vector2 direction)
    //    { 
    //        this.direction = direction;
    //        return this;
    //    }
    //    public Raycast2DBuilder SetDistance(float distance)
    //    {
    //        this.distance = distance;
    //        return this;
    //    }
    //    public Raycast2DBuilder SetLayerMask(int layerMask)
    //    {
    //        this.layerMask = layerMask;
    //        return this;
    //    }
    //    public Raycast2DBuilder SetDuration(float duration)
    //    {
    //        this.duration = duration;
    //        return this;
    //    }

    //    public RaycastHit2D Raycast2D()
    //    {
    //        return LibPhysics.Raycast2D(origin, direction, distance, layerMask, duration);
    //    }
    //}

    // 使い方
    //LibPhysics
    //        .BuildStart()
    //        .SetDirection()
    //        .SetDistance()
    //        .SetDuration()
    //        .SetOrigin()
    //        .SetLayerMask()
    //        .Raycast2D();

    #endregion
}
