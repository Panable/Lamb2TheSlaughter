using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour //Lachlan
{

    //IT DOESN'T  WORK AT THE MOMENT
    //NEED TO UPDATE THE OTHER SCRIPT BUT ITS 5AM AND I WANNA SLEEP. SORRY. <3
    private static readonly string firstPlay = "First Play";
    private static readonly string backgroundPref = "BackgroundPref";
    private static readonly string soundEffectPref = "SoundEffectPref";
    private int firstPlayINT;
    public Slider backgroundSlider, soundEffectsSlider;
    private float backgroundFloat, soundEffectsFloat;

    public AudioSource BGMAudio;
    public AudioSource[] soundEffectsAudio;

    void Start()
    {
        firstPlayINT = PlayerPrefs.GetInt(firstPlay);

        if (firstPlayINT == 0)
        {
            //SAVES THE LAMB'S FIRST PLAY
            backgroundFloat = 0.80f;
            soundEffectsFloat = 0.70f;
            backgroundSlider.value = backgroundFloat;
            soundEffectsSlider.value = soundEffectsFloat;
            PlayerPrefs.SetFloat(backgroundPref, backgroundFloat);
            PlayerPrefs.SetFloat(soundEffectPref, soundEffectsFloat);
            PlayerPrefs.SetInt(firstPlay, -1);
        }
        else
        {
            //IF ITS NOT THE USER'S FIRST TIME GRAB THEIR SETTINGS
            backgroundFloat = PlayerPrefs.GetFloat(backgroundPref);
            backgroundSlider.value = backgroundFloat;
            soundEffectsFloat = PlayerPrefs.GetFloat(soundEffectPref);
            soundEffectsSlider.value = soundEffectsFloat;
        }
    }

    //SAVE DEM SETTINGS
    public void SaveSoundSettings()
    {
        PlayerPrefs.SetFloat(backgroundPref, backgroundSlider.value);
        PlayerPrefs.SetFloat(soundEffectPref, soundEffectsSlider.value);
    }


    //WHEN THE APPLICATION IS CLICKED OFF, THEN TELL IT THAT ITS FAMILY GONNA LEAVE SO IT HAS TO SAVE
    private void OnApplicationFocus(bool inFocus)
    {
        if (!inFocus)
        {
            SaveSoundSettings();
        }
    }

    //THISCHANGESTHEAUDIOSTUFFWITHINTHEACTUALPROJECT(MYSPACEISBROKEN)
    public void UpdateSound()
    {
        BGMAudio.volume = backgroundSlider.value;

        for (int i = 0; i < soundEffectsAudio.Length; i++)
        {
            soundEffectsAudio[i].volume = soundEffectsSlider.value;
        }
    }
}
