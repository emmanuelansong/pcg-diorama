using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GridGen : MonoBehaviour
{
    [System.Serializable]
    public class Agent
    {
        public Agent(Transform agent, List<Transform> neighbours = null, Vector3 dir = new Vector3())
        {
            this.agent = agent;
            this.neighbours = neighbours;
            this.dir = dir;
        }
        public Transform agent;
        public List<Transform> neighbours;
        public Vector3 dir;
    }

   
    // Start is called before the first frame update
    public int count;
    public int minSize, maxSize;

    public float radius;
    public GameObject cube;

    public List<Agent> agents;
    List<Transform> context;
    void Start()
    {

        //CreateRoomsInRadius(minSize,maxSize,radius);
        
        CreateRoomsInRadius();
        
        
    }
    private void Update()
    {
        foreach (Agent agent in agents)
        {
            GetNearbyObjects(agent);
        }
    }
    void CreateRoomsInRadius()
    {
        maxSize++;
       
        for (int i = 0; i < count; i++)
        {
            Vector3 pos = new Vector3(RandomPointInRadius(radius).x, 0, RandomPointInRadius(radius).z);

            int randomScaleX = Random.Range(minSize, maxSize);
            int randomScaleZ = Random.Range(minSize, maxSize);
            int randomScaleY = Random.Range(minSize, maxSize);

            GameObject block = Instantiate(cube, pos, Quaternion.identity);

            block.transform.localScale = new Vector3(randomScaleX, randomScaleY, randomScaleZ);
                
            block.transform.SetParent(this.transform);

            agents.Add(new Agent(block.transform));
        }
        
        
        
    }
    public void GetNearbyObjects(Agent agent)
    {
        float boxX = agent.agent.GetComponent<MeshRenderer>().bounds.size.x;
        float boxY = agent.agent.GetComponent<MeshRenderer>().bounds.size.y;
        float boxZ = agent.agent.GetComponent<MeshRenderer>().bounds.size.z;

        List<Collider> colliders = Physics.OverlapBox(agent.agent.position, new Vector3(boxX/2, boxY/2, boxZ/2)).ToList();

        agent.neighbours = new List<Transform>();
        foreach(Collider c in colliders)
        {
            if(c != agent.agent.transform.GetComponent<Collider>())
            {
                agent.neighbours.Add(c.transform);
                
                if (agent.neighbours.Count > 0)
                {
                    agent.dir = c.transform.position - agent.agent.position;
                    agent.agent.Translate(agent.dir * -.1f * Time.deltaTime);
                }
                
            }
            
        }
    }


Vector3 RandomPointInRadius(float radius)
    {
        Vector3 refPoint = transform.position;
        var sample = refPoint + Random.insideUnitSphere * radius;

        return sample;
    }

}

//pick a random point on grid
//set random size of cuboid

//pick up down left right
//set random size of cuboid

//if cuboid = 1x1 == tower
//else tower connector