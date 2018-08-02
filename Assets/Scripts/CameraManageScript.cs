using UnityEngine;
using UnityEngine.UI;

public class CameraManageScript : MonoBehaviour
{
	public void ChangeCameraSize()
	{
	    Camera.main.orthographicSize = 100 - gameObject.GetComponent<Slider>().value;
	}
}
