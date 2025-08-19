using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    private PathRequestManager _pathRequestManager;
    private GridManager _gridManager;

    private void Awake()
    {
        _pathRequestManager = GetComponent<PathRequestManager>();
        _gridManager = GetComponent<GridManager>();
    }
    

    // starts finding path in A* Algo
    public void StartFindingPath(Vector3 startPos, Vector3 endPos)
    {
        StartCoroutine(FindPath(startPos, endPos));
    }
    
    IEnumerator FindPath(Vector3 startPos, Vector3 endPos)
    {
        Vector3[] wayPoints = Array.Empty<Vector3>();
        bool pathSuccess = false;
        
        Node startNode = _gridManager.CustomGrid.GetNode(startPos);
        Node targetNode = _gridManager.CustomGrid.GetNode(endPos);

        if (startNode.isWalkable && targetNode.isWalkable)
        {
            Heap<Node> openSet = new Heap<Node>(_gridManager.CustomGrid.MaxSize);
            HashSet<Node> closedSet = new HashSet<Node>();
        
            // add the first node to open set
            openSet.Add(startNode);
            // while open set loop not empty, select the node if lower fCost(IF walkable) 
            while (openSet.Count > 0)
            {
                Node currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                // if found then stop and construct path
                if (currentNode == targetNode)
                {
                    pathSuccess = true;
                    break;
                }

                // evaluate the neighbours fCost and reconfigure parent, add them to open set if not in closed set or
                // are walkable
                foreach (Node neighbour in _gridManager.CustomGrid.GetNeighbouringNode(currentNode))
                {
                    if (!neighbour.isWalkable || neighbour.isOccupied || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    int newMovementCost = currentNode.gCost + _gridManager.CustomGrid.Distance(currentNode, neighbour);
                    if (newMovementCost < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCost;
                        neighbour.hCost = _gridManager.CustomGrid.Distance(targetNode, neighbour);
                        neighbour.parentNode = currentNode;

                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                    }
                }
            }
        }
        yield return null;
        if (pathSuccess)
        {
            // construct the path by backtracking from end to start via parent node
            //print("START : " + startNode.gridCoordinate + "  End : " + targetNode.gridCoordinate);
            startNode.isOccupied = false;
            targetNode.isOccupied = true;
            wayPoints = RetracePath(startNode, targetNode);
        }
        _pathRequestManager.FinishedProcessingPath(wayPoints, pathSuccess);
    }

    // Get Vector3 pos as path
    private Vector3[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parentNode;
        }
        
        Vector3[] wayPoints =  SimplifyPath(path);
        Array.Reverse(wayPoints);
        return wayPoints;
    }

    private Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> wayPoints = new List<Vector3>();
        /*if (path.Count == 0)
            return wayPoints.ToArray();

        wayPoints.Add(path[0].worldCoordinates);

        Vector2 directionOld = Vector2.zero;
        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(
                path[i - 1].gridCoordinate.x - path[i].gridCoordinate.x,
                path[i - 1].gridCoordinate.y - path[i].gridCoordinate.y
            );

            if (directionNew != directionOld)
            {
                wayPoints.Add(path[i].worldCoordinates);
            }

            directionOld = directionNew;
        }

        // ensure adding last point
        if (wayPoints[^1] != path[^1].worldCoordinates)
        {
            wayPoints.Add(path[^1].worldCoordinates);
        }*/

        foreach (var pathNode in path)
        {
            wayPoints.Add(pathNode.worldCoordinates);
        }

        return wayPoints.ToArray();
    }
}
