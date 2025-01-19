using System;
using System.Collections.Generic;
using UnityEngine;

public class TrainPopup : MonoBehaviour
{
    //TODO add train object
    public GridManager gridManager;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
    public void OpenTrainPopup(/*TODO add train*/)
    {
        //TODO train object
        gameObject.SetActive(true);
        Time.timeScale = 0;
        //TODO fill out ui with data from Train: List of Stations from the schedule, Button if the train is stopped or not
    }

    public void HandleClose()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
        //TODO set train null
    }

    public void HandlePausePlayTrain()
    {
        //TODO
        //probably update train object, -> attribute that says if its moving or not
        //update the button to say stop or resume
    }


    public void AddStation(string stationName)
    {
        //TODO
    }

    public void UpdateStation(string stationName)
    {
        //TODO
    }

    public void RemoveLastStation()
    {
        //TODO
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
        List<Station> schedule = new List<Station>(); //TODO get List of Stations from current train instead
        List<Station> totalStations = gridManager.GetStations();

        if (indexOfEntry >= totalStations.Count)
            throw new NullReferenceException();
        
        int indexInStationList = totalStations.IndexOf(schedule[indexOfEntry]);
        return indexInStationList;
    }
    
    
}
