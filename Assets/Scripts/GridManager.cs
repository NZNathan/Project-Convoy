using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {

    public static GridManager instance;

    public GameObject gridSquare;
    public int tileSize = 2;

    //Size of the Grid
    [Range(1, 99)]
    public int gridWidth = 1;
    [Range(1, 99)]
    public int gridHeight = 1;

    private void Start()
    {
        #region Singleton
        if(instance != null)
        {
            Destroy(instance.gameObject);
            
        }

        instance = this;
    #endregion

    }

    /// <summary>
    /// Creates a grid starting at (1,0,1) at the size specified
    /// </summary>
    public void buildGrid()
    {
        //Delete old grid
        deleteGrid();

        //Create new grid
        for (int col = 0; col < gridWidth; col++)
        {
            for (int row = 0; row < gridHeight; row++)
            {
                GameObject square = Instantiate(gridSquare, new Vector3(1 + tileSize * col, 0.01f, 1 + tileSize * row), Quaternion.identity, transform); //0.01f to stick above road
                square.transform.localScale = new Vector3(tileSize, 1, tileSize);
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
}
