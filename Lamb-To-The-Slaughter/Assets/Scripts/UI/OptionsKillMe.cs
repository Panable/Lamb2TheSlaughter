using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OptionsKillMe : MonoBehaviour ////NEEDS COMMENTING
    public AudioMixer volumeMaster;

    public AudioMixer volumeSFX;

    //Set's slider to volume to match with mixer for BGM
    public void SetVolume(float volume)
    {
        volumeMaster.SetFloat("BGMixer", volume);
    }

    //Set's slider to volume to match with mixer for SFX
    public void SetVolumeSFX(float SFXvolume)
    {
        volumeSFX.SetFloat("SFXMixer", SFXvolume);
    }
}
