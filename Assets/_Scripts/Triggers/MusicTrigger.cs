using Assets._Scripts.BaseInfos;
using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			switch (gameObject.tag)
			{
				case nameof(Enumerations.MusicAreasLevel1.RockArea):
					AudioManager.Instance.InitializeMusic(FMODEvents.Instance.TestMusic, (int)Enumerations.MusicAreasLevel1.RockArea);
					break;

				case nameof(Enumerations.MusicAreasLevel1.ElectronicArea):
					AudioManager.Instance.InitializeMusic(FMODEvents.Instance.TestMusic, (int)Enumerations.MusicAreasLevel1.ElectronicArea);
					break;

				default:
					break;
			}
		}
	}
}
