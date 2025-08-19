using UnityEngine;

public class Node : MonoBehaviour, IHeapItem<Node>
{
    // stored inside the cube
    
    // colors for different state
    [SerializeField] Material isWalkableColor;
    [SerializeField] Material isObstacleColor;
    [SerializeField] Material isHighlightedColor;
    public bool isWalkable;
    public bool isOccupied;
    public Vector3 worldCoordinates;
    public Vector2Int gridCoordinate;

    // hCost -> distance from targetNode
    // gCost -> distance from startNode
    [HideInInspector] public int gCost, hCost;
    // sum of gCost, hCost
    public int FCost => gCost + hCost;
    
    [HideInInspector] public Node parentNode;
    private int _heapIndex;

    private MeshRenderer _meshRenderer;


    public void Init(bool isWalkable, Vector3 worldSpaceCoordinate, Vector2Int gridCoordinate)
    {
        this.isWalkable = isWalkable;
        this.worldCoordinates = worldSpaceCoordinate;
        this.gridCoordinate = gridCoordinate;
        if (!isWalkable)
        {
            isOccupied = true;
        }

        _meshRenderer = GetComponent<MeshRenderer>();
        _meshRenderer.sharedMaterial = this.isWalkable ? isWalkableColor : isObstacleColor;
    }

    // Implementation of comparision node, compare for lowest f cost, if not then compare, lower h cost
    public int CompareTo(Node nodeToCompare)
    {
        int compare = FCost.CompareTo(nodeToCompare.FCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }

    public int HeapIndex
    {
        get => _heapIndex;
        set => _heapIndex = value;
    }

    public void Highlight(bool value)
    {
        if (isWalkable)
        {
            _meshRenderer.sharedMaterial = value ? isHighlightedColor : isWalkableColor;
        }
    }
}
