using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Android;

public class PlatformController : RaycastController
{
	//dependencies
	[SerializeField]
	private LayerMask passengerMask;

	[SerializeField]
	private GameObject playerObject;
	private Controller2D controller2D;

	//waypoints
	[SerializeField]
	private Vector3[] localWaypoints;
	private Vector3[] globalWaypoints;

	private int startingWaypointIndex;
	private float percentBetweenWaypoints;

	[SerializeField]
	private float platformSpeed;
	[SerializeField]
	private bool isCyclic;

	[SerializeField]
	private float waitTime;
	private float moveTime;

	[SerializeField]
	[Range(0, 2)]
	private float easeAmount;

	//lists
	private List<PassengerMovement> passengerMovements;
	private Dictionary<Transform, Controller2D> passengerDictionary = new Dictionary<Transform, Controller2D>();

	public override void Start()
	{
		base.Start();

		controller2D = playerObject.GetComponent<Controller2D>(); 

		globalWaypoints = new Vector3[localWaypoints.Length];
		for (int i = 0; i < localWaypoints.Length; i++)
		{
			globalWaypoints[i] = localWaypoints[i] + transform.position;
		}
	}
	void Update()
	{
		UpdateRaycastOrigin();
		var velocity = CalculatePlatformMovement();
		CalculatePassengerMovement(velocity);

		MovePassenger(true);
		transform.Translate(velocity);
		MovePassenger(false);
	}

	public float Ease(float x)
	{
		var a = easeAmount + 1;
		return Mathf.Pow(x, a) / (Mathf.Pow(x, a) + Mathf.Pow(1 - x, a));
	}
	public Vector3 CalculatePlatformMovement()
	{
		if (Time.time < moveTime)
		{
			return Vector3.zero;
		}

		startingWaypointIndex %= globalWaypoints.Length;
		var nextWaypointIndex = (startingWaypointIndex + 1) % globalWaypoints.Length;
		var distanceBetweenWaypoints = Vector3.Distance(globalWaypoints[startingWaypointIndex], globalWaypoints[nextWaypointIndex]);

		percentBetweenWaypoints += Time.deltaTime * (platformSpeed / distanceBetweenWaypoints);
		percentBetweenWaypoints = Mathf.Clamp01(percentBetweenWaypoints);
		var easedPercent = Ease(percentBetweenWaypoints);

		var newPosition = Vector3.Lerp(globalWaypoints[startingWaypointIndex], globalWaypoints[nextWaypointIndex], easedPercent);

		if (percentBetweenWaypoints >= 1)
		{
			percentBetweenWaypoints = 0;
			startingWaypointIndex++;
			if (startingWaypointIndex >= globalWaypoints.Length - 1 && !isCyclic)
			{
				startingWaypointIndex = 0;
				Array.Reverse(globalWaypoints);
			}

			moveTime = Time.time + waitTime;
		}
		return newPosition - transform.position;
	}

	public void MovePassenger(bool isBeforePlatformMove)
	{
		foreach (var passenger in passengerMovements)
		{
			if (!passengerDictionary.ContainsKey(passenger.transform))
				passengerDictionary.Add(passenger.transform, passenger.transform.GetComponent<Controller2D>());
			 
			if (passenger.moveBeforePlatform == isBeforePlatformMove)
				passengerDictionary[passenger.transform].Move(passenger.velocity, downCommand: false, passenger.standingOnPlatform);
		}
	}

	public void CalculatePassengerMovement(Vector2 velocity)
	{
		var movedPassengers = new HashSet<Transform>();
		passengerMovements = new List<PassengerMovement>();

		var directionX = Mathf.Sign(velocity.x);
		var directionY = Mathf.Sign(velocity.y);

		//vertically moving platform (passenger being pushed up/down by the platform)
		if (velocity.y > 0f && controller2D.collisionDetector.bottomCollision)
		{
			var rayLength = Mathf.Abs(velocity.y) + skinWidth;

			for (int i = 0; i < verticalRayCount; i++)
			{
				var rayOrigin = (directionY < 0) ? raycastOrigin.bottomLeft : raycastOrigin.topLeft;
				rayOrigin += Vector2.right * (verticalRaySpace * i);
				var hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, passengerMask);

				if (hit && !movedPassengers.Contains(hit.transform) && hit.distance != 0f)
				{
					movedPassengers.Add(hit.transform);

					var pushX = (velocity.y >= 0) ? velocity.x : 0f;
					var pushY = velocity.y - (hit.distance - skinWidth) * directionY;

					passengerMovements.Add(new PassengerMovement
						(hit.transform, new Vector2(pushX, pushY), (directionY > 0), true));
				}
			}
		}

		//passenger being pushed by a horizontally moving platform
		if (velocity.x != 0f)
		{
			var rayLength = Mathf.Abs(velocity.x) + skinWidth;

			for (int i = 0; i < horizontalRayCount; i++)
			{
				var rayOrigin = (directionX < 0) ? raycastOrigin.bottomLeft : raycastOrigin.bottomRight;
				rayOrigin += Vector2.up * (horizontalRaySpace * i);
				var hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, passengerMask);

				if (hit && !movedPassengers.Contains(hit.transform) && hit.distance != 0f)
				{
					movedPassengers.Add(hit.transform);

					float pushX = velocity.x - (hit.distance - skinWidth) * directionX;
					float pushY = -skinWidth;

					passengerMovements.Add(new PassengerMovement
						(hit.transform, new Vector2(pushX, pushY), false, true));
				}
			}
		}

		//when passenger is on top of a horizontally or downward moving platform
		if (directionY < 0 || velocity.x != 0 && velocity.y == 0)
		{
			var rayLength = skinWidth * 2;

			for (int i = 0; i < verticalRayCount; i++)
			{
				var rayOrigin = raycastOrigin.topLeft + Vector2.right * (verticalRaySpace * i);
				var hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, passengerMask);

				if (hit && !movedPassengers.Contains(hit.transform) && hit.distance != 0f)
				{
					movedPassengers.Add(hit.transform);

					var pushX = velocity.x;
					var pushY = velocity.y;

					passengerMovements.Add(new PassengerMovement
						(hit.transform, new Vector2(pushX, pushY), true, false));
				}
			}
		}
	}

	public void OnDrawGizmos()
	{
		if (localWaypoints != null)
		{
			Gizmos.color = Color.red;
			var size = 0.3f;

			for (int i = 0; i < localWaypoints.Length; i++)
			{
				var waypointPosition = (Application.isPlaying) ? globalWaypoints[i] : localWaypoints[i] + transform.position;
				Gizmos.DrawLine(waypointPosition + Vector3.up * size, waypointPosition + Vector3.down * size);
				Gizmos.DrawLine(waypointPosition + Vector3.right * size, waypointPosition + Vector3.left * size);
			}
		}
	}
}

public readonly struct PassengerMovement
{
    public readonly Transform transform;
    public readonly Vector2 velocity;
    public readonly bool standingOnPlatform;
    public readonly bool moveBeforePlatform;

    public PassengerMovement(Transform transform, Vector2 velocity, bool standingOnPlatform, bool moveBeforePlatform)
    {
        this.transform = transform;
        this.velocity = velocity;
        this.standingOnPlatform = standingOnPlatform;
        this.moveBeforePlatform = moveBeforePlatform;
    }
}