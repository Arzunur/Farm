using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public  class Seed : MonoBehaviour
{
    public int id;
    public float growthTime;

    public GameObject[] cropPrefabs;
    private ObjectPool cropPool;

    [SerializeField] private int poolSize = 10;

    void Start()
    {
        InitializeObjectPool();
    }

    void InitializeObjectPool()
    {
        if (cropPrefabs != null && cropPrefabs.Length > id && cropPrefabs[id] != null)
        {
            cropPool = new ObjectPool(cropPrefabs[id], poolSize); 
        }
        else
        {
            Debug.LogError("Crops prefab'ý bulunamadý veya geçersiz id!");
            Destroy(gameObject);
        }
    }

    public void StartGrowth()
    {
        StartCoroutine(Grow());
    }

    IEnumerator Grow()
    {
        Debug.Log("Büyüme baþladý, süre: " + growthTime + " saniye");
        yield return new WaitForSeconds(growthTime);

        GameObject newCrops = cropPool.GetObject();
        if (newCrops != null)
        {
            newCrops.transform.position = transform.position;
            newCrops.transform.rotation = Quaternion.identity;
            newCrops.SetActive(true);

            Crops cropsComponent = newCrops.GetComponent<Crops>();
            if (cropsComponent != null)
            {
                cropsComponent.id = id;
            }
            else
            {
                Debug.LogError("Crops bileþeni bulunamadý!");
            }
        }

        Destroy(gameObject);
    }
}

public class ObjectPool
{
    private GameObject prefab;
    private Queue<GameObject> inactiveObjects;

    public ObjectPool(GameObject prefab, int initialPoolSize)
    {
        this.prefab = prefab;
        inactiveObjects = new Queue<GameObject>();

        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject obj = GameObject.Instantiate(prefab);
            obj.SetActive(false);
            inactiveObjects.Enqueue(obj);
        }
    }

    public GameObject GetObject()
    {
        if (prefab == null)
        {
            Debug.LogError("Prefab bulunamadý!");
            return null;
        }

        if (inactiveObjects.Count > 0)
        {
            return inactiveObjects.Dequeue();
        }

        GameObject obj = GameObject.Instantiate(prefab);
        inactiveObjects.Enqueue(obj);
        return obj;
    }

    

}
