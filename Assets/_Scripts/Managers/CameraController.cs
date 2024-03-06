using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField] private CinemachineVirtualCamera roomCam;

	private Controller2D controller2D;

	private float enterDirection;
	private float exitDirection;

	// Start is called before the first frame update
	void Start()
	{
		controller2D = GameObject.FindGameObjectWithTag("Player").GetComponent<Controller2D>();
	}

	// Update is called once per frame
	void Update()
	{

	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.name == "Player")
		{

			enterDirection = controller2D.info.faceDirection;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		exitDirection = controller2D.info.faceDirection;

		if (enterDirection == exitDirection)
		{
			if (roomCam.enabled)
				roomCam.enabled = false;
			else
				roomCam.enabled = true;
		}
	}
}
