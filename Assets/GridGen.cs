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
    public int count;
    public int minSize, maxSize;

    public float radius;
    public GameObject cube;

    public List<Room> rooms;
    public List<Transform> roomsTransform;
    public float percentageOfMainRooms = 0.3f;

    void Start()
    {

        //CreateRoomsInRadius(minSize,maxSize,radius);
        
        CreateRoomsInRadius();
    }
    private void Update()
    {
        foreach (Room room in rooms)
        {
            GetNearbyObjectsAndMove(room);
            
        }
    }
    void CreateRoomsInRadius()
    {
        maxSize++;
       
        for (int i = 0; i < count; i++)
        {
            //Vector3 pos = new Vector3(RandomPointInRadius(radius).x, 0, RandomPointInRadius(radius).z);
            Vector2 pos = new Vector2(Random.insideUnitCircle.x, Random.insideUnitCircle.y);
            int randomScaleX = Random.Range(minSize, maxSize);
            int randomScaleZ = Random.Range(minSize, maxSize);
            int randomScaleY = Random.Range(minSize, maxSize);

            GameObject block = Instantiate(cube, pos, Quaternion.identity);

            block.transform.localScale = new Vector3(randomScaleX, randomScaleY, 1);
                
            block.transform.SetParent(this.transform);

            rooms.Add(new Room(block.transform));
        }
        
        
        
    }
    public void GetNearbyObjectsAndMove(Room room)
    {
        float boxX = room.room.GetComponent<MeshRenderer>().bounds.size.x;
        float boxY = room.room.GetComponent<MeshRenderer>().bounds.size.y;
        float boxZ = room.room.GetComponent<MeshRenderer>().bounds.size.z;

        List<Collider> colliders = Physics.OverlapBox(room.room.position, new Vector3(boxX/2, boxY/2, boxZ/2)).ToList();

        room.neighbours = new List<Transform>();
        foreach(Collider c in colliders)
        {
            if(c != room.room.transform.GetComponent<Collider>())
            {
                room.neighbours.Add(c.transform);
                
                if(room.neighbours.Count > 0)
                {
                    room.dir = c.transform.position - room.room.position;
                    room.room.Translate(room.dir * -.1f);
                    break;
                }
            }
            
        }
    }
   

   /* public void IdentifyMainRooms()
    {
        roomsTransform = new List<Transform>();
        rooms.Sort((Room a, Room b) =>
        {
            return GetRoomArea(a).CompareTo(GetRoomArea(b));
        });

    }*/


}

//pick a random point on grid
//set random size of cuboid

//pick up down left right
//set random size of cuboid

//if cuboid = 1x1 == tower
//else tower connector