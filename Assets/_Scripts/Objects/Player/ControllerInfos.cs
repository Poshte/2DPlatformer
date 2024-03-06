using UnityEngine;

public struct ControllerInfos
{
	public bool rightCollision, leftCollision;
	public bool topCollision, bottomCollision;

	public bool climbingSlope;
	public bool descendingSlope;
	public bool slidingMaxSlope;
	public Vector2 slopeNormal;
	public float slopeAngle, OldSlopeAngle;

	public int faceDirection;

	public void Reset()
	{
		rightCollision = leftCollision = false;
		topCollision = bottomCollision = false;

		climbingSlope = descendingSlope = false;
		slidingMaxSlope = false;
		slopeNormal = Vector2.zero;
		OldSlopeAngle = slopeAngle;
		slopeAngle = 0f;
	}
}