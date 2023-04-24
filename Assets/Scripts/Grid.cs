using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class Grid : MonoBehaviour
{
    public SpriteRenderer spritePrefab;
    public float spriteSize = 1.0f;
    public Vector2Int startPoint = new Vector2Int(0, 2);
    public Vector2Int endPoint = new Vector2Int(9, 7);
    public int[,] map = new int[20, 20];
    public int[,] obstacles = new int[,]
        { {4, 4}, {5, 4}, {6, 4}, {7, 4}, {8, 4}};

    private void OnEnable()
    {
        LayoutGrid();
    }

    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            foreach (Transform child in gameObject.transform)
            {
                Destroy(child.gameObject);
            }
            LayoutGrid();
        }
    }

    private void LayoutGrid()
    {
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                // Check if the current map element matches any obstacle element
                for (int k = 0; k < obstacles.GetLength(0); k++)
                {
                    if (i == obstacles[k, 0] && j == obstacles[k, 1])
                    {
                        // Set the map element to 1 if it matches an obstacle element
                        map[i, j] = 1;
                    }
                }
            }
        }

        List<Node> path = Pathfinding.FindPath(map, startPoint.x, startPoint.y, endPoint.x, endPoint.y);

        bool[,] visited = new bool[map.GetLength(0), map.GetLength(1)];

        // Highlight the cells on the grid that are part of the path
        if (path != null)
        {
            // Loop through each obstacle
            for (int i = 0; i < obstacles.GetLength(0); i++)
            {
                // Get the x and y coordinates of the current obstacle
                int x = obstacles[i, 0];
                int y = obstacles[i, 1];
                // Set the corresponding node to black
                InstantiateSprite(x, y, Color.black);
                // Mark the corresponding position as visited
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

        // Method to instantiate a sprite at the given x and y coordinates with the given color
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
}
