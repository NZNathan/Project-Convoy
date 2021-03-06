﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {

    public static GridManager instance;

    //Obstacle Variables
    private LayerMask obstacleMask;

    //Physical grid Variables
    public GameObject gridSquare;
    public int tileSize = 2;

    //Virtual Grid Variables
    private Node[,] grid;
    public float nodeRadius; //??
    public Vector2 gridWorldSize; //??

    //Size of the Grid
    [Range(1, 99)]
    public int gridWidth = 1;
    [Range(1, 99)]
    public int gridHeight = 1;
    public int maxGridSize;

    private void Start()
    {
        #region Singleton
        if(instance != null)
        {
            Destroy(instance.gameObject);
            
        }

        instance = this;
        #endregion

        //Mask for raycast on mouse click
        obstacleMask = LayerMask.GetMask("Selectable");

        buildGrid();
    }

    /// <summary>
    /// Creates a grid starting at (1,0,1) at the size specified
    /// </summary>
    public void buildGrid()
    {
        //Delete old grid
        deleteGrid();

        //Get layer mask in case it isn't already defined
        obstacleMask = LayerMask.GetMask("Selectable");

        //Create new 2d array of nodes
        grid = new Node[gridWidth, gridHeight];
        maxGridSize = gridWidth * gridHeight;

        //Create new grid
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                //Set up node grid
                Vector3 worldPoint = new Vector3(1 + tileSize * x, 0.01f, 1 + tileSize * y);
                bool walkable = !(Physics.CheckSphere(worldPoint, tileSize / 2, obstacleMask));
                grid[x, y] = new Node(walkable, worldPoint, x, y);

                //Set up physical grid object to visualize grid in game
                //GameObject square = Instantiate(gridSquare, worldPoint, Quaternion.identity, transform); //0.01f to stick above road
                //square.transform.localScale = new Vector3(tileSize, 1, tileSize);
            }
        }
    }

    public void deleteGrid()
    {
        //Get all gridsquare objects 
        Transform[] gridSquares = GetComponentsInChildren<Transform>();

        foreach (Transform t in gridSquares)
        {
            //Check for null as destroying objects in the array as we go
            if(t != null && t.tag == "GridSquare")
                DestroyImmediate(t.gameObject);
        }
    }

    /// <summary>
    /// Returns the int that the right edge of the board (any point beyond is off the grid)
    /// </summary>
    /// <returns></returns>
    public int xBound()
    {
        return gridWidth * tileSize;
    }

    /// <summary>
    /// Returns the int that the top edge of the board (any point beyond is off the grid)
    /// </summary>
    /// <returns></returns>
    public int zBound()
    {
        return gridHeight * tileSize;
    }

    /// <summary>
    /// Takes a point and rounds it to the nearest odd number
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public float snapToGrid(float point)
    {
        point = point - (point %tileSize);

        if (Mathf.Floor(point) % tileSize == 1)
            return Mathf.Floor(point);

        //If point alreadyon the grid return it as a int
        return Mathf.Ceil(point) + 1;
    }

    public Vector3 snapToGrid(Vector3 point)
    {
        Vector3 gridPoint = new Vector3(GridManager.instance.snapToGrid(point.x), 0f, GridManager.instance.snapToGrid(point.z));

        return gridPoint;
    }

    /// <summary>
    /// Gets the node at the world point spceified
    /// </summary>
    /// <param name="worldPoint"></param>
    /// <returns></returns>
    public Node NodeFromWorldPoint(Vector3 worldPoint)
    {
        int xIndex = Mathf.FloorToInt(worldPoint.x / tileSize);
        int yIndex = Mathf.FloorToInt(worldPoint.z / tileSize);

        return grid[xIndex, yIndex];
    }

    public void setWalkable(Vector3 worldPoint, bool value)
    {
        Node node = NodeFromWorldPoint(worldPoint);

        node.walkable = value;
    }

    /// <summary>
    /// Gets all neighbours to the node in a 3x3 square
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public List<Node> getNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        //Search in a 3x3 square to get 4 neighbours of the node
        for(int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                //Skip middle node as this is the passed in node
                if ((x == 0 && y == 0) || ((x == -1 || x == 1) && y != 0) )
                    continue;

                int checkX = node.x + x;
                int checkY = node.y + y;

                //If node is within the grid bounds, add it to the list of neighbours
                if(checkX >= 0 && checkX < gridWidth && checkY >= 0 && checkY < gridHeight)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    public Vector3[] path;
    private void OnDrawGizmos()
    {
        if(grid != null)
        {
            foreach (Node n in grid)
            {
                //If walkabel set color to white, else set color to red
                Gizmos.color = (n.walkable) ? Color.white : Color.red;

                //Draw gizmo cube slightly smaller to get a gap between each grid point
                Gizmos.DrawCube(n.worldPos, new Vector3(tileSize - 0.1f, 0.1f, tileSize - 0.1f) );
            }
        }

        if (path != null)
        {
            foreach (Vector3 pos in path)
            {

               Gizmos.color = Color.black;

                //Draw gizmo cube slightly smaller to get a gap between each grid point
                Gizmos.DrawCube(pos, new Vector3(tileSize - 0.1f, 0.1f, tileSize - 0.1f));
            }
        }
    }
}
