public struct CollisionDetector
{
	public bool rightCollision, leftCollision;
	public bool topCollision, bottomCollision;

	public bool climbingSlope;
	public bool descendingSlope;
	public float slopeAngel, OldSlopeAngel;

	public void Reset()
	{
		rightCollision = leftCollision = false;
		topCollision = bottomCollision = false;

		climbingSlope = descendingSlope = false;
		OldSlopeAngel = slopeAngel;
		slopeAngel = 0f;
	}
}
