using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {

    public bool walkable;
    public Vector3 worldPos;

    //Grid points
    public int x;
    public int y;

    //Pathfinding costs
    public float gCost;
    public float hCost;

    public Node parent;

    public Node(bool walkable, Vector3 worldPos, int x, int y)
    {
        this.walkable = walkable;
        this.worldPos = worldPos;
        this.x = x;
        this.y = y;
    }

    public float fCost 
    {
        get {
            return gCost + hCost;
        }
    }
}
