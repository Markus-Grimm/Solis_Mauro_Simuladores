using System.Collections.Generic;
using UnityEngine;

public class TerrainDetails
{
    public GameObject gameObj;
    public int code;
    public Vector3 coordinates;
    public int[] bonusScore = new int[9];
    public string terrainTag;

}

public class TerrainList : MonoBehaviour
{
    public TerrainScript[] terrainList;
    public GameObject terrainPrefab;

    public static List<TerrainDetails> terrainDetails = new List<TerrainDetails>();

    public static void Main(TerrainScript[] terrains)
    {
        List<TerrainDetails> initialList = new List<TerrainDetails>();

        for (int i = 0; i < 174; i++)
        {
            initialList.Add(new TerrainDetails()
            {
                gameObj = terrains[i].gameObject,
                code = i,
                coordinates = terrains[i].gameObject.transform.position,
                bonusScore = terrains[i].bonus,
                terrainTag = terrains[i].terrainTag,
            });
            terrains[i].idNumber = i;
        }
        terrainDetails = initialList;
    }

    public void Awake()
    {
        terrainList.Initialize(); // Reset
        terrainPrefab = GameObject.FindGameObjectWithTag("TerrainPrefab");

        terrainList = terrainPrefab.GetComponentsInChildren<TerrainScript>();

        Main(terrainList);
    }
}