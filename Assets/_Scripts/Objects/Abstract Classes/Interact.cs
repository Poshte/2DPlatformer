using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
	void Start()
	{
		gameObject.SetActive(false);
		GameEvents.Instance.OnConversationEnded += DestroyInteractObject;
	}

	private void DestroyInteractObject()
	{
		if (gameObject.activeInHierarchy)
		{
			GameEvents.Instance.OnConversationEnded -= DestroyInteractObject;
			Destroy(gameObject);
		}
	}
}
