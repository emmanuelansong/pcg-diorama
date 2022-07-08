
using System.Collections.Generic;
using UnityEngine;

public class ObjectShuffler : MonoBehaviour
{
    
    public void ShuffleList(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int r = i + Random.Range(0, list.Count - i);

            var K = list[r];

            list[r] = list[i];
            list[i] = K;
        }
    }
}
