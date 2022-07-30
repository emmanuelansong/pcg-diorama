using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuilder : MonoBehaviour
{
    [System.Serializable]
    public class Base
    {
        public List<GameObject> roundBases;
        public List<GameObject> squareBases;

        public bool isRound;

    }
    [System.Serializable]
    public class Tower
    {
        public List<GameObject> roundTower;
        public List<GameObject> squareTower;

        public bool isRound;

    }
    [System.Serializable]
    public class Top
    {
        public List<GameObject> roundTops;
        public List<GameObject> squareTops;

        public bool isRound;

    }
    [System.Serializable]
    public class Roof
    {
        public List<GameObject> roundRoofs;
        public List<GameObject> squareRoofs;

        public bool isRound;

    }


    //Is it a round tower or not
    
    Quaternion randomY;
    int chooseBase;

    public Base bases;
    public Top tops;
    public Roof roofs;
    // Start is called before the first frame update
    void Start()
    {
        chooseBase = Random.Range(0, 2);

        if (chooseBase == 0)
        {
            bases.isRound = true;            
            tops.isRound = true;
            roofs.isRound = true;

        }
        else
        {
            return;
        }

        randomY = Quaternion.Euler(0, Random.rotation.y, 0);
        //CreateTower();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*GameObject InstantiateBase(Base bases)
    {
        int chooser = Random.Range(0, bases.Count);
        
        
        GameObject towerBase = Instantiate(bases[chooser], transform.position, randomY);

        return towerBase;
    }

    GameObject InstantiateTop(List<GameObject> tops,GameObject Base)
    {
        int chooser = Random.Range(0, tops.Count);
        
        if (chooseBase == 0)
            tops = squareTops;
        else
            tops = roundTops;
        Base = InstantiateBase(bases);
        float topHeight = GetHeight(Base);

        Vector3 pos = new Vector3(transform.position.x, topHeight, transform.position.z);

        GameObject towerTop = Instantiate(tops[chooser], pos, randomY);

        return towerTop;
    }

    GameObject InstantiateRoof(GameObject Top)
    {
        int chooser = Random.Range(0, roofs.Count);
        Top = InstantiateTop(InstantiateBase());
        float topHeight = GetHeight(Top);

        Vector3 pos = new Vector3(transform.position.x, topHeight, transform.position.z);

        GameObject towerRoof = Instantiate(roofs[chooser], pos, randomY);

        return towerRoof;
    }

    void CreateTower()
    {
        InstantiateBase();
        InstantiateTop(InstantiateBase());
        InstantiateRoof(InstantiateTop(InstantiateBase()));
    }
    float GetHeight(GameObject go)
    {
        float sizeY = go.GetComponentInChildren<MeshRenderer>().bounds.size.y;
        return sizeY;
    }*/

}
