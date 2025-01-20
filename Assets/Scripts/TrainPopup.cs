using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrainPopup : MonoBehaviour
{
    public GridManager gridManager;
    public GameObject stationList;
    public GameObject stationEntry;
    public GameObject pausePlayBtnLabel;
    private Train train;
    private List<GameObject> stationEntries;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.SetActive(false);
        InitializeStationList();
        stationEntries[0].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitializeStationList()
    {
        stationEntries = new List<GameObject>();
        for (int i = 0; i < 10; i++)
        {
            GameObject obj = Instantiate(stationEntry, stationList.transform);
            obj.GetComponent<StationEntryScript>().indexOfEntry = i;
            obj.GetComponent<RectTransform>().anchoredPosition = new Vector3(22.5f, (115f - (i*25f)) , 0f);
            stationEntries.Add(obj);
        }
        
    }
    
    public void OpenTrainPopup(Train train)
    {
        this.train = train;
        gameObject.SetActive(true);
        updatePausePlayBtnLabel();
        Time.timeScale = 0;
        
        //fill entries:
        foreach (GameObject entry in stationEntries)
        {
            entry.GetComponent<StationEntryScript>().UpdateEntry();
        }
    }

    public void HandleClose()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
        train = null;
    }

    public void HandlePausePlayTrain()
    {
        train.ToggleIsRunning();
        updatePausePlayBtnLabel();
    }

    private void updatePausePlayBtnLabel()
    {
        pausePlayBtnLabel.GetComponent<TextMeshProUGUI>().SetText(train.trainIsRunning ? "Stop Train" : "Run Train");
    }

    public void HandleDeleteTrain()
    {
        Destroy(train.transform.parent.gameObject);
        HandleClose();
    }

    public void AddStation(string stationName)
    {
        Debug.Log(stationName);
        Debug.Log(train);
        Debug.Log(train.schedule);
        train.schedule.Add(gridManager.GetStationByName(stationName));
    }

    public void UpdateStation(string stationName, int indexOfEntry)
    {
        train.schedule[indexOfEntry] = gridManager.GetStationByName(stationName);
    }

    public void RemoveLastStation()
    {
        if (train.schedule.Count > 0)
            train.schedule.RemoveAt(train.schedule.Count - 1);
    }

    public List<string> GetAvailableStationsToString()
    {
        List<string> stationNames = new List<string>();
        foreach (var station in gridManager.GetStations())
        {
            stationNames.Add(station.GetName());
        }
        return stationNames;
    }
    
    /**
     * Takes the index of the schedule entry (0-9)
     * returns the index, that identifies the station from the list of ALL STATIONS (needed to select station from dropdown)
     * returns -1 if the schedule entry is empty and shouldnt be displayed
     */
    public int GetSelectedStation(int indexOfEntry)
    {
        List<Station> totalStations = gridManager.GetStations();
        if (indexOfEntry >= train.schedule.Count)
            return -1;
        return totalStations.IndexOf(train.schedule[indexOfEntry]);
    }
    
    
}
