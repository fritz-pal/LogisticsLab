using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StationPopup : MonoBehaviour
{
    private Station station;
    public TMP_InputField stationName;
    public GameObject trainPrefab;

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void OpenStationPopup(Station station)
    {
        this.station = station;
        gameObject.SetActive(true);
        stationName.text = station.GetName();
        GridManager.Instance.menuOpen = true;
    }

    public void HandleClose()
    {
        gameObject.SetActive(false);
        station = null;
        GridManager.Instance.menuOpen = false;
    }

    public void HandleAddTrain()
    {
        GameObject train = Instantiate(trainPrefab);
        NodeGroup nodeGroup = station.GetNodeGroup();
        train.GetComponentInChildren<Train>().currentStation = station;
        train.GetComponentInChildren<Train>().direction = nodeGroup.GetAlignment();
        HandleClose();
    }

    public void HandleRename()
    {
        station.SetName(stationName.text);
    }
}
