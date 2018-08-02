using UnityEngine;
using Slider = UnityEngine.UI.Slider;

public class CameraManageScript : MonoBehaviour
{
    public Slider slider; 

    public void ChangeCameraSize()
    {
        Camera.main.orthographicSize = 100 - slider.value;
    }

    void Update()
    {
        float axis = Input.GetAxis("Mouse ScrollWheel");
        if (axis != 0)
        {
            ChangeSliderValue(axis * 20);
        } 
    }

    public void ChangeSliderValue(float axis)
    {
        if (slider.minValue <= slider.value + axis)
        {
            slider.value += axis;
        }
        else
        {
            slider.value = slider.minValue;
        }

        if (slider.maxValue >= slider.value + axis)
        {
            slider.value += axis;
        }
        else
        {
            slider.value = slider.maxValue;
        }

        ChangeCameraSize();
    }
}
