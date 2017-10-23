using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class Pathfinding : MonoBehaviour {

    public static Vector3[] FindPath(Vector3 startPos, Vector3 endPos)
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
                sw.Stop();
                UnityEngine.Debug.Log("Path found " + sw.ElapsedMilliseconds + "ms");
                GridManager.instance.path = retracePath(startNode, endNode);
                return retracePath(startNode, endNode);
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

        //No path
        return null;
    }

    private static Vector3[] retracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Add(startNode);
        Vector3[] waypoints = simplifyPath(path);

        return waypoints;
    }

    private static Vector3[] simplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        
        //Add end point
        waypoints.Add(path[0].worldPos);
        Vector2 directionOld = new Vector2(path[0].x, path[0].y);

        for (int i = 1; i < path.Count-1; i++)
        {
            Vector2 directionCurrent = new Vector2(path[i].x, path[i].y);
            Vector2 directionNext = new Vector2(path[i + 1].x, path[i + 1].y);

            if (directionNext.x != directionOld.x && directionNext.y != directionOld.y)
            {
                waypoints.Add(path[i].worldPos);
            }

            directionOld = directionCurrent;
        }

        waypoints.Reverse();
        return waypoints.ToArray();
    }

}
