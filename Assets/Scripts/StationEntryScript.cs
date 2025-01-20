using System;
using TMPro;
using UnityEngine;

public class StationEntryScript : MonoBehaviour
{
    public int indexOfEntry;
    public GameObject trainPopUp;
    public GameObject activatedView;
    public GameObject deactivatedView;
    public GameObject stationSelector;
    public GameObject stationNo;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        activatedView.gameObject.SetActive(false);
        deactivatedView.gameObject.SetActive(true);
        
        stationNo.GetComponent<TextMeshProUGUI>().SetText((indexOfEntry+1).ToString());
    }

    public void UpdateEntry()
    {
        stationSelector.GetComponent<TMP_Dropdown>().ClearOptions();
        stationSelector.GetComponent<TMP_Dropdown>().AddOptions(trainPopUp.GetComponent<TrainPopup>().GetAvailableStationsToString());
        
        int selectedStation = trainPopUp.GetComponent<TrainPopup>().GetSelectedStation(indexOfEntry);

        if (selectedStation >= 0)
        {
            activatedView.gameObject.SetActive(true);
            deactivatedView.gameObject.SetActive(false);
            stationSelector.GetComponent<TMP_Dropdown>().SetValueWithoutNotify(selectedStation);
        }
        else
        {
            activatedView.gameObject.SetActive(false);
            deactivatedView.gameObject.SetActive(true);
        }
        
        
    }
    

    public void HandleAddStationButton()
    {
        activatedView.gameObject.SetActive(true);
        deactivatedView.gameObject.SetActive(false);
        
        trainPopUp.GetComponent<TrainPopup>().AddStation(stationSelector.GetComponent<TMP_Dropdown>().options[stationSelector.GetComponent<TMP_Dropdown>().value].text);
        
        if (indexOfEntry < 9)
            trainPopUp.GetComponent<TrainPopup>().SetEntryActive(indexOfEntry+1,true);
    }

    public void HandleRemoveStationButton()
    {
        activatedView.gameObject.SetActive(false);
        deactivatedView.gameObject.SetActive(true);
        
        trainPopUp.GetComponent<TrainPopup>().RemoveLastStation();
        
        if (indexOfEntry < 9)
            trainPopUp.GetComponent<TrainPopup>().SetEntryActive(indexOfEntry+1,false);
        
        //TODO
        //if not the last entry is deleted, then what???? - fix this if still time!
    }

    public void HandleSelectStationChange()
    {
        trainPopUp.GetComponent<TrainPopup>().UpdateStation(stationSelector.GetComponent<TMP_Dropdown>().options[stationSelector.GetComponent<TMP_Dropdown>().value].text, indexOfEntry);
        
    }
}
