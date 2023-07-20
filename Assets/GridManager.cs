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
    
    
    public Vector3Int GetGridPosition(Vector3 worldPosition) {
        return Grid.WorldToCell(worldPosition);
    }
    
    public Vector3 GetWorldPosition(Vector3Int gridPosition) {
        return Grid.CellToWorld(gridPosition);
    }
}
