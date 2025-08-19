using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathRequestManager : MonoBehaviour
{
    // this scripts avail path finding to Movable units
    
    // queue that supports multiple path finds and spreads them across frames for optimisation
    private Queue<PathRequest> _pathRequestsQueue = new();
    private PathRequest _currentPathRequest;

    private bool _processingPath;
    private static PathRequestManager instance;
    private PathFinding _pathFinding;
    private void Awake()
    {
        instance = this;
        _pathFinding = GetComponent<PathFinding>();
    }

    // movable unit call this when requesting a path
    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
    {
        PathRequest pathRequest = new PathRequest(pathStart, pathEnd, callback);
        instance._pathRequestsQueue.Enqueue(pathRequest);
        instance.TryProcessNext();
    }

    // After completion of one path find, go for next one
    private void TryProcessNext()
    {
        if (!_processingPath && _pathRequestsQueue.Count > 0)
        {
            _currentPathRequest = _pathRequestsQueue.Dequeue();
            _processingPath = true;
            _pathFinding.StartFindingPath(_currentPathRequest.pathStart, _currentPathRequest.pathEnd);
        }
    }

    public void FinishedProcessingPath(Vector3[] path, bool success)
    {
        _currentPathRequest.callback(path, success);
       
        _processingPath = false;
        TryProcessNext();
    }

    // holds data necessary for path finding
    struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callback;

        public PathRequest(Vector3 start, Vector3 end, Action<Vector3[], bool> ct)
        {
            pathStart = start;
            pathEnd = end;
            callback = ct;
        }
    }
}
