using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class BuildingSystem : MonoBehaviour
{

    public static BuildingSystem instance;
    public Grids gridSystem;

    public GameObject prefab1;
    public GameObject prefab2;
    public GameObject prefab3;

    private PlacebleObject objectToPlace;
    private Vector3Int prevStart;
    private Vector3Int prevSize;
    private Color originalColor;
    private bool isDragging = false;
    private Vector3 touchOffset;


    private void Awake()
    {
        instance = this;
           mainCamera = Camera.main; 
    }

    private IGrid grid;

    private bool isPlacing = false; 
    private Camera mainCamera; 

    private void Start()
    {
        grid = FindObjectOfType<Grids>(); 
    }

    private void Update()
    {
        if (isPlacing)
        {
            HandleBuildingPlacement();
        }
    }

    public void SelectPrefab1()
    {
        InitializeWithObject(prefab1);
        isPlacing = true;
    }

    public void SelectPrefab2()
    {
        InitializeWithObject(prefab2);
        isPlacing = true;
    }

    public void SelectPrefab3()
    {
        InitializeWithObject(prefab3);
        isPlacing = true;
    }
    private void HandleBuildingPlacement()
    {
        if (objectToPlace == null) return;

        if (Input.touchCount > 0)
        {
            // Dokunmatik giriþ iþlemleri (mevcut kod)
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider.gameObject == objectToPlace.gameObject)
                    {
                        isDragging = true;
                        touchOffset = objectToPlace.transform.position - GetTouchWorldPosition(touch.position);
                    }
                }
            }
            else if (touch.phase == TouchPhase.Moved && isDragging)
            {
                Vector3 newPos = GetTouchWorldPosition(touch.position) + touchOffset;
                objectToPlace.transform.position = gridSystem.GetSnappedPosition(newPos, Vector2Int.one);
                UpdateBuildingAreaIndication();
            }
            else if (touch.phase == TouchPhase.Ended && isDragging)
            {
                isDragging = false;
                if (CanBePlaced(objectToPlace))
                {
                    PlaceObject();
                }
                else
                {
                    Destroy(objectToPlace.gameObject);
                    objectToPlace = null;
                    isPlacing = false;
                }
            }
        }
        else
        {
            // Fare ile yerleþtirme ve hareket iþlemleri
            Vector3 newPos = GetTouchWorldPosition(Input.mousePosition);
            objectToPlace.transform.position = gridSystem.GetSnappedPosition(newPos, Vector2Int.one);
            UpdateBuildingAreaIndication();

            if (Input.GetMouseButtonDown(0)) // Sol fare tuþu týklandýðýnda
            {
                if (CanBePlaced(objectToPlace))
                {
                    PlaceObject();
                }
                else
                {
                    Destroy(objectToPlace.gameObject);
                    objectToPlace = null;
                    isPlacing = false;
                }
            }
        }
    }
        private Vector3 GetTouchWorldPosition(Vector2 screenPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.point;
        }
        return Vector3.zero;
    }
    public void InitializeWithObject(GameObject prefab)
    {
        if (objectToPlace != null) Destroy(objectToPlace.gameObject);

        Vector3 position = gridSystem.GetSnappedPosition(GetTouchWorldPosition(), Vector2Int.one);
        position.y = 0.1f;

        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        objectToPlace = obj.GetComponent<PlacebleObject>();

        if (objectToPlace != null)
        {
            Renderer renderer = objectToPlace.GetComponentInChildren<Renderer>();
            if (renderer != null)
            {
                originalColor = renderer.material.color;
            }
        }
    }

    private bool CanBePlaced(PlacebleObject placebleObject)
    {
        Vector3Int gridPos = WorldToGrid(placebleObject.GetStartPosition());
        Vector3Int size = placebleObject.Size;

        for (int x = 0; x < size.x; x++)
        {
            for (int z = 0; z < size.z; z++)
            {
                if (gridSystem.IsCellOccupied(gridPos.x + x, gridPos.z + z))
                {
                    return false;
                }
            }
        }
        return true;
    }

    private void PlaceObject()
    {
        objectToPlace.Place();
        Vector3Int gridPos = WorldToGrid(objectToPlace.GetStartPosition());
        OccupyArea(gridPos, objectToPlace.Size);
        ChangeBuildingColor(originalColor);
        objectToPlace = null;
        isPlacing = false; // Yerlestirme modundan cikmak
    }

    private void CancelPlacement()
    {
        Destroy(objectToPlace.gameObject);
        objectToPlace = null;
        isPlacing = false; 
    }


    public void OccupyArea(Vector3Int start, Vector3Int size)
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int z = 0; z < size.z; z++)
            {
                gridSystem.SetCellOccupied(start.x + x, start.z + z, true);
            }
        }
    }

    private void UpdateBuildingAreaIndication()
    {
        if (objectToPlace == null) return;
        
        ClearPreviousIndication();

        Vector3Int start = WorldToGrid(objectToPlace.GetStartPosition());
        Vector3Int size = objectToPlace.Size;

        bool canPlace = CanBePlaced(objectToPlace);
        Color color = canPlace ? new Color(0, 1, 0, 0.5f) : new Color(1, 0, 0, 0.5f);

        ChangeBuildingColor(color);
        prevStart = start;
        prevSize = size;
    }

    private void ChangeBuildingColor(Color color)
    {
        Renderer[] renderers = objectToPlace.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            if (renderer != null)
            {
                renderer.material.color = color;
            }
        }
    }

    private void ClearPreviousIndication()
    {
        for (int x = 0; x < prevSize.x; x++)
        {
            for (int z = 0; z < prevSize.z; z++)
            {
                gridSystem.SetCellColor(prevStart.x + x, prevStart.z + z, new Color(1, 1, 1, 0));
            }
        }
    }

  

    private Vector3Int WorldToGrid(Vector3 worldPos)
    {
        return new Vector3Int(
            Mathf.RoundToInt(worldPos.x / gridSystem.cellSize),
            0,
            Mathf.RoundToInt(worldPos.z / gridSystem.cellSize)
        );
    }
    private Vector3 GetTouchWorldPosition()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Ray ray = mainCamera.ScreenPointToRay(touch.position);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                return hit.point;
            }
        }
        return Vector3.zero;
    }
}
