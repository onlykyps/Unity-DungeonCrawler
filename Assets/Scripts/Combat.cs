using UnityEngine;

public class Combat : MonoBehaviour
{
    public GameObject opponent;
    public AnimationClip attack;
    public AnimationClip die;

    public int damage;
    public int health;
    public int maxHealth;

    public double impactLength;
    public double impactTime;
    public bool impacted;

    public float enemyCloseEnoughRange;
    public bool started, ended;

    public float combatEscapeTime;
    public float countDown;

    public bool specialAttack;
    public bool inAction;

    void Start()
    {
        impactLength = GetComponent<Animation>()[attack.name].length * impactTime;
        health = maxHealth;
    }

    void Update()
    {
        if (!IsDead())
        {
            if (Input.GetKeyDown(KeyCode.Space) && !specialAttack)
            {
                inAction = true;
            }
            if (inAction)
            {
                if (PlayerAttackMethod(0, 1, KeyCode.Space, null, 0, true))
                {

                }
                else
                {
                    inAction = false;
                }
            }


        }
        else if (IsDead() && !ended)
        {
            PayerDieMethod();
        }
    }

    public void ResetAttackMethod()
    {
        ClickToMove.attack = false;
        impacted = false;
        GetComponent<Animation>().Stop(attack.name);
    }

    public void PayerDieMethod()
    {
        if (!started)
        {
            ClickToMove.die = true;
            started = true;
        }

        if (started && !GetComponent<Animation>().IsPlaying(die.name))
        {
            //from here we can do whatever we want to do when you are dead
            Debug.Log("You have died");
            RemovePlayer();
            ended = true;
        }
    }

    public bool PlayerAttackMethod(int stunSeconds, double scaledDamage, KeyCode keyCode, GameObject particleFX, int projectile, bool opponentBase)
    {
        if (opponentBase)
        {
            if (Input.GetKeyDown(keyCode) && EnemyInRange())
            {
                GetComponent<Animation>().CrossFade("attack");
                ClickToMove.attack = true;
                if (opponent != null)
                {
                    transform.LookAt(opponent.transform.position);
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(keyCode))
            {
                GetComponent<Animation>().CrossFade("attack");
                ClickToMove.attack = true;
                transform.LookAt(ClickToMove.cursorPosition);
            }
        }
        if (GetComponent<Animation>()[attack.name].time > 0.9 * GetComponent<Animation>()[attack.name].length)
        {
            ClickToMove.attack = false;
            impacted = false;
            if (specialAttack)
            {
                specialAttack = false;
            }
            return false;
        }

        Impact(stunSeconds, scaledDamage, particleFX, projectile, opponentBase);
        return true;
    }

    void StunMob()
    {

    }

    void Impact(int stunSeconds, double scaledDamage, GameObject particleFX, int projectile, bool opponentBase)
    {
        if ((!opponentBase || opponent != null) && GetComponent<Animation>().IsPlaying(attack.name) && !impacted)
        {
            if (GetComponent<Animation>()[attack.name].time > impactTime &&
                GetComponent<Animation>()[attack.name].time < 0.9 * GetComponent<Animation>()[attack.name].length)
            {
                countDown = combatEscapeTime + 2;
                CancelInvoke("CombatEscapeCountdown");
                InvokeRepeating("CombatEscapeCountdown", 0, 1);
                if (opponentBase)
                {
                    opponent.GetComponent<Mob>().GetsHit(damage * scaledDamage);
                    opponent.GetComponent<Mob>().GetStunned(stunSeconds);
                }
                Quaternion rot = transform.rotation;
                rot.x = 0;
                rot.z = 0f;
                if (projectile > 0)
                {
                    Instantiate(
                        Resources.Load("Projectile"),
                        new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z),
                        rot
                        );
                }
                if (particleFX != null)
                {
                    Instantiate(
                            particleFX,
                            new Vector3(opponent.transform.position.x, opponent.transform.position.y + 2.5f, opponent.transform.position.z),
                            Quaternion.identity
                            );
                }
                impacted = true;
            }
        }

    }

    void CombatEscapeCountdown()
    {
        countDown--;
        if (countDown == 0)
        {
            CancelInvoke("CombatEscapeCountdown");
        }
    }

    bool EnemyInRange()
    {
        return opponent != null ? Vector3.Distance(opponent.transform.position, transform.position) <= enemyCloseEnoughRange : false;
    }

    public void GetHit(int damage)
    {
        health = health - damage;
        if (health < 0)
        {
            health = 0;
        }
        //        Debug.Log(health);//
    }

    bool IsDead()
    {

        return health == 0;
    }

    void RemovePlayer()
    {
        //GetComponent<Animation>().Play(die.name);
        GetComponent<Animation>().CrossFade("die");
        //if (GetComponent<Animation>()[die.name].time > GetComponent<Animation>()[die.name].length * 0.95)
        //{
        //    
        //    //Destroy(this.gameObject);
        //}

    }
}
