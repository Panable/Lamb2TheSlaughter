using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using System.Runtime.CompilerServices;

public class LoadingManager : MonoBehaviour //Lachlan + Dhan
{
    public GameObject loadingScreen;
    public GameObject mainMenuUI;
    public Slider slider;
    public TMP_Text progressText;
    public RawImage rawImage;
    public GameObject videoSquare;
    public VideoPlayer video;
    public TMP_Text loadingTipsText;
    private int loadingTipNumber;
    public TMP_Text activeGuide;

    [Header("Loading Tips")]
    [TextArea]
    public string[] loadingTip;

    //Find the length of the LoadingTips array and set the text to a random one.
    private void Start()
    {
        loadingTipNumber = Random.Range(0, loadingTip.Length);
        loadingTipsText.SetText("Info: " + loadingTip[loadingTipNumber]);
    }

    //Updated the loading bar.
    void LateUpdate()
    {
        LoadingBar();
    }

    public static bool finished = false;

    //Takes into account the number of rooms to be generated vs the rooms generated. Sets the progress to how far the rooms have been generated. Once finished set loading screen to false.
    public void LoadingBar()
    {
        float numberOfRoomsGenerated = (float)ProceduralManager.numberOfRoomsGenerated;
        float totalNumberOfRoomsGenerating = (float)ProceduralManager.numberOfRoomsToGenerate;

        if (finished)
        {
            loadingScreen.SetActive(false);
            activeGuide.enabled = false;
            finished = false;
        }
        
        float progress = (numberOfRoomsGenerated / totalNumberOfRoomsGenerating);
        slider.value = progress;
        progressText.text = progress * 100 + "%";
    }

    //turn bool 'finished' to true when the loading bar is finished.
    public static void EndLoadingBar()
    {
        finished = true;
    }

    //Begin to load the level, turn off all UI elements, display loading UI; load video.
    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsync(sceneIndex));
        mainMenuUI.SetActive(false);
        loadingScreen.SetActive(true);
        videoSquare.SetActive(false);
        StartCoroutine(prepareVideo());
    }

    //Prepare video, while it is preparing have the texture be black/hidden in canvas, once its finished loading play video and replace texture.
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

    //Loads the next scene's assets while still in scene. Updates the loadding text and bar to match the progress of the load.
    IEnumerator LoadAsync(int sceneIndex)
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
