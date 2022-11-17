using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CityScript : MonoBehaviour
{
    public float economy, militarism, urbanism;
    public Text population, security, expansionIndex, happiness, rebellionIndex, collapse;

    GameObject objectController;
    GameController gameController;


    void Start()
    {
        objectController = GameObject.Find("GameController");
        gameController = objectController.GetComponent<GameController>();

    }

    void Update()
    {
        
    }
}
