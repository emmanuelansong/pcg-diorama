using UnityEngine;
using System.Collections;
using DungeonGen;
using System.Collections.Generic;


public class ScaleObject : MonoBehaviour
{
    public GameObject torii;
    private HashSet<Point> points;

    public List<GameObject> structures;
    private void Awake()
    {
        //gg = GameObject.Find("gridgen").GetComponent<GridGen>();
        points = new HashSet<Point>();
    }

    private void Start()
    {
        GeneratePoints();
        RegenerateLines();
    }

    public void GeneratePoints()
    {
        GenerationAlgorithms ga = new GenerationAlgorithms();

        List<Vector3> temp = new List<Vector3>(ga.GeneratePoints(100, new Vector3(1000, 500, 1000)));

        foreach (Vector3 point in temp)
        {
            
            if (Physics.Raycast(point, Vector3.down, out RaycastHit hit))
            {
                if (hit.point.y <= 0)
                {

                    //CheckFlatTerrain cft = GameObject.Find("StructureGen").GetComponent<CheckFlatTerrain>();
                    int random = Random.Range(0, structures.Count);
                    Vector3 randomRotY = new Vector3(transform.rotation.x, Random.rotation.y,transform.rotation.z);
                    points.Add(new Point(Mathf.RoundToInt(point.x), Mathf.RoundToInt(point.z)));
                    GameObject go = Instantiate(structures[random], hit.point, transform.rotation);
                    go.transform.Rotate(randomRotY);
                }
            }
        }
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
            Vector3 p1 = new Vector3(edge.a.x,0 ,edge.a.y);
            Vector3 p2 = new Vector3(edge.b.x,0 ,edge.b.y);

            float distance = Vector3.Distance(p1, p2);
            Vector3 midpoint = (p1 + p2) / 2;
            Vector3 dir = (p2 - p1);
            //int segmentsToCreate = Mathf.RoundToInt(Vector3.Distance(p1, p2) / 0.5f);

            Vector3 normalVec = dir.normalized;
            float vecMag = dir.magnitude;

            float spacing = vecMag / 10f;

            List<Vector3> bluePositions = new List<Vector3>();

            for(int i = 0; i < 10; i++)
            {
                    Vector3 bluePos = p1 + (normalVec * spacing * i);

                    bluePositions.Add(bluePos);
            }
            
            foreach(Vector3 pos in bluePositions)
            { 
                //Instantiate the object
                GameObject go = Instantiate(torii, pos, Quaternion.Euler(dir));
                go.transform.forward = dir;
                go.transform.parent = transform;
                go.transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(0,360), 0));
                if(Physics.OverlapSphere(go.transform.position, 10).Length > 0)
                {
                    Destroy(go);
                }
            }
            
        }
    }
}