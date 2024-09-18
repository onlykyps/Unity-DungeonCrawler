using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Texture2D image;
    public Rect position;
    public List<Item> items = new List<Item>();
    int slotWidthSize = 10;
    int slotHeightSize = 4;
    public Slot[,] slots;
    public int slotX;
    public int slotY;
    public int slotW;
    public int slotH;
    private bool test;
    private Vector2 selected;
    private Vector2 secondSelected;
    private Item temp;

    void Start()
    {
        SetSlots();
        test = false;
    }

    void Update()
    {
        if (!test)
        {
            TestMethod();
        }
    }

    public bool AddPickedUpItemToInventory(Item item)
    {
        for(int x =0; x<slotWidthSize;x++)
        {
            for (int y = 0; y < slotHeightSize; y++)
            {
                if(AddItemToInventory(x, y,item))
                {
                    return true;
                }
            }
        }
        return false;
    }

    void RemoveItemFromInventory(Item item)
    {
        for (int i = 0; i < item.x + item.width; i++)
        {
            for (int o = 0; o < item.y + item.height; o++)
            {
                slots[i, o].occupied = false;

            }
        }
        items.Remove(item);
    }

    void DetectMouseAction()
    {
        for (int x = 0; x < slotWidthSize; x++)
        {
            for (int y = 0; y < slotHeightSize; y++)
            {
                Rect slot = new Rect(slots[x, y].position.x + position.x, slots[x, y].position.y + position.y, slotW, slotH);
                if (slot.Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y)))
                {
                    if (Event.current.isMouse && Input.GetMouseButtonDown(0))
                    {
                        selected.x = x;
                        selected.y = y;
                        for (int i = 0; i < items.Count; i++)
                        {
                            for (int countX = items[i].x; countX < items[i].x + items[i].width; countX++)
                            {
                                for (int countY = items[i].y; countY < items[i].y + items[i].height; countY++)
                                {
                                    if (countX == x && countY == y)
                                    {
                                        temp = items[i];
                                        RemoveItemFromInventory(temp);
                                        return;
                                    }
                                }
                            }
                        }
                        slots[x, y].item = null;
                    }
                    else if (Event.current.isMouse && Input.GetMouseButtonUp(0))
                    {
                        secondSelected.x = x;
                        secondSelected.y = y;
                        if (secondSelected.x != selected.x || secondSelected.y != selected.y)
                        {
                            if (temp != null)
                            {
                                if (AddItemToInventory((int)secondSelected.x, (int)secondSelected.y, temp))
                                {

                                }
                                else
                                {
                                    AddItemToInventory((int)temp.x, (int)temp.y, temp);

                                }
                            }
                            temp = null;
                        }
                        else
                        {
                            AddItemToInventory((int)temp.x, (int)temp.y, temp);
                            temp = null;
                        }
                    }

                }
            }
        }
    }

    void DetectGUIAction()
    {
        if (Input.mousePosition.x > position.x && Input.mousePosition.x < position.x + position.width)
        {
            if (Screen.height - Input.mousePosition.y > position.y && Input.mousePosition.y < position.y + position.height)
            {
                DetectMouseAction();
                ClickToMove.busy = true;
                return;
            }
        }
        ClickToMove.busy = false;
    }

    void TestMethod()
    {
        //AddItemToInventory(0, 0, Items.GetArmour(0));
        //AddItemToInventory(2, 2, Items.GetArmour(1));
        test = true;
    }

    void DrawItems()
    {
        for (int count = 0; count < items.Count; count++)
        {
            GUI.DrawTexture(new Rect(
                                        slotX + position.x + items[count].x * slotW,
                                        slotY + position.y + items[count].y * slotH,
                                        items[count].width * slotW,
                                        items[count].height * slotH
                                    ),
                                    items[count].image);
        }
    }

    bool AddItemToInventory(int x, int y, Item item)
    {
        for (int sX = 0; sX < item.width; sX++)
        {
            for (int sY = 0; sY < item.height; sY++)
            {
                if (slots[x, y].occupied)
                {
                    Debug.Log("breaks " + x + ", " + y);
                    return false;
                }
            }
        }
        if (x + item.width > slotWidthSize)
        {
            Debug.Log("out of " + x + " bounds");
            return false;
        }
        else if (y + item.height > slotHeightSize)
        {
            Debug.Log("out of " + y + " bounds");
            return false;
        }

        Debug.Log("added " + x + ", " + y);
        item.x = x;
        item.y = y;
        items.Add(item);
        for (int sX = x; sX < item.width + x; sX++)
        {
            for (int sY = y; sY < item.height + y; sY++)
            {
                slots[sX, sY].occupied = true;
            }
        }
        return true;
    }

    void SetSlots()
    {
        slots = new Slot[slotWidthSize, slotHeightSize];
        for (int x = 0; x < slotWidthSize; x++)
        {
            for (int y = 0; y < slotHeightSize; y++)
            {
                slots[x, y] = new Slot(new Rect(slotX + slotW * x, slotY + slotH * y, slotW, slotH));

            }
        }
    }

    void OnGUI()
    {
        DrawInventory();
        DrawSlots();
        DrawItems();
        DetectGUIAction();
        DrawDraggedItemAtMousePos();
    }

    void DrawDraggedItemAtMousePos()
    {
        if (temp != null)
        {
            GUI.DrawTexture(new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, temp.width * slotW, temp.height * slotH), temp.image);
        }
    }
    void DrawInventory()
    {
        position.x = Screen.width - position.width;
        position.y = Screen.height - position.height - Screen.height * 0.2f;
        GUI.DrawTexture(position, image);
    }

    void DrawSlots()
    {
        for (int x = 0; x < slotWidthSize; x++)
        {
            for (int y = 0; y < slotHeightSize; y++)
            {
                slots[x, y].drawSlot(position.x, position.y);
            }
        }
    }
}
