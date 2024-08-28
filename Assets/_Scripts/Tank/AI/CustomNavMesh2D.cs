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
    public GameObject visualRepresentation; // Visual representation of the node
}

public class CustomNavMesh2D : MonoBehaviour
{
    public int gridWidth = 10;
    public int gridHeight = 10;
    public float nodeSize = 1f;
    public Color walkableColor = Color.green;
    public Color unwalkableColor = Color.red;
    public Color processedColor = Color.yellow;
    public Color pathColor = Color.blue;
    public Color startNodeColor = Color.cyan;
    public Color goalNodeColor = Color.magenta;
    public string nodeSortingLayer = "On Ground"; // Sorting layer for all nodes

    public Vector2 gridOffset;
    public List<string> tagsToIgnore = new List<string>();

    private GridNode[,] grid;
    private List<GridNode> path; // Store the final path

    private bool startSelected = false;
    private Vector2 startPosition;
    private Vector2 goalPosition;

    private GameObject startNodeObject; // Reference to the start node visual
    private GameObject goalNodeObject;  // Reference to the goal node visual

    void Start()
    {
        GenerateGrid();
    }

    void Update()
    {
        HandleMouseInput();
    }

    void GenerateGrid()
    {
        Vector3 objectScale = transform.localScale;
        float objectWidth = objectScale.x;
        float objectHeight = objectScale.y;

        gridWidth = Mathf.CeilToInt(objectWidth / nodeSize);
        gridHeight = Mathf.CeilToInt(objectHeight / nodeSize);

        grid = new GridNode[gridWidth, gridHeight];
        Vector2 objectPosition = transform.position;
        gridOffset = objectPosition - new Vector2(objectWidth, objectHeight) * 0.5f;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector2 worldPosition = gridOffset + new Vector2(x, y) * nodeSize;

                Collider2D[] colliders = Physics2D.OverlapCircleAll(worldPosition, nodeSize * 0.5f);
                bool isWalkable = true;
                bool hasIgnoredTag = false;

                foreach (Collider2D collider in colliders)
                {
                    if (tagsToIgnore.Contains(collider.tag))
                    {
                        hasIgnoredTag = true;
                    }
                    else
                    {
                        isWalkable = false;
                        break;
                    }
                }

                if (hasIgnoredTag && isWalkable)
                {
                    isWalkable = true;
                }

                GameObject nodeObject = CreateNodeVisual(worldPosition, isWalkable ? walkableColor : unwalkableColor);

                grid[x, y] = new GridNode { position = worldPosition, isWalkable = isWalkable, visualRepresentation = nodeObject };
            }
        }
    }

    GameObject CreateNodeVisual(Vector2 position, Color color)
    {
        GameObject nodeObject = new GameObject("GridNode");
        nodeObject.transform.position = position;
        nodeObject.transform.localScale = Vector3.one * nodeSize;

        SpriteRenderer spriteRenderer = nodeObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = CreateSprite(); // Generate a square sprite
        spriteRenderer.color = color;
        spriteRenderer.sortingLayerName = nodeSortingLayer; // Set the sorting layer

        return nodeObject;
    }

    Sprite CreateSprite()
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, Color.white);
        texture.Apply();

        Rect rect = new Rect(0, 0, 1, 1);
        Vector2 pivot = new Vector2(0.5f, 0.5f);
        return Sprite.Create(texture, rect, pivot, 1f);
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (!startSelected)
            {
                startPosition = mousePosition;
                SetNodeAsStartNode(startPosition);
                startSelected = true;
                Debug.Log("Start Position Selected: " + startPosition);
            }
            else
            {
                goalPosition = mousePosition;
                SetNodeAsGoalNode(goalPosition);
                Debug.Log("Goal Position Selected: " + goalPosition);

                List<GridNode> foundPath = FindPath(startPosition, goalPosition);
                if (foundPath != null)
                {
                    HighlightPath(foundPath);
                }

                startSelected = false; // Reset for the next path
            }
        }
    }

    void SetNodeAsStartNode(Vector2 position)
    {
        GridNode node = GetNodeFromWorldPosition(position);
        if (node != null)
        {
            // Clear previous start node visual if it exists
            if (startNodeObject != null)
            {
                startNodeObject.GetComponent<SpriteRenderer>().color = walkableColor;
            }

            startNodeObject = node.visualRepresentation;
            startNodeObject.GetComponent<SpriteRenderer>().color = startNodeColor;
        }
    }

    void SetNodeAsGoalNode(Vector2 position)
    {
        GridNode node = GetNodeFromWorldPosition(position);
        if (node != null)
        {
            // Clear previous goal node visual if it exists
            if (goalNodeObject != null)
            {
                goalNodeObject.GetComponent<SpriteRenderer>().color = walkableColor;
            }

            goalNodeObject = node.visualRepresentation;
            goalNodeObject.GetComponent<SpriteRenderer>().color = goalNodeColor;
        }
    }

    public List<GridNode> FindPath(Vector2 start, Vector2 goal)
    {
        GridNode startNode = GetNodeFromWorldPosition(start);
        GridNode goalNode = GetNodeFromWorldPosition(goal);

        if (startNode == null || goalNode == null || !startNode.isWalkable || !goalNode.isWalkable)
        {
            Debug.LogError("Invalid start or goal node.");
            return null;
        }

        List<GridNode> openList = new List<GridNode>();
        HashSet<GridNode> closedList = new HashSet<GridNode>();

        // Initialize the start node
        startNode.gCost = 0;
        startNode.hCost = Vector2.Distance(startNode.position, goalNode.position);
        openList.Add(startNode);

        while (openList.Count > 0)
        {
            GridNode currentNode = GetNodeWithLowestFCost(openList);
            Debug.Log("Processing node at position: " + currentNode.position);

            if (currentNode == goalNode)
            {
                Debug.Log("Path found!");
                path = RetracePath(startNode, goalNode);
                return path;
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            currentNode.visualRepresentation.GetComponent<SpriteRenderer>().color = processedColor; // Mark as processed

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

        Debug.LogError("No path found.");
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

                int checkX = Mathf.RoundToInt((node.position.x - gridOffset.x) / nodeSize) + x;
                int checkY = Mathf.RoundToInt((node.position.y - gridOffset.y) / nodeSize) + y;

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
        int x = Mathf.RoundToInt((worldPosition.x - gridOffset.x) / nodeSize);
        int y = Mathf.RoundToInt((worldPosition.y - gridOffset.y) / nodeSize);

        x = Mathf.Clamp(x, 0, gridWidth - 1);
        y = Mathf.Clamp(y, 0, gridHeight - 1);

        return grid[x, y];
    }

    void HighlightPath(List<GridNode> path)
    {
        foreach (var node in path)
        {
            node.visualRepresentation.GetComponent<SpriteRenderer>().color = pathColor; // Mark the path
        }
    }
}
