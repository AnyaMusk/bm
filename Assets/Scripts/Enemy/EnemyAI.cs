using System;
using System.Collections;
using UnityEngine;

public class EnemyAI : MovableUnit, AI
{
    // Moving enemy to adjacent node of player, closest to enemy 
    public IEnumerator MoveAdjacentToPlayer(Vector3 playerPosition)
    {
        Node closestNode = GameManager.instance.gridManager.CustomGrid.GetClosestAdjacentNode(transform.position, playerPosition);
        MoveUnit(closestNode.worldCoordinates);
        while (!_hasReached)
        {
            yield return null;
        }
    }

    protected override void TurnStartCallBack()
    {
        
    }

    protected override void TurnEndCallBack()
    {
        
    }
}
