using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using System.Runtime.CompilerServices;

public class LoadingManager : MonoBehaviour
{
    public GameObject loadingScreen;
    public GameObject mainMenuUI;
    public Slider slider;
    public TMP_Text progressText;
    public RawImage rawImage;
    public GameObject videoSquare;
    public VideoPlayer video;
    public TMP_Text loadingTipsText;
    private float loadingTipNumber;

    private void Start()
    {
        //Change this random.range value if you have more tips, Write the tips in the loadingTips() at the bottom of the script.
        loadingTipNumber = Random.Range(1, 10);
        loadingTips();
    }

    void LateUpdate()
    {
        LoadingBar();
    }

    public static bool finished = false;

    public void LoadingBar()
    {
        float numberOfRoomsGenerated = (float)ProceduralManager.numberOfRoomsGenerated;
        float totalNumberOfRoomsGenerating = (float)ProceduralManager.numberOfRoomsToGenerate;

        if (finished)
            Destroy(gameObject);

        float progress = (numberOfRoomsGenerated / totalNumberOfRoomsGenerating);
        slider.value = progress;
        progressText.text = progress * 100 + "%";
    }

    public static void EndLoadingBar()
    {
        finished = true;
    }

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

    private void loadingTips()
    {
        if (loadingTipNumber == 1)
        {
            loadingTipsText.SetText("Each time you enter the aslyum, its path always changes.");
        }
        if (loadingTipNumber == 2)
        {
            loadingTipsText.SetText("Husks walk around with spider-like legs, try to kill them from afar.");
        }
        if (loadingTipNumber == 3)
        {
            loadingTipsText.SetText("Don't forget to look for chests in each room. They contain useful utlities.");
        }
        if (loadingTipNumber == 4)
        {
            loadingTipsText.SetText("Make sure you are prepared before you verus the final foe.");
        }
        if (loadingTipNumber == 5)
        {
            loadingTipsText.SetText("Your bombs can come in handy when there are a lot of enemies, don't forget about them.");
        }
        if (loadingTipNumber == 6)
        {
            loadingTipsText.SetText("If you use a medpack and you go over 100% health, you go into an overdose state; allowing you to react quicker than before.");
        }
        if (loadingTipNumber == 7)
        {
            loadingTipsText.SetText("Your AOE scream attack is useful for pushing back enemies when in a pinch. Be careful though, as it has a cooldown.");
        }
        if (loadingTipNumber == 8)
        {
            loadingTipsText.SetText("Don't forget to reload after clearing out each room.");
        }
        if (loadingTipNumber == 9)
        {
            loadingTipsText.SetText("If it's small enough, try jumping over it.");
        }
        if (loadingTipNumber == 10)
        {
            loadingTipsText.SetText("Use your GPS to navigate the ever changing aslyum.");
        }
    }
}
