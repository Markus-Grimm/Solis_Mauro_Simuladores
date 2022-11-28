using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject selectObj;
    
    public int cantCities, maxCities;
    public bool startSim;

    private void Awake()
    {
        
    }

    void Start()
    {
        selectObj = GameObject.Find("Selector");

        if (PlayerPrefs.HasKey("CantCities")) PlayerPrefs.GetInt("CantCities", cantCities);
        else
        {
            cantCities = 0;
            PlayerPrefs.SetInt("CantCities", cantCities);
        }

        if (PlayerPrefs.HasKey("MaxCities") && PlayerPrefs.GetInt("MaxCities", maxCities) > 0) PlayerPrefs.GetInt("MaxCities", maxCities);
        else
        {
            maxCities = 1;
            PlayerPrefs.SetInt("MaxCities", maxCities);
        }

        startSim = false;
    }

    public void StartButton()
    {
        startSim = true;
        selectObj.GetComponent<Select>().HideSelect();
    }

    public void PauseButton()
    {
        startSim = false;
    }

}
