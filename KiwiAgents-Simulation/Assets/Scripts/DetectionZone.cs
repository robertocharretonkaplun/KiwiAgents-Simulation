using UnityEngine;

public class DetectionZone : MonoBehaviour
{
  public bool hasDetectPlayer = false;
  public GameObject detectPlayerRef;

  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.CompareTag("Player"))
    {
      hasDetectPlayer = true;
      detectPlayerRef = other.gameObject;
    }
  }

  private void OnTriggerExit(Collider other)
  {
    if (other.gameObject.CompareTag("Player"))
    {
      hasDetectPlayer = false;
      detectPlayerRef = null;
    }
  }
}
