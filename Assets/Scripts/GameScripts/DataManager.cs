using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DataManager : MonoBehaviour
{
    public float _econ, _mili, _urba;
    public int econ, mili, urba, popu, reso, secu, expInd, happ, rebInd, collInd, value;
    public int[] bonus = new int[9];
    public Text econtxt, militxt, urbatxt, poputxt, resotxt, secutxt, expIndtxt, happtxt, rebIndtxt, collIndtxt;
    public GameObject economy, militarism, urbanism, population, resources, security, expanIndex, happines, rebelIndex, collapseIndex;

    //Change values in time
    public Slider sliderSpeed;
    public GameObject sliderGameobj, speedObj, cityEvent, collapseObj, conquestObj, timeObj;
    public Text speedText, cityEventtxt, collapsetxt, conquesttxt, timetxt;
    public int speedMult, speed, minValue, maxValue, econValue, miliValue, urbaValue, resoValue, timeEcon, timeMili, timeUrba, limit;
    public float time, econPercent, miliPercent, urbaPercent, econTimer, miliTimer, urbaTimer;

    public GameObject gc, city;
    public TerrainList terrainList;
    public TerrainScript terrainScript;

    public bool pause, collBool;


    private void Awake()
    {
        gc = GameObject.Find("GameController");
        
        terrainList = gc.GetComponent<TerrainList>();

        if (PlayerPrefs.HasKey("Economy"))
        {
            _econ = PlayerPrefs.GetFloat("Economy");
            econ = Mathf.RoundToInt(_econ);
        } else econ = 1;

        if (PlayerPrefs.HasKey("Military"))
        {
            _mili = PlayerPrefs.GetFloat("Military");
            mili = Mathf.RoundToInt(_mili);
        } else mili = 1;

        if (PlayerPrefs.HasKey("Urbanism"))
        {
            _urba = PlayerPrefs.GetFloat("Urbanism");
            urba = Mathf.RoundToInt(_urba);
        } else urba = 1;

        econPercent = 0.2f;
        miliPercent = 0.2f;
        urbaPercent = 0.2f;

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
        happ = Mathf.RoundToInt(urbaPercent * urba);
        happtxt.text = "Happiness: " + happ;

        rebelIndex = GameObject.Find("Rebellionindex");
        rebIndtxt = rebelIndex.GetComponent<Text>();
        rebInd = 0;
        rebIndtxt.text = "Rebellion index: " + rebInd;

        collapseIndex = GameObject.Find("Collapseindex");
        collIndtxt = collapseIndex.GetComponent<Text>();
        collInd = 0;
        collIndtxt.text = "Collapse index: " + collInd;


        sliderGameobj = GameObject.Find("SpeedSlider");
        sliderSpeed = sliderGameobj.GetComponent<Slider>();
        sliderSpeed.value = 1f;
        speedObj = GameObject.Find("Speed");
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

        collBool = false;

        speedMult = 100;

        minValue = 1;
        maxValue = 1000;

        limit = 10000;

        value = 0;

        econTimer = (maxValue - econ) / (speed * speedMult);
        miliTimer = (maxValue - mili) / (speed * speedMult);
        urbaTimer = (maxValue - urba) / (speed * speedMult);
    }

    private void Start()
    {
                
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
    }

    public void StartSim()
    {        
        city = GameObject.FindGameObjectWithTag("City");
        city.GetComponent<TerrainScript>().SetCity();
        terrainScript = city.GetComponent<TerrainScript>();
        terrainScript.SetCity();

        bonus[0] =+ terrainScript.bonus[0];
        bonus[1] =+ terrainScript.bonus[1];
        bonus[2] =+ terrainScript.bonus[2];
        bonus[3] =+ terrainScript.bonus[3];
        bonus[4] =+ terrainScript.bonus[4];
        bonus[5] =+ terrainScript.bonus[5];
        bonus[6] =+ terrainScript.bonus[6];
        bonus[7] =+ terrainScript.bonus[7];
        bonus[8] =+ terrainScript.bonus[8];

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

        StartCoroutine(Timer(time));
        StartCoroutine(PopulationModify(econTimer));
        StartCoroutine(ResourceModify(econTimer));
        StartCoroutine(SecurityModify(miliTimer));
        StartCoroutine(ExpansionModify(miliTimer));
        StartCoroutine(HappinessModify(urbaTimer));
        StartCoroutine(RebellionModify(urbaTimer));

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
        
        if (speed != 0 && !collBool && !pause)
        {
            econTimer = (maxValue - econ) / (speed * speedMult);
            miliTimer = (maxValue - mili) / (speed * speedMult);
            urbaTimer = (maxValue - urba) / (speed * speedMult);
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
            if (reso > popu)
            {
                popu++;
                popu = popu + bonus[3];
                poputxt.text = "Population: " + popu;

                if (secu - 1 + bonus[4] > 0)
                {
                    secu--;
                    secu = secu + bonus[4];
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

                if (secu + 1 + bonus[4] < 100)
                {
                    secu++;
                    secu = secu + bonus[4];
                    secutxt.text = "Security: " + secu;
                    if (cityEventtxt.text.Length > 300) cityEventtxt.text = "";
                    cityEventtxt.text = "Last event: Population decreased.\r\n" + cityEventtxt.text;
                }
                else
                {
                    secu = 100;
                    secutxt.text = "Security: " + secu;
                    if (cityEventtxt.text.Length > 300) cityEventtxt.text = "";
                    cityEventtxt.text = "Last event: City security decreased.\r\n" + cityEventtxt.text;
                }

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
                reso = reso + Mathf.RoundToInt(popu * (1 - econPercent));
                resotxt.text = "Resources: " + reso;
                if (cityEventtxt.text.Length > 300) cityEventtxt.text = "";
                cityEventtxt.text = "Last event: The citizens are working hard.\r\n" + cityEventtxt.text;
            }

            if (reso >= (popu + (reso * econPercent)))
            {
                if (resoValue <= econ)
                {
                    if ((reso - (reso * econPercent)) >= 0)
                    {
                        reso = Mathf.RoundToInt(reso - (reso * econPercent));
                        resotxt.text = "Resources: " + reso;
                        econ = Mathf.RoundToInt(econ + (reso * econPercent));
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
                        mili = Mathf.RoundToInt(mili + (reso * econPercent));
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
                        urba = Mathf.RoundToInt(urba + (reso * econPercent));
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
                if (secu + 1 + bonus[4] > 100)
                {
                    secu = 100;
                    secutxt.text = "Security: " + secu;
                    if (cityEventtxt.text.Length > 300) cityEventtxt.text = "";
                    cityEventtxt.text = "Last event: City security decreased.\r\n" + cityEventtxt.text;
                }
                else
                {
                    secu++;
                    secu = secu + bonus[4];
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
                    secu = secu + bonus[4];
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
                expInd++;
                expInd = expInd + bonus[5];
                expIndtxt.text = "Expansion Index: " + expInd;
            }

            if (expInd >= 100)
            {
                value = Random.Range(0, 6);
                //ExpansionTerritory();    
                expInd = 0;
                expIndtxt.text = "Expansion Index: " + expInd;
            }
        }

        StartCoroutine(ExpansionModify(miliTimer));
    }

    private void ExpansionTerritory()
    {
        if (terrainScript.terrainLimit[value] != null && terrainScript.terrainLimit[value].GetComponent<TerrainScript>().isCity == false)
        {
            terrainScript.terrainLimit[value].GetComponent<TerrainScript>().isCity = true;
            bonus[0] = +terrainScript.terrainLimit[value].GetComponent<TerrainScript>().bonus[0];
            bonus[1] = +terrainScript.terrainLimit[value].GetComponent<TerrainScript>().bonus[1];
            bonus[2] = +terrainScript.terrainLimit[value].GetComponent<TerrainScript>().bonus[2];
            bonus[3] = +terrainScript.terrainLimit[value].GetComponent<TerrainScript>().bonus[3];
            bonus[4] = +terrainScript.terrainLimit[value].GetComponent<TerrainScript>().bonus[4];
            bonus[5] = +terrainScript.terrainLimit[value].GetComponent<TerrainScript>().bonus[5];
            bonus[6] = +terrainScript.terrainLimit[value].GetComponent<TerrainScript>().bonus[6];
            bonus[7] = +terrainScript.terrainLimit[value].GetComponent<TerrainScript>().bonus[7];
            bonus[8] = +terrainScript.terrainLimit[value].GetComponent<TerrainScript>().bonus[8];
            bonus[9] = +terrainScript.terrainLimit[value].GetComponent<TerrainScript>().bonus[9];
        }
        else
        {
            value = Random.Range(0, 6);
            ExpansionTerritory();
        }
    }

    IEnumerator HappinessModify(float timer)
    {
        yield return new WaitForSeconds(timer);
        if (sliderSpeed.value != 0 && !collBool && !pause)
        {
            urbaValue = Random.Range(minValue, maxValue);

            if (reso > popu && ((secu < popu) && (secu > popu * 0.2)))
            {
                if (happ + 1 + bonus[6] <= 100 && happ + 1 + bonus[6] >= 0)
                {
                    happ = happ + 1 + bonus[6];
                    happtxt.text = "Happines: " + happ;
                    if (cityEventtxt.text.Length > 300) cityEventtxt.text = "";
                    cityEventtxt.text = "Last event: The citizens are happier.\r\n" + cityEventtxt.text;
                }
                else if (happ + 1 + bonus[6] > 100)
                {
                    happ = 100;
                    happtxt.text = "Happines: " + happ;
                    if (cityEventtxt.text.Length > 300) cityEventtxt.text = "";
                    cityEventtxt.text = "Last event: The citizens are happier.\r\n" + cityEventtxt.text;
                }
                else
                {
                    happ = 0;
                    happtxt.text = "Happines: " + happ;
                    if (cityEventtxt.text.Length > 300) cityEventtxt.text = "";
                    cityEventtxt.text = "Last event: The citizens are unhappy.\r\n" + cityEventtxt.text;
                }
            }
            else
            {
                if (happ - 1 + bonus[6] > 0)
                {
                    happ--;
                    happ = happ + bonus[6];
                    happtxt.text = "Happines: " + happ;
                    if (cityEventtxt.text.Length > 300) cityEventtxt.text = "";
                    cityEventtxt.text = "Last event: The citizens are unhappy.\r\n" + cityEventtxt.text;
                }
                else
                {
                    happ = 0;
                    happtxt.text = "Happines: " + happ;
                    if (cityEventtxt.text.Length > 300) cityEventtxt.text = "";
                    cityEventtxt.text = "Last event: The citizens are unhappy.\r\n" + cityEventtxt.text;
                }
            }
        }

        StartCoroutine(HappinessModify(urbaTimer));
    }

    IEnumerator RebellionModify(float timer)
    {
        yield return new WaitForSeconds(timer);
        if (sliderSpeed.value != 0 && !collBool && !pause)
        {
            if (happ < Mathf.RoundToInt(urba * urbaPercent))
            {
                if (secu < popu)
                {
                    if (rebInd + 1 + bonus[7] <= 100)
                    {
                        rebInd++;
                        rebInd = rebInd + bonus[7];
                        rebIndtxt.text = "Rebellion index: " + rebInd;
                        if (cityEventtxt.text.Length > 300) cityEventtxt.text = "";
                        cityEventtxt.text = "Last event: The citizens started a revolt, resources have been lost.\r\n" + cityEventtxt.text;
                        reso = Mathf.RoundToInt(reso - (secu * (econPercent + miliPercent)));
                        resotxt.text = "Resources: " + reso;
                    }
                    else if (collInd + 1 + bonus[8] < 100)
                    {
                        rebInd = 100;
                        rebIndtxt.text = "Rebellion index: " + rebInd;
                        if (cityEventtxt.text.Length > 300) cityEventtxt.text = "";
                        cityEventtxt.text = "Last event: The citizens started a revolt, resources have been lost.\r\n" + cityEventtxt.text;
                        reso = Mathf.RoundToInt(reso - (secu * (econPercent + miliPercent)));
                        resotxt.text = "Resources: " + reso;

                        collInd++;
                        collInd = collInd + bonus[8];
                        if (cityEventtxt.text.Length > 300) cityEventtxt.text = "";
                        cityEventtxt.text = "Last event: The city has started to collapse.\r\n" + cityEventtxt.text;
                        collIndtxt.text = "Collapse index: " + collInd;
                    } 
                    else
                    {                        
                        Collapse("The city has collapsed.");
                    }
                }
                else
                {
                    if (happ - 1 + bonus[6] > 0)
                    {
                        happ--;
                        happ = happ + bonus[6];
                        happtxt.text = "Happines: " + happ;
                        if (cityEventtxt.text.Length > 300) cityEventtxt.text = "";
                        cityEventtxt.text = "Last event: The authorities reduced the riots.\r\n" + cityEventtxt.text;
                    }
                    else
                    {
                        happ = 0;
                        happtxt.text = "Happines: " + happ;
                        if (cityEventtxt.text.Length > 300) cityEventtxt.text = "";
                        cityEventtxt.text = "Last event: The authorities reduced the riots.\r\n" + cityEventtxt.text;
                    }                
                }                
            }
            else
            {
                if (rebInd - 1 + bonus[8] <= 0)
                {
                    rebInd = 0;
                    rebIndtxt.text = "Rebellion index: " + rebInd;
                    if (cityEventtxt.text.Length > 300) cityEventtxt.text = "";
                    cityEventtxt.text = "Last event: Riots have been reduced.\r\n" + cityEventtxt.text;
                }
                else
                {
                    rebInd--;
                    rebInd = rebInd + bonus[7];
                    rebIndtxt.text = "Rebellion index: " + rebInd;
                    if (cityEventtxt.text.Length > 300) cityEventtxt.text = "";
                    cityEventtxt.text = "Last event: Riots have been reduced.\r\n" + cityEventtxt.text;
                }
            }
        }

        StartCoroutine(RebellionModify(urbaTimer));

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
