using UnityEngine;

public class StationPreview : MonoBehaviour
{
    public GridManager gridManager;
    void Start()
    {
        
    }

    void Update()
    {
        Vector3 mousePos = gridManager.GetMousePosition();
        Vector2Int gridPos = new Vector2Int(Mathf.RoundToInt(mousePos.x), Mathf.RoundToInt(mousePos.y));

        NodeGroup nodeGroup = gridManager.GetNodeGroup(gridPos);
        Vector2Int? position = GetStationPosition(nodeGroup, gridPos);

        if (position != null)
        {
            transform.position = new Vector3(position.Value.x, position.Value.y, 0);
            
        }
        else
        {
            transform.position = new Vector3(gridPos.x, gridPos.y, 0);
        }
    }

    public Vector2Int? GetStationPosition(NodeGroup nodeGroup, Vector2Int gridPos)
    {
        if (nodeGroup != null)
        {
            Direction alignment = gridManager.GetNodeGroup(gridPos).GetAlignment();
            Vector2Int vector = GridManager.VectorFromDirection((Direction)(((int)alignment + 2) % 8));
            Vector2Int position = gridPos - vector;
            return position;
        }

        return null;
    }
}
