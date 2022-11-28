using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingMap : MonoBehaviour
{
    public GameObject[] terrains;
    public Text economy, militarism, urbanism;
    public TerrainList terrainList;

    void Awake()
    {
        terrainList = GetComponent<TerrainList>();
        ActiveMap();
    }

    private void ActiveMap()
    {
        switch (PlayerPrefs.GetString("MapChoosen"))
        {
            case "Plains":
                Instantiate(terrains[0], new Vector3(0, 0, 0), Quaternion.identity);
                break;

            case "Desert":
                Instantiate(terrains[1], new Vector3(0, 0, 0), Quaternion.identity);
                break;

            case "SnowForest":
                Instantiate(terrains[2], new Vector3(0, 0, 0), Quaternion.identity);
                break;

            case "Jungle":
                Instantiate(terrains[3], new Vector3(0, 0, 0), Quaternion.identity);
                break;

            case "Mountain":
                Instantiate(terrains[4], new Vector3(0, 0, 0), Quaternion.identity);
                break;

            default:
                break;
        }
    }

}
