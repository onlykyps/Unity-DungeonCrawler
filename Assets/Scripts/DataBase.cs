using UnityEngine;

public class DataBase : MonoBehaviour
{
    //in terms of frame
    int interval = 120;
    int count;
    void Start()
    {
    }

    void Update()
    {
        if (count == interval)
        {
            SavePosition();
            count = 0;
        }
        count++;
    }

    void SavePosition()
    {
        PlayerPrefs.SetFloat("x", ClickToMove.currentPosition.x);
        PlayerPrefs.SetFloat("y", ClickToMove.currentPosition.y);
        PlayerPrefs.SetFloat("z", ClickToMove.currentPosition.z);
    }

    public static Vector3 ReadPlayerPosition()
    {
        Vector3 position = new Vector3();
        position.x = PlayerPrefs.GetFloat("x");
        position.y = PlayerPrefs.GetFloat("y");
        position.z = PlayerPrefs.GetFloat("z");
        return position;
    }

    public static void SaveMobHealth(int id, double health)
    {
        PlayerPrefs.SetInt("MobHealth" + id, (int)health);
    }

    public static int ReadMobHealth(int id)
    {
        if (PlayerPrefs.HasKey("MobHealth" + id))
        {
            return PlayerPrefs.GetInt("MobHealth" + id);
        }
        else
        { return -1; }
    }
}
