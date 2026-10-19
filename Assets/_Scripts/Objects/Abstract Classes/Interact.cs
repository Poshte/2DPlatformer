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