public struct CollisionDetector
{
	public bool rightCollision, leftCollision;
	public bool topCollision, bottomCollision;

	public void Reset()
	{
		rightCollision = leftCollision = false;
		topCollision = bottomCollision = false;
	}
}
