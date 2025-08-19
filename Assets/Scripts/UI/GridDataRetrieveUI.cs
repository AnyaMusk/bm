using System;
using TMPro;
using UnityEngine;

public class GridDataRetrieveUI : MonoBehaviour
{
    [SerializeField] private GameObject detailPanelGameObject;
    [SerializeField] private TextMeshProUGUI worldPositionText;
    [SerializeField] private TextMeshProUGUI cellCoordinateText;
    [SerializeField] private TextMeshProUGUI isWalkableText;

    private void Start()
    {
        detailPanelGameObject.SetActive(false);
    }

    // update UI descriptions for nodes and disable when not on valid cell
    public void UpdateDescriptions(bool value, Vector3 worldPos, Vector2Int cellCoordinate, bool isWalkable)
    {
        if (value)
        {
            detailPanelGameObject.SetActive(true);
            worldPositionText.text = $" <b>World Pos</b> : {worldPos.x} {worldPos.y} {worldPos.z}";
            cellCoordinateText.text = $"<b>Cell Coordinate</b> : {cellCoordinate.x} {cellCoordinate.y}";
            isWalkableText.text = "<b>IsWalkable</b> : " + (isWalkable ? "true" : "false");
        }
        else
        {
            detailPanelGameObject.SetActive(false);
        }
    }
}
