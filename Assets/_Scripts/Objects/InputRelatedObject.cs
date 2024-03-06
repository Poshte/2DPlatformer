using UnityEngine;

public class InputRelatedObject : MonoBehaviour
{
	//control speed of the objects
	[SerializeField] private float horizontalFactor;
	[SerializeField] private float verticalFactor;

	private Player playerScript;

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
