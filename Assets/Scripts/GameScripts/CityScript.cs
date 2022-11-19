using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CityScript : MonoBehaviour
{
    public float economy, militarism, urbanism;
    public Text poputxt, secutxt, expIndtxt, happtxt, rebIndtxt, collIndtxt;
    public GameObject population, security, expanIndex, happines, rebelIndex, collapseIndex, objectData, terrainObj;
    public GameObject[] limiTerrains = new GameObject[6];

    private bool collDetect;

    [SerializeField] DataManager dataManager;
    [SerializeField] TerrainScript terrainScript;


    void Start()
    {
        objectData = GameObject.Find("GameController");
        dataManager = objectData.GetComponent<DataManager>();

        economy = dataManager.econ;
        militarism = dataManager.mili;
        urbanism = dataManager.urba;

        population = GameObject.Find("Population");
        poputxt = population.GetComponent<Text>();

        security = GameObject.Find("Security");
        secutxt = security.GetComponent<Text>();

        expanIndex = GameObject.Find("Expansionindex");
        expIndtxt = expanIndex.GetComponent<Text>();

        happines = GameObject.Find("Happiness");
        happtxt = happines.GetComponent<Text>();

        rebelIndex = GameObject.Find("Rebellionindex");
        rebIndtxt = rebelIndex.GetComponent<Text>();

        collapseIndex = GameObject.Find("Collapseindex");
        collIndtxt = collapseIndex.GetComponent<Text>();

        terrainObj = this.gameObject;
        terrainScript = terrainObj.GetComponent<TerrainScript>();

        collDetect = true;

    }

    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collDetect)
        {
            for (int i = 0; i < limiTerrains.Length - 1; i++)
            {
                if (limiTerrains[i] == null)
                {
                    limiTerrains[i] = collision.gameObject;
                    Debug.Log(collision.gameObject.tag);
                    continue;
                }
                else if (limiTerrains[i].gameObject != collision.gameObject)
                {
                    limiTerrains[i] = collision.gameObject;
                    Debug.Log(collision.gameObject.tag);
                    continue; 
                }
            }
            collDetect = false;
        }
        
    }
    

    
}
