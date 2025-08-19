using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MovableUnit : MonoBehaviour
{
    [SerializeField] protected float speed = 0.5f;
    protected Vector3[] _path;
    protected int _targetIndex;

    private Coroutine _coroutine;
    protected bool _hasReached;
    

    // Requests for a path to PathRequestManager
    public void MoveUnit(Vector3 targetPosition)
    {
        PathRequestManager.RequestPath(transform.position, targetPosition, OnPathFound);
        _hasReached = false;
    }

    // Callback function when path found
    private void OnPathFound(Vector3[] newPath, bool pathSuccess)
    {
        if (pathSuccess)
        {
            // assignment of path list from callback
            _path = newPath;
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
            _coroutine = StartCoroutine(FollowPath());
        }
    }

    // moves the body according to the given path list
    private IEnumerator FollowPath()
    {
        if (_path.Length == 0)
        {
            _hasReached = true;
            TurnEndCallBack();
            yield break;
        }
        
        Vector3 currentWayPoint = _path[0];
        _targetIndex = 0;
       TurnStartCallBack();
        while (true)
        {
            if (transform.position == currentWayPoint)
            {
                _targetIndex++;
                if (_targetIndex >= _path.Length)
                {
                    _hasReached = true;
                    TurnEndCallBack();
                    yield break;
                }
                currentWayPoint = _path[_targetIndex];
            }
            transform.position = Vector3.MoveTowards(transform.position, currentWayPoint, speed * Time.deltaTime);
            yield return null;
        }
    }

    protected virtual void TurnStartCallBack()
    {
        
    }

    protected virtual void TurnEndCallBack()
    {
        
    }

    private void OnDrawGizmos()
    {
        if (_path != null)
        {
            Vector3 lineOffset = Vector3.up;
            for (int i = _targetIndex; i < _path.Length; i++)
            {
                Gizmos.color = Color.black;

                if (i == _targetIndex)
                {
                    Gizmos.DrawLine(transform.position + lineOffset, _path[i] + lineOffset);
                }
                else
                {
                    Gizmos.DrawLine(_path[i-1] + lineOffset, _path[i]+ lineOffset);

                }
            }
        }
    }
}
