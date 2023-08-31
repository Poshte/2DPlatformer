using System.Transactions;
using UnityEngine;

public struct PassengerMovement
{
	public readonly Transform transform;
	public readonly Vector3 velocity;
	public readonly bool standingOnPlatform;
	public readonly bool moveBeforePlatform;

	public PassengerMovement(Transform transform, Vector3 velocity, bool standingOnPlatform, bool moveBeforePlatform)
    {
		this.transform = transform;
		this.velocity = velocity;
		this.standingOnPlatform = standingOnPlatform;
		this.moveBeforePlatform = moveBeforePlatform;
	}
}
