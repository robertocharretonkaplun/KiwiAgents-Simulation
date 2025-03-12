using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Settings")]
    public AudioSource audioSource;    
    public AudioClip footstepSound;

    private AudioSource flyAudioSource; 

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayFootstep()
    {
    if (footstepSound != null && audioSource != null)
    {
        audioSource.PlayOneShot(footstepSound);
    }
    else
    {
        Debug.LogWarning("Falta asignar AudioSource o FootstepSound en el AudioManager");
    }
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("Falta asignar AudioSource o el AudioClip en PlaySound.");
        }
    }

    public void PlayLoopingSound(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("Intentando reproducir un sonido en loop sin asignar el AudioClip.");
            return;
        }

        if (flyAudioSource == null)
        {
            flyAudioSource = gameObject.AddComponent<AudioSource>();
            flyAudioSource.loop = true;
        }

        flyAudioSource.clip = clip;
        flyAudioSource.Play();
    }

    
    public void StopLoopingSound()
    {
        if (flyAudioSource != null && flyAudioSource.isPlaying)
        {
            flyAudioSource.Stop();
        }
    }
}
