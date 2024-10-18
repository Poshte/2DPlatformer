using System;
using UnityEngine;

[Serializable]
public class GameData
{
	public Vector3 PlayerPosition;
	public int SceneIndex;
	public int ActiveDiaryPages;

	public GameData()
	{
		PlayerPosition = Vector3.zero;
		SceneIndex = 0;
		ActiveDiaryPages = 0;
	}
}
