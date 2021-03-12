using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    List<QuadTree> qt = new List<QuadTree>();
    public Material material;

    public float r = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        //F
        qt.Add(new QuadTree(new Vector3(-r, r, r), new Vector3(r, r, r), new Vector3(-r, -r, r), new Vector3(r, -r, r), Mathf.Sqrt(3 * r)));
        //B
        qt.Add(new QuadTree(new Vector3(r, r, -r), new Vector3(-r, r, -r), new Vector3(r, -r, -r), new Vector3(-r, -r, -r), Mathf.Sqrt(3 * r)));
        //L
        qt.Add(new QuadTree(new Vector3(r, -r, -r), new Vector3(r, -r, r), new Vector3(r, r, -r), new Vector3(r, r, r), Mathf.Sqrt(3 * r)));
        //R
        qt.Add(new QuadTree(new Vector3(-r, r, -r), new Vector3(-r, r, r), new Vector3(-r, -r, -r), new Vector3(-r, -r, r), Mathf.Sqrt(3 * r)));

        //U
        qt.Add(new QuadTree(new Vector3(-r, r, r), new Vector3(-r, r, -r), new Vector3(r, r, r), new Vector3(r, r, -r), Mathf.Sqrt(3 * r)));
        //D
        qt.Add(new QuadTree(new Vector3(r, -r, r), new Vector3(r, -r, -r), new Vector3(-r, -r, r), new Vector3(-r, -r, -r), Mathf.Sqrt(3 * r)));

        qt[0].setNeighbors(qt[2], qt[3], qt[4], qt[5]);
        qt[1].setNeighbors(qt[3], qt[2], qt[4], qt[5]);
        qt[2].setNeighbors(qt[1], qt[0], qt[4], qt[5]);
        qt[3].setNeighbors(qt[0], qt[1], qt[4], qt[5]);
        qt[4].setNeighbors(qt[2], qt[3], qt[1], qt[0]);
        qt[5].setNeighbors(qt[2], qt[3], qt[0], qt[1]);
        //qt.Subdivide(4);
        for (int i = 0; i < qt.Count; i++)
        {
            qt[i].material = material;
            qt[i].GenerateMesh();
        }
    }

    public void Subd()
    {
        for (int i = 0; i < qt.Count; i++)
        {
            qt[i].SubdivideAllLast();
            qt[i].GenerateMesh();
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < qt.Count; i++)
        {
            bool res = qt[i].SubdivideClosest(Camera.main.transform.position, 6, 10f);
            if(res)
                qt[i].GenerateMesh();
        }
    }
}
