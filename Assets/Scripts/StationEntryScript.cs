using System;
using TMPro;
using UnityEngine;

public class StationEntryDisplayScript : MonoBehaviour
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
        stationSelector.GetComponent<TMP_Dropdown>().ClearOptions();
        stationSelector.GetComponent<TMP_Dropdown>().AddOptions(trainPopUp.GetComponent<TrainPopup>().GetAvailableStationsToString());
        
        
        try
        {
            int selectedStation = trainPopUp.GetComponent<TrainPopup>().GetSelectedStation(indexOfEntry);
            stationSelector.GetComponent<TMP_Dropdown>().SetValueWithoutNotify(selectedStation);
            gameObject.SetActive(true);
        }
        catch (NullReferenceException)
        {
            gameObject.SetActive(false);
        }
        
    }
    

    public void HandleAddStationButton()
    {
        activatedView.gameObject.SetActive(true);
        deactivatedView.gameObject.SetActive(false);
        
        trainPopUp.GetComponent<TrainPopup>().AddStation(stationSelector.GetComponent<TMP_Dropdown>().options[stationSelector.GetComponent<TMP_Dropdown>().value].text);
        
        //TODO handle adding or removing next/previsous entry
    }

    public void HandleRemoveStationButton()
    {
        activatedView.gameObject.SetActive(false);
        deactivatedView.gameObject.SetActive(true);
        
        trainPopUp.GetComponent<TrainPopup>().RemoveLastStation();
        
        //TODO handle adding or removing next/previsous entry
    }

    public void HandleSelectStationChange()
    {
        trainPopUp.GetComponent<TrainPopup>().UpdateStation(stationSelector.GetComponent<TMP_Dropdown>().options[stationSelector.GetComponent<TMP_Dropdown>().value].text, indexOfEntry);
    }
}
