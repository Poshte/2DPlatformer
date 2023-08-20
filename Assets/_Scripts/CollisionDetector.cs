public struct CollisionDetector
{
	public bool rightCollision, leftCollision;
	public bool topCollision, bottomCollision;

	public bool climbingSlope;
	public float slopeAngel, OldSlopeAngel; 
	public void Reset()
	{
		rightCollision = leftCollision = false;
		topCollision = bottomCollision = false;

		climbingSlope = false;
		OldSlopeAngel = slopeAngel;
		slopeAngel = 0f;
	}
}
