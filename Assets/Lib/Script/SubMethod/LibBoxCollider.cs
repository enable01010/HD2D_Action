using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LibBoxCollider
{
    // 親にBoxCollider付いてる
    // 親のTransformの Position, Rotation, Scale 0じゃなくてOK
    // min, max は　親のTransformの Position(0,0,0), Rotation(0,0,0), Scale(1,1,1) のときの相対位置

    /// <summary>
    /// 引数のlocalPositionでBoxColliderを設定する
    /// </summary>
    /// <param name="boxCollider"></param>
    /// <param name="min">最小頂点</param>
    /// <param name="max">最大頂点</param>
    /// <returns></returns>
    static public BoxCollider SetColliderAreaOfLocal(this BoxCollider boxCollider, Vector3 min, Vector3 max)
    {
        Vector3 midpoint = Vector3.Lerp(min, max, 0.5f);
        boxCollider.center = midpoint;
        boxCollider.size = max - min;

        return boxCollider;
    }
}
