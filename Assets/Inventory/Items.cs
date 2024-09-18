using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    public List<Armour> armourInspector;
    private static List<Armour> armour;

    void Start()
    {
        armour = armourInspector;
    }

    public static Armour GetArmour(int id)
    {
        Armour armour = new Armour();
        armour.image = Items.armour[id].image;
        armour.width = Items.armour[id].width;
        armour.height = Items.armour[id].height;
        return armour;
    }
}
