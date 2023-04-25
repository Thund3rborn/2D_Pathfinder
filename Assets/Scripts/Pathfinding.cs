using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Sprites;
using UnityEngine;

public class Node
{
    public int x;           // x-coordinate of the node on the map
    public int y;           // y-coordinate of the node on the map
    public int f;           // total cost of the node (g + h)
    public int g;           // cost of the node from the start node
    public int h;           // estimated cost of the node to the end node
    public Node parent;     // parent node of the current node in the path

    public Node(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}

public class Pathfinding : MonoBehaviour
{
    private static int CalculateDistance(int x1, int y1, int x2, int y2)
    {
        //calculate the distance between two nodes using the Manhattan distance formula
        int xDiff = Mathf.Abs(x1 - x2);
        int yDiff = Mathf.Abs(y1 - y2);
        return xDiff + yDiff;
    }

    public static List<Node> FindPath(int[,] map, int startX, int startY, int endX, int endY)
    {
        int width = map.GetLength(0);
        int height = map.GetLength(1);

        //create start and end nodes
        Node startNode = new Node(startX, startY);
        Node endNode = new Node(endX, endY);

        //initialize the open and closed lists
        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();

        //add the start node to the open list
        openList.Add(startNode);

        while (openList.Count > 0)
        {
            //get the node with the lowest F score from the open list
            Node currentNode = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].f < currentNode.f)
                {
                    currentNode = openList[i];
                }
            }

            //remove the current node from the open list and add it to the closed list
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            //if the current node is the end node, return the path
            if (currentNode.x == endNode.x && currentNode.y == endNode.y)
            {
                List<Node> path = new List<Node>();
                Node node = currentNode;
                while (node != null)
                {
                    path.Add(node);
                    node = node.parent;
                }
                path.Reverse();
                return path;
            }

            //generate the successors of the current node
            List<Node> successors = new List<Node>();
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                    {
                        continue;
                    }

                    int newX = currentNode.x + x;
                    int newY = currentNode.y + y;
                    if (newX < 0 || newX >= width || newY < 0 || newY >= height)
                    {
                        continue;
                    }

                    if (map[newX, newY] == 1)
                    {
                        continue;
                    }

                    Node newNode = new Node(newX, newY);
                    successors.Add(newNode);
                }
            }

            //for each successor, calculate its G and H scores and add it to the open list
            foreach (Node successor in successors)
            {
                if (closedList.Contains(successor))
                {
                    continue;
                }

                int gScore = currentNode.g + 1;
                int hScore = CalculateDistance(successor.x, successor.y, endNode.x, endNode.y);
                int fScore = gScore + hScore;

                if (openList.Contains(successor))
                {
                    if (gScore < successor.g)
                    {
                        successor.g = gScore;
                        successor.h = hScore;
                        successor.f = fScore;
                        successor.parent = currentNode;
                    }
                }
                else
                {
                    successor.g = gScore;
                    successor.h = hScore;
                    successor.f = fScore;
                    successor.parent = currentNode;
                    openList.Add(successor);
                }
            }
        }

        //if the open list is empty and we haven't found the end node, there is no path
        return null;
    }
}
