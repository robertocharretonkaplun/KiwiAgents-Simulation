using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoopController : MonoBehaviour
{
    public static PoopController instance;

    [Header("Agente Popo")]
    public GameObject Poop;
    public GameObject PoopPosition;
    CustomAccion input;

    [Header("Limitar Prefabs")]
    public int maxPoops = 3;
    private List<GameObject> activePoops = new List<GameObject>();

    public delegate void PoopEvent(bool hasPoop);
    public static event PoopEvent OnPoopStatusChanged;

    [Header("Audio Settings")]
    public AudioClip[] poopSounds;  
    public AudioClip flySound;       

    private void Awake() {
        if (instance != null && instance != this) {
            return;
        }
        instance = this;
        input = new CustomAccion();
    }

    private void Start() {
        AssingInputs();
    }

    void AssingInputs() {
        input.Main.Poop.performed += ctx => ClicKToPoop();
    }

    void ClicKToPoop()
    {
        if (activePoops.Count < maxPoops)
        {
            GameObject poopTemporal = Instantiate(Poop, PoopPosition.transform.position, PoopPosition.transform.rotation);
            activePoops.Add(poopTemporal);
            OnPoopStatusChanged?.Invoke(true);
            Destroy(poopTemporal, 5f);
            StartCoroutine(RemovePoopFromList(poopTemporal, 5f));

            // Reproducir sonido de popó aleatorio (global, a través del AudioManager)
            if (AudioManager.instance != null && poopSounds.Length > 0)
            {
                int randomIndex = Random.Range(0, poopSounds.Length);
                AudioManager.instance.PlaySound(poopSounds[randomIndex]);
            }

            // Iniciar el sonido de moscas en el objeto de la popó después de un pequeño retraso
            StartCoroutine(PlayFlySoundAfterDelay(poopTemporal, 0.1f));
        }
    }

    private IEnumerator PlayFlySoundAfterDelay(GameObject poopInstance, float delay) {
        yield return new WaitForSeconds(delay);
        //Obtener el componente AudioSource del objeto popó
        AudioSource audioSource = poopInstance.GetComponent<AudioSource>();
        if (AudioManager.instance != null) {
            audioSource = poopInstance.AddComponent<AudioSource>();
        }
        // Configura el AudioSource para sonido 3D
        audioSource.clip = flySound;
        audioSource.loop = true;
        audioSource.spatialBlend = 1f;  // 1 = 3D
        audioSource.Play();
    }

    private IEnumerator RemovePoopFromList(GameObject poop, float delay) {
        yield return new WaitForSeconds(delay);
        activePoops.Remove(poop);

        if (activePoops.Count == 0) {
            OnPoopStatusChanged?.Invoke(false);

            // Detener el sonido de las moscas 
            if (AudioManager.instance != null) {
                AudioManager.instance.StopLoopingSound();
            }
        }
    }

    void OnEnable() {
        input.Enable();
    }

    void OnDisable() {
        input.Disable();
    }
}
