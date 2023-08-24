using UnityEngine;

public struct CollisionDetector
{
	public bool rightCollision, leftCollision;
	public bool topCollision, bottomCollision;

	public bool climbingSlope;
	public bool descendingSlope;
	public float slopeAngle, OldSlopeAngle;

	public void Reset()
	{
		rightCollision = leftCollision = false;
		topCollision = bottomCollision = false;

		climbingSlope = descendingSlope = false;
		OldSlopeAngle = slopeAngle;
		slopeAngle = 0f;
	}
}
