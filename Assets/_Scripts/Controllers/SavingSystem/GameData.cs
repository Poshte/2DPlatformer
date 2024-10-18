using System;
using UnityEngine.SceneManagement;

[Serializable]
public class GameData
{
	public float[] PlayerPosition { get; set; }
	public int SceneIndex { get; set; }
	public int ActiveDiaryPages { get; set; }

    public GameData()
	{
		PlayerPosition = new float[] { 0, 0 };
		SceneIndex = 0;
		ActiveDiaryPages = 0;
	}
}
