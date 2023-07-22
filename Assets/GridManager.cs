using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }
    
    public Grid Grid { get; private set; }


    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Debug.LogError("Multiple GridManagers in scene");
            Destroy(gameObject);
        }
        
        Grid = GetComponent<Grid>();
    }
    
    
    public Vector2Int GetGridPosition(Vector3 worldPosition) {
        var pos = Grid.WorldToCell(worldPosition);
        return new Vector2Int(pos.x, pos.y);
    }
    
    public Vector3 GetWorldPosition(Vector2Int gridPosition) {
        var pos = new Vector3Int(gridPosition.x, gridPosition.y, 0);
        return Grid.CellToWorld(pos);
    }
}
