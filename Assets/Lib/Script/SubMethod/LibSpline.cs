using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public static class LibSpline
{
    static public SplineContainer SetColliderArea(this SplineContainer splineContainer, BoxCollider boxCollider, float margin)
    {
        List<Vector3> tempVertexes = new List<Vector3>();
        foreach (var splin in splineContainer.Spline)
        {
            tempVertexes.Add(splin.Position);
        }

        Vector3[] vertexes;
        vertexes = tempVertexes.ToArray();

        LibVector.GetRange(vertexes, out Vector3 min, out Vector3 max);
        LibVector.AddSpaceToRange(margin, ref min, ref max);

        boxCollider.SetColliderAreaOfLocal(min, max);

        return splineContainer;
    }
}
