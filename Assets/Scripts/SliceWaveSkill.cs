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
   
    public override void ActivateSkill(GameObject player)
    {
        Scr_PlayerCtrl playerScr = player.GetComponent<Scr_PlayerCtrl>();
        AttackPos = player.transform.Find("Arrow");

        if(isShot == false)
        {
            swordWave.dmg = playerScr.meleeDmg + damage;
            //launch sword wave
            Instantiate(swordWave, AttackPos.position, Quaternion.identity);
            isShot = true;
        }
    }

    public override void StartSkillCD(GameObject player)
    {
        isShot = false;
        
    }

}
