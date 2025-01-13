using UnityEngine;
using UnityEngine.InputSystem;

public class StationPreview : MonoBehaviour
{
    public GridManager gridManager;
    public GameObject stationPrefab;
    public AudioClip errorSound;
    public AudioClip placeSound;

    void Update()
    {
        Vector3 mousePos = gridManager.GetMousePosition();
        Vector2Int gridPos = new Vector2Int(Mathf.RoundToInt(mousePos.x), Mathf.RoundToInt(mousePos.y));

        NodeGroup nodeGroup = gridManager.GetNodeGroup(gridPos);
        Vector2Int? position = GetStationPosition(nodeGroup, gridPos);

        if (position != null)
        {
            transform.localScale = new Vector3(1, 1, 1);
            transform.position = new Vector3(position.Value.x, position.Value.y, 0);
            transform.rotation = Quaternion.Euler(0, 0, (int)nodeGroup.GetAlignment() * 45);
        }
        else
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            transform.position = new Vector3(mousePos.x, mousePos.y, 0);
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
    }

    void OnDisable()
    {
        GetComponent<StationPreview>().enabled = false;
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

    public void CreateStation()
    {
        Vector3 mousePos = gridManager.GetMousePosition();
        Vector2Int gridPos = new Vector2Int(Mathf.RoundToInt(mousePos.x), Mathf.RoundToInt(mousePos.y));

        NodeGroup nodeGroup = gridManager.GetNodeGroup(gridPos);
        Vector2Int? position = GetStationPosition(nodeGroup, gridPos);

        if (nodeGroup != null && !nodeGroup.HasStation())
        {
            nodeGroup.SetStation(Instantiate(stationPrefab, new Vector3(position.Value.x, position.Value.y, -0.2f), Quaternion.Euler((int)nodeGroup.GetAlignment() * -45, 90, -90)));
            AudioSource.PlayClipAtPoint(placeSound, Camera.main.transform.position);
        }
        else
        {
            AudioSource.PlayClipAtPoint(errorSound, Camera.main.transform.position);
        }
    }

    public void HandleLeftClick(InputAction.CallbackContext context)
    {
        if(!enabled) return;
        if (context.ReadValue<float>() > 0)
        {
            CreateStation();
        }
    }
}
