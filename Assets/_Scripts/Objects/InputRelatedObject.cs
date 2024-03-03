using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputRelatedObject : MonoBehaviour
{
	private Player playerScript;
	[SerializeField]
	private float horizontalFactor;
	[SerializeField]
	private float verticalFactor;

	private void Awake()
	{
		playerScript = FindObjectOfType<Player>().GetComponent<Player>();
	}

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		var targetVelocity = playerScript.GetPlayerVelocity();
		targetVelocity.x *= horizontalFactor;
		targetVelocity.y *= verticalFactor;
		gameObject.transform.Translate(targetVelocity);
	}
}
