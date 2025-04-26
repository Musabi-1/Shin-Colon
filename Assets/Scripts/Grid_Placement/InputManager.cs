using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera sceneCamera;
    [SerializeField] private LayerMask placementLayerMask;
    private Vector3 lastPosition;
    
    public Vector3 ScreenToWorldPosition(PointerEventData eventData){
        Vector2 pointerPos = eventData.position;
        Ray ray = sceneCamera.ScreenPointToRay(pointerPos);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 100f, placementLayerMask)){
            lastPosition = hit.point;
        }
        return lastPosition;
    }
}