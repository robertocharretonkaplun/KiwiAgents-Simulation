using Unity.Cinemachine;
 using UnityEngine;
 
 //Bruwu
 
 public class CamSwitchNZoom : MonoBehaviour
 {
     [Header("Cinemachine")]
     public CinemachineCamera followCam; 
     public CinemachineCamera freeCam;
 
     [Header("Zoom")]
     [SerializeField] private float minZoom;
     [SerializeField] private float maxZoom;
     [SerializeField] private float smoothSpeed;
     [SerializeField] private float zoomMultiplier = 2f;
 
     // Separamos los zoom para cada cámara
     [Header("Zoom Variables")]
     [SerializeField] private float freeCamZoom;
     [SerializeField] private float mainCamZoom;
     // Variable para SmoothDamp, el cual se usa en el metodo SmoothDamp como referencia
     private float zoomVelocity;
 
     public bool isFreeCamActive = false;
 
     void Start()
     {
         // Se obtiene el zoom inicial de cada cámara
         mainCamZoom = followCam.Lens.FieldOfView;
         freeCamZoom = freeCam.Lens.FieldOfView;
     }
 
     void Update()
     {
         // Cambio de cámara mediante la tecla C + adición de un bool para saber si la cámara libre está activa o no
         if (Input.GetKeyDown(KeyCode.C))
         {
             followCam.Priority = 0;
             freeCam.Priority = 1;
             isFreeCamActive = true;
         }
         else if (Input.GetKeyUp(KeyCode.C))
         {
             followCam.Priority = 1;
             freeCam.Priority = 0;
             isFreeCamActive = false;
         }
 
         float scroll = Input.GetAxis("Mouse ScrollWheel");
         if (isFreeCamActive)
         {
             // Zoom para la cámara libre
             freeCamZoom -= scroll * zoomMultiplier;
             freeCamZoom = Mathf.Clamp(freeCamZoom, minZoom, maxZoom);
             freeCam.Lens.FieldOfView = Mathf.SmoothDamp(freeCam.Lens.FieldOfView, freeCamZoom, ref zoomVelocity, smoothSpeed);
         }
         else
         {
             // Zoom para la cámara principal
             mainCamZoom -= scroll * zoomMultiplier;
             mainCamZoom = Mathf.Clamp(mainCamZoom, minZoom, maxZoom);
             followCam.Lens.FieldOfView = Mathf.SmoothDamp(followCam.Lens.FieldOfView, mainCamZoom, ref zoomVelocity, smoothSpeed);
         }
     }
     //hola juan como tas bro
 }