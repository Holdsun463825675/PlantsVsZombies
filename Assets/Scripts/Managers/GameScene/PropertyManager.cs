using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertyManager : MonoBehaviour
{
    public static PropertyManager Instance { get; private set; }

    private List<Property> propertyList = new List<Property>();

    private void Awake()
    {
        Instance = this;
    }

    // ÔÝÍ£¼ÌÐø
    public void Pause()
    {
        foreach (Property property in propertyList) if (property) property.Pause();
    }

    public void Continue()
    {
        foreach (Property property in propertyList) if (property) property.Continue();
    }

    public void addProperty(Property property)
    {
        propertyList.Add(property);
    }

    public void removeProperty(Property property)
    {
        propertyList.Remove(property);
    }
}
