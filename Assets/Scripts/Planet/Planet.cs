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
        qt.Add(new QuadTree(new Vector3(-r, r, r), new Vector3(r, r, r), new Vector3(-r, -r, r), new Vector3(r, -r, r), Mathf.Sqrt(3 * r)));
        qt.Add(new QuadTree(new Vector3(r, r, -r), new Vector3(-r, r, -r), new Vector3(r, -r, -r), new Vector3(-r, -r, -r), Mathf.Sqrt(3 * r)));
        qt.Add(new QuadTree(new Vector3(r, -r, -r), new Vector3(r, -r, r), new Vector3(r, r, -r), new Vector3(r, r, r), Mathf.Sqrt(3 * r)));
        qt.Add(new QuadTree(new Vector3(-r, r, -r), new Vector3(-r, r, r), new Vector3(-r, -r, -r), new Vector3(-r, -r, r), Mathf.Sqrt(3 * r)));

        qt.Add(new QuadTree(new Vector3(-r, r, r), new Vector3(-r, r, -r), new Vector3(r, r, r), new Vector3(r, r, -r), Mathf.Sqrt(3 * r)));
        qt.Add(new QuadTree(new Vector3(r, -r, r), new Vector3(r, -r, -r), new Vector3(-r, -r, r), new Vector3(-r, -r, -r), Mathf.Sqrt(3 * r)));
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
            qt[i].SubdivideLast();
            qt[i].GenerateMesh();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
