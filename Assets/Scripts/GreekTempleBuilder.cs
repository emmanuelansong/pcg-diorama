 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreekTempleBuilder : MonoBehaviour
{
    public List<GameObject> basePrefabs;
    public List<GameObject> roofPrefabs;
    public List<GameObject> columnPrefabs;
    public int basePicker;
    public int roofPicker;
    // Start is called before the first frame update
    void Start()
    {
        basePicker = Random.Range(0, basePrefabs.Count);
        roofPicker = Random.Range(0, roofPrefabs.Count);
        var tempColumn = columnPrefabs[0];
        float baseHeight = basePrefabs[basePicker].transform.GetChild(1).GetComponent<Renderer>().bounds.size.y;
        float baseWidth = basePrefabs[basePicker].transform.GetChild(2).GetComponent<Renderer>().bounds.size.x;
        float baseLength = basePrefabs[basePicker].transform.GetChild(2).GetComponent<Renderer>().bounds.size.z;

        var roofHeight = tempColumn.transform.position.y + tempColumn.transform.GetChild(0).GetComponent<Renderer>().bounds.size.y;
        if (basePrefabs[basePicker].GetComponentInChildren<MeshFilter>().sharedMesh.name == "Cube")
        {

            baseHeight *= 2;


            Vector3 c1, c2;

            Vector3 c3, c4;


            //var roofHeight = tempColumn.transform.GetChild(0).GetComponent<Renderer>().bounds.size.y + (baseHeight / 2);

            //instantiate base
            var basePrefab = Instantiate(basePrefabs[basePicker], transform.position, transform.rotation);
            basePrefab.transform.parent = transform;

            //instantiate  roof
            var roofPrefab = Instantiate(roofPrefabs[roofPicker], new Vector3(basePrefab.transform.position.x, roofHeight, basePrefab.transform.position.z), transform.rotation);
            roofPrefab.transform.parent = transform;

            //first row of columns position
            c1 = basePrefab.transform.position - new Vector3((baseWidth / 2) - 1, 0, (baseLength / 2) - 1);

            //second row of columns, other side
            c2 = basePrefab.transform.position + new Vector3((baseWidth / 2) - 1, 0, (-baseLength / 2) + 1);

            c3 = basePrefab.transform.position - new Vector3((baseWidth / 2) - 1, 0, (baseLength / 2) - 1);

            //second row of columns, other side
            c4 = basePrefab.transform.position + new Vector3((-baseWidth / 2) - 1, 0, (baseLength / 2) + 1);
            Debug.Log(c4 +" c3:" +c3);

            for (int i = 0; i < 4; i++)
            {
                var column = Instantiate(tempColumn, new Vector3(c1.x, 0, c1.z), transform.rotation);
                column.transform.position += new Vector3(0, 0, 3 * i);
                column.transform.parent = transform;

                column = Instantiate(tempColumn, new Vector3(c2.x, 0, c2.z), transform.rotation);
                column.transform.position += new Vector3(0, 0, 3 * i);
                column.transform.parent = transform;
            }


            for (int i = 1; i < 7; i++)
            {
                var column = Instantiate(tempColumn, new Vector3(c3.x, 0, c3.z), transform.rotation);
                column.transform.position += new Vector3(5 * i, 0,0);

                column = Instantiate(tempColumn, new Vector3(c4.x, 0, c4.z), transform.rotation);
                column.transform.position += new Vector3(5 * i, 0, 0);
            }

        }
    }

}
