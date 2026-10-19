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
				case nameof(MusicAreas_LevelOne.RockArea):
					AudioManager.Instance.InitializeMusic(FMODEvents.Instance.TestMusic, (int)MusicAreas_LevelOne.RockArea);
					break;

				case nameof(MusicAreas_LevelOne.ElectronicArea):
					AudioManager.Instance.InitializeMusic(FMODEvents.Instance.TestMusic, (int)MusicAreas_LevelOne.ElectronicArea);
					break;

				//TODO
				//case nameof(MusicAreas_LevelOne.RainTrigger):
				//	AudioManager.Instance.InitializeAmbience(FMODEvents.Instance.ForestRain);
				//	break;

				case nameof(MusicAreas_LevelOne.RainMusic_1):
					AudioManager.Instance.InitializeMusic(FMODEvents.Instance.RainMusic_1);
					break;

				case nameof(MusicAreas_LevelOne.RainMusic_2):
					AudioManager.Instance.InitializeMusic(FMODEvents.Instance.RainMusic_2);
					break;

				default:
					break;
			}
		}
	}
}