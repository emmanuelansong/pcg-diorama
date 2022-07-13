using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Node(Transform node, bool active = false)
    {
        this.node = node;
        this.active = active;

    }
    public Transform node;

    public bool active;


}
