using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class VolumeButton : MonoBehaviour //Lachlan (UI Functionality) & Ansaar (Value Conversion)
{
    #region Variables
    private int displayedMusicValue;
    private int musicValue;
    private int soundValue;

    public TMP_Text BGMText;
    public TMP_Text BGMTextUnder;
    public TMP_Text SFXText;
    public TMP_Text SFXTextUnder;
    public TMP_Text QualityText;
    public TMP_Text QualityTextUnder;
    public Slider BGvolume;
    public Slider SFXvolume;
    public int qualityLevel;
    #endregion

    //Initialisation
    private void Start()
    {
        musicValue = 100;
        BGvolume.value = 1;
        soundValue = 100;
        SFXvolume.value = 1;
        qualityLevel = 4;
        //This Ensures the text changes to the current setting
        QualitySettings.SetQualityLevel(qualityLevel);
        QualityText.SetText(qualityLevel.ToString());
        QualityTextUnder.SetText(qualityLevel.ToString());
        BGMText.SetText(musicValue.ToString() + "%");
        BGMTextUnder.SetText(musicValue.ToString() + "%");
        SFXText.SetText(soundValue.ToString() + "%");
        SFXTextUnder.SetText(soundValue.ToString() + "%");

    }

    //Increases Background Music
    public void ButtonPlusClickedBG()
    {
        if (musicValue == 100)
        {
            musicValue = 100;
        }
        else
        {
            musicValue += 10;
        }
        BGMText.SetText(musicValue.ToString() + "%"); //Add the text component here and above make it public! :D
        BGMTextUnder.SetText(musicValue.ToString() + "%");
    }

    //Lowers Background Music
    public void ButtonMinusClickedBG()
    {
        if (musicValue == 0)
        {
            musicValue = 0;
        }
        else
        {
            musicValue -= 10;
        }
        BGMText.SetText(musicValue.ToString() + "%"); //Add the text component here and above make it public! :D
        BGMTextUnder.SetText(musicValue.ToString() + "%");
    }

    //Increases Sound Effects Volume 
    public void ButtonPlusClickedSFX()
    {
        if (soundValue == 100)
        {
            soundValue = 100;
        }
        else
        {
            soundValue += 10;
        }
        SFXText.SetText(soundValue.ToString() + "%");
        SFXTextUnder.SetText(soundValue.ToString() + "%");
    }

    //Decreases Sound Effects Volume
    public void ButtonMinusClickedSFX()
    {
        if (soundValue == 0)
        {
            soundValue = 0;
        }
        else
        {
            soundValue -= 10;
        }
        SFXText.SetText(soundValue.ToString() + "%");
        SFXTextUnder.SetText(soundValue.ToString() + "%");
    }

    //Value Regulation
    private void Update()
    {
        //Debug.Log(qualityLevel);
        musicValue = Mathf.Clamp(musicValue, 0, 100);
        soundValue = Mathf.Clamp(soundValue, 0, 100);
        qualityLevel = Mathf.Clamp(qualityLevel, 0, 3);
        MusicDecibelConverter();
        SoundEffectDecibelConverter();
        QualityLevelConverter();

        qualityLevel = QualitySettings.GetQualityLevel();
    }

    //Converts Decibel to a displayable percentage value (Music)
    void MusicDecibelConverter()
    {
        if (musicValue == 0)
        {
            BGvolume.value = -80f;
        }
        else if (musicValue == 10)
        {
            BGvolume.value = -60f;
        }
        else if (musicValue == 20)
        {
            BGvolume.value = -40f;
        }
        else if (musicValue == 30)
        {
            BGvolume.value = -20f;
        }
        else if (musicValue == 40)
        {
            BGvolume.value = -10f;
        }
        else if (musicValue == 50)
        {
            BGvolume.value = -8f;
        }
        else if (musicValue == 60)
        {
            BGvolume.value = -6f;
        }
        else if (musicValue == 70)
        {
            BGvolume.value = -4f;
        }
        else if (musicValue == 80)
        {
            BGvolume.value = -2f;
        }
        else if (musicValue == 90)
        {
            BGvolume.value = 0f;
        }
        else if (musicValue == 100)
        {
            BGvolume.value = 1f;
        }
    }

    //Converts Decibel to a displayable percentage value (SFX)
    void SoundEffectDecibelConverter()
    {

        if (soundValue == 0)
        {
            SFXvolume.value = -80f;
        }
        else if (soundValue == 10)
        {
            SFXvolume.value = -60f;
        }
        else if (soundValue == 20)
        {
            SFXvolume.value = -40f;
        }
        else if (soundValue == 30)
        {
            SFXvolume.value = -20f;
        }
        else if (soundValue == 40)
        {
            SFXvolume.value = -10f;
        }
        else if (soundValue == 50)
        {
            SFXvolume.value = -8f;
        }
        else if (soundValue == 60)
        {
            SFXvolume.value = -6f;
        }
        else if (soundValue == 70)
        {
            SFXvolume.value = -4f;
        }
        else if (soundValue == 80)
        {
            SFXvolume.value = -2f;
        }
        else if (soundValue == 90)
        {
            SFXvolume.value = 0f;
        }
        else if (soundValue == 100)
        {
            SFXvolume.value = 1f;
        }
    }

    //Converts Quality to a displayable string
    void QualityLevelConverter()
    {
        if (qualityLevel == 0)
        {
            QualityText.SetText("Low");
            QualityTextUnder.SetText("Low");
        }
        else if (qualityLevel == 1)
        {
            QualityText.SetText("Medium");
            QualityTextUnder.SetText("Medium");
        }
        else if (qualityLevel == 2)
        {
            QualityText.SetText("High");
            QualityTextUnder.SetText("High");
        }
        else if(qualityLevel == 3)
        {
            QualityText.SetText("Ultra");
            QualityTextUnder.SetText("Ultra");
        }
    }

    //Set the Graphic Quality
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    //Increase Graphic Quality
    public void QualityPlus()
    {
        if (qualityLevel == 3)
        {
            qualityLevel = 3;
        }
        else
        {
            qualityLevel = qualityLevel + 1;
        }
        QualitySettings.SetQualityLevel(qualityLevel);
        QualityText.SetText(qualityLevel.ToString());
        QualityTextUnder.SetText(qualityLevel.ToString());
    }

    //Decrease Graphic Quality
    public void QualityMinuis()
    {
        if (qualityLevel == 0)
        {
            qualityLevel = 0;
        }
        else
        {
            qualityLevel = qualityLevel - 1;
        }
        QualitySettings.SetQualityLevel(qualityLevel);
        QualityText.SetText(qualityLevel.ToString());
        QualityTextUnder.SetText(qualityLevel.ToString());
    }
}