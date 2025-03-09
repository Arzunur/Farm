using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fields : MonoBehaviour
{

    public GameObject field;
    public Vector2Int fieldSize = new Vector2Int(1, 1);
    public Transform fieldsParent;
    private bool isCreatingFields = false;
    public void ToggleCreateFields()
    {
        isCreatingFields = !isCreatingFields;
    }

    private IGrid grid;

    public void CreateFields()
    {
        isCreatingFields = !isCreatingFields;
    }

    private void Start()
    {
        grid = FindObjectOfType<Grids>();
    }

    private void Update()
    {
        if (isCreatingFields && Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity))
            {
                if (hit.transform.CompareTag("Grid"))
                {
                    Vector3 snappedPos = grid.GetSnappedPosition(hit.point, fieldSize);
                    int startX = Mathf.RoundToInt(snappedPos.x / grid.CellSize);
                    int startZ = Mathf.RoundToInt(snappedPos.z / grid.CellSize);

                    if (CanPlaceField(startX, startZ))
                    {
                        GameObject newField = Instantiate(field, snappedPos, Quaternion.identity);
                        newField.transform.localScale = new Vector3(grid.CellSize * 0.2f, 0.2f, grid.CellSize * 0.2f);
                        newField.transform.SetParent(fieldsParent);
                        newField.tag = "Field";
                        OccupyCells(startX, startZ);
                       // Debug.Log("Yeni tarla oluþturuldu ve tag atandý: " + newField.tag);
                    }
                    else
                    {
                     //   Debug.Log("Bu alan dolu!");
                    }
                }
            }
        }
    }

    private bool CanPlaceField(int startX, int startZ)
    {
        for (int x = 0; x < fieldSize.x; x++)
        {
            for (int z = 0; z < fieldSize.y; z++)
            {
                if (grid.IsCellOccupied(startX + x, startZ + z))
                {
                    return false;
                }
            }
        }

        Vector3 fieldCenter = grid.GetSnappedPosition(new Vector3(startX * grid.CellSize, 0, startZ * grid.CellSize), fieldSize);
        Vector3 fieldHalfExtents = new Vector3(fieldSize.x * grid.CellSize / 2f, 0.1f, fieldSize.y * grid.CellSize / 2f);
        Collider[] hitColliders = Physics.OverlapBox(fieldCenter, fieldHalfExtents);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Field"))
            {
                return false;
            }
        }

        return true;
    }

    private void OccupyCells(int startX, int startZ)
    {
        for (int x = 0; x < fieldSize.x; x++)
        {
            for (int z = 0; z < fieldSize.y; z++)
            {
                grid.SetCellOccupied(startX + x, startZ + z, true);
            }
        }
    }

    public void ReturnToNormal() => enabled = false;


}
