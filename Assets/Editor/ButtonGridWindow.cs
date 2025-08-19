using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.Serialization;

public class ButtonGridWindow : EditorWindow
{
    [SerializeField] private GridDataScriptableObject gridWalkableDataSo;
    private int _rows = 10;
    private int _cols = 10;
    
    // for persistant date saving or loading the old scriptable object
    private const string PREF_KEY = "ButtonGrid_LastSO";
    
    [MenuItem("Tools/Button Grid")]
    public static void Open()
    {
        GetWindow<ButtonGridWindow>("Button Grid");
    }

    private void OnEnable()
    {
        LoadLastSO();
        Repaint();
    }

    private void OnDisable()
    {
        SaveLastSO();
    }

    // any changes in gui changes the scriptable object data
    private void OnGUI()
    {
        // designing the layout
        EditorGUILayout.BeginHorizontal();
        var newSO = (GridDataScriptableObject)EditorGUILayout.ObjectField(
            "Grid Data", gridWalkableDataSo,
            typeof(GridDataScriptableObject), false);

        // if old SO is not new SO, then have to repaint GUI and save
        if (newSO != gridWalkableDataSo)
        {
            gridWalkableDataSo = newSO;
            SaveLastSO();
            Repaint();
        }
        EditorGUILayout.EndHorizontal();

        if (gridWalkableDataSo == null)
        {
            EditorGUILayout.HelpBox("Assign a Grid Data SO asset.", MessageType.Info);
            return;
        }
        
        // Grid size init
        EditorGUILayout.BeginHorizontal();
        _rows = EditorGUILayout.IntField("Rows", _rows);
        _cols = EditorGUILayout.IntField("Columns", _cols);

        if (GUILayout.Button("Apply", GUILayout.Width(60)))
        {
            // Make undo support, upon hitting apply save data, repaint GUI, init the Scriptable Object
            Undo.RecordObject(gridWalkableDataSo, "Resize Grid");
            gridWalkableDataSo.Init(_rows, _cols);
            EditorUtility.SetDirty(gridWalkableDataSo);
            AssetDatabase.SaveAssets();
            Repaint();
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();


        // Drawing buttons here
        // Make undo support, upon changing the button grid, repaint GUI, init the Scriptable Object
        for (int r = gridWalkableDataSo.GetTotalRows - 1; r >= 0; r--)
        {
            EditorGUILayout.BeginHorizontal();
            for (int c = 0; c < gridWalkableDataSo.GetTotalCols; c++)
            {
                bool val = gridWalkableDataSo.GetCell(r, c);
                string label = val ? "X" : "";

                if (GUILayout.Button(label, GUILayout.Width(30), GUILayout.Height(30)))
                {
                    Undo.RecordObject(gridWalkableDataSo, "Toggle Cell");
                    gridWalkableDataSo.SetCell(r, c, !val);
                    EditorUtility.SetDirty(gridWalkableDataSo);
                    AssetDatabase.SaveAssets();
                    Repaint();
                }
            }
            EditorGUILayout.EndHorizontal();
        }
    }
    
    // save and load the so
    private void SaveLastSO()
    {
        if (gridWalkableDataSo != null)
            EditorPrefs.SetString(PREF_KEY, AssetDatabase.GetAssetPath(gridWalkableDataSo));
    }

    private void LoadLastSO()
    {
        if (gridWalkableDataSo == null && EditorPrefs.HasKey(PREF_KEY))
        {
            string path = EditorPrefs.GetString(PREF_KEY, "");
            gridWalkableDataSo = AssetDatabase.LoadAssetAtPath<GridDataScriptableObject>(path);
        }
    }
    
}