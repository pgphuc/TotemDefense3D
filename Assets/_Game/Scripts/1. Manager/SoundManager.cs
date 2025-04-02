using System.Collections;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{

    public AudioSource musicSource;

    public AudioClip bgMusic;
    public AudioClip bossMusic;

    public AudioClip battan;
    public AudioClip startWave;
    public AudioClip buildTotem;
    public AudioClip villageDamaged;
    public AudioClip victory;
    public AudioClip defeated;

    private void Start()
    {
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        PlayMusic(bgMusic);
    }

    public void PlayMusic(AudioClip clip, float fadeTime = 1.5f)
    {
        if (musicSource.isPlaying && musicSource.clip == clip) return;

        StartCoroutine(FadeInMusic(clip, fadeTime));
    }

    public void StopMusic(float fadeTime = 1.5f)
    {
        StartCoroutine(FadeOutMusic(fadeTime));
    }

    private IEnumerator FadeInMusic(AudioClip clip, float fadeTime)
    {
        if (musicSource.isPlaying) yield return FadeOutMusic(fadeTime);

        musicSource.clip = clip;
        musicSource.volume = 0;
        musicSource.Play();

        float t = 0;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(0, 1, t / fadeTime);
            yield return null;
        }
    }

    private IEnumerator FadeOutMusic(float fadeTime)
    {
        float startVolume = musicSource.volume;
        float t = 0;

        while (t < fadeTime)
        {
            t += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, 0, t / fadeTime);
            yield return null;
        }

        musicSource.Stop();
    }

    public void PlaySoundOneShot(AudioClip clip)
    {
        
        GameObject tempAudioObject = new GameObject("TempAudioSource");
        AudioSource audioSource = tempAudioObject.AddComponent<AudioSource>();

        audioSource.volume = 2f;
        audioSource.clip = clip;
        audioSource.spatialBlend = 0f;
        audioSource.PlayOneShot(clip);

        Destroy(tempAudioObject, clip.length);
    }
}