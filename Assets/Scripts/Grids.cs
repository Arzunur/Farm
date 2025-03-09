using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IGrid
{
    Vector3 GetSnappedPosition(Vector3 worldPosition, Vector2Int fieldSize);
    bool IsCellOccupied(int x, int z);
    void SetCellOccupied(int x, int z, bool occupied);
    void SetCellColor(int x, int z, Color color);
    int ColumnLength { get; }
    int RowLength { get; }
    float CellSize { get; }
}
public class Grids : MonoBehaviour, IGrid
{
    [SerializeField] private int columnLength, rowLength;
    public float cellSize;
    [SerializeField] private GameObject grassPrefab;
    public Transform gridParent;

    private GameObject[,] gridArray;
    private bool[,] occupiedCells;

    public int ColumnLength => columnLength;
    public int RowLength => rowLength;
    public float CellSize => cellSize;

    private void Start()
    {
        InitializeGrid();
    }

    private void InitializeGrid()
    {
        gridArray = new GameObject[columnLength, rowLength];
        occupiedCells = new bool[columnLength, rowLength];

        if (gridParent == null)
        {
            gridParent = new GameObject("GridParent").transform;
        }

        for (int x = 0; x < columnLength; x++)
        {
            for (int z = 0; z < rowLength; z++)
            {
                Vector3 position = new Vector3(x * cellSize, 0, z * cellSize);
                GameObject newCell = Instantiate(grassPrefab, position, Quaternion.identity);
                newCell.transform.SetParent(gridParent);
                gridArray[x, z] = newCell;
                occupiedCells[x, z] = false;
            }
        }
    }

    public Vector3 GetSnappedPosition(Vector3 worldPosition, Vector2Int fieldSize)
    {
        int x = Mathf.RoundToInt(worldPosition.x / cellSize);
        int z = Mathf.RoundToInt(worldPosition.z / cellSize);

        float snappedX = (x - (fieldSize.x - 1) / 2f) * cellSize;
        float snappedZ = (z - (fieldSize.y - 1) / 2f) * cellSize;

        return new Vector3(snappedX, 0, snappedZ);
    }

    public bool IsCellOccupied(int x, int z)
    {
        if (x >= 0 && x < columnLength && z >= 0 && z < rowLength)
        {
            return occupiedCells[x, z];
        }
        return true;
    }

    public void SetCellOccupied(int x, int z, bool occupied)
    {
        if (x >= 0 && x < columnLength && z >= 0 && z < rowLength)
        {
            occupiedCells[x, z] = occupied;
        }
    }

    public void SetCellColor(int x, int z, Color color)
    {
        if (x >= 0 && x < columnLength && z >= 0 && z < rowLength)
        {
            Renderer renderer = gridArray[x, z].GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = color;
            }
        }
    }
}

