using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class Item : ScriptableObject
{
    [Header("Item Information")]
    public string itemName;
    public GameObject itemModel;
}
