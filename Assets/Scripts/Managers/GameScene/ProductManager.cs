using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductManager : MonoBehaviour
{
    public static ProductManager Instance { get; private set; }

    private List<Product> productList = new List<Product>();

    private void Awake()
    {
        Instance = this;
    }

    // ÔÝÍ£¼ÌÐø
    public void Pause()
    {
        foreach (Product product in productList) if (product) product.Pause();
    }

    public void Continue()
    {
        foreach (Product product in productList) if (product) product.Continue();
    }

    public void addProduct(Product product)
    {
        productList.Add(product);
    }

    public void removeProduct(Product product)
    {
        productList.Remove(product);
    }
}
