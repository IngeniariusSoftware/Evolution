using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Slider = UnityEngine.UI.Slider;

public class TimeManageScript : MonoBehaviour
{
    public Slider slider;

    public static int TimeSpeed;

    void Awake()
    {
        TimeSpeed = (int)slider.value;
    }

    public void ChangeTimeSped()
    {
        TimeSpeed = (int)slider.value;
    }
}
