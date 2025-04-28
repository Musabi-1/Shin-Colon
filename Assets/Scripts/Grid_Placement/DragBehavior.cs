using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragBehaviour : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private GameObject spawnObj;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Elixir elixirSystem;
    [SerializeField] private GameObject gridVisuals;
    [SerializeField] private Grid grid;
    [SerializeField] private int count = 3;
    [SerializeField] private int cost = 2;
    [SerializeField] private GridData gridData;
    private GameObject draggedObj;
    private Vector3 worldPos;
    private Vector3Int gridPos;
    private Vector3 objPos;

    private void Awake()
    {
        gridVisuals.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(cost <= elixirSystem.elixirCount && count > 0){
            gridVisuals.SetActive(true);
            worldPos = inputManager.ScreenToWorldPosition(eventData);
            gridPos = grid.WorldToCell(worldPos);
            objPos = grid.CellToWorld(gridPos);
            objPos.y = 0.02f;
            draggedObj = Instantiate(spawnObj, objPos, Quaternion.identity);
        }
        else{
            eventData.pointerDrag = null;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(draggedObj != null){
            worldPos = inputManager.ScreenToWorldPosition(eventData);
            gridPos = grid.WorldToCell(worldPos);
            objPos = grid.CellToWorld(gridPos);
            objPos.y = 0.02f;
            draggedObj.transform.position = objPos;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector3Int currentCell = grid.WorldToCell(draggedObj.transform.position);
        bool isPath = gridData.IsPath(currentCell);
        if(EventSystem.current.IsPointerOverGameObject() || gridData.occupiedCells.Contains(currentCell) || isPath){
            Destroy(draggedObj);
            gridVisuals.SetActive(false);
            return;
        }

        else{
            gridData.occupiedCells.Add(currentCell);
            elixirSystem.RemoveElixir(cost);
            count--;
            if(count < 1){
                this.gameObject.SetActive(false);
            }
        }
        gridVisuals.SetActive(false);
        draggedObj = null;
    }
}
