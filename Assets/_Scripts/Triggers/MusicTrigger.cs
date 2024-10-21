using Assets._Scripts.BaseInfos;
using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag(Constants.Tags.Player))
		{
			switch (gameObject.tag)
			{
				case nameof(Enumerations.MusicAreas_LevelOne.RockArea):
					AudioManager.Instance.InitializeMusic(FMODEvents.Instance.TestMusic, (int)Enumerations.MusicAreas_LevelOne.RockArea);
					break;

				case nameof(Enumerations.MusicAreas_LevelOne.ElectronicArea):
					AudioManager.Instance.InitializeMusic(FMODEvents.Instance.TestMusic, (int)Enumerations.MusicAreas_LevelOne.ElectronicArea);
					break;

				default:
					break;
			}
		}
	}
}
