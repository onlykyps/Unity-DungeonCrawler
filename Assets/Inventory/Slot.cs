using UnityEngine;

[System.Serializable]
public class Slot
{
    public Item item;
    public bool occupied;
    public Rect position;

    public void drawSlot(float frameX, float frameY)
    {
        if (item != null)
        {
            GUI.DrawTexture(new Rect(frameX + position.x, frameY + position.y, position.width, position.height), item.image);
        }
    }

    public Slot(Rect position)
    {
        this.position = position;
    }
}
