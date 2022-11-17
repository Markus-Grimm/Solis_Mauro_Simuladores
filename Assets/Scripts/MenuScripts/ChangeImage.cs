using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeImage : MonoBehaviour
{
    public GameObject backGroundObj;
    public Image backGround;
    public Sprite city, plains, desert, snowForest, jungle, mountains;
    public Dropdown mapList;

    private void Start()
    {
        backGroundObj = GameObject.Find("ImageBackground");
        backGround = backGroundObj.GetComponent<Image>();
    }

    public void Image()
    {
        switch (mapList.value)
        {
            case 0:
                PlayerPrefs.SetString("MapChoosen", "Plains");
                break;
            
            case 1:
                backGround.sprite = plains;
                PlayerPrefs.SetString("MapChoosen", "Plains");
                break;

            case 2:
                backGround.sprite = desert;
                PlayerPrefs.SetString("MapChoosen", "Desert");
                break;

            case 3:
                backGround.sprite = snowForest;
                PlayerPrefs.SetString("MapChoosen", "SnowForest");
                break;

            case 4:
                backGround.sprite = jungle;
                PlayerPrefs.SetString("MapChoosen", "Jungle");
                break;

            case 5:
                backGround.sprite = mountains;
                PlayerPrefs.SetString("MapChoosen", "Mountain");
                break;

            default:                
                break;
        }
        
    }

}
