using System.Collections;
using UnityEngine;

public enum TurnState
{
    PlayerTurn,
    EnemyTurn, 
    Busy,
};

public class TurnManager : MonoBehaviour
{
    // script manages spawning and turns of player and enemy
    [SerializeField] private Player playerUnitToSpawn;
    [SerializeField] private EnemyAI enemyAIToSpawn;
    private GameManager _gameManager;
    
    private Player _player;
    private EnemyAI _enemyAI;

    private TurnState turnState;
    public bool PlayerTurn => turnState == TurnState.PlayerTurn;

    public void AwakeInit()
    {
        _gameManager = GameManager.instance;
    }
    
    // init player at origin and enemy at top right
    public void StartInit()
    {
        _player = Instantiate(playerUnitToSpawn,
            GameManager.instance.gridManager.CustomGrid.GetNode(new Vector2Int(0, 0)).worldCoordinates,
            Quaternion.identity);
        Vector2Int lastCell = GameManager.instance.gridManager.CustomGrid.GridSize;
        _enemyAI = Instantiate(enemyAIToSpawn,
            GameManager.instance.gridManager.CustomGrid.GetNode(new Vector2Int(lastCell.x - 1, lastCell.y - 1)).worldCoordinates,
            Quaternion.identity);
        _gameManager.inputManager.OnLmbClick += OnLmbClick;
        turnState = TurnState.PlayerTurn;
    }

    private void OnDestroy()
    {
        _gameManager.inputManager.OnLmbClick -= OnLmbClick;
    }

    // used by player to declare his end of turn
    public void EndPlayerTurn()
    {
        if (turnState != TurnState.PlayerTurn) return;
        StartCoroutine(EnemyTurnRoutine());
    }
    
    // starts enemy movement coroutine
    private IEnumerator EnemyTurnRoutine()
    {
        turnState = TurnState.Busy; 
        yield return new WaitForSeconds(0.1f);

        turnState = TurnState.EnemyTurn;

        // Move enemies
        yield return _enemyAI.MoveAdjacentToPlayer(_player.transform.position);

        yield return new WaitForSeconds(0.1f);

        turnState = TurnState.PlayerTurn;
        _gameManager.inputAllowed = true;
    }

    private void OnLmbClick()
    {
       MovePlayer();
    }

    public void TurnManagerUpdate()
    {
        
    }
    // manages the movement of player by giving position
    private void MovePlayer()
    {
        if (_gameManager.inputAllowed)
        {
            if (_gameManager.inputManager.GetMousePosition(out Vector3 worldPosition))
            {
                _player.MoveUnit(worldPosition);
            }        
        }
    }
    
}
