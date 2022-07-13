using UnityEngine;
using System.Collections;
using DungeonGen;
using System.Collections.Generic;

public class ScaleObject : MonoBehaviour
{
    public GameObject obj;

    private HashSet<Point> points;
    GridGen gg;
    private void Awake()
    {
        gg = GameObject.Find("gridgen").GetComponent<GridGen>();
        points = new HashSet<Point>();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            BuildObject();
            RegenerateLines();
        }
    }
    public void BuildObject()
    {
        

        foreach (GridGen.Room room in gg.rooms)
        {
            points.Add(new Point(Mathf.RoundToInt(room.room.position.x), Mathf.RoundToInt(room.room.position.y)));
        }
        //RegenerateLines();
    }


    void RegenerateLines()
    {

        var triangles = BowyerWatson.Triangulate(points);

        var graph = new HashSet<Edge>();
        foreach (var triangle in triangles)
            graph.UnionWith(triangle.edges);

        var tree = Kruskal.MinimumSpanningTree(graph);

        foreach (var edge in tree)
        {
            Vector3 p1 = new Vector3(edge.a.x, edge.a.y);
            Vector3 p2 = new Vector3(edge.b.x, edge.b.y);

            float distance = Vector3.Distance(p1, p2);
            Vector3 midpoint = (p1 + p2) / 2;
            Vector3 dir = (p2 - p1);
            
            int randomScaleX = Random.Range(gg.minSize, gg.maxSize);
            int randomScaleZ = Random.Range(gg.minSize, gg.maxSize);

            GameObject go = Instantiate(obj, midpoint, Quaternion.Euler(dir));
            go.transform.localScale = new Vector3(randomScaleX, distance / 2f, randomScaleZ);
            go.transform.up = dir;


        }
    }
}