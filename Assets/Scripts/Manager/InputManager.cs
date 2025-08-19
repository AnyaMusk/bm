using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask nodeLayerMask;

    private GameManager _gameManager;
    public Action OnLmbClick;

    public void AwakeInit()
    {
        _gameManager = GameManager.instance;
    }
    // Getting Mouse position on grid
    public bool GetMousePosition(out Vector3 worldPosition)
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, 100f, nodeLayerMask, QueryTriggerInteraction.Ignore))
        {
            worldPosition = hit.point;
            return true;
        }
        
        worldPosition = Vector3.zero;
        return false;
    }

    // Input action for moving the player
    public void InputManagerUpdate()
    {
        if (_gameManager.inputAllowed)
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnLmbClick?.Invoke();
            }
        }
    }
}
