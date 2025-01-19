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
        Time.timeScale = 0;
        stationName.text = station.GetName();
    }

    public void HandleClose()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
        station = null;
    }

    public void HandleAddTrain()
    {
        GameObject train = Instantiate(trainPrefab);
        NodeGroup nodeGroup = station.GetNodeGroup();
        train.GetComponentInChildren<Train>().nodeGroup = nodeGroup;
        HandleClose();
    }

    public void HandleRename()
    {
        station.SetName(stationName.text);
    }
}
