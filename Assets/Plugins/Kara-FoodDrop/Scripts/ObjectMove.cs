using UnityEngine;
using System.Collections;

public class ObjectMove : MonoBehaviour 
{
	void FixedUpdate () 
	{
		transform.position = new Vector3(transform.position.x , transform.position.y - 0.05f, 0);

		if (transform.position.y <= -4.5f)
		{
			if (gameObject.tag == "Food")
				GameObject.FindObjectOfType<GameManager>().LostFood++;

			Destroy(gameObject);
		}
	}
}
