using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] List<AudioClip> clips;
    [SerializeField] AudioClip connectionClip;
    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();   
    }

    public void ActivatedByPlayerSound()
    {
        int indexRandom = Random.Range(0, clips.Count);
        audioSource.clip = clips[indexRandom];
        audioSource.Play();
    }
    public void AddChainSound()
    {
        audioSource.clip = connectionClip;
        audioSource.Play();
    }
}
