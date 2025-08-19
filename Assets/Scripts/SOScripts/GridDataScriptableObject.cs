using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "GridDataHolderSO")]
public class GridDataScriptableObject : ScriptableObject
{
    // rows are y
    [SerializeField] private int currentRows;
    [SerializeField] private int currentCols;
    [SerializeField] private bool[] walkableGridDataArray;

    // init the array upon change in level editor
    public void Init(int r, int c)
    {
        if (walkableGridDataArray == null || walkableGridDataArray.Length != r * c)
        {
            currentRows = r;
            currentCols = c;
            walkableGridDataArray = new bool[r * c];
        }
    }

    // setting and getting cells during the change in level editor
    public bool GetCell(int row, int col) => walkableGridDataArray[row * currentCols + col];
    public void SetCell(int row, int col, bool value) => walkableGridDataArray[row * currentCols + col] = value;

    public bool[,] GetAll()
    {
        bool[,] tempGrid = new bool[currentRows, currentCols];
        for (int x = 0; x < currentRows; x++)
        {
            for (int y = 0; y < currentCols; y++)
            {
                tempGrid[x, y] = GetCell(x, y);
            }
        }

        return tempGrid;
    }

    public int GetTotalRows => currentRows;
    public int GetTotalCols => currentCols;
}
