using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySlamSkill : Skill
{
    [SerializeField] private float jumpForce;
    [SerializeField] private float hoverDuration;
    [SerializeField] private float slamSpeed;
    [SerializeField] private float slamDamage;

    private Rigidbody2D enemyRigidbody;
    private Transform player;
    private int currentState;
    private float hoverTimer;

    protected EnemySlamSkill(string name, SkillEnum.SkillType skillType, int level) : base(name, skillType, level)
    {
        base.name = name;
        base.skillType = skillType;
        base.Level = level;
    }

    public bool IsSkillActive { get; private set; }

    public override void Initialize(string name, SkillEnum.SkillType skillType, int level)
    {
        base.Initialize(name, skillType, level);
    
        //enemyRigidbody = obj.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public override void ActivateSkill(GameObject obj)
    {
        base.ActivateSkill(obj);
        enemyRigidbody = obj.GetComponent<Rigidbody2D>();
        currentState = 0;
        enemyRigidbody.velocity = new Vector2(0, jumpForce);
    }

    public override void StartSkillCD(GameObject obj)
    {
        base.StartSkillCD(obj);
        currentState = -1;
    }


    public void FixedUpdateSkill(GameObject obj)
    {
        if (!IsSkillActive)
        {
            return;
        }

        switch (currentState)
        {
            case 0:
                if (enemyRigidbody.velocity.y <= 0)
                {
                    currentState = 1;
                    hoverTimer = 0;
                }
                break;

            case 1:
                enemyRigidbody.velocity = Vector2.zero;
                hoverTimer += Time.fixedDeltaTime;
                if (hoverTimer >= hoverDuration)
                {
                    currentState = 2;
                }
                break;
            case 2:
                Vector2 slamDirection = (player.position - obj.transform.position).normalized;
                enemyRigidbody.velocity = new Vector2(slamDirection.x, -slamSpeed);
                break;
        }
    }
    
    public void HandleCollision(GameObject obj, Collision2D collision)
    {
        if (currentState == 2 && collision.gameObject.CompareTag("Player"))
        {
            // Deal damage to the player
            // Replace this line with your own damage logic
            Debug.Log($"Player takes {slamDamage} damage from enemy slam.");
            StartSkillCD(obj);
        }

        if (currentState == 2 && collision.gameObject.CompareTag("Ground"))
        {
            StartSkillCD(obj);
        }
    }
}
