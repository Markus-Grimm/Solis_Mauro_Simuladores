using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TerrainScript : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite plains, mountain, desert, water, forest, snowForest, snowPlains, jungle, volcano, city;
    public int cantMov;
    public bool movement;

    GameObject objectController;
    GameController gameController;

    // Selection
    public GameObject selectPrefab;
    public bool isSelected;
    public string terrainTag;


    void Start()
    {
        selectPrefab = GameObject.Find("Selector");
        isSelected = false;

        objectController = GameObject.Find("GameController");
        gameController = objectController.GetComponent<GameController>();


        terrainTag = this.transform.tag;
                
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        SetSprite();
    }

    private void Update()
    {
        if (gameController.startSim && this.gameObject.tag == "City" && this.gameObject.GetComponent<CityScript>() == null)
        {
            this.gameObject.AddComponent<CityScript>();
        }
    }

    public void SetCity()
    {
        if (gameController.startSim && this.gameObject.tag == "City" && this.gameObject.GetComponent<CityScript>() == null)
        {
            this.gameObject.AddComponent<CityScript>();
        }
    }

    public void SetSprite()
    {
        switch (this.gameObject.tag)
        {
            case "Plains":
                spriteRenderer.sprite = plains;
                movement = true;
                cantMov = 1;
                break;

            case "Mountain":
                spriteRenderer.sprite = mountain;
                movement = false;
                cantMov = 3;
                break;

            case "Desert":
                spriteRenderer.sprite = desert;
                movement = true;
                cantMov = 3;
                break;

            case "Water":
                spriteRenderer.sprite = water;
                movement = false;
                break;

            case "Forest":
                spriteRenderer.sprite = forest;
                movement = true;
                cantMov = 2;
                break;

            case "SnowForest":
                spriteRenderer.sprite = snowForest;
                movement = true;
                cantMov = 3;
                break;

            case "Jungle":
                spriteRenderer.sprite = jungle;
                movement = true;
                cantMov = 2;
                break;

            case "Volcano":
                spriteRenderer.sprite = volcano;
                movement = false;
                break;

            case "SnowPlains":
                spriteRenderer.sprite = snowPlains;
                movement = true;
                cantMov = 2;
                break;

            case "City":
                spriteRenderer.sprite = city;
                movement = true;
                cantMov = 0;
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
        SetSprite();
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
}
