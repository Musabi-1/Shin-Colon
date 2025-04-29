using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TowerTargetEnemy : MonoBehaviour
{
    [SerializeField] private float moveToEnemySpeed = 2f;
    [SerializeField] private GameObject detectRange;
    public float attackRange = 2f;

    private TowerMovement towerMovement;
    [HideInInspector] public Transform targetEnemy;
    [HideInInspector] public bool isChasing = false;
    private List<Transform> enemiesInRange = new List<Transform>();
    private DetectRange detectRangeTrigger;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        towerMovement = GetComponent<TowerMovement>();
        detectRangeTrigger = detectRange.GetComponent<DetectRange>();
        if(detectRangeTrigger != null)
        {
            detectRangeTrigger.OnEnemyEnter += HandleEnemyEnter;
            detectRangeTrigger.OnEnemyExit += HandleEnemyExit;
        }
    }

    private void HandleEnemyEnter(Collider other)
    {
        Transform enemyTransform = other.transform;
        if (!enemiesInRange.Contains(enemyTransform))
        {
            enemiesInRange.Add(enemyTransform);
            FindClosestEnemy();
            Debug.Log("Finding closest enemy");
            if (towerMovement != null && targetEnemy != null)
            {
                isChasing = true;
                towerMovement.enabled = false;
            }
        }
    }

    private void HandleEnemyExit(Collider other)
    {
        Transform enemyTransform = other.transform;
        if (enemiesInRange.Contains(enemyTransform))
        {
            Debug.Log("Enemy exited range: " + enemyTransform.name);  // Log when enemy exits
            enemiesInRange.Remove(enemyTransform);
            FindClosestEnemy();
            if (targetEnemy == null)
            {
                StopChase();
            }
        }
    }

    private void FindClosestEnemy()
    {
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;
        foreach (Transform enemy in enemiesInRange)
        {
            if (enemy == null) continue;

            float distance = Vector3.Distance(transform.position, enemy.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }
        targetEnemy = closestEnemy;

        if (targetEnemy != null)
        {
            Debug.Log("Closest enemy found: " + targetEnemy.name);
        }
    }

    private Vector3 lastEnemyPosition;

    private void FixedUpdate()
    {
        if (targetEnemy == null || targetEnemy.gameObject == null)
        {
            enemiesInRange.Remove(targetEnemy);
            FindClosestEnemy();
            if (targetEnemy == null)
            {
                StopChase();
            }
            return;
        }

        if (isChasing && targetEnemy != null)
        {
            float distance = Vector3.Distance(transform.position, targetEnemy.position);
            if (distance <= attackRange) return;
            Vector3 offset = targetEnemy.position - transform.position;
            Vector3 direction = new Vector3(offset.x, 0f, offset.z).normalized;

            Vector3 nextPosition = transform.position + direction * moveToEnemySpeed * Time.deltaTime;
            Vector3Int nextCell = towerMovement.gridData.tilemap.WorldToCell(nextPosition);

            if (!towerMovement.gridData.IsPath(nextCell) && Vector3.Distance(nextPosition, towerMovement.circleCenter.position) <= towerMovement.circleRadius)
            {
                rb.MovePosition(nextPosition);
                lastEnemyPosition = targetEnemy.position;
            }
        }
    }

    private void StopChase()
    {
        isChasing = false;
        targetEnemy = null;

        if (towerMovement != null)
        {
            towerMovement.enabled = true;
        }
    }

    private void OnDestroy()
    {   
        if (detectRangeTrigger != null)
        {
            detectRangeTrigger.OnEnemyEnter -= HandleEnemyEnter;
            detectRangeTrigger.OnEnemyExit -= HandleEnemyExit;
        }
    }
}
