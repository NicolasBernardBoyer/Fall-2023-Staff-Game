using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance = new UIManager(); // Singleton pattern

    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject gameOverTextUI;
    [SerializeField] private GameObject gameOverButtonUI;


    [SerializeField] private float fadeTime = 1.0f;
    [SerializeField] private float gameOverTextTime = 2.0f;

    private CanvasGroup pauseGroup;
    private CanvasGroup gameOverTextGroup;
    private CanvasGroup gameOverButtonGroup;

    private Action onFadeEnd;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            pauseGroup = pauseUI.GetComponent<CanvasGroup>();
            gameOverTextGroup = gameOverTextUI.GetComponent<CanvasGroup>();
            gameOverButtonGroup = gameOverButtonUI.GetComponent<CanvasGroup>();

            pauseUI.SetActive(false);
            gameOverTextUI.SetActive(false);
            gameOverButtonUI.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Multiple UIManager instances found. Destroying the extra one.");
            Destroy(gameObject);
        }
    }

    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new UIManager();
            }
            return instance;
        }
    }

    public void Pause()
    {
        Debug.Log("PAUSE UI");

        pauseGroup.alpha = 0.0f;
        pauseUI.SetActive(true);

        StartCoroutine(FadeUI(pauseGroup, fadeTime, 0.0f, 1.0f));
    }

    public void GameOver()
    {
        Debug.Log("GAME OVER UI");

        gameOverTextGroup.alpha = 0.0f;
        gameOverTextUI.SetActive(true);

        onFadeEnd += GameOverButtons;
        StartCoroutine(FadeUI(gameOverTextGroup, fadeTime, 0.0f, 1.0f));
    }

    private void GameOverButtons()
    {
        StartCoroutine(DelayButtons());
    }

    public void Resume()
    {
        StopAllCoroutines();
        GameManager.Instance.Resume();
        pauseUI.SetActive(false);
        StartCoroutine(FadeUI(pauseGroup, fadeTime, pauseGroup.alpha, 0.0f));
    }

    public void Restart()
    {
        GameManager.Instance.Restart();
    }

    public void ExitToMenu()
    {
        GameManager.Instance.LoadMenu();
    }

    private IEnumerator DelayButtons()
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < gameOverTextTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(FadeUI(gameOverTextGroup, fadeTime, 1.0f, 0.0f));
        onFadeEnd = null;
        gameOverButtonUI.SetActive(true);
        StartCoroutine(FadeUI(gameOverButtonGroup, fadeTime, 0.0f, 1.0f));
    }

    private IEnumerator FadeUI(CanvasGroup group, float fadeTime, float initialAlpha, float finalAlpha)
    {
        float timeElapsed = 0.0f;

        while (group.alpha != finalAlpha)
        {

            float t = timeElapsed / fadeTime;
            if (t < 0.0f) t = 0.0f;
            if (t > 1.0f) t = 1.0f;

            float alpha = Mathf.Lerp(initialAlpha, finalAlpha, t);

            group.alpha = alpha;

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        onFadeEnd?.Invoke();
    }
}
