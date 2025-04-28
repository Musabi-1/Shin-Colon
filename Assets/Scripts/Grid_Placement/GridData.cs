using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridData : MonoBehaviour
{
    public HashSet<Vector3Int> occupiedCells = new HashSet<Vector3Int>();

    public Tilemap tilemap;
    [SerializeField] private TileBase pathTile;

    public bool IsPath(Vector3Int cellPosition){
        return tilemap.GetTile(cellPosition) == pathTile;
    }
}
