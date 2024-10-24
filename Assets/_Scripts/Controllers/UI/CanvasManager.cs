using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
	private IScreenFadeService screenFade;
	private CanvasGroup canvasGroup;

	private void Awake()
	{
		screenFade = ServiceLocator.Instance.Get<IScreenFadeService>();
		canvasGroup = gameObject.GetComponent<CanvasGroup>();
	}

	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void Start()
	{
		GameEvents.Instance.OnBeforeSceneDestroyed += BeforeSceneDestroyed;
	}
		
	private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
	{
		StartCoroutine(screenFade.Fade(canvasGroup, 1f, 0f, SceneManager.GetActiveScene().buildIndex));
	}

	private void BeforeSceneDestroyed()
	{
		StartCoroutine(screenFade.Fade(canvasGroup, 0f, 1f, SceneManager.GetActiveScene().buildIndex));
	}

	private void OnDestroy()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
		GameEvents.Instance.OnBeforeSceneDestroyed -= BeforeSceneDestroyed;
	}
}
