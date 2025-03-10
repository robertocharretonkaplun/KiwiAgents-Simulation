using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Settings")]
    public AudioSource audioSource;    
    public AudioClip footstepSound;    

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
            Debug.Log("Reproduciendo sonido de paso");
            audioSource.clip = footstepSound;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Falta asignar AudioSource o FootstepSound en el AudioManager");
        }
    }
}
