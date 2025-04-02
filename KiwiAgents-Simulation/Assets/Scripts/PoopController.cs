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

    private 
    void 
    Awake() {
        if (instance != null && instance != this) {
            return;
        }
        instance = this;
        input = new CustomAccion();
    }

    private 
    void 
    Start() {
        AssingInputs();
    }

    void 
    AssingInputs() {
        input.Main.Poop.performed += ctx => ClicKToPoop();
    }

    void 
    ClicKToPoop() {
        if (activePoops.Count < maxPoops) {
            GameObject PoopTemporal = Instantiate(Poop, PoopPosition.transform.position, PoopPosition.transform.rotation);
            activePoops.Add(PoopTemporal);
            OnPoopStatusChanged?.Invoke(true); // Notificar a los enemigos que hay un Poop activo
            Destroy(PoopTemporal, 5f);
            StartCoroutine(RemovePoopFromList(PoopTemporal, 5f));
        }
    }

    private 
    IEnumerator 
    RemovePoopFromList(GameObject poop, 
                       float delay) {
        yield return new WaitForSeconds(delay);
        activePoops.Remove(poop);
        if (activePoops.Count == 0) {
            OnPoopStatusChanged?.Invoke(false); // Notificar que ya no hay Poop activo
        }
    }

    void 
    OnEnable() {
        input.Enable();
    }

    void OnDisable()
    {
        if (input != null)
            input.Disable();
    }
}