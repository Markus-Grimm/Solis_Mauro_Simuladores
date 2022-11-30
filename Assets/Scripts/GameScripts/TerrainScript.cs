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

    //econBonus, miliBonus, urbaBonus, popuBonus, resoBonus, secuBonus, expaBonus, happBonus, rebeBonus, collBonus;
    public int[] bonus = new int[10];

    //Terrain limits
    public int idNumber;
    public GameObject[] terrainLimit = new GameObject[6];
    public TerrainList objectList;
    public bool isCity;
    
    void Start()
    {
        bonus.Initialize(); // Reset
        terrainLimit.Initialize(); // Reset

        selectPrefab = GameObject.Find("Selector");
        isSelected = false;

        objectController = GameObject.Find("GameController");
        gameController = objectController.GetComponent<GameController>();
        dataManager = objectController.GetComponent<DataManager>();
        objectList = objectController.GetComponent<TerrainList>();
                        
        terrainTag = transform.tag;

        isCity = false;
                
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(0.7f, 0.7f, 0.7f, 1f);
        SetTerrainDetails();

        SetLimitTerrain();
    }

    public void SetCity()
    {        
        isCity = true;
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
    }


    public void SetTerrainDetails()
    {
        switch (this.gameObject.tag)
        {
            case "Plains":
                spriteRenderer.sprite = plains;
                movement = true;
                conquest = false;
                bonus = new int [10]{ 3, 1, 2, 3, 4, 1, 2, 2, 0, 0};
                // Econ1, Mili2, Urba3, Popu4, Reso5, Secu6, Expa7, Happ8, Rebe9, Coll10
                break;

            case "Mountain":
                spriteRenderer.sprite = mountain;
                movement = false;
                conquest = false;
                bonus = new int[10] { 2, 3, 1, 0, 6, 0, 2, 1, 1, 1};
                // Econ1, Mili2, Urba3, Popu4, Reso5, Secu6, Expa7, Happ8, Rebe9, Coll10
                break;

            case "Desert":
                spriteRenderer.sprite = desert;
                movement = true;
                conquest = false;
                bonus = new int[10] { -1, 2, -1, 0, 1, 2, -1, -1, 2, 3};
                // Econ1, Mili2, Urba3, Popu4, Reso5, Secu6, Expa7, Happ8, Rebe9, Coll10
                break;

            case "Water":
                spriteRenderer.sprite = water;
                movement = false;
                conquest = false;
                bonus = new int[10] { 6, 0, 3, 4, 4, 0, -1, 3, -1, -1};
                // Econ1, Mili2, Urba3, Popu4, Reso5, Secu6, Expa7, Happ8, Rebe9, Coll10
                break;

            case "Forest":
                spriteRenderer.sprite = forest;
                movement = true;
                conquest = false;
                bonus = new int[10] { 5, 1, 4, 3, 3, -1, 2, 3, -1, -1};
                // Econ1, Mili2, Urba3, Popu4, Reso5, Secu6, Expa7, Happ8, Rebe9, Coll10
                break;

            case "SnowForest":
                spriteRenderer.sprite = snowForest;
                movement = true;
                conquest = false;
                bonus = new int[10] { 4, 0, 3, 2, 2, -2, 0, 2, 0, 0};
                // Econ1, Mili2, Urba3, Popu4, Reso5, Secu6, Expa7, Happ8, Rebe9, Coll10
                break;

            case "Jungle":
                spriteRenderer.sprite = jungle;
                movement = true;
                conquest = false;
                bonus = new int[10] { 6, -1, 1, 2, 4, -2, -1, 5, 0, 1};
                // Econ1, Mili2, Urba3, Popu4, Reso5, Secu6, Expa7, Happ8, Rebe9, Coll10
                break;

            case "Volcano":
                spriteRenderer.sprite = volcano;
                movement = false;
                conquest = false;
                bonus = new int[10] { -2, 5, -4, 0, 6, 0, -1, -1, 3, 1};
                // Econ1, Mili2, Urba3, Popu4, Reso5, Secu6, Expa7, Happ8, Rebe9, Coll10
                break;

            case "SnowPlains":
                spriteRenderer.sprite = snowPlains;
                movement = true;
                conquest = false;
                bonus = new int[10] { -1, 4, 1, -1, -1, 3, 1, 1, 0, 0};
                // Econ1, Mili2, Urba3, Popu4, Reso5, Secu6, Expa7, Happ8, Rebe9, Coll10
                break;

            case "City":
                spriteRenderer.sprite = city;
                movement = true;
                conquest = true;
                bonus = new int[10] { 1, 1, 1, 1, 1, 1, 1, 1, -1, -1};
                // Econ1, Mili2, Urba3, Popu4, Reso5, Secu6, Expa7, Happ8, Rebe9, Coll10
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
            for (int i = 0; i < 9; i++)
            {
                dataManager.bonus[i] =+ bonus[i];
            }
        }
        else if (gameController.cantCities > 0)
        {
            gameController.cantCities--;
            for (int i = 0; i < 9; i++)
            {
                dataManager.bonus[i] = 0;
            }
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
