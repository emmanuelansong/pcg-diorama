using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleGen : MonoBehaviour
{
    [System.Serializable]
    public class Node
    {
        public Node(Transform node, bool active = false)
        {
            this.node = node;
            this.active = active;

        }
        public Transform node;

        public bool active;


    }
    [System.Serializable]
    public class Room
    {
        public Room(Transform room, List<Node> nodes = null, float area = 0)
        {
            this.room = room;
            this.nodes = nodes;
            this.area = area;
            
        }
        public Transform room;
        public List<Node> nodes;
        public float area;


    }
    public int count;
    public int minSize, maxSize;

    public float radius;
    public GameObject cube;
    public GameObject node;
    public List<Room> rooms;
    // Start is called before the first frame update
    void Start()
    {
        rooms = new List<Room>();
        CreateRoom();
        foreach(Room room in rooms)
        {
            CreateSubRooms(room);
        }
    }
    void CreateRoom()
    {
        List<Node> nodes = new List<Node>();
        int randomScaleX = Random.Range(minSize, maxSize);
        int randomScaleZ = Random.Range(minSize, maxSize);
        int randomScaleY = Random.Range(minSize, maxSize);

        GameObject block = Instantiate(cube, transform.position, Quaternion.identity);

        block.transform.position = transform.position;
        block.transform.localScale = new Vector3(randomScaleX, randomScaleY, randomScaleZ);

        block.transform.SetParent(this.transform);

        //append block to new room (with transform, nodes, area)
        rooms.Add(new Room(block.transform, AddNodes(block, nodes), GetRoomArea(block)) );

    }

    List<Node> AddNodes(GameObject room, List<Node> nodes)
    {
        float boxX = room.GetComponent<MeshRenderer>().bounds.size.x / 2;
        float boxY = room.GetComponent<MeshRenderer>().bounds.size.y / 2;
        float boxZ = room.GetComponent<MeshRenderer>().bounds.size.z / 2;
        
        GameObject node1 = Instantiate(node, room.transform.position + new Vector3(boxX, 0, 0), Quaternion.identity);
        GameObject node2 = Instantiate(node, room.transform.position + new Vector3(-boxX, 0, 0), Quaternion.identity);
        GameObject node3 = Instantiate(node, room.transform.position + new Vector3(0, 0, boxZ), Quaternion.identity);
        GameObject node4 = Instantiate(node, room.transform.position + new Vector3(0, 0, -boxZ), Quaternion.identity);

        node1.transform.SetParent(room.transform);
        node2.transform.SetParent(room.transform);
        node3.transform.SetParent(room.transform);
        node4.transform.SetParent(room.transform);

        
        nodes.Add(new Node(node1.transform)); 
        nodes.Add(new Node(node2.transform)); 
        nodes.Add(new Node(node3.transform)); 
        nodes.Add(new Node(node4.transform));

        return nodes;

    }
    public float GetRoomArea(GameObject go)
    {
        float boxX = go.GetComponent<MeshRenderer>().bounds.size.x;
        float boxY = go.GetComponent<MeshRenderer>().bounds.size.y;
        float boxZ = go.GetComponent<MeshRenderer>().bounds.size.z;

        float area = boxX * boxY * boxZ;

        return area;

    }

    void SelectRandomNode(Room room)
    {
        List<Node> nodes = room.nodes;
        //nodes = rooms
        //random index
        int selector = Random.Range(0, nodes.Count);
        //get random node

        foreach(Node node in nodes)
        {
            if (node == nodes[selector])
            {
                node.active = true;
                Debug.Log("Active");
            }
            else
                node.active = false;
        }
    }

    void CreateSubRooms(Room room)
    {
        SelectRandomNode(room);
        List<Node> nodes = room.nodes;
        foreach (Node node in nodes)
        {
            if (node.active == true)
            {
                CreateRoom();
            }
        }
    }

}
