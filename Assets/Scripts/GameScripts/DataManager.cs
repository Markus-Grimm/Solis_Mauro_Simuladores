using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DataManager : MonoBehaviour
{
    [Header("Economy Input")]
    public float _econ;
    [Header("Military Input")]
    public float _mili;
    [Header("Urbanism Input")]
    public float _urba;
    [Header("Output Data")]
    public int econ;
    public int mili, urba, popu, reso, secu, expInd, happ, rebInd, collInd;
    [Header("Not Visible Data")]
    public int value;
    public int minValue, maxValue, econValue, miliValue, urbaValue, resoValue, limit, range, territories;
    public float econPercent, miliPercent, urbaPercent, econTimer, miliTimer, urbaTimer;
    public int[] bonus = new int[9];

    [Header("GameObjects and Texts")]
    public GameObject economy;
    public GameObject militarism, urbanism, population, resources, security, expanIndex, happines, rebelIndex, collapseIndex;
    public Text econtxt, militxt, urbatxt, poputxt, resotxt, secutxt, expIndtxt, happtxt, rebIndtxt, collIndtxt;


    //Change values in time
    [Header("Slider and Speed")]
    public Slider sliderSpeed;
    public GameObject sliderGameobj, speedObj;
    public Text speedText;
    public int speedMult, speed;

    [Header("Events and Time")]
    public GameObject cityEvent;
    public GameObject collapseObj, conquestObj, timeObj;
    public Text cityEventtxt, collapsetxt, conquesttxt, timetxt;
    public float time;

    [Header("Scripts")]
    public GameObject gc;
    public GameObject city;
    public TerrainList terrainList;
    public TerrainScript terrainScript;

    [Header("Pause boolean")]
    public bool pause, collBool, startSim;


    private void Awake()
    {        
        gc = GameObject.Find("GameController");
        
        terrainList = gc.GetComponent<TerrainList>();

        if (PlayerPrefs.HasKey("Economy"))
        {
            _econ = PlayerPrefs.GetFloat("Economy");
            econ = Mathf.RoundToInt(_econ);
        } else econ = Random.Range(1, 333);

        if (PlayerPrefs.HasKey("Military"))
        {
            _mili = PlayerPrefs.GetFloat("Military");
            mili = Mathf.RoundToInt(_mili);
        } else mili = Random.Range(1, 333);

        if (PlayerPrefs.HasKey("Urbanism"))
        {
            _urba = PlayerPrefs.GetFloat("Urbanism");
            urba = Mathf.RoundToInt(_urba);
        } else urba = Random.Range(1, 333);

        econPercent = 0.2f;
        miliPercent = 0.2f;
        urbaPercent = 0.2f;

        collBool = false;
        pause = false;
        startSim = false;

        speedMult = 100;

        minValue = 1;
        maxValue = 1000;

        limit = 2000;

        value = 0;
        range = 0;
        territories = 0;

        economy = GameObject.Find("Economy");
        econtxt = economy.GetComponent<Text>();
        econtxt.text = "Economy: " + econ;

        militarism = GameObject.Find("Militarism");
        militxt = militarism.GetComponent<Text>();
        militxt.text = "Militarism: " + mili;

        urbanism = GameObject.Find("Urbanism");
        urbatxt = urbanism.GetComponent<Text>();
        urbatxt.text = "Urbanism: " + urba;

        population = GameObject.Find("Population");
        poputxt = population.GetComponent<Text>();
        popu = Mathf.RoundToInt(econPercent * econ);
        poputxt.text = "Population: " + popu;

        resources = GameObject.Find("Resources");
        resotxt = resources.GetComponent<Text>();
        reso = Mathf.RoundToInt((1 - econPercent) * econ);
        resotxt.text = "Resources: " + reso;

        security = GameObject.Find("Security");
        secutxt = security.GetComponent<Text>();
        secu = Mathf.RoundToInt(miliPercent * mili);
        secutxt.text = "Security: " + secu;

        expanIndex = GameObject.Find("Expansionindex");
        expIndtxt = expanIndex.GetComponent<Text>();
        expInd = Mathf.RoundToInt(miliPercent * mili);
        expIndtxt.text = "Expansion index: " + expInd;

        happines = GameObject.Find("Happiness");
        happtxt = happines.GetComponent<Text>();
        happ = Mathf.RoundToInt((urbaPercent * 2) * urba);
        happtxt.text = "Happiness: " + happ;

        rebelIndex = GameObject.Find("Rebellionindex");
        rebIndtxt = rebelIndex.GetComponent<Text>();
        rebInd = 0;
        rebIndtxt.text = "Rebellion index: " + rebInd;

        collapseIndex = GameObject.Find("Collapseindex");
        collIndtxt = collapseIndex.GetComponent<Text>();
        collInd = 0;
        collIndtxt.text = "Collapse index: " + collInd;

        sliderGameobj = GameObject.FindGameObjectWithTag("Slider");
        sliderSpeed = sliderGameobj.GetComponent<Slider>();
        sliderSpeed.value = 1f;
        speedObj = GameObject.FindGameObjectWithTag("Speed");
        speedText = speedObj.GetComponent<Text>();
        speed = 1;
        speedText.text = "X" + 1;

        cityEvent = GameObject.Find("CityEvents");
        cityEventtxt = cityEvent.GetComponent<Text>();

        collapseObj = GameObject.Find("CollapseSignal");
        collapsetxt = collapseObj.GetComponent<Text>();

        conquestObj = GameObject.Find("ConquestSignal");
        conquesttxt = conquestObj.GetComponent<Text>(); 

        timeObj = GameObject.Find("Time");
        timetxt = timeObj.GetComponent<Text>();
        time = 0f;
                
        econTimer = (maxValue - econ) / (speed * speedMult);
        miliTimer = (maxValue - mili) / (speed * speedMult);
        urbaTimer = (maxValue - urba) / (speed * speedMult);
    }

    private void Update()
    {
        if (time >= limit)
        {
            if (econ >= limit) Conquest("Victory\r\n" + "The economic progress of your city became history");
            else if (mili >= limit) Conquest("Victory\r\n" + "Your city's military progress has gone down in history");
            else if (urba >= limit) Conquest("Victory\r\n" + "The urban progress of your city went down in history");
            else Collapse("Your city has not progressed enough over time\r\nThe city has collapsed");
        }

        if (territories >= 90)
        {
            Conquest("Victory\r\n" + "You have conquered most of the nearby territory");
        }

        if (popu <= 0)
        {
            Collapse("Your citizens abandoned the city and it is now uninhabited.\r\nThe city has collapsed");
        }

        if (Input.GetKey(KeyCode.E))
        {
            econ = limit;
            econtxt.text = "Economy: " + econ;
        }
        if (Input.GetKey(KeyCode.M))
        {
            mili = limit;
            militxt.text = "Militarism: " + mili;
        }
        if (Input.GetKey(KeyCode.U))
        {
            urba = limit;
            urbatxt.text = "Urbanism: " + urba;
        }
        if (Input.GetKey(KeyCode.D))
        {
            popu = 0;
            poputxt.text = "Population: " + popu;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            ExpansionTerritory();
            territories++;
        }
        if (Input.GetKey(KeyCode.T))
        {
            time = limit;
            timetxt.text = "Year: " + time;
        }

        if (expInd >= 100)
        {
            expInd = -100;

        }
    }

    public void StartSim()
    {
        if (!pause)
        {
            city = GameObject.FindGameObjectWithTag("City");
            city.GetComponent<TerrainScript>();
            terrainScript = city.GetComponent<TerrainScript>();
            territories++;

            bonus[0] = bonus[0] + terrainScript.bonus[0];
            bonus[1] = bonus[1] + terrainScript.bonus[1];
            bonus[2] = bonus[2] + terrainScript.bonus[2];
            bonus[3] = bonus[3] + terrainScript.bonus[3];
            bonus[4] = bonus[4] + terrainScript.bonus[4];
            bonus[5] = bonus[5] + terrainScript.bonus[5];
            bonus[6] = bonus[6] + terrainScript.bonus[6];
            bonus[7] = bonus[7] + terrainScript.bonus[7];
            bonus[8] = bonus[8] + terrainScript.bonus[8];
            bonus[9] = bonus[9] + terrainScript.bonus[9];

            terrainScript.SetCity();
        }        

        if (sliderSpeed.value >= 1)
        {
            speed = Mathf.RoundToInt(sliderSpeed.value);
        }
        else
        {
            sliderSpeed.value = 1;
            speed = 1;
        }

        pause = false;
        startSim = true;

        StartCoroutine(Timer(time));
        StartCoroutine(PopulationModify(econTimer));
        StartCoroutine(ResourceModify(econTimer));
        StartCoroutine(SecurityModify(miliTimer));
        StartCoroutine(ExpansionModify(miliTimer));
        StartCoroutine(HappinessModify(urbaTimer));
    }

    public void PauseSim()
    {
        speed = 0;
        pause = true;
    }

    public void SpeedControlling()
    {
        speed = Mathf.RoundToInt(sliderSpeed.value);
        speedText.text = "X: " + speed;
        
        if (speed != 0)
        {
            econTimer = (maxValue - econ) / (speed * speedMult);
            miliTimer = (maxValue - mili) / (speed * speedMult);
            urbaTimer = (maxValue - urba) / (speed * speedMult);
        }

        if (startSim)
        {
            StartCoroutine(Timer(time));
            StartCoroutine(PopulationModify(econTimer));
            StartCoroutine(ResourceModify(econTimer));
            StartCoroutine(SecurityModify(miliTimer));
            StartCoroutine(ExpansionModify(miliTimer));
            StartCoroutine(HappinessModify(urbaTimer));
        }
    }

    IEnumerator Timer(float timer)
    {
        if (sliderSpeed.value != 0 && !collBool && !pause)
        {
            time = time + speed; 
            timetxt.text = "Year: " + time;            
        }
        yield return new WaitForSeconds(timer);
        StartCoroutine(Timer(1f));
    }

    IEnumerator PopulationModify(float timer)
    {
        yield return new WaitForSeconds(timer); 
        if (sliderSpeed.value != 0 && !collBool && !pause)
        {
            if (reso > (popu + 1 + bonus[3]))
            {
                popu++;
                popu = popu + bonus[3];
                poputxt.text = "Population: " + popu;

                if (secu - 1 + bonus[5] > 0)
                {
                    secu--;
                    secu = secu + bonus[5];
                    secutxt.text = "Security: " + secu;
                    if (cityEventtxt.text.Length > 300) cityEventtxt.text = "";
                    cityEventtxt.text = "Last event: Population increased.\r\n" + cityEventtxt.text;
                }
                else
                {
                    secu = 0;
                    secutxt.text = "Security: " + secu;
                    if (cityEventtxt.text.Length > 300) cityEventtxt.text = "";
                    cityEventtxt.text = "Last event: City security decreased.\r\n" + cityEventtxt.text;
                }
            }
            else
            {
                popu--;
                popu = popu + bonus[3];
                poputxt.text = "Population: " + popu;

                secu++;
                secu = secu + bonus[5];
                secutxt.text = "Security: " + secu;
                if (cityEventtxt.text.Length > 300) cityEventtxt.text = "";
                cityEventtxt.text = "Last event: Population decreased.\r\n" + cityEventtxt.text;
                
                if (popu <= 0)
                {
                    Collapse("The city has collapsed.");
                }
            }
        }
        StartCoroutine(PopulationModify(econTimer));
    }

    IEnumerator ResourceModify(float timer)
    {
        yield return new WaitForSeconds(timer);
        if (sliderSpeed.value != 0 && !collBool && !pause)
        {
            econValue = Random.Range(minValue, maxValue);
            resoValue = Random.Range(minValue, maxValue);

            if (econValue <= econ)
            {
                if (happ <= popu)
                {
                    reso = reso + bonus[4] + Mathf.RoundToInt(popu * (1 - econPercent));
                    resotxt.text = "Resources: " + reso;
                    if (cityEventtxt.text.Length > 300) cityEventtxt.text = "";
                    cityEventtxt.text = "Last event: The citizens are working hard.\r\n" + cityEventtxt.text;
                }
                else
                {
                    reso = reso + bonus[4] + Mathf.RoundToInt((popu * (1 - econPercent)) * 2);
                    resotxt.text = "Resources: " + reso;
                    if (cityEventtxt.text.Length > 300) cityEventtxt.text = "";
                    cityEventtxt.text = "Last event: The citizens are happy and work very hard.\r\n" + cityEventtxt.text;
                }                
            }

            if (reso >= (popu + (reso * econPercent)))
            {
                if (resoValue <= econ)
                {
                    if ((reso - (reso * econPercent)) >= 0)
                    {
                        reso = Mathf.RoundToInt(reso + bonus[4] - (reso * econPercent));
                        resotxt.text = "Resources: " + reso;
                        econ = Mathf.RoundToInt(econ + bonus[0] + (reso * econPercent));
                        econtxt.text = "Economy: " + econ;
                        if (cityEventtxt.text.Length > 300) cityEventtxt.text = "";
                        cityEventtxt.text = "Last event: Surplus resources have been used to boost the economy.\r\n" + cityEventtxt.text;
                    }
                }
                else if (econ < resoValue && resoValue <= (econ + mili))
                {
                    if ((reso - (reso * econPercent)) >= 0)
                    {
                        reso = Mathf.RoundToInt(reso - (reso * econPercent));
                        resotxt.text = "Resources: " + reso;
                        mili = Mathf.RoundToInt(mili + bonus[1] + (reso * econPercent));
                        militxt.text = "Militarism: " + mili;
                        if (cityEventtxt.text.Length > 300) cityEventtxt.text = "";
                        cityEventtxt.text = "Last event: Surplus resources have been used to boost the militarism.\r\n" + cityEventtxt.text;
                    }
                }
                else if (econ < resoValue && (econ + mili) < resoValue && resoValue <= (econ + mili + urba))
                {
                    if ((reso - (reso * econPercent)) >= 0)
                    {
                        reso = Mathf.RoundToInt(reso - (reso * econPercent));
                        resotxt.text = "Resources: " + reso;
                        urba = Mathf.RoundToInt(urba + bonus[3] + (reso * econPercent));
                        urbatxt.text = "Urbanism: " + urba;
                        if (cityEventtxt.text.Length > 300) cityEventtxt.text = "";
                        cityEventtxt.text = "Last event: Surplus resources have been used to increase urbanism.\r\n" + cityEventtxt.text;
                    }
                }
            }

            maxValue = (econ + mili + urba);
            econTimer = (maxValue - econ) / (speed * speedMult);
            miliTimer = (maxValue - mili) / (speed * speedMult);
            urbaTimer = (maxValue - urba) / (speed * speedMult);
        }

        StartCoroutine(ResourceModify(econTimer));
    }

    IEnumerator SecurityModify(float timer)
    {
        yield return new WaitForSeconds(timer);
        if (sliderSpeed.value != 0 && !collBool && !pause)
        {
            miliValue = Random.Range(minValue, maxValue);

            if (miliValue <= mili)
            {
                if (secu + 1 + bonus[4] >= 0)
                {
                    secu++;
                    secu = secu + bonus[5];
                    secutxt.text = "Security: " + secu;
                    if (cityEventtxt.text.Length > 300) cityEventtxt.text = "";
                    cityEventtxt.text = "Last event: City security increased.\r\n" + cityEventtxt.text;
                }
                else
                {
                    secu = 0;
                    secutxt.text = "Security: " + secu;
                    if (cityEventtxt.text.Length > 300) cityEventtxt.text = "";
                    cityEventtxt.text = "Last event: City security increased.\r\n" + cityEventtxt.text;
                }
            }
            else
            {
                if (secu - 1 + bonus[5] > 0)
                {
                    secu--;
                    secu = secu + bonus[5];
                    secutxt.text = "Security: " + secu;
                    if (cityEventtxt.text.Length > 300) cityEventtxt.text = "";
                    cityEventtxt.text = "Last event: City security decreased.\r\n" + cityEventtxt.text;
                }
                else
                {
                    secu = 0;
                    secutxt.text = "Security: " + secu;
                    if (cityEventtxt.text.Length > 300) cityEventtxt.text = "";
                    cityEventtxt.text = "Last event: City security decreased.\r\n" + cityEventtxt.text;
                }
            }
        }

        StartCoroutine(SecurityModify(miliTimer));
    }

    IEnumerator ExpansionModify(float timer)
    {
        yield return new WaitForSeconds(timer);
        if (sliderSpeed.value != 0 && !collBool && !pause)
        {
            miliValue = Random.Range(minValue, maxValue);

            if (miliValue <= mili)
            {
                if ((expInd + 1 + bonus[6]) >= 100)
                {
                    expInd = expInd - 100;
                    ExpansionTerritory();
                    expIndtxt.text = "Expansion Index: " + expInd;
                    territories++;
                    if (cityEventtxt.text.Length > 300) cityEventtxt.text = "";
                    cityEventtxt.text = "Last event: Nearby territory has been conquered.\r\n" + cityEventtxt.text;
                }
                else if ((expInd + 1 + bonus[6]) < 100 && (expInd + 1 + bonus[6]) > 0)
                {
                    expInd++;
                    expInd = expInd + bonus[6];
                    expIndtxt.text = "Expansion Index: " + expInd;
                }
                else if ((expInd + 1 + bonus[6]) < 100)
                {
                    expInd = 0;
                    expIndtxt.text = "Expansion Index: " + expInd;
                }
            }
        }

        StartCoroutine(ExpansionModify(miliTimer));
    }

    private void ExpansionTerritory()
    {
        if (range < 6)
        {
            if (terrainScript.terrainLimit[value] != null && terrainScript.terrainLimit[value].GetComponent<TerrainScript>().isCity == false)
            {
                terrainScript.terrainLimit[value].GetComponent<TerrainScript>().isCity = true;
                terrainScript.terrainLimit[value].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                bonus[0] = bonus[0] + terrainScript.terrainLimit[value].GetComponent<TerrainScript>().bonus[0];
                bonus[1] = bonus[1] + terrainScript.terrainLimit[value].GetComponent<TerrainScript>().bonus[1];
                bonus[2] = bonus[2] + terrainScript.terrainLimit[value].GetComponent<TerrainScript>().bonus[2];
                bonus[3] = bonus[3] + terrainScript.terrainLimit[value].GetComponent<TerrainScript>().bonus[3];
                bonus[4] = bonus[4] + terrainScript.terrainLimit[value].GetComponent<TerrainScript>().bonus[4];
                bonus[5] = bonus[5] + terrainScript.terrainLimit[value].GetComponent<TerrainScript>().bonus[5];
                bonus[6] = bonus[6] + terrainScript.terrainLimit[value].GetComponent<TerrainScript>().bonus[6];
                bonus[7] = bonus[7] + terrainScript.terrainLimit[value].GetComponent<TerrainScript>().bonus[7];
                bonus[8] = bonus[8] + terrainScript.terrainLimit[value].GetComponent<TerrainScript>().bonus[8];
                bonus[9] = bonus[9] + terrainScript.terrainLimit[value].GetComponent<TerrainScript>().bonus[9];
                range++;
            }
            else
            {
                if (value + 1 < 6)
                {
                    value++;
                    ExpansionTerritory();
                }
                else
                {
                    value = 0;
                    range++;
                    ExpansionTerritory();
                }                
            }
        }
        else
        {
            int randNum = Random.Range(0, 6);
            ChangeCityExpansion(randNum);            
        }
        
    }

    private void ChangeCityExpansion(int num)
    {        
        if (terrainScript.terrainLimit[num] != null && terrainScript.terrainLimit[num].GetComponent<TerrainScript>().isCity)
        {
            city = terrainScript.terrainLimit[num];
            terrainScript = city.GetComponent<TerrainScript>();
            range = 0;
            value = 0;
            ExpansionTerritory();
        }
        else
        {
            num = Random.Range(0, 6);
            ChangeCityExpansion(num);
        }
    }

    IEnumerator HappinessModify(float timer)
    {
        yield return new WaitForSeconds(timer);
        if (sliderSpeed.value != 0 && !collBool && !pause)
        {
            urbaValue = Random.Range(minValue, maxValue);

            if (reso > popu && secu > popu * 0.2 && secu <= popu && urbaValue < urba)
            {
                if (happ + 1 + bonus[7] > 0)
                {
                    if (happ + 1 + bonus[7] > happ)
                    {
                        happ = happ + 1 + bonus[7];
                        happtxt.text = "Happines: " + happ;
                        if (cityEventtxt.text.Length > 300) cityEventtxt.text = "";
                        cityEventtxt.text = "Last event: The citizens are happier.\r\n" + cityEventtxt.text;
                        RebellionModify();
                    }
                }
                else
                {
                    happ = 0;
                    happtxt.text = "Happines: " + happ;
                    if (cityEventtxt.text.Length > 300) cityEventtxt.text = "";
                    cityEventtxt.text = "Last event: The citizens are unhappy.\r\n" + cityEventtxt.text;
                    RebellionModify();
                }
            }
            else if (reso < popu)
            {
                if (happ - 1 + bonus[7] > 0)
                {
                    if (happ - 1 + bonus[7] < happ)
                    {
                        happ--;
                        happ = happ + bonus[7];
                        happtxt.text = "Happines: " + happ;
                        if (cityEventtxt.text.Length > 300) cityEventtxt.text = "";
                        cityEventtxt.text = "Last event: Resources are insufficient, the citizens are unhappy.\r\n" + cityEventtxt.text;
                        RebellionModify();
                    }
                    else if (happ - 1 + bonus[7] != happ)
                    {
                        happ--;
                        happ = happ + bonus[7];
                        happtxt.text = "Happines: " + happ;
                        if (cityEventtxt.text.Length > 300) cityEventtxt.text = "";
                        cityEventtxt.text = "Last event: The citizens are happier.\r\n" + cityEventtxt.text;
                        RebellionModify();
                    }                    
                }
                else
                {
                    happ = 0;
                    happtxt.text = "Happines: " + happ;
                    if (cityEventtxt.text.Length > 300) cityEventtxt.text = "";
                    cityEventtxt.text = "Last event: Resources are insufficient, the citizens are unhappy.\r\n" + cityEventtxt.text;
                    RebellionModify();
                }
            }
            else if (secu < popu * 0.2)
            {
                if (happ - 1 + bonus[7] > 0)
                {
                    if (happ - 1 + bonus[7] < happ)
                    {
                        happ--;
                        happ = happ + bonus[7];
                        happtxt.text = "Happines: " + happ;
                        if (cityEventtxt.text.Length > 300) cityEventtxt.text = "";
                        cityEventtxt.text = "Last event: The citizens feel insecure and unhappy.\r\n" + cityEventtxt.text;
                        RebellionModify();
                    }
                    else if (happ - 1 + bonus[7] != happ)
                    {
                        happ--;
                        happ = happ + bonus[7];
                        happtxt.text = "Happines: " + happ;
                        if (cityEventtxt.text.Length > 300) cityEventtxt.text = "";
                        cityEventtxt.text = "Last event: The citizens are happier.\r\n" + cityEventtxt.text;
                        RebellionModify();
                    }
                }
                else
                {
                    happ = 0;
                    happtxt.text = "Happines: " + happ;
                    if (cityEventtxt.text.Length > 300) cityEventtxt.text = "";
                    cityEventtxt.text = "Last event: The citizens feel insecure and unhappy.\r\n" + cityEventtxt.text;
                    RebellionModify();
                }
            }
            else if (secu > popu)
            {
                if (happ - 1 + bonus[7] > 0)
                {
                    if (happ - 1 + bonus[7] < happ)
                    {
                        happ--;
                        happ = happ + bonus[7];
                        happtxt.text = "Happines: " + happ;
                        if (cityEventtxt.text.Length > 300) cityEventtxt.text = "";
                        cityEventtxt.text = "Last event: The citizens feel oppressed and unhappy.\r\n" + cityEventtxt.text;
                        RebellionModify();
                    }
                    else if (happ - 1 + bonus[7] != happ)
                    {
                        happ--;
                        happ = happ + bonus[7];
                        happtxt.text = "Happines: " + happ;
                        if (cityEventtxt.text.Length > 300) cityEventtxt.text = "";
                        cityEventtxt.text = "Last event: The citizens are happier.\r\n" + cityEventtxt.text;
                        RebellionModify();
                    }
                }
            }
            else
            {
                if (happ + 1 + bonus[7] > 0)
                {
                    if (happ + 1 + bonus[7] > happ)
                    {
                        happ = happ + 1 + bonus[7];
                        happtxt.text = "Happines: " + happ;
                        if (cityEventtxt.text.Length > 300) cityEventtxt.text = "";
                        cityEventtxt.text = "Last event: The citizens are happier.\r\n" + cityEventtxt.text;
                        RebellionModify();
                    }
                    else
                    {
                        happ = happ + 1 + bonus[7];
                        happtxt.text = "Happines: " + happ;
                        if (cityEventtxt.text.Length > 300) cityEventtxt.text = "";
                        cityEventtxt.text = "Last event: The citizens are unhappy.\r\n" + cityEventtxt.text;
                        RebellionModify();
                    }
                }
                else
                {
                    happ = 0;
                    happtxt.text = "Happines: " + happ;
                    if (cityEventtxt.text.Length > 300) cityEventtxt.text = "";
                    cityEventtxt.text = "Last event: The citizens are unhappy.\r\n" + cityEventtxt.text;
                    RebellionModify();
                }
            }
        }
        StartCoroutine(HappinessModify(urbaTimer));
    }

    public void RebellionModify()
    {        
        if (happ < Mathf.RoundToInt(popu * (1 - urbaPercent)))
        {
            if (secu < popu)
            {
                if (Mathf.RoundToInt(rebInd + ((popu - secu) * 0.05f) + bonus[8]) > 0 && Mathf.RoundToInt(rebInd + ((popu - secu) * 0.05f) + bonus[8]) < 100)
                {
                    rebInd = Mathf.RoundToInt(rebInd + ((popu - secu) * 0.1f) + bonus[8]);
                    rebIndtxt.text = "Rebellion index: " + rebInd;
                    if (cityEventtxt.text.Length > 300) cityEventtxt.text = "";
                    cityEventtxt.text = "Last event: The citizens started a revolt, resources have been lost.\r\n" + cityEventtxt.text;
                    if (Mathf.RoundToInt(reso - (secu * (econPercent + miliPercent))) > 0)
                    {
                        reso = Mathf.RoundToInt(reso - (secu * (econPercent + miliPercent)));
                        resotxt.text = "Resources: " + reso;
                    }
                    else
                    {
                        reso = 0;
                        resotxt.text = "Resources: " + reso;
                        if (cityEventtxt.text.Length > 300) cityEventtxt.text = "";
                        cityEventtxt.text = "Last event: Your resources have been exhausted, the citizens are starving.\r\n" + cityEventtxt.text;
                        popu = Mathf.RoundToInt(popu - Random.Range(1, (popu * 0.1f)));
                        poputxt.text = "Population: " + popu;
                    }
                }
                else if (Mathf.RoundToInt(rebInd + ((popu - secu) * 0.05f) + bonus[8]) <= 0)
                {
                    rebInd = 0;
                    rebIndtxt.text = "Rebellion index: " + rebInd;
                    if (cityEventtxt.text.Length > 300) cityEventtxt.text = "";
                    cityEventtxt.text = "Last event: The riots have cleared up.\r\n" + cityEventtxt.text;
                }
                else if (Mathf.RoundToInt(rebInd + ((popu - secu) * 0.05f) + bonus[8]) >= 100 && Mathf.RoundToInt(collInd + ((popu - secu) * 0.05f) + bonus[9]) < 100)
                {
                    rebInd = 100;
                    rebIndtxt.text = "Rebellion index: " + rebInd;
                    if (cityEventtxt.text.Length > 300) cityEventtxt.text = "";
                    cityEventtxt.text = "Last event: The citizens started a revolt, resources have been lost.\r\n" + cityEventtxt.text;
                    if (Mathf.RoundToInt(reso - (secu * (econPercent + miliPercent))) > 0)
                    {
                        reso = Mathf.RoundToInt(reso - (secu * (econPercent + miliPercent)));
                        resotxt.text = "Resources: " + reso;
                    }
                    else
                    {
                        reso = 0;
                        resotxt.text = "Resources: " + reso;
                        if (cityEventtxt.text.Length > 300) cityEventtxt.text = "";
                        cityEventtxt.text = "Last event: Your resources have been exhausted, the citizens are starving.\r\n" + cityEventtxt.text;
                        popu = Mathf.RoundToInt(popu - Random.Range(1, (popu * 0.1f)));
                        poputxt.text = "Population: " + popu;
                    }
                    collInd = Mathf.RoundToInt(collInd + ((popu - secu) * 0.05f) + bonus[9]);
                    if (cityEventtxt.text.Length > 300) cityEventtxt.text = "";
                    cityEventtxt.text = "Last event: The city has started to collapse.\r\n" + cityEventtxt.text;
                    collIndtxt.text = "Collapse index: " + collInd;
                }
                else if (Mathf.RoundToInt(collInd + ((popu - secu) * 0.05f) + bonus[9]) >= 100)
                {
                    Collapse("The riots have led to the city collapsing.");
                }
            }
            else
            {
                if (happ - 1 + bonus[7] > 0 && happ - 1 + bonus[7] < 100)
                {
                    happ--;
                    happ = happ + bonus[7];
                    happtxt.text = "Happines: " + happ;
                    if (cityEventtxt.text.Length > 300) cityEventtxt.text = "";
                    cityEventtxt.text = "Last event: The authorities reduced the riots.\r\n" + cityEventtxt.text;
                }
                else if (happ - 1 + bonus[7] < 100)
                {
                    happ = 0;
                    happtxt.text = "Happines: " + happ;
                    if (cityEventtxt.text.Length > 300) cityEventtxt.text = "";
                    cityEventtxt.text = "Last event: The authorities reduced the riots.\r\n" + cityEventtxt.text;
                }
                else
                {
                    happ = 100;
                    happtxt.text = "Happines: " + happ;
                    if (cityEventtxt.text.Length > 300) cityEventtxt.text = "";
                    cityEventtxt.text = "Last event: The authorities reduced the riots.\r\n" + cityEventtxt.text;
                }
            }                
        }
        else
        {
            if (Mathf.RoundToInt(rebInd + ((popu - secu) * 0.05f) + bonus[8]) <= 0)
            {
                rebInd = 0;
                rebIndtxt.text = "Rebellion index: " + rebInd;
                if (cityEventtxt.text.Length > 300) cityEventtxt.text = "";
                cityEventtxt.text = "Last event: Riots have been reduced.\r\n" + cityEventtxt.text;
            }
            else
            {
                rebInd = Mathf.RoundToInt(rebInd + ((popu - secu) * 0.05f) + bonus[8]);
                rebIndtxt.text = "Rebellion index: " + rebInd;
                if (cityEventtxt.text.Length > 300) cityEventtxt.text = "";
                cityEventtxt.text = "Last event: Riots have been reduced.\r\n" + cityEventtxt.text;
            }
        }        
    }

    public void Conquest(string messege)
    {
        conquesttxt.text = messege;
        pause = true;

    }

    public void Collapse(string messege)
    {
        collInd = 100;
        collIndtxt.text = "Collapse index: " + collInd; 
        collBool = true;
        collapsetxt.text = messege;

    }
}
