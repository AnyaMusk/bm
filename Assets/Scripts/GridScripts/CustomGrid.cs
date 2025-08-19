using System.Collections.Generic;
using UnityEngine;

public class CustomGrid
{
    // contains the multidimensional array of nodes
    private Node[,] _grid;
    // grid width and height
    public Vector2Int GridSize;
    // grid cell size
    private readonly float _cellSize;
    // grid origin - bottom left
    private readonly Vector3 _origin;

    public int MaxSize => GridSize.x * GridSize.y;

    public CustomGrid(Vector2Int gridSize, float cellSize, Vector3 origin)
    {
        GridSize = gridSize;
        this._cellSize = cellSize;
        _grid = new Node[gridSize.x, gridSize.y];
        _origin = origin;
    }
    
    public void SetValue(Vector2Int coordinate, Node node)
    {
        if (node != null)
            _grid[coordinate.x, coordinate.y] = node;
    }
    
    // Get node based on position or coordinates
    public Node GetNode(Vector2Int coordinate)
    {
        if (IsInBounds(coordinate))
        {
            return _grid[coordinate.x, coordinate.y];
        }
        return null;
    }

    public Node GetNode(Vector3 worldPosition)
    {
        Vector2Int coord = GetXYByWorld(worldPosition);
        return GetNode(coord);
    }
    
    // get world position by coordinate
    public Vector3 GetWorldPosition(Vector2Int coordinate)
    {
        return _origin + new Vector3(coordinate.x * _cellSize, 0f, coordinate.y * _cellSize);
    }
    
    // Get grid coordinate by world coordinates
    public Vector2Int GetXYByWorld(Vector3 worldPosition)
    {
        Vector2 rel = new Vector2(worldPosition.x, worldPosition.z) - new Vector2(_origin.x, _origin.z);
        return new Vector2Int(Mathf.RoundToInt(rel.x / _cellSize), Mathf.RoundToInt(rel.y / _cellSize));
    }
    
    // Get neighbouring 8 cells
    public List<Node> GetNeighbouringNode(Node node)
    {
        List<Node> nodes = new List<Node>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector2Int nCoord = new Vector2Int(x + node.gridCoordinate.x, y + node.gridCoordinate.y);
                if (x == 0 && y == 0)
                    continue;
                if (IsInBounds(nCoord))
                    nodes.Add(_grid[nCoord.x, nCoord.y]);
            }
        }

        return nodes;
    }
    
    // Check if coordinates are in bounds
    private bool IsInBounds(Vector2Int coord)
    {
        return coord.x >= 0 && coord.x < GridSize.x && coord.y >= 0 && coord.y < GridSize.y;
    }
    
    // Distance b/w two nodes
    public int Distance(Node a, Node b)
    {
        int distanceX = Mathf.Abs(a.gridCoordinate.x - b.gridCoordinate.x);
        int distanceY = Mathf.Abs(a.gridCoordinate.y - b.gridCoordinate.y);

        if (distanceX > distanceY)
            return 14 * distanceY + 10 * (distanceX - distanceY);
        return 14 * distanceX + 10 * (distanceY - distanceX);
    }

    // Getting the closest adjacent node to player for enemy movement
    private Node GetClosestAdjacentNode(Node currentNode, Node targetNode)
    {
        // getting adj neighbours
        List<Node> nodes = new List<Node>();
        Node ret = null;
        int distance = int.MaxValue;
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector2Int nCoord = new Vector2Int(x + targetNode.gridCoordinate.x, y + targetNode.gridCoordinate.y);
                if (x == 0 && y == 0)
                    continue;
                // getting on side adjacent
                if (IsInBounds(nCoord) && Mathf.Abs(x + y) == 1)
                    nodes.Add(_grid[nCoord.x, nCoord.y]);
            }
        }

        foreach (Node n in nodes)
        {
            int currentNodeDistance = Distance(n, currentNode);
            if (currentNodeDistance < distance && n.isWalkable)
            {
                distance = currentNodeDistance;
                ret = n;
            }
        }
        return ret;
    }
    
    public Node GetClosestAdjacentNode(Vector3 currentPosition, Vector3 targetPosition)
    {
        Node currentNode = GetNode(currentPosition);
        Node targetNode = GetNode(targetPosition);
        return GetClosestAdjacentNode(currentNode, targetNode);
    }

    // highlighting selected node
    public void HighlightCell(Node node, bool value)
    {
        node.Highlight(value);
    }
    
    
}
