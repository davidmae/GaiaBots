using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour
{
	void Update()
	{
		if (Input.GetMouseButton(0))
		{
			Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y,0);
			Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
			
			RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			
			if(hit.collider == null)
			{
				Move(curPosition.x);
			}
		}
	}

	void Move(float posX)
	{
		if (transform.position.x > posX)
		{
			transform.position = new Vector3(transform.position.x - 0.1f, -3, 0);
		}
		else if (transform.position.x < posX)
		{
			transform.position = new Vector3(transform.position.x + 0.1f, -3, 0);
		}
	}

	void OnTriggerEnter2D(Collider2D col) 
	{
		if (col.tag == "Food")
		{
			GameObject.FindObjectOfType<GameManager>().Score++;
			GetComponent<AudioSource>().Play();
			Destroy(col.gameObject);
		}
		else if (col.tag == "Finish")
		{
			Application.LoadLevel("MainMenu");
		}
	}
}
