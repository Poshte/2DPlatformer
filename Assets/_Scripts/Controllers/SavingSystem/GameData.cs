using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
	public Vector3 playerPosition;
	public int sceneIndex;
	public int activeDiaryPages;
	public SerializableDictionary<int, bool> diaryEntries;
	public GameData()
	{
		playerPosition = Vector3.zero;
		sceneIndex = 0;
		activeDiaryPages = 0;
		diaryEntries = new();
	}
}