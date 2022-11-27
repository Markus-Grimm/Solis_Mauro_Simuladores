using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TerrainScript : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite plains, mountain, desert, water, forest, snowForest, snowPlains, jungle, volcano, city;      
    public bool movement, conquest;

    GameObject objectController;
    GameController gameController;
    DataManager dataManager;

    // Selection
    public GameObject selectPrefab;
    public bool isSelected;
    public string terrainTag;

    //econBonus, miliBonus, urbaBonus, popuBonus, secuBonus, expaBonus, happBonus, rebeBonus, collBonus;
    public int[] bonus = new int[9];

    //Terrain limits
    public int idNumber;
    public GameObject[] terrainLimit = new GameObject[6];
    public TerrainList objectList;
    public bool isCity;
    
    void Start()
    {
        selectPrefab = GameObject.Find("Selector");
        isSelected = false;

        objectController = GameObject.Find("GameController");
        gameController = objectController.GetComponent<GameController>();
        dataManager = objectController.GetComponent<DataManager>();
        objectList = objectController.GetComponent<TerrainList>();
                        
        terrainTag = transform.tag;

        isCity = false;
                
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        SetTerrainDetails();

        SetLimitTerrain();
    }

    public void SetCity()
    {
        if (this.GetComponent<CityScript>() == null) 
        {
            isCity = true;

            this.gameObject.AddComponent<CityScript>();
            //this.gameObject.GetComponent<CityScript>().territory[0] = this.gameObject;
        }
    }

    public void SetTerrainDetails()
    {
        switch (this.gameObject.tag)
        {
            case "Plains":
                spriteRenderer.sprite = plains;
                movement = true;
                conquest = false;
                bonus = new int [9]{ 3, 1, 2, 3, 1, 2, 2, -1, -1};
                // Econ1, Mili2, Urba3, Popu4, Secu5, Expa6, Happ7, Rebe8, Coll9
                break;

            case "Mountain":
                spriteRenderer.sprite = mountain;
                movement = false;
                conquest = false;
                bonus = new int[9] { 2, 3, 1, 0, 0, 2, 1, 0, 0};
                // Econ, Mili, Urba, Popu, Secu, Expa, Happ, Rebe, Coll
                break;

            case "Desert":
                spriteRenderer.sprite = desert;
                movement = true;
                conquest = false;
                bonus = new int[9] { -1, 2, -1, 0, 2, -1, -1, 1, 2};
                // Econ, Mili, Urba, Popu, Secu, Expa, Happ, Rebe, Coll
                break;

            case "Water":
                spriteRenderer.sprite = water;
                movement = false;
                conquest = false;
                bonus = new int[9] { 6, 0, 3, 4, 0, -1, 3, -2, -2};
                // Econ, Mili, Urba, Popu, Secu, Expa, Happ, Rebe, Coll
                break;

            case "Forest":
                spriteRenderer.sprite = forest;
                movement = true;
                conquest = false;
                bonus = new int[9] { 5, 1, 4, 3, -1, 2, 3, -2, -2};
                // Econ, Mili, Urba, Popu, Secu, Expa, Happ, Rebe, Coll
                break;

            case "SnowForest":
                spriteRenderer.sprite = snowForest;
                movement = true;
                conquest = false;
                bonus = new int[9] { 4, 0, 3, 2, -2, 0, 2, -1, -1};
                // Econ, Mili, Urba, Popu, Secu, Expa, Happ, Rebe, Coll
                break;

            case "Jungle":
                spriteRenderer.sprite = jungle;
                movement = true;
                conquest = false;
                bonus = new int[9] { 6, -1, 1, 4, -2, -1, 5, -1, 0};
                // Econ, Mili, Urba, Popu, Secu, Expa, Happ, Rebe, Coll
                break;

            case "Volcano":
                spriteRenderer.sprite = volcano;
                movement = false;
                conquest = false;
                bonus = new int[9] { -2, 5, -4, 0, 0, -1, -1, 2, 0};
                // Econ, Mili, Urba, Popu, Secu, Expa, Happ, Rebe, Coll
                break;

            case "SnowPlains":
                spriteRenderer.sprite = snowPlains;
                movement = true;
                conquest = false;
                bonus = new int[9] { -1, 4, 1, -1, 3, 1, 1, -1, -1};
                // Econ, Mili, Urba, Popu, Secu, Expa, Happ, Rebe, Coll
                break;

            case "City":
                spriteRenderer.sprite = city;
                movement = true;
                conquest = true;
                bonus = new int[9] { 1, 1, 1, 1, 1, 1, 1, -1, -1};
                // Econ0, Mili1, Urba2, Popu3, Secu4, Expa5, Happ6, Rebe7, Coll8
                break;

            default:
                break;
        }
    }
        
    private void SelectTerrain(bool select, bool contador, string tag)
    {
        isSelected = select;
        if (contador && gameController.cantCities < gameController.maxCities)
        {
            gameController.cantCities++;
        }
        else if (gameController.cantCities > 0)
        {
            gameController.cantCities--;
        }
        this.gameObject.tag = tag;
        SetTerrainDetails();
        PlayerPrefs.SetInt("CantCities", gameController.cantCities);
    }
    
    private void OnMouseDown()
    {
        if (!gameController.startSim)
        {
            if (this.gameObject.tag != "Mountain" && this.gameObject.tag != "Water" && this.gameObject.tag != "Volcano")
            {
                if (this.gameObject.tag == "City")
                {
                    SelectTerrain(false, false, terrainTag);
                }
                else
                {
                    if (gameController.cantCities < gameController.maxCities && gameController.cantCities >= 0)
                    {
                        if (!isSelected)
                        {
                            SelectTerrain(true, true, "City");
                        }
                    }
                }
            }
        }
    }

    private void OnMouseEnter()
    {
        if (!gameController.startSim)
        {
            selectPrefab.GetComponent<Select>().Translate(this.transform.position);
        }        
    }

    private void SetLimitTerrain()
    {
        // East
        if (idNumber + 1 <= objectList.terrainList.Length - 1)
            terrainLimit[0] = objectList.terrainList[idNumber + 1].gameObject;
        else terrainLimit[0] = null;
        // SE
        if (idNumber + 15 <= objectList.terrainList.Length - 1)
            terrainLimit[1] = objectList.terrainList[idNumber + 15].gameObject;
        else terrainLimit[1] = null;
        // SW
        if (idNumber + 14 <= objectList.terrainList.Length - 1)
            terrainLimit[2] = objectList.terrainList[idNumber + 14].gameObject;
        else terrainLimit[2] = null;
        // West
        if (idNumber - 1 >= 0)
            terrainLimit[3] = objectList.terrainList[idNumber - 1].gameObject;
        else terrainLimit[3] = null;
        // NW
        if (idNumber - 15 >= 0)
            terrainLimit[4] = objectList.terrainList[idNumber - 15].gameObject;
        else terrainLimit[4] = null;
        // NE
        if (idNumber - 14 >= 0)
            terrainLimit[5] = objectList.terrainList[idNumber - 14].gameObject;
        else terrainLimit[5] = null;
    }

}
