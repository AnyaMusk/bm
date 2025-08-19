using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    // manages the construction of grid reading data from SO
    [SerializeField] private GridDataScriptableObject gridDataScriptableObject;
    
    [Tooltip("Prefab for spawning cubes")][SerializeField] private Node cellToSpawn;
    [Tooltip("Parent for holding the node cubes")][SerializeField] private Transform gridTransform;

    private ObstacleManager _obstacleManager;
    private GameManager _gameManager;
    
    [Tooltip("Each cell size ")]
    public int cellSize = 1;
    public CustomGrid CustomGrid;

    private Vector2Int _lastCellTracked;
    private bool _insideBounds;


    // For firing the event for UI and selection of grid
    private bool InsideBounds
    {
        get => _insideBounds;
        set
        {
            if (_insideBounds != value)
            {
                if (!value)
                {
                    _gameManager.OnCellValueChanged?.Invoke(false, Vector3.zero, Vector2Int.zero, false);
                    if (_lastCellTracked != new Vector2Int(-1, -1))
                    {
                        Node lastNode = CustomGrid.GetNode(_lastCellTracked);
                        CustomGrid.HighlightCell(lastNode, false);
                    }
                }
            }
            _insideBounds = value;

        }
    }
    // initialising of grid
    public void AwakeInit()
    {
        _gameManager = GameManager.instance;
        _obstacleManager = GetComponent<ObstacleManager>();
        Vector2Int gridSize = new Vector2Int(gridDataScriptableObject.GetTotalCols, gridDataScriptableObject.GetTotalRows);
        Vector3 origin = transform.position -
                  new Vector3((gridSize.x + 1) * (cellSize * 0.5f), 0f, (gridSize.y + 1) * (cellSize * 0.5f));
        CustomGrid = new CustomGrid(gridSize, cellSize, origin);
    }

    // initialising and spawning of nodes
    public void StartInit()
    {
        List<Vector3> obstaclePos = new List<Vector3>();
        // col
        for (int x = 0; x < CustomGrid.GridSize.x; x++)
        {
            // row
            for (int y = 0; y < CustomGrid.GridSize.y; y++)
            {
                Vector2Int gridCoord = new Vector2Int(x, y);
                Vector3 worldCoordinate = CustomGrid.GetWorldPosition(gridCoord);
                Node n = Instantiate(cellToSpawn, Vector3.zero, Quaternion.identity);
                n.transform.position = worldCoordinate;
                n.transform.localScale = Vector3.one * cellSize;
                bool isWalkable = !gridDataScriptableObject.GetCell(y, x);
                n.Init(isWalkable, worldCoordinate, gridCoord);
                n.transform.SetParent(gridTransform);

                if (!isWalkable)
                {
                    obstaclePos.Add(worldCoordinate);
                }
                
                CustomGrid.SetValue(gridCoord, n);
            }
        }
        
        _obstacleManager.PlaceObstacles(obstaclePos);
    }

    // updating the current selected node for highlighting and UI
    public void GridManagerUpdate()
    {
        /*if (_gameManager.inputAllowed)
        {
            
        }
        else
        {
            InsideBounds = false;
            _lastCellTracked = new Vector2Int(-1, -1);
        }*/
        
        if (_gameManager.inputManager.GetMousePosition(out var position))
        {
            Vector2Int coord = CustomGrid.GetXYByWorld(position);
            InsideBounds = true;
            if (coord != _lastCellTracked)
            {
                if (_lastCellTracked != new Vector2Int(-1, -1))
                {
                    Node lastNode = CustomGrid.GetNode(_lastCellTracked);
                    CustomGrid.HighlightCell(lastNode, false);
                }
                Node node = CustomGrid.GetNode(coord);
                CustomGrid.HighlightCell(node, true);
                _gameManager.OnCellValueChanged?.Invoke(true, node.worldCoordinates, node.gridCoordinate, node.isWalkable);
                _lastCellTracked = coord;
            }
        }
        else
        {
            InsideBounds = false;
            _lastCellTracked = new Vector2Int(-1, -1);
        }
    }
}
