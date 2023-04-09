using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MoonSliceSkill : Skill
{
    // Start is called before the first frame update
    [SerializeField]
    private float damage;
    [SerializeField]
    private bool sliceStarted = false;

    protected MoonSliceSkill(string name, SkillEnum.SkillType skillType, int level) : base(name, skillType, level)
    {
        base.name = name;
        base.skillType = skillType;
        Level = level;
    }

    public override void ActivateSkill(GameObject player)
    {

        if (sliceStarted == false)
        {
            sliceStarted = true;
            player.GetComponent<Scr_PlayerCtrl>().StartMoonSlicing(damage);
            Scr_PlayerAudioCtrl playeraudio = player.GetComponent<Scr_PlayerAudioCtrl>();
            playeraudio.PlayAudio(2);
        }
    }

    public override void StartSkillCD(GameObject player)
    {
        sliceStarted = false;

        player.GetComponent<Scr_PlayerCtrl>().EndMoonSlicingEntryPoint();
    }
}
