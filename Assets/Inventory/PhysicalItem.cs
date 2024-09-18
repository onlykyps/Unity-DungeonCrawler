using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalItem : MonoBehaviour
{
    public int itemRep;
    public static Inventory inventory;
    public GameObject gui;

    void Update()
    {
        
    }

    void OnMouseDown()
    {
        inventory = gui.GetComponent<Inventory>();
        inventory.AddPickedUpItemToInventory(Items.GetArmour(itemRep));
        Debug.Log("Pick me up");
        Destroy(gameObject);
    }
}
