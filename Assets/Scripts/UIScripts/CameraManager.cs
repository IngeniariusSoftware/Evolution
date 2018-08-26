using  System;
using UnityEngine;
using Slider = UnityEngine.UI.Slider;

public class CameraManager : MonoBehaviour
{
    public Slider slider;

    public Transform Background;

    public static Vector2 mouseDelta = new Vector2(0, 0);

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
        float axis = Input.GetAxis("Mouse ScrollWheel");
        if (axis != 0)
        {
            ChangeSliderValue(axis * 20);
        }

        if (Input.GetKey(KeyCode.Plus) || Input.GetKey(KeyCode.KeypadPlus))
        {
            ChangeSliderValue(1);
        }

        if (Input.GetMouseButton(2) || Input.GetMouseButton(1))
        {
            mouseDelta = (MousePosition - (Vector2)Input.mousePosition) * 0.1f;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Coordinates clickCoordinate = new Coordinates
                                              {
                                                  Y = -(int)Math.Round(
                                                          (Input.mousePosition.y - Screen.height / 2.0)
                                                          / 100 / Cell.SizeY / 5.4
                                                          * Camera.main.orthographicSize - Map.Size.Y / 2
                                                          + CameraPosition.y / Cell.SizeY,
                                                          MidpointRounding.AwayFromZero),
                                                  X = (int)Math.Round(
                                                      (Input.mousePosition.x - Screen.width / 2.0) / 100
                                                                                                   / Cell
                                                                                                       .SizeX
                                                                                                   / 5.4
                                                      * Camera.main.orthographicSize + Map.Size.X / 2
                                                                                     + CameraPosition.x
                                                                                     / Cell.SizeX,
                                                      MidpointRounding.AwayFromZero)
                                              };
            }

        if (Input.GetKey(KeyCode.Minus) || Input.GetKey(KeyCode.KeypadMinus))
        {
            ChangeSliderValue(-1);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            mouseDelta += Vector2.up;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            mouseDelta += Vector2.right;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            mouseDelta += Vector2.down;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            mouseDelta += Vector2.left;
        }

        if (Map.Size.Y / 2.0 * Cell.SizeY > Math.Abs(mouseDelta.y + CameraPosition.y))
        {
            CameraPosition.y += mouseDelta.y;
        }

        if (Map.Size.X / 2.0 * Cell.SizeX > Math.Abs(mouseDelta.x + CameraPosition.x))
        {
            CameraPosition.x += mouseDelta.x;
        }

        Camera.main.transform.position = new Vector3(CameraPosition.x, CameraPosition.y, Camera.main.transform.position.z);
        MousePosition = Input.mousePosition;
        Background.position = new Vector3(CameraPosition.x, CameraPosition.y, Background.position.z);
        mouseDelta = new Vector2(0, 0);
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
