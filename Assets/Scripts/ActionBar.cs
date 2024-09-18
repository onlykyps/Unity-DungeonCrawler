using UnityEngine;

public class ActionBar : MonoBehaviour
{
    public Texture2D actionBar;
    public Rect position;
    public SkillSlots[] skill;

    public float skill_X;
    public float skill_Y;
    public float skillWidth;
    public float skillHeight;
    public float skillDistance;

    private int keyBindSlot = -1;

    void Start()
    {
        Initialize();
    }

    void Update()
    {
        UpdateSkillSlot();
    }

    void SetKeyBind()
    {
        for (int i = 0; i < skill.Length; i++)
        {
            if (Input.GetMouseButtonDown(0) &&
                 Event.current.isMouse &&
                 GetScreenRect(skill[i].skillPosition).Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y)))
            {
                if (keyBindSlot == -1)
                {
                    keyBindSlot = i;
                }
                else
                {
                    keyBindSlot = -1;
                }
            }
        }

        if (keyBindSlot != -1 && Event.current.isKey)
        {
            skill[keyBindSlot].ActivateSkill(Event.current.keyCode);
            keyBindSlot = -1;
        }
    }

    private void UpdateSkillSlot()
    {
        for (int i = 0; i < skill.Length; i++)
        {
            skill[i].skillPosition.Set(skill_X + i * (skillWidth + skillDistance),
                                        skill_Y,
                                        skillWidth,
                                        skillHeight);
        }
    }

    private void Initialize()
    {
        SpecialAttack[] attacks = GameObject.FindGameObjectWithTag("Player").GetComponents<SpecialAttack>();
        skill = new SkillSlots[attacks.Length];
        for (int i = 0; i < attacks.Length; i++)
        {
            skill[i] = new SkillSlots();
            skill[i].skill = attacks[i];
        }

        skill[0].ActivateSkill(KeyCode.Q);
        skill[1].ActivateSkill(KeyCode.W);
        skill[2].ActivateSkill(KeyCode.E);
        skill[3].ActivateSkill(KeyCode.R);
    }

    void OnGUI()
    {
        DrawActionBar();
        DrawSkillSlots();
        SetKeyBind();
    }

    void DrawActionBar()
    {
        GUI.DrawTexture(GetScreenRect(position), actionBar);
    }

    void DrawSkillSlots()
    {
        for (int i = 0; i < skill.Length; i++)
        {
            GUI.DrawTexture(GetScreenRect(skill[i].skillPosition), skill[i].skill.skillPicture);
        }
    }

    Rect GetScreenRect(Rect position)
    {
        return new Rect(Screen.width * position.x,
                        Screen.height * position.y,
                        Screen.width * position.width,
                        Screen.height * position.height);
    }
}
