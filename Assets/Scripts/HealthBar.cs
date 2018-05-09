﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    //[SerializeField]
    private float fillAmount;

    [SerializeField]
    private float lerpSpeed;

    [SerializeField]
    private Text valueText;

    [SerializeField]
    private Image content;

    public float MaxValue { get; set; }

    public float Value
    {
        set
        {
            string[] tmp = valueText.text.Split(':');
            valueText.text = tmp[0] + ":" + value; 
            fillAmount = Map(value, 0, MaxValue, 0, 1); 
        }
    }

	// Use this for initialization
	void Start () {


		
	}
	
	// Update is called once per frame
	void Update () {

        HandleBar();
		
	}

    private void HandleBar()
    {

        if (fillAmount != content.fillAmount)
        {
            content.fillAmount = Mathf.Lerp(content.fillAmount, fillAmount, Time.deltaTime * lerpSpeed);
        }
        
    }

    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;

        //(80 - 0) * (1 - 0) / (100 - 0) + 0
        //80 * 1 / 100 = 0.8

        //(78 - 0) * (1 - 0) / (230 - 0) + 0
        //78 * 1 / 230 = 0.339
    }
}
