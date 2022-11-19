using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CitySettings : MonoBehaviour
{
    public GameObject econObj, miliObj, urbaObj;
    public Slider econ, mili, urba;
    private int _maxValue;

    private void Start()
    {
        econObj = GameObject.Find("EconomySlider");
        miliObj = GameObject.Find("MilitarySlider");
        urbaObj = GameObject.Find("UrbanismSlider");

        econ = econObj.GetComponent<Slider>();
        mili = miliObj.GetComponent<Slider>();
        urba = urbaObj.GetComponent<Slider>();

        _maxValue = 1000;
    }

    public void EconomySetting()
    {
        if (econ.value + mili.value + urba.value <= _maxValue)
        {
            PlayerPrefs.SetFloat("Economy", econ.value);
        }
        else
        {
            mili.value = mili.value - (econ.value + mili.value + urba.value - _maxValue) / 2;
            urba.value = urba.value - (econ.value + mili.value + urba.value - _maxValue) / 2;
            PlayerPrefs.SetFloat("Economy", econ.value);
            PlayerPrefs.SetFloat("Military", mili.value);
            PlayerPrefs.SetFloat("Urbanism", urba.value);
        }
        
    }

    public void MilitarySetting()
    {
        if (econ.value + mili.value + urba.value <= _maxValue)
        {
            PlayerPrefs.SetFloat("Military", mili.value);
        }
        else
        {
            econ.value = econ.value - (econ.value + mili.value + urba.value - _maxValue) / 2;
            urba.value = urba.value - (econ.value + mili.value + urba.value - _maxValue) / 2;
            PlayerPrefs.SetFloat("Economy", econ.value);
            PlayerPrefs.SetFloat("Military", mili.value);
            PlayerPrefs.SetFloat("Urbanism", urba.value);
        }

    }

    public void UrbanismSetting()
    {
        if (econ.value + mili.value + urba.value <= _maxValue)
        {
            PlayerPrefs.SetFloat("Urbanism", urba.value);

        }
        else
        {
            mili.value = mili.value - (econ.value + mili.value + urba.value - _maxValue) / 2;
            econ.value = econ.value - (econ.value + mili.value + urba.value - _maxValue) / 2;
            PlayerPrefs.SetFloat("Economy", econ.value);
            PlayerPrefs.SetFloat("Military", mili.value);
            PlayerPrefs.SetFloat("Urbanism", urba.value);
        }

    }
}
