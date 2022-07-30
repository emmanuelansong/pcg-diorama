using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GridGen : MonoBehaviour
{
    [System.Serializable]
    public class Room
    {
        public Room(Transform room, List<Transform> neighbours = null, Vector3 dir = new Vector3(), float area = 0)
        {
            this.room = room;
            this.neighbours = neighbours;
            this.dir = dir;
            this.area = area;
        }
        public Transform room;
        public List<Transform> neighbours;
        public Vector3 dir;
        public float area;

        
    }

   
    // Start is called before the first frame update
    int count;
    public int minSize, maxSize;

    public float radius;
    public GameObject cube;
    public GameObject tower;
    public List<Room> rooms;
    public List<Transform> roomsTransform;
    public float percentageOfMainRooms = 0.3f;

    void Start()
    {
        count = Random.Range(3, 6);
        //CreateRoomsInRadius(minSize,maxSize,radius);
        
        CreateRoomsInRadius();
    }
    private void Update()
    {
        foreach (Room room in rooms)
        {
            GetNearbyObjectsAndMove(room);
            
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            RotateTower();
        }
    }
    void CreateRoomsInRadius()
    {
        maxSize++;
       
        for (int i = 0; i < count; i++)
        {
            //Vector3 pos = new Vector3(RandomPointInRadius(radius).x, 0, RandomPointInRadius(radius).z);
            Vector3 pos = new Vector3(Random.insideUnitSphere.x,0, Random.insideUnitSphere.z);
            float randomScaleX = Random.Range(minSize, maxSize);
            float randomScaleZ = Random.Range(minSize, maxSize+1);
            float randomScaleY = Random.Range(minSize, maxSize);

            GameObject block = Instantiate(cube, pos, Quaternion.identity);

            block.transform.localScale = new Vector3(randomScaleX,i+1,randomScaleZ);
                
            block.transform.SetParent(this.transform);

            rooms.Add(new Room(block.transform));
        }
        
        
        
    }
    public void GetNearbyObjectsAndMove(Room room)
    {
        float boxX = room.room.GetComponentInChildren<MeshRenderer>().bounds.size.x;
        float boxY = room.room.GetComponentInChildren<MeshRenderer>().bounds.size.y;
        float boxZ = room.room.GetComponentInChildren<MeshRenderer>().bounds.size.z;

        List<Collider> colliders = Physics.OverlapBox(room.room.position, new Vector3(boxX, boxY, boxZ)/2f).ToList();

        room.neighbours = new List<Transform>();
        foreach(Collider c in colliders)
        {
            if(c != room.room.transform.GetComponent<Collider>())
            {
                room.neighbours.Add(c.transform);
                
                if(room.neighbours.Count > 0)
                {
                    
                    room.dir = c.transform.position - room.room.position;
                    room.dir.y = 0;
                    room.room.Translate(room.dir * -.1f);
                    break;
                }
            }
            
        }
        GetEmptySpace(room);
    }
   

    public void GetEmptySpace(Room room)
    {
        float boxX = room.room.GetComponentInChildren<MeshRenderer>().bounds.size.x;
        float boxZ = room.room.GetComponentInChildren<MeshRenderer>().bounds.size.z;

        int randomTowerAmount = Random.Range(0, 4);

        Vector3 corner = new Vector3(room.room.position.x + boxX, 0, room.room.position.z + boxZ);
        
        for(int i = 0; i < randomTowerAmount; i++)
        {
            Instantiate(tower, corner, transform.rotation);
        }

    }

    void RotateTower()
    {
        for(int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).transform.Rotate(Vector3.right, 90);
    }

}

//pick a random point on grid
//set random size of cuboid

//pick up down left right
//set random size of cuboid

//if cuboid = 1x1 == tower
//else tower connector