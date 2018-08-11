using  System;
using UnityEngine;
using Slider = UnityEngine.UI.Slider;

public class CameraManageScript : MonoBehaviour
{
    public Slider slider;

    public Transform Background;

    public static Vector2 MousePosition;

    public static Vector3 CameraPosition;

    public void ChangeCameraSize()
    {
        Camera.main.orthographicSize = 100 - slider.value;
        Background.localScale = new Vector2(5, 4) * Camera.main.orthographicSize / 50;
    }

    void Start()
    {
        MousePosition = Input.mousePosition;
        CameraPosition = Camera.main.transform.position;
    }

    void Update()
    {
        Vector2 delta = new Vector2(0, 0);
        float axis = Input.GetAxis("Mouse ScrollWheel");
        if (axis != 0)
        {
            ChangeSliderValue(axis * 20);
        }

        if (Input.GetKey(KeyCode.Plus) || Input.GetKey(KeyCode.KeypadPlus))
        {
            ChangeSliderValue(1);
        }

        if (Input.GetKey(KeyCode.Minus) || Input.GetKey(KeyCode.KeypadMinus))
        {
            ChangeSliderValue(-1);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            delta += Vector2.up;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            delta += Vector2.right;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            delta += Vector2.down;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            delta += Vector2.left;
        }

        if (Input.GetMouseButton(2))
        {
            delta = (MousePosition - (Vector2)Input.mousePosition) * 0.1f;
        }

        if (Map.Size.Y / 2.0 * Cell.SizeY > Math.Abs(delta.y + CameraPosition.y))
        {
            CameraPosition.y += delta.y;
        }

        if (Map.Size.X / 2.0 * Cell.SizeX > Math.Abs(delta.x + CameraPosition.x))
        {
            CameraPosition.x += delta.x;
        }

        Camera.main.transform.position = new Vector3(CameraPosition.x, CameraPosition.y, Camera.main.transform.position.z);
        MousePosition = Input.mousePosition;
        Background.position = new Vector3(CameraPosition.x, CameraPosition.y, Background.position.z); 
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
