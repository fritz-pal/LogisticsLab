using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = System.Random;

public class StationPreview : MonoBehaviour
{
    public GridManager gridManager;
    public GameObject stationPrefab;
    public AudioClip errorSound;
    public AudioClip placeSound;
    public StationPopup stationPopup;

    void Update()
    {
        Vector3? mousePos = gridManager.GetMousePosition(false);
        if (mousePos == null) return;
        Vector2Int gridPos = new(Mathf.RoundToInt(mousePos.Value.x), Mathf.RoundToInt(mousePos.Value.y));

        NodeGroup nodeGroup = gridManager.GetNodeGroup(gridPos);
        Vector2Int? position = GetStationPosition(nodeGroup, gridPos);

        gameObject.SetActive(!GridManager.Instance.menuOpen);
        if (position != null)
        {
            transform.localScale = new Vector3(1, 1, 1);
            transform.SetPositionAndRotation(new Vector3(position.Value.x, position.Value.y, 0), Quaternion.Euler(0, 0, (int)nodeGroup.GetAlignment() * 45));
        }
        else
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            transform.SetPositionAndRotation(new Vector3(mousePos.Value.x, mousePos.Value.y, 0), Quaternion.Euler(0, 0, 90));
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
        Vector3? mousePos = gridManager.GetMousePosition(true);
        if (mousePos == null) return;
        Vector2Int gridPos = new Vector2Int(Mathf.RoundToInt(mousePos.Value.x), Mathf.RoundToInt(mousePos.Value.y));

        NodeGroup nodeGroup = gridManager.GetNodeGroup(gridPos);
        Vector2Int? position = GetStationPosition(nodeGroup, gridPos);

        if (nodeGroup != null && !nodeGroup.HasStation())
        {
            GameObject obj = Instantiate(stationPrefab, new Vector3(position.Value.x, position.Value.y, -0.2f), Quaternion.Euler((int)nodeGroup.GetAlignment() * -45, 90, -90));
            Station station = new(position.Value, nodeGroup, GetRandomStationName(), obj);
            GridManager.Instance.AddStation(station);
            nodeGroup.SetStation(station);
            AudioSource.PlayClipAtPoint(placeSound, Camera.main.transform.position);
        }
        else
        {
            if (nodeGroup != null)
            {
                stationPopup.OpenStationPopup(nodeGroup.GetStation());
            }
            AudioSource.PlayClipAtPoint(errorSound, Camera.main.transform.position);
        }
    }

    private string RandomString()
    {
        return Guid.NewGuid().ToString("N")[..8];
    }

    public void HandleLeftClick(InputAction.CallbackContext context)
    {
        if (!enabled) return;
        if (context.ReadValue<float>() > 0)
        {
            CreateStation();
        }
    }

    private string GetRandomStationName()
    {
        string randomStationName = StationNamesLoader.GetRandomName();
        foreach (Station s in gridManager.GetStations())
        {
            if (s.GetName() == randomStationName)
                return GetRandomStationName();
        }
        return randomStationName;
    }
    
}
