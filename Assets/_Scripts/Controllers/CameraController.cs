using Assets._Scripts.BaseInfos;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField] private CinemachineVirtualCamera roomCam;

	private Controller2D controller2D;

	private float enterDirection;
	private float exitDirection;


	void Start()
	{
		controller2D = GameObject.FindGameObjectWithTag(Constants.Tags.Player).GetComponent<Controller2D>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag(Constants.Tags.Player))
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
