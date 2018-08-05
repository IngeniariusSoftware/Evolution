using UnityEditor;
using  System;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

using Slider = UnityEngine.UI.Slider;

public class CameraManageScript : MonoBehaviour
{
    public Slider slider;

    private static float _currentMousePositionX;

    private static float _currentMousePositionY;

    public void ChangeCameraSize()
    {
        Camera.main.orthographicSize = 100 - slider.value;
    }

    void Start()
    {
        _currentMousePositionX = Input.mousePosition.x;
        _currentMousePositionY = Input.mousePosition.y;
    }

    void Update()
    {
        float axis = Input.GetAxis("Mouse ScrollWheel");
        if (axis != 0)
        {
            ChangeSliderValue(axis * 20);
        }

        if (Input.GetMouseButton(2))
        {
            float deltaX = (_currentMousePositionX - Input.mousePosition.x) * 0.05f;
            float deltaY = (_currentMousePositionY - Input.mousePosition.y) * 0.05f;

            if (Data.MapSize.X / 2.0 * Data.CellSizeX > Math.Abs(Camera.main.transform.position.x + deltaX))
            {
                Camera.main.transform.position = new Vector3(
                    Camera.main.transform.position.x + deltaX,
                    Camera.main.transform.position.y,
                    Camera.main.transform.position.z);
            }

            if (Data.MapSize.Y / 2.0 * Data.CellSizeY > Math.Abs(Camera.main.transform.position.y + deltaY))
            {
                Camera.main.transform.position = new Vector3(
                    Camera.main.transform.position.x,
                    Camera.main.transform.position.y + deltaY,
                    Camera.main.transform.position.z);

            }
        }

        _currentMousePositionX = Input.mousePosition.x;
        _currentMousePositionY = Input.mousePosition.y;
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
