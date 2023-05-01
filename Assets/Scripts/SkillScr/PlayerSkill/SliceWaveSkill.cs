using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class SliceWaveSkill : Skill
{
    public float damage;
    public bool isShot = false;
    public Transform AttackPos;
    public scr_swordWaveObj swordWave;

    protected SliceWaveSkill(string name, SkillEnum.SkillType skillType, int level) : base(name, skillType, level)
    {
        base.name = name;
        base.skillType = skillType;
        Level = level;
    }

    public override void ActivateSkill(GameObject player)
    {
        Scr_PlayerCtrl playerScr = player.GetComponent<Scr_PlayerCtrl>();
        AttackPos = player.transform.Find("Arrow");

        if(isShot == false)
        {
            swordWave.dmg = playerScr.meleeDmg + damage;
            //launch sword wave
            swordWave.level = Level;
            Instantiate(swordWave, AttackPos.position, Quaternion.identity);
            isShot = true;
            Scr_PlayerAudioCtrl playeraudio = player.GetComponent<Scr_PlayerAudioCtrl>();
            playeraudio.PlayAudio(2);
        }
    }

    public override void StartSkillCD(GameObject player)
    {
        isShot = false;
        
    }

}
