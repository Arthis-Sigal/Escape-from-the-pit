using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    public CanvasGroup fadeCanvas; // Ton CanvasGroup sur FadeImage
    public GameObject FadePanel;
    public float fadeDuration = 1f; // Durée du fondu (en secondes)

    private void Start()
    {
        StartCoroutine(FadeIn());
        FadePanel.SetActive(true);
    }

    // 🔹 Charger une scène par NOM
    public void FadeToSceneString(string sceneName)
    {
        StartCoroutine(FadeOutByName(sceneName));
    }

    // 🔹 Charger une scène par INDEX
    public void FadeToSceneInt(int sceneIndex)
    {
        StartCoroutine(FadeOutByIndex(sceneIndex));
    }

    private IEnumerator FadeIn()
    {
        float t = fadeDuration;
        while (t > 0)
        {
            t -= Time.deltaTime;
            fadeCanvas.alpha = t / fadeDuration;
            yield return null;
        }

        fadeCanvas.alpha = 0;
        FadePanel.SetActive(false);
    }

    private IEnumerator FadeOutByName(string sceneName)
    {

        FadePanel.SetActive(true);
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeCanvas.alpha = t / fadeDuration;
            yield return null;
        }

        fadeCanvas.alpha = 1;
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator FadeOutByIndex(int sceneIndex)
    {
        FadePanel.SetActive(true);
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeCanvas.alpha = t / fadeDuration;
            yield return null;
        }

        fadeCanvas.alpha = 1;
        SceneManager.LoadScene(sceneIndex);
    }
}
