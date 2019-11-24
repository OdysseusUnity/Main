using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadTree
{
    List<QuadTree> children = new List<QuadTree>();
    public QuadTree parent = null;
    public Transform transform;
    Vector3 NW;
    Vector3 NE;
    Vector3 SW;
    Vector3 SE;

    float planetRadius;
    GameObject planetMesh;
    public Material material;

    public QuadTree(Vector3 nw, Vector3 ne, Vector3 sw, Vector3 se, float r)
    {
        NW = nw;
        NE = ne;
        SW = sw;
        SE = se;
        planetRadius = r;
    }

    public void Subdivide()
    {
        if(children.Count == 0)
        {
            Vector3 center = (((NW + SW) + (NE + SE)) / 4.0f);
            Vector3 NC = (NW + NE) / 2.0f;
            Vector3 WC = (NW + SW) / 2.0f;
            Vector3 EC = (NE + SE) / 2.0f;
            Vector3 SC = (SW + SE) / 2.0f;

            center = center.normalized * planetRadius;
            NC = NC.normalized * planetRadius;
            WC = WC.normalized * planetRadius;
            EC = EC.normalized * planetRadius;
            SC = SC.normalized * planetRadius;

            children.Add(new QuadTree(NW, NC, WC, center, planetRadius));
            children[0].parent = this;
            children.Add(new QuadTree(NC, NE, center, EC, planetRadius));
            children[1].parent = this;
            children.Add(new QuadTree(WC, center, SW, SC, planetRadius));
            children[2].parent = this;
            children.Add(new QuadTree(center, EC, SC, SE, planetRadius));
            children[3].parent = this;
        }
        else
        {
            Debug.Log("Cannot Subdivide");
        }
    }

    public void SubdivideLast()
    {
        if (HaveChildren())
        {
            for(int i = 0; i < children.Count; i++)
            {
                children[i].SubdivideLast();
            }
        }
        else
        {
            Subdivide();
        }
    }

    public void Subdivide(int Lvl)
    {
        if(children.Count == 0)
        {
            Subdivide();
            if (Lvl > 0)
            {
                for (int i = 0; i < children.Count; i++)
                {
                    children[i].Subdivide(Lvl - 1);
                }
            }
        }
        else
        {
            Debug.Log("Cannot Subdivide");
        }
    }

    public bool HaveChildren()
    {
        return children.Count > 0;
    }

    public void RemoveLastChildren()
    {
        if (HaveChildren())
        {
            for (int i = 0; i < children.Count; ++i)
            {
                children[i].RemoveLastChildren();
            }
        }
        else
        {
            children.Clear();
        }
    }

    public void RemoveAllChildren()
    {
        for(int i = 0; i < children.Count; i++)
        {
            while (children[i].HaveChildren())
            {
                RemoveLastChildren();
            }
        }
        children.Clear();
    }

    public void GenerateMesh()
    {
        if (planetMesh)
            GameObject.Destroy(planetMesh);

        planetMesh = new GameObject("Planet Mesh");

        MeshRenderer surfaceRenderer = planetMesh.AddComponent<MeshRenderer>();
        surfaceRenderer.material = material;

        Mesh terrainMesh = new Mesh();
        int quadCount = (int)Math.Pow(4, LvlNb() - 1);
        int vertexCount = quadCount * 6;

        int[] indices = new int[vertexCount];

        Vector3[] vertices = new Vector3[vertexCount];
        Vector3[] normals = new Vector3[vertexCount];
        Color32[] colors = new Color32[vertexCount];

        Color32 green = new Color32(20, 255, 30, 255);
        Color32 brown = new Color32(220, 150, 70, 255);

        for (int i = 0; i < quadCount; i++)
        {
            var poly = m_Polygons[i];

            indices[i * 3 + 0] = i * 3 + 0;
            indices[i * 3 + 1] = i * 3 + 1;
            indices[i * 3 + 2] = i * 3 + 2;

            vertices[i * 3 + 0] = m_Vertices[poly.m_Vertices[0]];
            vertices[i * 3 + 1] = m_Vertices[poly.m_Vertices[1]];
            vertices[i * 3 + 2] = m_Vertices[poly.m_Vertices[2]];

            Color32 polyColor = Color32.Lerp(green, brown, Random.Range(0.0f, 1.0f));

            colors[i * 3 + 0] = polyColor;
            colors[i * 3 + 1] = polyColor;
            colors[i * 3 + 2] = polyColor;

            normals[i * 3 + 0] = m_Vertices[poly.m_Vertices[0]];
            normals[i * 3 + 1] = m_Vertices[poly.m_Vertices[1]];
            normals[i * 3 + 2] = m_Vertices[poly.m_Vertices[2]];
        }

        terrainMesh.vertices = vertices;
        terrainMesh.normals = normals;
        terrainMesh.colors32 = colors;

        terrainMesh.SetTriangles(indices, 0);

        MeshFilter terrainFilter = planetMesh.AddComponent<MeshFilter>();
        terrainFilter.mesh = terrainMesh;
    }

    public int LvlNb()
    {
        int Lvl = 0;
        if (HaveChildren())
        {
            Lvl += children[0].LvlNb();
        }
        else
        {
            Lvl++;
        }
        return Lvl;
    }

    public ref List<QuadTree> LastLeafList(ref List<QuadTree> res)
    {
        if (HaveChildren())
        {
            for(int i = 0; i < children.Count; i++)
            {
                children[i].LastLeafList(ref res);
            }
        }
        else
        {
            res.Add(this);
        }
        return ref res;
    }


    public static float PerlinNoise3D(float x, float y, float z)
    {
        y += 1;
        z += 2;
        float xy = _perlin3DFixed(x, y);
        float xz = _perlin3DFixed(x, z);
        float yz = _perlin3DFixed(y, z);
        float yx = _perlin3DFixed(y, x);
        float zx = _perlin3DFixed(z, x);
        float zy = _perlin3DFixed(z, y);
        return xy * xz * yz * yx * zx * zy;
    }
    static float _perlin3DFixed(float a, float b)
    {
        return Mathf.Sin(Mathf.PI * Mathf.PerlinNoise(a, b));
    }

}
