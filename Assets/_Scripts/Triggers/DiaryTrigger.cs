using Assets._Scripts.BaseInfos;
using UnityEngine;

public class DiaryTrigger : MonoBehaviour
{
	[SerializeField]
	private int[] photoIndexes;

	private DiaryController diaryController;

	private void Awake()
	{
		diaryController = GameObject.FindGameObjectWithTag(Constants.Tags.DiaryController).GetComponent<DiaryController>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag(Constants.Tags.Player))
		{
			diaryController.AddPhotosToDiary(photoIndexes);

			Destroy(gameObject);
		}
	}
}
