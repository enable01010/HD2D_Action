using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCreater : MonoBehaviour
{
    [SerializeField] Transform[] VertexesTrasnform;
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;

    [SerializeField] Texture texture;

    void Start()
    {
        ResetMesh();
    }

    public void ResetMesh()
    {
        Vector3[] vertexes = new Vector3[VertexesTrasnform.Length];
        for (int i = 0; i < VertexesTrasnform.Length; i++)
        {
            vertexes[i] = VertexesTrasnform[i].localPosition;
        }

        int[] triangles = new int[6];
        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;
        triangles[3] = 2;
        triangles[4] = 3;
        triangles[5] = 0;

        Vector2[] uv = new Vector2[4];
        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(0, 1);
        uv[2] = new Vector2(1, 1);
        uv[3] = new Vector2(1, 0);

        meshFilter = GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();

        mesh.SetVertices(vertexes);
        mesh.SetTriangles(triangles, 0);
        mesh.SetUVs(0, uv);

        meshFilter.mesh = mesh;

        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.SetTexture("_MainTex", texture);
    }
}
