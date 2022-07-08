using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGen : MonoBehaviour
{
    public List<Transform> corners;
    public List<GameObject> basePrefabs;
    public List<GameObject> columnPrefabs;

    public GameObject wellPrefab;
    PillarBuilder builder;

    private int basePicker;
    int columnPicker;

    public int numberOfLevels;

    void Start()
    {
        builder = GetComponent<PillarBuilder>();

        basePicker = Random.Range(0, basePrefabs.Count);
        columnPicker = Random.Range(0, columnPrefabs.Count);

        numberOfLevels = Random.Range(1, 6);

        BuildTemple();

        
    }

    void BuildTemple()
    {
        var tempColumn = columnPrefabs[0];
        float baseHeight = basePrefabs[basePicker].transform.GetComponent<Renderer>().bounds.size.y;
        float baseWidth = basePrefabs[basePicker].transform.GetComponent<Renderer>().bounds.size.x;
        float baseLength = basePrefabs[basePicker].transform.GetComponent<Renderer>().bounds.size.z;

        

        var roofHeight = tempColumn.transform.GetChild(0).GetComponent<Renderer>().bounds.size.y + (baseHeight / 2);
        //inital cylinder base
        if (basePrefabs[basePicker].GetComponentInChildren<MeshFilter>().sharedMesh.name == "Cylinder.001")
        {
            var basePrefab = Instantiate(basePrefabs[basePicker], transform.position, basePrefabs[basePicker].transform.rotation);
            basePrefab.transform.parent = transform;
           
            //float radius = Random.Range(4, (baseLength / 2) - 1);
            //int noOfSpawns = Random.Range(4, 9);

            //CheckForColumn_Circle(tempColumn, noOfSpawns, baseHeight, radius);
            TowerBuilder_Circle(tempColumn);
        }

        //inital cube base
        if (basePrefabs[basePicker].GetComponentInChildren<MeshFilter>().sharedMesh.name == "SquareBlock")
        {
            foreach (Transform child in transform)
            {
                corners.Add(child);
            }

            Transform c1, c2;
            c1 = corners[0]; c2 = corners[1];

            //var roofHeight = tempColumn.transform.GetChild(0).GetComponent<Renderer>().bounds.size.y + (baseHeight / 2);

            //instantiate base
            var basePrefab = Instantiate(basePrefabs[basePicker], transform.position, basePrefabs[basePicker].transform.rotation);
            basePrefab.transform.parent = transform;

            //first row of columns position
            c1.position = basePrefab.transform.position - new Vector3((baseWidth / 2) - 1, 0, (baseLength / 2) - 1);

            //second row of columns, other side
            c2.position = basePrefab.transform.position + new Vector3((baseWidth / 2) - 1, 0, (-baseLength / 2) + 1);

            TowerBuilder_Cube(tempColumn, c1, c2, roofHeight);
        }


    }

    //method for building multiple levels
    void TowerBuilder_Cube(GameObject tempColumn, Transform c1, Transform c2, float height)
    {
        //foreach level, starting from 1st floor
        for (int j = 0; j < numberOfLevels; j++)
        {
            var basePrefab = Instantiate(basePrefabs[basePicker], new Vector3(transform.position.x, (height * j), transform.position.z), basePrefabs[basePicker].transform.rotation);
            basePrefab.transform.parent = transform;

            for (int i = 0; i < 2; i++)
            {
                //instantiate on roof
                var column = Instantiate(tempColumn, new Vector3(c1.position.x, height * j, c1.position.z), transform.rotation);
                column.transform.position += new Vector3(0, 0, 3.5f * i);
                column.transform.parent = transform;

                column = Instantiate(tempColumn, new Vector3(c2.position.x, height * j, c2.position.z), transform.rotation);
                column.transform.position += new Vector3(0, 0, 3.5f * i);
                column.transform.parent = transform;
            }
        }
        var roofHeight = (height * numberOfLevels);
        var roofPrefab = Instantiate(basePrefabs[basePicker], new Vector3(transform.position.x, roofHeight, transform.position.z), basePrefabs[basePicker].transform.rotation);
        roofPrefab.transform.parent = transform;
        roofPrefab.name = "RoofPrefab";

        
        
    }

    void TowerBuilder_Circle(GameObject tempColumn)
    {
        var height = tempColumn.transform.GetChild(0).GetComponent<Renderer>().bounds.size.y;
        int noOfObjects = 3;
        float radius = 2;
        //foreach level, starting from 1st floor
        numberOfLevels = 3;
        for (int j = 0; j < numberOfLevels; j++)
        {
            var basePrefab = Instantiate(basePrefabs[basePicker], new Vector3(transform.position.x, (height * j), transform.position.z), basePrefabs[basePicker].transform.rotation);
            basePrefab.transform.parent = transform;

            for (int i = 0; i < noOfObjects; i++)
            {
                float angle = i * Mathf.PI * 2 / noOfObjects;
                float x = Mathf.Cos(angle) * radius;
                float z = Mathf.Sin(angle) * radius;
                Vector3 pos = basePrefab.transform.position + new Vector3(x, height, z);
                float angleDeg = angle * Mathf.Rad2Deg;
                Quaternion rot = Quaternion.Euler(0, angleDeg, 0);

                var column = Instantiate(tempColumn, pos, rot);

                //column.transform.position += new Vector3(0, 0, 0 * i);
                if(j == 0)
                {
                    column.transform.position = new Vector3(column.transform.position.x, 0, column.transform.position.z);
                }
                column.transform.parent = transform;
            }


        }
        var roofHeight = (height * numberOfLevels);
        var roofPrefab = Instantiate(basePrefabs[basePicker], new Vector3(transform.position.x, roofHeight, transform.position.z), basePrefabs[basePicker].transform.rotation);

        //var roofPrefab = Instantiate(basePrefabs[basePicker], new Vector3(basePrefab.transform.position.x, roofHeight + baseHeight, basePrefab.transform.position.z), transform.rotation);
        roofPrefab.transform.parent = transform;


    }
    void CheckForColumn_Circle(GameObject tempColumn, int noOfObjects, float height, float radius)
    {
        if (basePrefabs[basePicker].GetComponentInChildren<MeshFilter>().sharedMesh.name == "Cylinder")
        {
            //column instantiation for c1
            for (int i = 0; i < noOfObjects; i++)
            {
                builder.ShuffleList(columnPrefabs);

                float angle = i * Mathf.PI * 2 / noOfObjects;
                float x = Mathf.Cos(angle) * radius;
                float z = Mathf.Sin(angle) * radius;
                Vector3 pos = transform.position + new Vector3(x, height, z);
                float angleDeg = angle * Mathf.Rad2Deg;
                Quaternion rot = Quaternion.Euler(0, angleDeg, 0);

                //temporary index to check for certain gameobject
                var tempIndex = Random.Range(0, i);

                if (tempIndex == 0 && columnPrefabs[0] != tempColumn)
                {
                    //if gameobject isnt included, include it.
                    var column = Instantiate(tempColumn, pos, rot);
                    //column.transform.position += new Vector3(z, height, z);
                    column.transform.parent = transform;
                }

                else
                {
                    //otherwise leave as is
                    var column = Instantiate(columnPrefabs[columnPicker], pos, rot);
                    //column.transform.position += new Vector3(x, height, z);
                    column.transform.parent = transform;
                }
            }
        }

    
    }

}
