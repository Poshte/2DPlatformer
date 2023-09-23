using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public interface ILevelManager
{
    void LoadNextScene();
}

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private float waitTime = 1f;

    private IScreenFadeService screenFade;

    [SerializeField]
    private GameObject canvas;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        screenFade = ServiceLocator.Instance.Get<IScreenFadeService>();
        canvasGroup = canvas.gameObject.GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        StartCoroutine(screenFade.Fade(canvasGroup, 1f, 0f));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            LoadNextScene();
        }
    }

    public void LoadNextScene()
    {
        StartCoroutine(screenFade.Fade(canvasGroup, 0f, 1f));
        StartCoroutine(LoadSceneAsynchronously(SceneManager.GetActiveScene().buildIndex + 1));
    }

    private IEnumerator LoadSceneAsynchronously(int levelIndex)
    {
        yield return new WaitForSeconds(waitTime);

        SceneManager.LoadSceneAsync(levelIndex);
        StartCoroutine(screenFade.Fade(canvasGroup, 1f, 0f));

    }
}
