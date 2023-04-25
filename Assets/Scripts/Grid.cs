using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class Grid : MonoBehaviour
{
    //public variables
    public SpriteRenderer spritePrefab;                     //prefab for the sprite
    public float spriteSize = 1.0f;                         //size of the sprite
    public Vector2Int startPoint = new Vector2Int(0, 0);    //start point coordinate
    public Vector2Int endPoint = new Vector2Int(5, 5);      //end point coordinate
    public int[,] map = new int[10, 10];                    //size of the map
    public int[,] obstacles = new int[,]                    //obstacles' coordinates
        { {1, 3}, {2, 3}, {3, 3}, {4, 3}, {5, 3},{6, 3}, {7, 3}, {8, 3}, {9, 3},
        {0, 5}, {1, 5}, {2, 5}, {3, 5}, {4, 5}, {5, 5},{6, 5}, {7, 5}, {8, 5} };

    //method called when the object is enabled
    private void OnEnable()
    {
        LayoutGrid();// Set up the grid
    }

    //method called when a value is changed in the Inspector
    private void OnValidate()
    {
        //check if the game is running
        if (Application.isPlaying)
        {
            //destroy all the child objects of the game object
            foreach (Transform child in gameObject.transform)
            {
                Destroy(child.gameObject);
            }
            LayoutGrid();//set up the grid
        }
    }

    //method to lay out the grid
    private void LayoutGrid()
    {
        ErrorChecking();//check for errors in the input

        //loop through each element in the map
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                //check if the current map element matches any obstacle element
                for (int k = 0; k < obstacles.GetLength(0); k++)
                {
                    if (i == obstacles[k, 0] && j == obstacles[k, 1])
                    {
                        //set the map element to 1 if it matches an obstacle element
                        map[i, j] = 1;
                    }
                }
            }
        }

        //find the path from the start point to the end point
        List<Node> path = Pathfinding.FindPath(map, startPoint.x, startPoint.y, endPoint.x, endPoint.y);

        //boolean for drawing to indicate whether or not a coordinate has been visited
        bool[,] visited = new bool[map.GetLength(0), map.GetLength(1)];

        //highlight the cells on the grid that are part of the path
        if (path != null)
        {
            //loop through each obstacle
            for (int i = 0; i < obstacles.GetLength(0); i++)
            {
                //get the x and y coordinates of the current obstacle
                int x = obstacles[i, 0];
                int y = obstacles[i, 1];
                //set the corresponding node to black
                InstantiateSprite(x, y, Color.black);
                //mark the corresponding position as visited
                visited[x, y] = true;
            }
            for (int i = 1; i < path.Count-1; i++)
            {
                Node node = path[i];
                InstantiateSprite(node.x, node.y, Color.grey);
                visited[node.x, node.y] = true;
            }
            InstantiateSprite(startPoint.x, startPoint.y, Color.green);
            InstantiateSprite(endPoint.x, endPoint.y, Color.red);
        }

        //method to instantiate a sprite at the given x and y coordinates with the given color
        void InstantiateSprite(int x, int y, Color color)
        {
            if (!visited[x, y])
            {
                Vector2 position = new Vector2(x * spriteSize, y * spriteSize);
                SpriteRenderer sprite = Instantiate(spritePrefab, position, Quaternion.identity, transform);
                sprite.transform.parent = transform;
                sprite.color = color;
                visited[x, y] = true;
            }
        }
    }

    private void ErrorChecking()
    {
        //check if the start point or end point is outside the map boundaries
        if (startPoint.x >= map.GetLength(0) || startPoint.x < 0 ||
            startPoint.y >= map.GetLength(1) || startPoint.y < 0)
        {
            Debug.LogError("Start point is outside the map boundaries!");
            return;
        }

        //check if the end point is outside the map boundaries
        if (endPoint.x >= map.GetLength(0) || endPoint.x < 0 ||
            endPoint.y >= map.GetLength(1) || endPoint.y < 0)
        {
            Debug.LogError("End point is outside the map boundaries!");
            return;
        }
    }
}
