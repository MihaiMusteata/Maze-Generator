using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class  PlayerInputController : MonoBehaviour
{
     [SerializeField] private Slider slider;
     [SerializeField] private TMP_InputField inputTMP;
     private bool sliderEventActive = true;
     private bool inputFieldEventActive = true;

     internal static bool GetKeyDown(KeyCode alpha1)
     {
          throw new NotImplementedException();
     }

     void Start()
     {
          slider.onValueChanged.AddListener(HandleSliderValueChanged);
          inputTMP.onValueChanged.AddListener(HandleInputFieldValueChanged);

          void HandleSliderValueChanged(float value)
          {
               if (sliderEventActive)
               {
                    value *= 100;
                    inputFieldEventActive = false;
                    inputTMP.text = ((int)value).ToString();
                    inputFieldEventActive = true;
               }
          }

          void HandleInputFieldValueChanged(string value)
          {
               if (inputFieldEventActive)
               {
                    int parsedValue = 0;
                    int.TryParse(value, out parsedValue);

                    if (parsedValue > 100)
                    {
                         parsedValue = 100;
                         inputTMP.text = "100";

                    }
                    if (parsedValue < 0)
                    {
                         parsedValue = 0;
                         inputTMP.text = "0";
                    }
                    sliderEventActive = false;
                    slider.value = parsedValue / 100f;
                    sliderEventActive = true;

               }
          }

     }
}
