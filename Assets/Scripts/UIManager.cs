using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance = new UIManager(); // Singleton pattern

    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject gameOverUI;

    [SerializeField] private float fadeTime = 1.0f;

    [SerializeField] private CanvasGroup pauseGroup;
    [SerializeField] private CanvasGroup gameOverGroup;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            pauseGroup = pauseUI.GetComponent<CanvasGroup>();
            gameOverGroup = gameOverUI.GetComponent<CanvasGroup>();

            pauseUI.SetActive(false);
            gameOverUI.SetActive(false);
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

        gameOverGroup.alpha = 0.0f;
        gameOverUI.SetActive(true);

        StartCoroutine(FadeUI(gameOverGroup, fadeTime, 0.0f, 1.0f));
    }

    public void Resume()
    {
        StopAllCoroutines();
        GameManager.Instance.Resume();
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
    }
}
