using UnityEngine;

public class Word : MonoBehaviour
{
	void Start()
	{
			
	}
	
	void Update()
    {

	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.name == "Player")
		{
			gameObject.SetActive(false);
		}
	}
}