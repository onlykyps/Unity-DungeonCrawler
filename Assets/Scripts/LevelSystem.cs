using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    //we need 100 exp to ding up every level

    public int level;
    public float exp;

    public Combat player;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        LevelUP();
    }

    void LevelUP()
    {
        //100 * Level
        if (exp >= (Mathf.Pow(level, 2) + 100) )
        {
            exp = exp - (Mathf.Pow(level, 2) + 100);
            level++;

            LevelEffect();
        }
    }

    void LevelEffect()
    {
        player.maxHealth = player.maxHealth + 100;
        player.damage = player.damage + 50;
    }
}
