using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class TerrainDetails
{
    public GameObject gameObject;
    public int code;
    public Vector3 coordinates;
    public int[] bonusScore = new int[9];
    public string terrainTag;
    
}

public class TerrainList : MonoBehaviour
{
    [SerializeField] TerrainScript[] terrainList;
    [SerializeField] GameObject terrainPrefab;

    [SerializeField] static List<TerrainDetails> terrainDetails = new List<TerrainDetails>();
    
    public static void Main(TerrainScript[] terrains)
    {
        List<TerrainDetails> initialList = new List<TerrainDetails>();
        
        
        for (int i = 0; i < 174; i++)
        {
            initialList.Add(new TerrainDetails() 
            { 
                gameObject = terrains[i].gameObject, 
                code = i, 
                coordinates = terrains[i].gameObject.transform.position,
                bonusScore = terrains[i].bonus,               
                terrainTag = terrains[i].terrainTag,
            });
        }
        terrainDetails = initialList;
    }

    void Start()
    {
        terrainPrefab = GameObject.FindGameObjectWithTag("TerrainPrefab");

        terrainList = terrainPrefab.GetComponentsInChildren<TerrainScript>();

        Main(terrainList);

    }

    void Update()
    {
        
    }
}
