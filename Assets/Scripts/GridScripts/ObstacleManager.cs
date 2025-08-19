using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] private Transform obstaclesToPlacePrefab;
    [SerializeField] private Transform obstacleT;
    public void PlaceObstacles(List<Vector3> placeObsList)
    {
        foreach (Vector3 v in placeObsList)
        {
            Transform t = Instantiate(obstaclesToPlacePrefab, Vector3.zero, Quaternion.identity);
            t.position = v;
            t.SetParent(obstacleT);
        }
    }
}
