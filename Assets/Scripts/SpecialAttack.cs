using UnityEngine;

public class SpecialAttack : MonoBehaviour
{
    public double damagePercentage;
    public int stunTime;
    public KeyCode key;
    public Combat player;
    public bool inAction;
    public GameObject particleFX;
    public int projectile;
    public bool opponentBased;
    public Texture2D skillPicture;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(key) && !player.specialAttack)
        {
            player.ResetAttackMethod();
            player.specialAttack = true;
            inAction = true;
        }

        if (inAction)
        {
            if(player.PlayerAttackMethod(stunTime, damagePercentage, key, particleFX, projectile, opponentBased))
            {
                
            }
            else
            {
                inAction = false;
            }
        }
    }
}
