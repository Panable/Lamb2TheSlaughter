using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

public class LoadingManager : MonoBehaviour
{
    public GameObject loadingScreen;
    public GameObject mainMenuUI;
    public Slider slider;
    public TMP_Text progressText;
    public RawImage rawImage;
    public GameObject videoSquare;
    public VideoPlayer video;

    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsync(sceneIndex));
        mainMenuUI.SetActive(false);
        loadingScreen.SetActive(true);
        videoSquare.SetActive(false);
        StartCoroutine(prepareVideo());
    }

    IEnumerator prepareVideo()
    {
        video.Prepare();
        WaitForSeconds waitForSeconds = new WaitForSeconds(1);
        while (!video.isPrepared)
        {
            yield return waitForSeconds;
            break;
        }
        rawImage.texture = video.texture;
        video.Play();
        videoSquare.SetActive(true);
    }


    IEnumerator LoadAsync (int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;
            progressText.text = progress * 100 + "%";
            yield return null;
        }
    }
}
    