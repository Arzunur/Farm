using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacebleObject : MonoBehaviour
{
    public bool Placed { get; private set; }
    public Vector3Int Size { get; private set; }
    private Vector3[] Vertices;

    public BoundsInt area;

    private void GetColliderVertexPositionsLocal()
    {
        BoxCollider b = gameObject.GetComponent<BoxCollider>();
        Vertices = new Vector3[4];
        Vertices[0] = b.center + new Vector3(-b.size.x, -b.size.y + b.size.y / 2.0f, -b.size.z) * 0.5f;
        Vertices[1] = b.center + new Vector3(b.size.x, -b.size.y + b.size.y / 2.0f, -b.size.z) * 0.5f;
        Vertices[2] = b.center + new Vector3(b.size.x, -b.size.y + b.size.y / 2.0f, b.size.z) * 0.5f;
        Vertices[3] = b.center + new Vector3(-b.size.x, -b.size.y + b.size.y / 2.0f, b.size.z) * 0.5f;
    }

    private void CalculateSizeInCells()
    {
        Vector3Int[] vertices = new Vector3Int[Vertices.Length];

        for(int i = 0; i < Vertices.Length; i++)
        {
            Vector3 worldPos = transform.TransformPoint(Vertices[i]);
        }

        Size = new Vector3Int(Mathf.Abs((vertices[0] - vertices[1]).x)+1,
                              Mathf.Abs((vertices[0] - vertices[3]).y)+1,
                              1); 

        Debug.Log("Building Size (x,y,z):" + Size.ToString());
    }

    public Vector3 GetStartPosition()
    {
        return transform.TransformPoint(Vertices[0]);
    }

    private void Awake()
    {
        GetColliderVertexPositionsLocal();
    }


    private void Start()
    {
        CalculateSizeInCells();
        InitialiseArea();
    }

    private void InitialiseArea()
    {
        area = new BoundsInt(new Vector3Int(0, 0, 0), Size);
    }

    public virtual void Place()
    {
        ObjectDrag drag = gameObject.GetComponent<ObjectDrag>();
        Destroy(drag);

        Placed = true;
    }

}
