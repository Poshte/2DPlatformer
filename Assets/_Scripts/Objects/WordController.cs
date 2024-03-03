using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WordController : MonoBehaviour
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