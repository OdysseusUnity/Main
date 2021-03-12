using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadTree
{
    List<QuadTree> children = new List<QuadTree>();
    List<QuadTree> neighbors = new List<QuadTree>();
    public Vector3[] corners = new Vector3[4];
    //public Vector3[] norms = new Vector3[4];
    public Vector3 NW;
    public Vector3 NE;
    public Vector3 SW;
    public Vector3 SE;
    public QuadTree parent = null;
    public Transform transform;

    float planetRadius;
    GameObject planetMesh;
    public Material material;

    public Vector3 centre
    {
        get { return (NW + SE) / 2.0f; }
    }

    public QuadTree(Vector3 nw, Vector3 ne, Vector3 sw, Vector3 se, float r)
    {
        Vector3 nwn = nw.normalized;
        Vector3 nen = ne.normalized;
        Vector3 swn = sw.normalized;
        Vector3 sen = se.normalized;
        int oct = 4;
        float scale = 2f;

        Vector3 NC = (NW + NE) / 2.0f;
        Vector3 WC = (NW + SW) / 2.0f;
        Vector3 EC = (NE + SE) / 2.0f;
        Vector3 SC = (SW + SE) / 2.0f;

        corners[0] = nwn * (r + Fbm(nwn, oct, scale));
        corners[1] = nen * (r + Fbm(nen, oct, scale));
        corners[2] = swn * (r + Fbm(swn, oct, scale));
        corners[3] = sen * (r + Fbm(sen, oct, scale));
        /*corners[0] = sen * (r + Fbm(sen, oct, scale));
        corners[1] = nwn * (r + Fbm(nwn, oct, scale));
        corners[2] = swn * (r + Fbm(swn, oct, scale));
        corners[3] = sen * (r + Fbm(sen, oct, scale));
        corners[4] = nen * (r + Fbm(nen, oct, scale));
        corners[5] = nwn * (r + Fbm(nwn, oct, scale));*/


        /*Vector3 nwne = NC.normalized;
        nwne = nwne * (r + Fbm(nwne, oct, scale));
        Vector3 nwnw = (nwn + -(nen - nwn) / 2.0f).normalized;
        nwnw = nwnw * (r + Fbm(nwnw, oct, scale));
        Vector3 nwnn = (nwn + -(swn - nwn) / 2.0f).normalized;
        nwnn = nwnn * (r + Fbm(nwnn, oct, scale));
        Vector3 nwns = WC.normalized;
        nwns = nwns * (r + Fbm(nwns, oct, scale));

        Vector3 nene = (nen + -(nwn - nen) / 2.0f).normalized;
        nene = nene * (r + Fbm(nene, oct, scale));
        Vector3 nenw = NC.normalized;
        nenw = nenw * (r + Fbm(nenw, oct, scale));
        Vector3 nenn = (nen + -(sen - nen) / 2.0f).normalized;
        nenn = nenn * (r + Fbm(nenn, oct, scale));
        Vector3 nens = EC.normalized;
        nens = nens * (r + Fbm(nens, oct, scale));

        Vector3 swne = SC.normalized;
        swne = swne * (r + Fbm(swne, oct, scale));
        Vector3 swnw = (swn + -(sen - swn) / 2.0f).normalized;
        swnw = swnw * (r + Fbm(swnw, oct, scale));
        Vector3 swnn = WC.normalized;
        swnw = swnw * (r + Fbm(swnw, oct, scale));
        Vector3 swns = (swn + -(nwn - swn) / 2.0f).normalized;
        swns = swns * (r + Fbm(swns, oct, scale));

        Vector3 sene = (sen + -(swn - sen)/2.0f).normalized;
        sene = sene * (r + Fbm(sene, oct, scale));
        Vector3 senw = SC.normalized;
        senw = senw * (r + Fbm(senw, oct, scale));
        Vector3 senn = EC.normalized;
        senn = senn * (r + Fbm(senn, oct, scale));
        Vector3 sens = (sen + -(nen - sen) / 2.0f).normalized;
        sens = sens * (r + Fbm(sens, oct, scale));*/

        /*norms[0] = (Vector3.Cross(nwnn-corners[0], nwnw-corners[0]) +
            Vector3.Cross(nwne - corners[0], nwnn - corners[0]) +
            Vector3.Cross(nwns - corners[0], nwne - corners[0]) +
            Vector3.Cross(nwnw - corners[0], nwns - corners[0])).normalized;


        norms[1] = (Vector3.Cross(nenn - corners[1], nenw - corners[1]) +
            Vector3.Cross(nene - corners[1], nenn - corners[1]) +
            Vector3.Cross(nens - corners[1], nene - corners[1]) +
            Vector3.Cross(nenw - corners[1], nens - corners[1])).normalized;


        norms[2] = (Vector3.Cross(swnn - corners[2], swnw - corners[2]) +
            Vector3.Cross(swne - corners[2], swnn - corners[2]) +
            Vector3.Cross(swns - corners[2], swne - corners[2]) +
            Vector3.Cross(swnw - corners[2], swns - corners[2])).normalized;


        norms[3] = -(Vector3.Cross(senn - corners[3], senw - corners[3]) +
            Vector3.Cross(sene - corners[3], senn - corners[3]) +
            Vector3.Cross(sens - corners[3], sene - corners[3]) +
            Vector3.Cross(senw - corners[3], sens - corners[3])).normalized;
        norms[0] = (Vector3.Cross(swn * (r + Fbm(swn, oct, scale)) - nwn * (r + Fbm(nwn, oct, scale)), sen * (r + Fbm(sen, oct, scale)) - nwn * (r + Fbm(nwn, oct, scale))).normalized+
            Vector3.Cross(sen * (r + Fbm(sen, oct, scale)) - nwn * (r + Fbm(nwn, oct, scale)), nen * (r + Fbm(nen, oct, scale)) - nwn * (r + Fbm(nwn, oct, scale))).normalized).normalized;
        norms[1] = (Vector3.Cross(nwn * (r + Fbm(nwn, oct, scale)) - nen * (r + Fbm(nen, oct, scale)), swn * (r + Fbm(swn, oct, scale)) - nen * (r + Fbm(nen, oct, scale))).normalized +
            Vector3.Cross(swn * (r + Fbm(swn, oct, scale)) - nen * (r + Fbm(nen, oct, scale)), sen * (r + Fbm(sen, oct, scale)) - nen * (r + Fbm(nen, oct, scale))).normalized).normalized;
        norms[2] = (Vector3.Cross(sen * (r + Fbm(sen, oct, scale)) - swn * (r + Fbm(swn, oct, scale)), nen * (r + Fbm(nen, oct, scale)) - swn * (r + Fbm(swn, oct, scale))).normalized +
            Vector3.Cross(nen * (r + Fbm(nen, oct, scale)) - swn * (r + Fbm(swn, oct, scale)), nwn * (r + Fbm(nwn, oct, scale)) - swn * (r + Fbm(swn, oct, scale))).normalized).normalized;
        norms[3] = (Vector3.Cross(nen * (r + Fbm(nen, oct, scale)) - sen * (r + Fbm(sen, oct, scale)), swn * (r + Fbm(swn, oct, scale)) - sen * (r + Fbm(sen, oct, scale))).normalized +
            Vector3.Cross(nen * (r + Fbm(nen, oct, scale)) - sen * (r + Fbm(sen, oct, scale)), nwn * (r + Fbm(nwn, oct, scale)) - sen * (r + Fbm(sen, oct, scale))).normalized).normalized;
            */
        NW = nw;
        NE = ne;
        SW = sw;
        SE = se;
        planetRadius = r;
    }

    public void setNeighbors(QuadTree W, QuadTree E, QuadTree N, QuadTree S)
    {
        neighbors.Clear();
        neighbors.Add(W);
        neighbors.Add(E);
        neighbors.Add(N);
        neighbors.Add(S);
    }

    public void SubdivideAll()
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

            children[0].setNeighbors(neighbors[0],children[1],neighbors[2],children[2]);
            children[1].setNeighbors(children[0],neighbors[1],neighbors[2],children[3]);
            children[2].setNeighbors(neighbors[0],children[3],children[0],neighbors[3]);
            children[3].setNeighbors(children[2],neighbors[1],children[1],neighbors[3]);
        }
        else
        {
            Debug.Log("Cannot Subdivide");
        }
    }

    public void SubdivideAll(Vector3 coord)
    {
        if (children.Count == 0)
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

            children[0].setNeighbors(neighbors[0], children[1], neighbors[2], children[2]);
            children[1].setNeighbors(children[0], neighbors[1], neighbors[2], children[3]);
            children[2].setNeighbors(neighbors[0], children[3], children[0], neighbors[3]);
            children[3].setNeighbors(children[2], neighbors[1], children[1], neighbors[3]);
        }
        else
        {
            Debug.Log("Cannot Subdivide");
        }
    }

    public void SubdivideAllLast()
    {
        if (HaveChildren())
        {
            for(int i = 0; i < children.Count; i++)
            {
                children[i].SubdivideAllLast();
            }
        }
        else
        {
            SubdivideAll();
        }
    }

    public bool SubdivideAll(int Lvl)
    {
        bool res = false;
        //if(children.Count == 0)
        //{
            if (Lvl > 0)
            {
                res = true;
                SubdivideAll();
                for (int i = 0; i < children.Count; i++)
                {
                    children[i].SubdivideAll(Lvl - 1);
                }
            }
        //}
        //else
        //{
            //Debug.Log("Cannot Subdivide");
        //}
        return res;
    }

    public bool SubdivideAll(int Lvl, Vector3 coord)
    {
        bool res = false;
        Vector3 center = (corners[0] + corners[3]) / 2.0f;
        int lvl = (int)(Vector3.Distance(center, coord) / 50.0f);
        //if (children.Count == 0)
        //{
            if (Lvl > 0)
            {
                res = true;
                SubdivideAll();
                for (int i = 0; i < children.Count; i++)
                {
                    children[i].SubdivideAll(Lvl - 1);
                }
            }
        //}
        //else
        //{
            //Debug.Log("Cannot Subdivide");
        //}
        return res;
    }

    public bool SubdivideClosest(Vector3 coord, int LvlMax, float distThres)
    {
        Vector3 center = (corners[0] + corners[3]) / 2.0f;
        float dist = Vector3.Distance(center, coord);
        if (Math.Abs(dist) < 0.1f)
            dist = 1;
        int lvlMax = (int)(20.0f / dist) + 1;
        if (lvlMax > 10)
            lvlMax = 10;
        bool res = SubdivideAll(lvlMax);
        return res;
    }

    public bool SubdivideClosest(Vector3 coord)
    {
        Vector3 center = (corners[0] + corners[3]) / 2.0f;
        float dist = Vector3.Distance(center, coord);
        if (Math.Abs(dist) < 0.1f)
            dist = 1;
        int lvlMax = (int)(20.0f / dist)+1;
        if (lvlMax > 10)
            lvlMax = 10;
        bool res = SubdivideAll(lvlMax);
        return res;
    }

    public bool HaveChildren()
    {
        return children.Count > 0;
    }

    public bool isLeaf()
    {
        return children.Count == 0;
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
        if (HaveChildren())
        {
            for (int i = 0; i < children.Count; i++)
            {
                children[i].RemoveAllChildren();
            }
            children.Clear();
        }
    }

    public void GenerateMesh()
    {
        Mesh terrainMesh;
        if (!planetMesh)
        {
            planetMesh = new GameObject("Planet Mesh");
            MeshRenderer surfaceRenderer = planetMesh.AddComponent<MeshRenderer>();
            surfaceRenderer.material = material;

            planetMesh.AddComponent<MeshFilter>();
            terrainMesh = new Mesh();
        }
        else
        {
            terrainMesh = planetMesh.GetComponent<MeshFilter>().mesh;
        }

        List<QuadTree> quads = new List<QuadTree>();
        LastLeafList(ref quads);
        int quadCount = quads.Count;
        int vertexCount = quadCount * 4;
        int indexCount = quadCount * 6;

        int[] indices = new int[indexCount];

        Vector3[] vertices = new Vector3[vertexCount];
        //Vector3[] normals = new Vector3[vertexCount];
        Color32[] colors = new Color32[vertexCount];

        Color32 green = new Color32(20, 255, 30, 255);
        Color32 brown = new Color32(220, 150, 70, 255);

        for (int i = 0; i < quadCount; i++)
        {
            indices[i * 6 + 0] = i * 4 + 3;
            indices[i * 6 + 1] = i * 4 + 0;
            indices[i * 6 + 2] = i * 4 + 2;
            indices[i * 6 + 3] = i * 4 + 3;
            indices[i * 6 + 4] = i * 4 + 1;
            indices[i * 6 + 5] = i * 4 + 0;

            //System.Buffer.BlockCopy(vertices, i * 4, corners, 0, 4);
            Array.Copy(quads[i].corners, 0, vertices, i * 4, 4);

            Color32 polyColor = Color32.Lerp(green, brown, UnityEngine.Random.Range(0.0f, 1.0f));

            colors[i * 4 + 0] = polyColor;
            colors[i * 4 + 1] = polyColor;
            colors[i * 4 + 2] = polyColor;
            colors[i * 4 + 3] = polyColor;

            //System.Buffer.BlockCopy(vertices, i * 4, corners, 0, 4);
            //Array.Copy(quads[i].norms, 0, normals, i * 4, 4);
        }

        terrainMesh.vertices = vertices;
        //terrainMesh.normals = normals;
        terrainMesh.colors32 = colors;

        terrainMesh.SetTriangles(indices, 0);
        terrainMesh.RecalculateNormals();
        terrainMesh.RecalculateBounds();
        //NormalSolver.RecalculateNormals(terrainMesh, 60f);
        MeshFilter terrainFilter = planetMesh.GetComponent<MeshFilter>();
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
