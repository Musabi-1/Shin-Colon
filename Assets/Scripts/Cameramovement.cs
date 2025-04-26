using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameramovement : MonoBehaviour
{
    [SerializeField] private float cameraSpeed = 4f;
    [SerializeField] private float zoomSpeed = 2f;

    private StageControls controls;
    private Coroutine zoomCoroutine;
    private Transform cameraTransform;

    private void Awake()
    {
        controls = new StageControls();
        cameraTransform = Camera.main.transform;
    }

    private void OnEnable()
    {
        controls.Enable();        
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Start()
    {
        controls.Touch.SecondaryTouchContact.started += _ => ZoomStart();
        controls.Touch.SecondaryTouchContact.canceled += _ => ZoomEnd();   
    }

    private void Update(){
        if(controls.Touch.PrimaryFingerContact.inProgress){
            Vector2 delta = controls.Touch.PrimaryFingerDelta.ReadValue<Vector2>();

            Vector3 move = new Vector3(-delta.x, 0, -delta.y) * Time.deltaTime * cameraSpeed;
            cameraTransform.position += move;
        }
    }

    private void ZoomStart(){
        zoomCoroutine = StartCoroutine(ZoomDetection());
    }

    private void ZoomEnd(){
        StopCoroutine(zoomCoroutine);
    }

    IEnumerator ZoomDetection(){
        float previousDistance = 0f, distance = 0f;
        while(true){
            distance = Vector2.Distance(controls.Touch.PrimaryFingerPosition.ReadValue<Vector2>(), controls.Touch.SecondaryFingerPosition.ReadValue<Vector2>());
            if(distance < previousDistance){
                Vector3 targetPosition = cameraTransform.position; 
                targetPosition += cameraTransform.forward * 2;
                cameraTransform.position = Vector3.Slerp(cameraTransform.position, targetPosition, Time.deltaTime * zoomSpeed);
            }
            else if(distance > previousDistance){
                Vector3 targetPosition = cameraTransform.position; 
                targetPosition -= cameraTransform.forward * 2;
                cameraTransform.position = Vector3.Slerp(cameraTransform.position, targetPosition, Time.deltaTime * zoomSpeed);
            }
            
            previousDistance = distance;
            yield return null;
        }
    }
}
