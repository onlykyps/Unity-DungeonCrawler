using UnityEngine;

public class Mob : MonoBehaviour
{
    public int ID;
    public static int assigner;
    public float speed;
    public float rangeSight;
    public float rangeAttack;

    public Transform playerPosition;
    public CharacterController controller;
    private Combat player;
    public LevelSystem playerLevel;

    public double impactTime = 0.36;
    private bool impacted;
    private bool removed;

    public AnimationClip run;
    public AnimationClip idle;
    public AnimationClip attack;
    public AnimationClip die;

    public double health;
    public int damage;
    public int maxHealth;

    private bool stunned;
    public int stunnedTime;

    void Start()
    {
        health = maxHealth;
        if (playerPosition != null)
        {
            player = playerPosition.GetComponent<Combat>();
        }
        AssisgnID();
        int databaseHealth = DataBase.ReadMobHealth(ID);
        if (databaseHealth == -1)
        {

        }
        else
        {
            health = databaseHealth;
        }
    }

    void AssisgnID()
    {
        this.ID = assigner;
        assigner++;
    }

    void Update()
    {
        //Debug.Log(InRange());
        if (!IsDead())
        {
            if (stunnedTime == 0)
            {
                if (InSight() && !InRangeAttack())
                { Chase(); }
                else if (InRangeAttack())
                {
                    GetComponent<Animation>().CrossFade("attack");
                    AttackPlayer();
                    if (GetComponent<Animation>()[attack.name].time > 0.9 * GetComponent<Animation>()[attack.name].length)
                    {
                        impacted = false;
                    }
                }
                else
                {
                    GetComponent<Animation>().CrossFade("idle");
                }
            }
            else
            {

            }
        }
        else
        {
            RemoveDeadOp();
        }

    }

    public void GetStunned(int seconds)
    {
        CancelInvoke("StunCountDown");
        stunnedTime = seconds;
        InvokeRepeating("StunCountDown", 0f, 1f);
    }

    void StunCountDown()
    {
        stunnedTime--;
        if(stunnedTime<=0)
        {
            CancelInvoke("StunCountDown");
        }
    }



    public void GetsHit(double damage)
    {
        health = health - damage;
        if (health < 0)
        {
            health = 0;
        }
        DataBase.SaveMobHealth(ID, health);
    }

    bool InSight()
    {
        //if (Vector3.Distance(transform.position, playerPosotion.position) < range)
        //{return true;}
        //return false;
        return playerPosition != null ? Vector3.Distance(transform.position, playerPosition.position) < rangeSight : false;
    }

    bool InRangeAttack()
    {
        return playerPosition != null ? Vector3.Distance(transform.position, playerPosition.position) < rangeAttack : false;
    }

    void Chase()
    {
        if (playerPosition != null)
        {
            transform.LookAt(playerPosition.position);
            controller.SimpleMove(transform.forward * speed);
            GetComponent<Animation>().CrossFade("run");
        }
    }

    private void OnMouseOver()
    {
        if (playerPosition != null)
        {
            playerPosition.GetComponent<Combat>().opponent = gameObject;
        }
    }

    bool IsDead()
    {
        return health == 0;
    }

    void RemoveDeadOp()
    {
        if (playerPosition.GetComponent<Combat>().opponent != null)
        {
            GetComponent<Animation>().Play(die.name);
            if (GetComponent<Animation>()[die.name].time > GetComponent<Animation>()[die.name].length * 0.95 && !removed)
            {
                playerLevel.exp = playerLevel.exp + 100;
                GetComponent<Animation>().CrossFade("die");
                Destroy(playerPosition.GetComponent<Combat>().opponent);
                removed = true;
                
            }
        }
    }
    //metoda de atac jucator
    void AttackPlayer()
    {
        if (GetComponent<Animation>().IsPlaying(attack.name) && !impacted)
        {
            //GetComponent<Animation>()[attack.name].time > GetComponent<Animation>()[attack.name].length * impactTime
            //
            if (GetComponent<Animation>()[attack.name].time > impactTime &&
                    GetComponent<Animation>()[attack.name].time < 0.9 * GetComponent<Animation>()[attack.name].length)
            {
                player.GetHit(damage);
                impacted = true;
            }
        }
    }
}
