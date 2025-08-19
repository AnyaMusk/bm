using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [HideInInspector]public GridManager gridManager;
    [HideInInspector]public InputManager inputManager;
    [HideInInspector]public GridDataRetrieveUI gridDataRetrieveUI;
    [HideInInspector]public TurnManager turnManager;

    // allows player input
    public bool inputAllowed;
    // for UI description
    public Action<bool, Vector3, Vector2Int, bool> OnCellValueChanged;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        gridManager = GetComponentInChildren<GridManager>();
        inputManager = GetComponentInChildren<InputManager>();
        gridDataRetrieveUI = GetComponentInChildren<GridDataRetrieveUI>();
        turnManager = GetComponentInChildren<TurnManager>();
        
        inputManager.AwakeInit();
        gridManager.AwakeInit();
        turnManager.AwakeInit();
    }

    private void Start()
    {
        inputAllowed = true;
        OnCellValueChanged += gridDataRetrieveUI.UpdateDescriptions;
        
        gridManager.StartInit();
        turnManager.StartInit();
    }

    private void OnDestroy()
    {
        OnCellValueChanged -= gridDataRetrieveUI.UpdateDescriptions;
    }

    // All updates loops are cumulated
    private void Update()
    {
        inputManager.InputManagerUpdate();
        gridManager.GridManagerUpdate();
        turnManager.TurnManagerUpdate();
    }
}
