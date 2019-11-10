using UnityEngine;
using System.Collections;

public class CameraAspect : MonoBehaviour 
{
	public GameObject[] gm;

	void Start ()
	{
		Camera.main.aspect = 16/10f;
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		Time.timeScale = 1;
    }

	private void OnInjectionDetected()
	{
		foreach(GameObject go in gm)
		{
			go.SetActive(false);
		}

	}

	void LateUpdate()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}
}
