using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPreset : MonoBehaviour
{
    public static PlayerPreset Instance { get; private set; }

    [SerializeField]
    public List<Skill> PresetSkills { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetPresetSkills(List<Skill> skills)
    {
        PresetSkills = skills;
    }
}
