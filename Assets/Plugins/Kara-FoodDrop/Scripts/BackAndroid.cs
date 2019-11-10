using UnityEngine;
using System.Collections;

public class BackAndroid : MonoBehaviour 
{
	public string ParentLeveleName;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (ParentLeveleName != "Quit")
				Application.LoadLevel(ParentLeveleName);
			else
				Application.Quit();
		}
	}
}
