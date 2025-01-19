using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StationPopup : MonoBehaviour
{
    private Station station;
    public TMP_InputField stationName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
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
        Debug.Log("Add Train");
    }

    public void HandleRename()
    {
        station.SetName(stationName.text);
    }
}
