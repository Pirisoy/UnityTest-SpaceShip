using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private List<AudioSource> actionAudioSources;
    [SerializeField] private AudioSource musicAudioSource;

    private int actionIndex = 0;

    public static SoundManager Singelton { get; private set; }
    private void Awake()
    {
        Singelton = this;
    }
    private void OnDestroy()
    {
        Singelton = null;
    }
    private void Start()
    {
        actionIndex = 0;
    }
    public void PlayMusic(AudioClip musicClip)
    {
        if (musicClip == null)
            return;
        if (musicAudioSource.clip != null)
            if (musicAudioSource.clip.name == musicClip.name)
                return;
        musicAudioSource.clip = musicClip;
        musicAudioSource.Play();
    }

    public void PlayActionClip(AudioClip actionClip)
    {
        if (actionClip == null)
            return;

        AudioSource audioSource = GetAudioSource();

        audioSource.clip = actionClip;
        audioSource.Play();
    }
    private AudioSource GetAudioSource()
    {
        bool hasEmpty = false;
        int tempIndex = actionIndex;
        while (true)
        {
            if (!actionAudioSources[actionIndex].isPlaying)
            {
                hasEmpty = true;
                break;
            }

            actionIndex++;
            if (actionIndex == actionAudioSources.Count)
                actionIndex = 0;

            if (actionIndex == tempIndex)
                break;
        }

        if (hasEmpty)
            return actionAudioSources[actionIndex];

        GameObject obj = new GameObject();
        obj.transform.SetParent(this.transform);
        AudioSource audioListener =  obj.AddComponent<AudioSource>();
        actionAudioSources.Add(audioListener);

        return audioListener;
    }
}