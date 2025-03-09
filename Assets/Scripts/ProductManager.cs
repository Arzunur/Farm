using System.Collections;
using System.Collections.Generic;
using UnityEngine;





// 
public interface IProduct
{
    int Id { get; set; }
    string Name { get; set; }
    void Use();
}
// Ürün Yöneticisi Sýnýfý
public class ProductManager : MonoBehaviour
{
    public List<IProduct> products = new List<IProduct>();

    public void AddProduct(IProduct product)
    {
        products.Add(product);
    }

    public void RemoveProduct(IProduct product)
    {
        products.Remove(product);
    }
}

