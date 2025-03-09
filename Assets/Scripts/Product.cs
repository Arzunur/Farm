using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Product : MonoBehaviour
{
    [Header("Shop")]
    public GameObject shop;
    public int id;

    [Header("Product")]
    [SerializeField] private string productName;
    [SerializeField] private TextMeshProUGUI nameText;

    [Header("Fields")]
    public Fields fields;

    [Header("Planting")]
    public static bool placeSeeds;
    public static int whichSeed;
    public static bool isSowing;

    [Header("Raycast")]
    public GameObject hitted;
    public GameObject seed;
    public RaycastHit hit;

    void Start()
    {
        shop = GameObject.Find("ShopManager");
        productName = shop.GetComponent<Shop>().productName[id];
    }

    void Update()
    {
        nameText.text = productName;
        productName = shop.GetComponent<Shop>().productName[id];

        //

        if (Input.GetMouseButtonDown(0))
        {

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) ;
            {
                if (Product.isSowing == true)
                {
                    if (hit.transform.tag == "Field")
                    {
                        hitted = hit.transform.gameObject;
                        Instantiate(seed, hitted.transform.position, Quaternion.identity);
                        Destroy(hitted);
                    }
                }
            }
        }
    }


    public void Buy(int seedType)
    {
        isSowing = true;
        placeSeeds = true;
        whichSeed = seedType;

        Debug.Log(seedType == 0 ? "Buðday ekildi!" : "Domates ekildi!");
        hitted = hit.transform.gameObject;
        GameObject newSeed = Instantiate(seed, hitted.transform.position, Quaternion.identity);
        Seed seedScript = newSeed.GetComponent<Seed>();
        seedScript.id = whichSeed;
        seedScript.growthTime = (whichSeed == 0) ? 5f : 3f;
        seedScript.StartGrowth();

        isSowing = true;
        placeSeeds = true;
    }
}


