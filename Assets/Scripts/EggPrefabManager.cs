using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class EggPrefabManager : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI time; 
    public DateTime hatchTime;
    public double totalHours;
    private bool isSet = false;
    public TextMeshProUGUI name;

    public void SetEggDetails(DateTime hatch, DateTime create, string username){
        hatchTime = hatch;

        TimeSpan total = hatchTime - create;
        totalHours = total.TotalHours;
        slider.maxValue = (float)totalHours;
        name.text = username;

        isSet = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (isSet){
            TimeSpan remaining = hatchTime - DateTime.Now;
            double remainingHours = remaining.TotalHours;
            int hours = (int)remainingHours;
            int minutes = (int)((remainingHours - hours) * 60); // to calculate minutes to display in the text box

            time.text = $"{hours}h {minutes}min";
            slider.value = (float)(totalHours - remainingHours);
        }
    }
}
