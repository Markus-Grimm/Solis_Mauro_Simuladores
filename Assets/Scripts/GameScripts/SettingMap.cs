using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingMap : MonoBehaviour
{
    public GameObject[] terrains;
    public Text economy, militarism, urbanism;

    void Start()
    {
        ActiveMap();
    }

    private void ActiveMap()
    {
        terrains[0].SetActive(false);
        terrains[1].SetActive(false);
        terrains[2].SetActive(false);
        terrains[3].SetActive(false);
        terrains[4].SetActive(false);

        switch (PlayerPrefs.GetString("MapChoosen"))
        {
            case "Plains":
                terrains[0].SetActive(true);
                break;

            case "Desert":
                terrains[1].SetActive(true);
                break;

            case "SnowForest":
                terrains[2].SetActive(true);
                break;

            case "Jungle":
                terrains[3].SetActive(true);
                break;

            case "Mountain":
                terrains[4].SetActive(true);
                break;


            default:
                break;
        }
    }

    void Update()
    {
        
    }
}
