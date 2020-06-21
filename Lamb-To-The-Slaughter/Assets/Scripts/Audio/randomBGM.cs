using UnityEngine;

public class randomBGM : MonoBehaviour //Lachlan
{
    //Audio
    public AudioSource audioSourceBGM;
    public AudioClip[] BGM;

    // Start is called before the first frame update
    void Awake()
    {
        audioSourceBGM = GetComponent<AudioSource>();
        audioSourceBGM.clip = BGM[Random.Range(0, BGM.Length)];
        audioSourceBGM.loop = true;
        audioSourceBGM.Play();
    }
}
