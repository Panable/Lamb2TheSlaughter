using UnityEngine;

public class randomBGM : MonoBehaviour //Lachlan
{
    //Audio source and BGM array
    public AudioSource audioSourceBGM;
    public AudioClip[] BGM;

    //Gets audio source and plays a random song from the BGM array and turns loop to true.
    void Awake()
    {
        audioSourceBGM = GetComponent<AudioSource>();
        audioSourceBGM.clip = BGM[Random.Range(0, BGM.Length)];
        audioSourceBGM.loop = true;
        audioSourceBGM.Play();
    }
}
