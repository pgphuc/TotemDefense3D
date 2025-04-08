using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : Singleton<SoundManager>
{
    public AudioSource musicSource;
    public float sfxVolume;

    public AudioClip bgMusic;
    public AudioClip bossMusic;

    public AudioClip battan;
    public AudioClip startWave;
    public AudioClip buildTotem;
    public AudioClip villageDamaged;
    public AudioClip victory;
    public AudioClip defeated;
    
    public AudioClip onClickButton;

    private void Start()
    {
        LoadVolume();
        PlayMusic(bgMusic);
    }

    private void LoadVolume()
    {
        float savedVolume = PlayerPrefs.GetFloat("Volume", 1f);//Mặc định lấy 1f
        musicSource.volume = savedVolume;
        sfxVolume = savedVolume;
    }

    public void SetMusicVolume(float value)
    {
        musicSource.volume = value;
        PlayerPrefs.SetFloat("Volume", value); // Lưu lại
    }
    public float GetMusicVolume()
    {
        return musicSource.volume;
    }

    public void SetSfxVolume(float value)
    {
        sfxVolume = value;
        PlayerPrefs.SetFloat("Volume", value); // Lưu lại
    }
    public float GetSfxVolume()
    {
        return sfxVolume;
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
        float savedVolumn = GetMusicVolume();
        musicSource.Play();

        float t = 0;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(0, savedVolumn, t / fadeTime);
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
        tempAudioObject.transform.SetParent(transform);
        
        AudioSource audioSource = tempAudioObject.AddComponent<AudioSource>();
        audioSource.volume = GetSfxVolume();
        audioSource.clip = clip;
        audioSource.spatialBlend = 0f;
        audioSource.PlayOneShot(clip);

        Destroy(tempAudioObject, clip.length);
    }
}