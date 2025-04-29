    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using UnityEngine;

    public class EnemyPathing : MonoBehaviour
    {
        [SerializeField] private float enemySpeed = 1f;
        private GameObject grid;
        private GridData gridData;
        [HideInInspector] public GameObject targetObject;

        private List<Vector3Int> path = new List<Vector3Int>();
        private Vector3 target;
        private int currenntWayPoint = 0;
        [HideInInspector] public bool isAtTarget = false;
        private bool isMoving = true;

        private void Awake()
        {
            if(targetObject == null){
                targetObject = GameObject.FindWithTag("CastleTarget");
                if(targetObject != null){
                    target = targetObject.transform.position;
                }
            }
            if(grid == null){
                grid = GameObject.FindWithTag("Grid");
                if(grid != null){
                    gridData = grid.GetComponent<GridData>();
                }
            }
        }

        private void Start()
        {
            if(gridData != null){
                FindPath(transform.position, target);
            }
            isAtTarget = false;
        }

        private void Update()
        {
            if(!isMoving){
                return;
            }
            if(targetObject == null){
                FindNewTarget();
            }

            if(path.Count == 0){
                return;
            }
            Vector3 targetWorldPos = gridData.tilemap.CellToWorld(path[currenntWayPoint]);
            targetWorldPos.y = transform.position.y;

            transform.position = Vector3.MoveTowards(transform.position, targetWorldPos, enemySpeed * Time.deltaTime);

            if(transform.position == targetWorldPos){
                currenntWayPoint ++;
                if(currenntWayPoint >= path.Count){
                    Debug.Log("target reached");
                    isAtTarget = true;
                    path.Clear();
                }
            }
        }

        public void StopMoving(){
            isMoving = false;
        }

        public void ResumeMovement(){
            isMoving = true;
            FindNewTarget();
        }

        private void FindNewTarget(){
            targetObject = GameObject.FindWithTag("CastleTarget");

            if(targetObject != null){
                target = targetObject.transform.position;
                isAtTarget = false;
                path.Clear();
                currenntWayPoint = 0;

                if(gridData!= null){
                    FindPath(transform.position, target);
                }
            }
            else{
                Debug.Log("No target found");
                return;
            }
        }

        private void FindPath(Vector3 startPos, Vector3 endPos){
            Vector3Int startCell = gridData.tilemap.WorldToCell(startPos);
            Vector3Int endCell = gridData.tilemap.WorldToCell(endPos);

            List<Vector3Int> openSet = new List<Vector3Int>();
            HashSet<Vector3Int> closedSet = new HashSet<Vector3Int>();
            Dictionary<Vector3Int, Vector3Int> cameFrom = new Dictionary<Vector3Int, Vector3Int>();

            Dictionary<Vector3Int, float> gCost = new Dictionary<Vector3Int, float>();
            Dictionary<Vector3Int, float> fCost = new Dictionary<Vector3Int, float>();

            openSet.Add(startCell);
            gCost[startCell] = 0;
            fCost[startCell] = Vector3.Distance(startCell, endCell);

            while(openSet.Count > 0){
                Vector3Int current = GetLowestFScoreNode(openSet, fCost);
                openSet.Remove(current);

                if(current == endCell){
                    RetracePath(cameFrom, startCell, endCell);
                    return;
                }
                closedSet.Add(current);

                foreach(Vector3Int neighbour in GetNeighbours(current)){
                    if(closedSet.Contains(neighbour) || !gridData.IsPath(neighbour)){
                        continue;
                    }
                    float tentativeGCost = gCost[current] + 1;
                    if(!openSet.Contains(neighbour) || tentativeGCost < gCost[neighbour]){
                        cameFrom[neighbour] = current;
                        gCost[neighbour] = tentativeGCost;
                        fCost[neighbour] = gCost[neighbour] + Vector3.Distance(neighbour, endCell);
                        if(!openSet.Contains(neighbour)){
                            openSet.Add(neighbour);
                        }
                    }
                }
            }
            path.Clear();
        }

        private List<Vector3Int> GetNeighbours(Vector3Int current)
        {
            List<Vector3Int> neighbours = new List<Vector3Int>();
            Vector3Int[] directions = new Vector3Int[]{
                new Vector3Int(1,0,0), new Vector3Int(-1,0,0), new Vector3Int(0,1,0), new Vector3Int(0,-1,0)
            };

            foreach (var direction in directions){
                Vector3Int neighbour = current + direction;
                neighbours.Add(neighbour);
            }

            return neighbours;
        }

        private void RetracePath(Dictionary<Vector3Int, Vector3Int> cameFrom, Vector3Int startCell, Vector3Int endCell)
        {
            List<Vector3Int> retracedPath = new List<Vector3Int>();
            Vector3Int current = endCell;

            while(cameFrom.ContainsKey(current) && current != startCell){
                retracedPath.Add(current);
                current = cameFrom[current];
            }

            retracedPath.Reverse();
            path = retracedPath;
        }

        private Vector3Int GetLowestFScoreNode(List<Vector3Int> openSet, Dictionary<Vector3Int, float> fCost)
        {
            Vector3Int lowest = openSet[0];
            foreach (var node in openSet)
            {
                if(fCost[node] < fCost[lowest]){
                    lowest = node;
                }
            }
            return lowest;
        }
    }
