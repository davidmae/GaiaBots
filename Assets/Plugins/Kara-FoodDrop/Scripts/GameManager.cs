using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
	public GameObject[] objects;

	private float lastCreated;

	public TextMesh lostFoodLabel;
	private int lostFood;
	public int LostFood
	{
		set
		{
			lostFood = value;

			lostFoodLabel.text = lostFood.ToString();

			if (LostFood >= 8)
				Application.LoadLevel("MainMenu");
		}
		get
		{
			return lostFood;
		}
	}

	public TextMesh scoreLabel;
	public static int score;
	public int Score
	{
		set
		{
			score = value;

			scoreLabel.text = Score.ToString();
		}
		get
		{
			return score;
		}
	}

	private float speed;

	void Start () 
	{
		score = 0;
		lastCreated = 0;

		lastCreated = Time.time;

		speed = 2.5f;

		Invoke("CreateObjects", 0.5f);
	}

	void CreateObjects()
	{
		Instantiate(objects[Random.Range(0,objects.Length)], new Vector3(Random.Range(-5.5f, 5.6f), 5.5f, 0) ,Quaternion.identity);

		if (Score % 5 == 0)
			speed -= 0.1f;

		Invoke("CreateObjects", speed);
	}
}
