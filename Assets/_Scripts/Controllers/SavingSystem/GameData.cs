using Assets._Scripts.BaseInfos;
using System;
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
		sceneIndex = (int)Enumerations.Scenes.Forest;
		activeDiaryPages = 0;
		diaryEntries = new();
	}
}