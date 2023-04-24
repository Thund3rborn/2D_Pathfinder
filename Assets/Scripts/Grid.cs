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
        map[5, 4] = 1;
        map[6, 4] = 1;
        map[7, 4] = 1;
        map[8, 4] = 1;

        List<Node> path = Pathfinding.FindPath(map, startPoint.x, startPoint.y, endPoint.x, endPoint.y);


        // Highlight the cells on the grid that are part of the path
        if (path != null)
        {
            foreach (Node node in path)
            {
                Vector2 position = new Vector2(node.x * spriteSize, node.y * spriteSize);
                SpriteRenderer sprite = Instantiate(spritePrefab, position, Quaternion.identity, transform);
                sprite.transform.parent = transform;
                sprite.color = Color.red; // set path color
            }
            // Set starting and ending point colors
            SpriteRenderer startSprite = Instantiate(spritePrefab, new Vector2(startPoint.x, startPoint.y) * spriteSize, Quaternion.identity, transform);
            startSprite.transform.parent = transform;
            startSprite.color = Color.green;
            SpriteRenderer endSprite = Instantiate(spritePrefab, new Vector2(endPoint.x, endPoint.y) * spriteSize, Quaternion.identity, transform);
            endSprite.transform.parent = transform;
            endSprite.color = Color.blue;
        }
        else
        {
            // Set starting and ending point colors if no path is found
            SpriteRenderer startSprite = Instantiate(spritePrefab, new Vector2(startPoint.x, startPoint.y) * spriteSize, Quaternion.identity, transform);
            startSprite.transform.parent = transform;
            startSprite.color = Color.green;
            SpriteRenderer endSprite = Instantiate(spritePrefab, new Vector2(endPoint.x, endPoint.y) * spriteSize, Quaternion.identity, transform);
            endSprite.transform.parent = transform;
            endSprite.color = Color.blue;
        }
    }
}
