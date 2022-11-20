using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CityScript : MonoBehaviour
{
    public GameObject objectData, terrainObj;
    public GameObject[] limiTerrains = new GameObject[6];

    private bool collDetect;

    [SerializeField] DataManager dataManager;
    [SerializeField] TerrainScript terrainScript;


    void Start()
    {
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
