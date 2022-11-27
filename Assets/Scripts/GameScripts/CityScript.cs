using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CityScript : MonoBehaviour
{
    public GameObject objectData, terrainObj;
    public GameObject[] limiTerrains = new GameObject[6];

    [SerializeField] DataManager dataManager;
    [SerializeField] TerrainScript terrainScript;

    //public GameObject[] territory = new GameObject[174];

    void Start()
    {
        objectData = GameObject.Find("GameController");
        dataManager = objectData.GetComponent<DataManager>();

        terrainObj = this.gameObject;
        terrainScript = terrainObj.GetComponent<TerrainScript>();
    }
    

    
}
