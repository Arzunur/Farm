using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public int[] id;
    public string[] productName;
    public int numberOfProducts;
    public static bool BeInShop;

    public GameObject shopUI;
    public GameObject[] products;

    private bool isShopOpen;

    void Start()
    {
        shopUI.SetActive(false);
        isShopOpen = false;

        for (int i = 0; i < Mathf.Min(numberOfProducts, products.Length); i++)
        {
            products[i].SetActive(false);
        }

        Refresh();
    }

       public void OpenShop()
    {
        if (!isShopOpen) 
        {
            shopUI.SetActive(true);
            Refresh();
            isShopOpen = true;
        }
    }

    public void CloseShop()
    {
        
            shopUI.SetActive(false);
            isShopOpen = false;
        
    }

    public void Refresh()
    {
        for (int i = 0; i < Mathf.Min(numberOfProducts, products.Length); i++)
        {
            products[i].SetActive(false);
        }

        for (int i = 0; i < numberOfProducts; i++)
        {
            products[i].GetComponent<Product>().id = id[i];
            products[i].SetActive(true);
        }
    }

    public void Update()
    {
        if (Product.isSowing)
        {
            CloseShop();
        }

    }
}
