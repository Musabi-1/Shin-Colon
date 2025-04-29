using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TowerMovement : MonoBehaviour
{
    [SerializeField] private float movementInterval = 1f;
    [SerializeField] private float movementTime = 2f;
    [SerializeField] private float movementSpeed = 2f;
    [SerializeField] private GameObject movableCircle;
    [HideInInspector] public GridData gridData;
    [HideInInspector] public float circleRadius;
    [HideInInspector] public Transform circleCenter;
    private Vector3 moveDirection;
    private bool isMoving = false;
    private Rigidbody rb;

    private void Start()
    {
        GameObject grid = GameObject.FindWithTag("Grid");
        gridData=grid.GetComponent<GridData>();
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        rb.useGravity = false;

        circleCenter = movableCircle.transform;
        SpriteRenderer sr = movableCircle.GetComponent<SpriteRenderer>();
        float spriteWidth = sr.sprite.bounds.size.x;
        float worldWidth = spriteWidth * movableCircle.transform.lossyScale.x;
        circleRadius = worldWidth / 2f;

        TryRecoverFromInvalidPosition();
        StartCoroutine(MoveRoutine());
    }

    private void FixedUpdate()
    {
        if(TryRecoverFromInvalidPosition()){
            isMoving = false;
            return;
        }

        if(isMoving){
            Vector3 targetPosition = rb.position + moveDirection * movementSpeed * Time.deltaTime;

            if(Vector3.Distance(circleCenter.position, targetPosition) <= circleRadius){
                Vector3Int cellPosition = gridData.tilemap.WorldToCell(targetPosition);
                if(!gridData.IsPath(cellPosition)){
                    rb.MovePosition(targetPosition);
                }
                else{
                    isMoving = false;
                }
            }
            else{
                isMoving = false;
            }
        }
    }

    private IEnumerator MoveRoutine(){
        while(true){
            for(int i = 0; i< 10; i++){
                Vector2 randomDir2D = Random.insideUnitCircle.normalized;
                Vector3 candidateDirection = new Vector3(randomDir2D.x, 0f, randomDir2D.y);
                Vector3 projectedPosition = transform.position + candidateDirection * 0.5f;
                if(Vector3.Distance(circleCenter.position, projectedPosition) <= circleRadius){
                    Vector3Int cellPos = gridData.tilemap.WorldToCell(projectedPosition);
                    if(!gridData.IsPath(cellPos)){
                        moveDirection = candidateDirection;
                        break;
                    }
                }
                if(i == 9){
                    Vector3 fallbackDirection = (circleCenter.position - transform.position).normalized;
                    fallbackDirection.y = 0f;
                    moveDirection = fallbackDirection;
                }
            }
            isMoving = true;
            yield return new WaitForSeconds(movementTime);

            isMoving = false;
            yield return new WaitForSeconds(movementInterval);
        }
    }
    private bool TryRecoverFromInvalidPosition()
    {
        Vector3Int cell = gridData.tilemap.WorldToCell(rb.position);
        bool invalid = gridData.IsPath(cell) || Vector3.Distance(circleCenter.position, rb.position) > circleRadius;
        if (invalid)
        {
            Vector3 fallbackDirection = (circleCenter.position - transform.position).normalized;
            fallbackDirection.y = 0f;
            Vector3 recoveryPosition = transform.position + fallbackDirection * movementSpeed * Time.deltaTime;
            Vector3Int recoveryCell = gridData.tilemap.WorldToCell(recoveryPosition);
            if (!gridData.IsPath(recoveryCell))
            {
                rb.MovePosition(recoveryPosition);
                return true;
            }
        }
        return false;
    }
}
