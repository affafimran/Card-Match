using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioSource m_AudioSource;

    public AudioClip soundDealCard;
    public AudioClip soundButtonClick;
    public AudioClip soundUncoverCard;
    public AudioClip soundFoundPair;
    public AudioClip soundNoPair;

    // Start is called before the first frame update
    void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip audioClip)
    {
        m_AudioSource.PlayOneShot(audioClip);
    }
}
