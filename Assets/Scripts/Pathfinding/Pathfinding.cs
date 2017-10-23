using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class Pathfinding : MonoBehaviour {

    public Transform seeker;
    public Transform target;

    private void Update()
    {
        if(Input.GetButtonDown("Jump"))
            FindPath(seeker.position, target.position);
    }



    private void FindPath(Vector3 startPos, Vector3 endPos)
    {
        //DEBUG
        Stopwatch sw = new Stopwatch();
        sw.Start();
        Node startNode = GridManager.instance.NodeFromWorldPoint(startPos);
        Node endNode = GridManager.instance.NodeFromWorldPoint(endPos);

        Heap<Node> openSet = new Heap<Node>(GridManager.instance.maxGridSize);
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        while(openSet.Count > 0)
        {
            Node currentNode = openSet.RemoveFirst();
            closedSet.Add(currentNode);

            //If at goal, then stop pathfinding
            if (currentNode == endNode)
            {
                retracePath(startNode, endNode);
                sw.Stop();
                UnityEngine.Debug.Log("Path found " + sw.ElapsedMilliseconds + "ms");
                return;
            }      

            foreach (Node neighbour in GridManager.instance.getNeighbours(currentNode))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                    continue;

                //Calculate distance to neighbour
                float newMovementCostToNeightbour = currentNode.gCost + Vector3.Distance(currentNode.worldPos, neighbour.worldPos);

                //Check if new cost is better than neighbours
                if(newMovementCostToNeightbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeightbour;
                    neighbour.hCost = Vector3.Distance(neighbour.worldPos, endNode.worldPos);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                    else
                        openSet.UpdateItem(neighbour);
                }
            }
        }
    }

    private void retracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        GridManager.instance.path = path;
    }

    private int getDistance(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.x - nodeB.x);
        int distY = Mathf.Abs(nodeA.y - nodeB.y);

        if (distX > distY)
            return 14 * distY + 10 * (distX - distY);
        else
            return 14 * distX + 10 * (distY - distX);
    }
}
