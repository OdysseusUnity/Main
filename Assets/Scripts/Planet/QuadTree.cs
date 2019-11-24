using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadTree
{
    List<QuadTree> children = new List<QuadTree>();
    public Vector3[] corners = new Vector3[4];
    public Vector3[] norms = new Vector3[4];
    public Vector3 NW;
    public Vector3 NE;
    public Vector3 SW;
    public Vector3 SE;
    public QuadTree parent = null;
    public Transform transform;

    float planetRadius;
    GameObject planetMesh;
    public Material material;

    public QuadTree(Vector3 nw, Vector3 ne, Vector3 sw, Vector3 se, float r)
    {
        Vector3 nwn = nw.normalized;
        Vector3 nen = ne.normalized;
        Vector3 swn = sw.normalized;
        Vector3 sen = se.normalized;
        float dist = Vector3.Distance(nen, nwn);
        int oct = 10;

        /*Vector4 nwne = (nwn + new Vector3(dist,0,0)).normalized;
        Vector4 nwnw = (nwn + new Vector3(-dist, 0, 0)).normalized;
        Vector4 nwnn = (nwn + new Vector3(0, 0, dist)).normalized;
        Vector4 nwns = (nwn + new Vector3(0, 0, -dist)).normalized;

        Vector4 nene = (nen + new Vector3(dist, 0, 0)).normalized;
        Vector4 nenw = (nen + new Vector3(-dist, 0, 0)).normalized;
        Vector4 nenn = (nen + new Vector3(0, 0, dist)).normalized;
        Vector4 nens = (nen + new Vector3(0, 0, -dist)).normalized;

        Vector4 swne = (swn + new Vector3(dist, 0, 0)).normalized;
        Vector4 swnw = (swn + new Vector3(-dist, 0, 0)).normalized;
        Vector4 swnn = (swn + new Vector3(0, 0, dist)).normalized;
        Vector4 swns = (swn + new Vector3(0, 0, -dist)).normalized;

        Vector4 sene = (sen + new Vector3(dist, 0, 0)).normalized;
        Vector4 senw = (sen + new Vector3(-dist, 0, 0)).normalized;
        Vector4 senn = (sen + new Vector3(0, 0, dist)).normalized;
        Vector4 sens = (sen + new Vector3(0, 0, -dist)).normalized;*/

        corners[0] = nwn * (r + Fbm(nwn, oct, 1f));
        corners[1] = nen * (r + Fbm(nen, oct, 1f));
        corners[2] = swn * (r + Fbm(swn, oct, 1f));
        corners[3] = sen * (r + Fbm(sen, oct, 1f));

        /*norms[0] = Vector3.Cross(nwnw * (r + Fbm(nwnw, oct, 1f)) - nwne * (r + Fbm(nwne, oct, 1f)), nwnn * (r + Fbm(nwnn, oct, 1f)) - nwns * (r + Fbm(nwns, oct, 1f))).normalized;
        norms[1] = Vector3.Cross(nenw * (r + Fbm(nenw, oct, 1f)) - nene * (r + Fbm(nene, oct, 1f)), nenn * (r + Fbm(nenn, oct, 1f)) - nens * (r + Fbm(nens, oct, 1f))).normalized;
        norms[2] = Vector3.Cross(swnw * (r + Fbm(swnw, oct, 1f)) - swne * (r + Fbm(swne, oct, 1f)), swnn * (r + Fbm(swnn, oct, 1f)) - swns * (r + Fbm(swns, oct, 1f))).normalized;
        norms[3] = Vector3.Cross(senw * (r + Fbm(senw, oct, 1f)) - sene * (r + Fbm(sene, oct, 1f)), senn * (r + Fbm(senn, oct, 1f)) - sens * (r + Fbm(sens, oct, 1f))).normalized;*/
        norms[0] = Vector3.Cross(swn * (r + Fbm(swn, oct, 1f)) - nwn * (r + Fbm(nwn, oct, 1f)), sen * (r + Fbm(sen, oct, 1f)) - nwn * (r + Fbm(nwn, oct, 1f))).normalized;
        norms[1] = Vector3.Cross(nwn * (r + Fbm(nwn, oct, 1f)) - nen * (r + Fbm(nen, oct, 1f)), sen * (r + Fbm(sen, oct, 1f)) - nen * (r + Fbm(nen, oct, 1f))).normalized;
        norms[2] = Vector3.Cross(sen * (r + Fbm(sen, oct, 1f)) - swn * (r + Fbm(swn, oct, 1f)), nwn * (r + Fbm(nwn, oct, 1f)) - swn * (r + Fbm(swn, oct, 1f))).normalized;
        norms[3] = -Vector3.Cross(swn * (r + Fbm(swn, oct, 1f)) - sen * (r + Fbm(sen, oct, 1f)), nen * (r + Fbm(nen, oct, 1f)) - sen * (r + Fbm(sen, oct, 1f))).normalized;

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
            Vector3 NC = (NW + NE) / 2.0f;
            Vector3 WC = (NW + SW) / 2.0f;
            Vector3 EC = (NE + SE) / 2.0f;
            Vector3 SC = (SW + SE) / 2.0f;
            Vector3 center = ((NC + SC) / 2.0f);

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
        List<QuadTree> quads = new List<QuadTree>();
        LastLeafList(ref quads);
        int quadCount = quads.Count;
        int vertexCount = quadCount * 4;
        int indexCount = quadCount * 6;

        int[] indices = new int[indexCount];

        Vector3[] vertices = new Vector3[vertexCount];
        Vector3[] normals = new Vector3[vertexCount];
        Color32[] colors = new Color32[vertexCount];

        Color32 green = new Color32(20, 255, 30, 255);
        Color32 brown = new Color32(220, 150, 70, 255);

        int[] index = { 3, 0, 2, 3, 1, 0 };

        for (int i = 0; i < quadCount; i++)
        {
            indices[i * 6 + 0] = i * 4 + index[0];
            indices[i * 6 + 1] = i * 4 + index[1];
            indices[i * 6 + 2] = i * 4 + index[2];
            indices[i * 6 + 3] = i * 4 + index[3];
            indices[i * 6 + 4] = i * 4 + index[4];
            indices[i * 6 + 5] = i * 4 + index[5];

            //System.Buffer.BlockCopy(vertices, i * 4, corners, 0, 4);
            Array.Copy(quads[i].corners, 0, vertices, i * 4, 4);

            Color32 polyColor = Color32.Lerp(green, brown, UnityEngine.Random.Range(0.0f, 1.0f));

            colors[i * 4 + 0] = polyColor;
            colors[i * 4 + 1] = polyColor;
            colors[i * 4 + 2] = polyColor;
            colors[i * 4 + 3] = polyColor;

            //System.Buffer.BlockCopy(vertices, i * 4, corners, 0, 4);
            Array.Copy(quads[i].norms, 0, normals, i * 4, 4);
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


    public static float PerlinNoise3D(float x, float y, float z, float s)
    {
        y += 1;
        z += 2;
        float xy = _perlin3DFixed(x, y, s);
        float xz = _perlin3DFixed(x, z, s);
        float yz = _perlin3DFixed(y, z, s);
        float yx = _perlin3DFixed(y, x, s);
        float zx = _perlin3DFixed(z, x, s);
        float zy = _perlin3DFixed(z, y, s);
        return xy * xz * yz * yx * zx * zy;
    }
    static float _perlin3DFixed(float a, float b, float s)
    {
        return Mathf.Sin(Mathf.PI * Mathf.PerlinNoise(a*s, b*s));
    }


    static float Fbm(Vector3 coord, int octave, float s)
    {
        var f = 0.0f;
        var w = 0.5f;
        for (var i = 0; i < octave; i++)
        {
            f += w * PerlinNoise3D(coord.x, coord.y, coord.z, s);
            coord *= 2.0f;
            w *= 0.5f;
        }
        return f;
    }

}
