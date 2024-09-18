using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillSlots
{
    public SpecialAttack skill;
    public KeyCode key;
    public Rect skillPosition;
 
    public void ActivateSkill(KeyCode activationKey)
    {
        if(skill !=null)
        {
            skill.key = activationKey;
            key = activationKey;
        }
    }
}
