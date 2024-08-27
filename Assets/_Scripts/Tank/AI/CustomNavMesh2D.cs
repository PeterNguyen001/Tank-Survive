using System.Collections.Generic;
using UnityEngine;

public class GridNode
{
    public Vector2 position;
    public bool isWalkable;
    public GridNode parent;
    public float gCost; // Distance from start node
    public float hCost; // Heuristic cost (distance to goal)
    public float fCost { get { return gCost + hCost; } }
}

public class CustomNavMesh2D : MonoBehaviour
{
    public int gridWidth = 10;
    public int gridHeight = 10;
    public float nodeSize = 1f;

    public Vector2 gridOffset;

    public List<string> tagsToIgnore = new List<string>();

    private GridNode[,] grid;

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        // Get the object's dimensions in world space using its scale
        Vector3 objectScale = transform.localScale;
        float objectWidth = objectScale.x;
        float objectHeight = objectScale.y;

        // Calculate the grid dimensions based on the object's size and node size
        gridWidth = Mathf.CeilToInt(objectWidth / nodeSize);
        gridHeight = Mathf.CeilToInt(objectHeight / nodeSize);

        // Initialize the grid
        grid = new GridNode[gridWidth, gridHeight];

        // Calculate the grid offset in world space based on the object's position
        Vector2 objectPosition = transform.position;
        gridOffset = objectPosition - new Vector2(objectWidth, objectHeight) * 0.5f;

        // Generate the grid nodes
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector2 worldPosition = gridOffset + new Vector2(x, y) * nodeSize;

                // Get all colliders in this node's area
                Collider2D[] colliders = Physics2D.OverlapCircleAll(worldPosition, nodeSize * 0.5f);

                bool isWalkable = true;
                bool hasIgnoredTag = false;

                foreach (Collider2D collider in colliders)
                {
                    // Check if the collider's tag is in the tagsToIgnore list
                    if (tagsToIgnore.Contains(collider.tag))
                    {
                        hasIgnoredTag = true;
                    }
                    else
                    {
                        // If there is any non-ignored object, mark the node as unwalkable
                        isWalkable = false;
                        break;
                    }
                }

                // If the node only has objects with ignored tags, mark it as walkable
                if (hasIgnoredTag && isWalkable)
                {
                    isWalkable = true;
                }

                grid[x, y] = new GridNode { position = worldPosition, isWalkable = isWalkable };
            }
        }
    }

    void OnDrawGizmos()
    {
        if (grid == null)
        {
            return;
        }

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                GridNode node = grid[x, y];

                // Set the Gizmo color based on whether the node is walkable
                Gizmos.color = node.isWalkable ? Color.green : Color.red;

                // Draw a wire cube to represent the grid node
                Gizmos.DrawWireCube(node.position, Vector3.one * nodeSize);
            }
        }
    }

    public List<GridNode> FindPath(Vector2 start, Vector2 goal)
    {
        GridNode startNode = GetNodeFromWorldPosition(start);
        GridNode goalNode = GetNodeFromWorldPosition(goal);

        List<GridNode> openList = new List<GridNode>();
        HashSet<GridNode> closedList = new HashSet<GridNode>();

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            GridNode currentNode = GetNodeWithLowestFCost(openList);

            if (currentNode == goalNode)
            {
                return RetracePath(startNode, goalNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (GridNode neighbor in GetNeighbors(currentNode))
            {
                if (!neighbor.isWalkable || closedList.Contains(neighbor))
                {
                    continue;
                }

                float newGCost = currentNode.gCost + Vector2.Distance(currentNode.position, neighbor.position);
                if (newGCost < neighbor.gCost || !openList.Contains(neighbor))
                {
                    neighbor.gCost = newGCost;
                    neighbor.hCost = Vector2.Distance(neighbor.position, goalNode.position);
                    neighbor.parent = currentNode;

                    if (!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                    }
                }
            }
        }

        return null; // No path found
    }

    GridNode GetNodeWithLowestFCost(List<GridNode> nodes)
    {
        GridNode lowestFCostNode = nodes[0];

        foreach (var node in nodes)
        {
            if (node.fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = node;
            }
        }

        return lowestFCostNode;
    }

    List<GridNode> RetracePath(GridNode startNode, GridNode endNode)
    {
        List<GridNode> path = new List<GridNode>();
        GridNode currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        return path;
    }

    List<GridNode> GetNeighbors(GridNode node)
    {
        List<GridNode> neighbors = new List<GridNode>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }

                int checkX = (int)(node.position.x / nodeSize) + x;
                int checkY = (int)(node.position.y / nodeSize) + y;

                if (checkX >= 0 && checkX < gridWidth && checkY >= 0 && checkY < gridHeight)
                {
                    neighbors.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbors;
    }

    GridNode GetNodeFromWorldPosition(Vector2 worldPosition)
    {
        int x = Mathf.RoundToInt(worldPosition.x / nodeSize);
        int y = Mathf.RoundToInt(worldPosition.y / nodeSize);

        x = Mathf.Clamp(x, 0, gridWidth - 1);
        y = Mathf.Clamp(y, 0, gridHeight - 1);

        return grid[x, y];
    }
}
