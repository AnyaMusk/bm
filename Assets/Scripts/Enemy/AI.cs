using System.Collections;
using UnityEngine;

public interface AI
{
    public IEnumerator MoveAdjacentToPlayer(Vector3 playerPosition);
}
