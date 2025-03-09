using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crops : MonoBehaviour
{
    public int id;
    public bool isReadyToHarvest = false;

    void Start()
    {
        if (id == 0)
        {
           // Debug.Log("Bu�day bitkisi olu�turuldu!");
        }
        else if (id == 1)
        {
            //Debug.Log("Domates bitkisi olu�turuldu!");
        }

        Invoke("SetReadyToHarvest", 5f); 
    }

    void SetReadyToHarvest()
    {
        isReadyToHarvest = true;
       // Debug.Log("Bitki hasat edilebilir!");
    }

    void OnMouseDown()
    {
        if (isReadyToHarvest)
        {
            Harvest();
        }
    }

    void Harvest()
    {
       // Debug.Log(id == 0 ? "Bu�day hasat edildi!" : "Domates hasat edildi!");
        Destroy(this.gameObject);
    }

}
