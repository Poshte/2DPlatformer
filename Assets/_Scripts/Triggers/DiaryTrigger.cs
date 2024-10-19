using Assets._Scripts.BaseInfos;
using UnityEngine;

public class DiaryTrigger : MonoBehaviour, IDataPersistence
{
	[SerializeField]
	private int[] photoIndexes;

	private DiaryController diaryController;

	[SerializeField]
	private int id;

	private bool isActive = true;

	public void LoadData(GameData data)
	{
		if (data.diaryEntries.TryGetValue(id, out isActive))
		{
			gameObject.SetActive(isActive);
		}
	}

	public void SaveData(GameData data)
	{
		if (!data.diaryEntries.ContainsKey(id))
		{
			data.diaryEntries.Add(id, gameObject.activeInHierarchy);
		}
		else
		{
			data.diaryEntries[id] = gameObject.activeInHierarchy;
		}
	}

	private void Awake()
	{
		diaryController = GameObject.FindGameObjectWithTag(Constants.Tags.DiaryController).GetComponent<DiaryController>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag(Constants.Tags.Player))
		{
			diaryController.AddPhotosToDiary(photoIndexes);
			diaryController.ShowPageAddedToDiaryVisual();
			diaryController.PlayPageAddedToDiarySound();

			gameObject.SetActive(false);
		}
	}
}
