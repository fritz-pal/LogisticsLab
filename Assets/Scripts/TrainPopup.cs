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
        gridManager.menuOpen = true;
        
        //fill entries:
        int i = 0;
        foreach (GameObject entry in stationEntries)
        {
            entry.SetActive(false);
            entry.GetComponent<StationEntryScript>().UpdateEntry();
            if (i == 0 || i <= train.schedule.Count)
                entry.SetActive(true);
            else
                entry.SetActive(false);
            i++;
        }
    }

    public void SetEntryActive(int index, bool active)
    {
        stationEntries[index].SetActive(active);
    }

    public void StopTrain()
    {
        train.isRunning = false;
    }

    public void HandleClose()
    {
        gameObject.SetActive(false);
        gridManager.menuOpen = false;
        train = null;
    }

    public void HandlePausePlayTrain()
    {
        train.ToggleIsRunning();
        updatePausePlayBtnLabel();
    }

    private void updatePausePlayBtnLabel()
    {
        pausePlayBtnLabel.GetComponent<TextMeshProUGUI>().SetText(train.isRunning ? "Stop Train" : "Run Train");
    }

    public void HandleDeleteTrain()
    {
        Destroy(train.transform.parent.gameObject);
        HandleClose();
    }

    public void AddStation(string stationName)
    {
        train.schedule.Add(gridManager.GetStationByName(stationName));
        StopTrain();
    }

    public void UpdateStation(string stationName, int indexOfEntry)
    {
        train.schedule[indexOfEntry] = gridManager.GetStationByName(stationName);
        StopTrain();
    }

    public void RemoveLastStation()
    {
        if (train.schedule.Count > 0)
            train.schedule.RemoveAt(train.schedule.Count - 1);
        StopTrain();
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
