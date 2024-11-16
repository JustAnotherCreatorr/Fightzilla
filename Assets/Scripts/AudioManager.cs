using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    public AudioClip Fighting;
    public AudioClip MainMenu;
    public AudioClip hurt;
    public AudioClip block;
    public AudioClip KO;
    public AudioClip countdown;
    public AudioClip win;
    public AudioClip beep;

    public SceneManager2 sceneManager;
    public PlayerHealthUIManager p1;
    public PlayerHealthUIManager p2;

    // Start is called before the first frame update
    void Start()
    {

        //these are mixed up for some reason, but work

        if (sceneManager.currentScene == 0)
        {
            musicSource.clip = Fighting;
            musicSource.Play();
        }

        if (sceneManager.currentScene == 1)
        {
            musicSource.clip = MainMenu;
            musicSource.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void PauseAudio()
    {
        musicSource.Pause();
    }

    public void ResumeAudio()
    {
        musicSource.Play();
    }
}
