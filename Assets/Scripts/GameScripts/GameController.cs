using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject sliderGameobj, selectObj;
    public Slider sliderSpeed;
    public int speed, cantCities, maxCities;
    public bool startSim;

    void Start()
    {        
        sliderGameobj = GameObject.Find("SpeedSlider");
        sliderSpeed = sliderGameobj.GetComponent<Slider>();

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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            PlayerPrefs.DeleteAll();
        }
    }

    public void StartButton()
    {
        startSim = true;
        selectObj.GetComponent<Select>().HideSelect();
    }

    public void Interface()
    {

    }

    public void SpeedControlling()
    {
        speed = Mathf.RoundToInt(sliderSpeed.value);
        Debug.Log(speed);
    }
}
