using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrainPopup : MonoBehaviour
{
    public GridManager gridManager;
    private Train train;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
    public void OpenTrainPopup(Train train)
    {
        this.train = train;
        gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void HandleClose()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
        train = null;
    }

    public void HandlePausePlayTrain(GameObject button)
    {
        train.ToggleIsMoving();
        button.GetComponentInChildren<TextMeshProUGUI>().SetText(train.IsMoving() ? "Stop Train" : "Move Train");
        //TODO test this
    }


    public void AddStation(string stationName)
    {
        train.schedule.Add(gridManager.GetStationByName(stationName));
    }

    public void UpdateStation(string stationName, int indexOfEntry)
    {
        train.schedule[indexOfEntry] = gridManager.GetStationByName(stationName);
    }

    public void RemoveLastStation()
    {
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
    
    public int GetSelectedStation(int indexOfEntry)
    {
        List<Station> totalStations = gridManager.GetStations();
        if (indexOfEntry >= totalStations.Count)
            throw new NullReferenceException();
        int indexInStationList = totalStations.IndexOf(train.schedule[indexOfEntry]);
        return indexInStationList;
    }
    
    
}
