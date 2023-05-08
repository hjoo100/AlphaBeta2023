using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class EnemySlamSkill : Skill
{
    [SerializeField] private float jumpForce;
    [SerializeField] private float hoverDuration;
    [SerializeField] private float slamSpeed;
    [SerializeField] private float slamDamage;

    public float knockbackRadius = 5f; 
    public float knockbackForce = 12f;

    private Rigidbody2D enemyRigidbody;
    private Transform player;
    private int currentState;
    private float hoverTimer;
    private Vector2 targetPosition;

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

        IsSkillActive = true;

        obj.GetComponent<scr_ShieldBossEnemy>().lastSlamTime = Time.time;
        obj.GetComponent<scr_ShieldBossEnemy>().PlayJumpSlashAnimation();
    }

    public override void StartSkillCD(GameObject obj)
    {
        base.StartSkillCD(obj);
        currentState = -1;

        IsSkillActive = false;
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
                    // Record the player's position when starting to fall
                    targetPosition = new Vector2(player.position.x, obj.transform.position.y);
                    // Move the enemy horizontally to the recorded target position
                    obj.transform.position = new Vector3(targetPosition.x, obj.transform.position.y, obj.transform.position.z);
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
                // Enemy falls straight down
                enemyRigidbody.velocity = new Vector2(0, -slamSpeed);
                //obj.GetComponent<scr_ShieldBossEnemy>().invokeCheckLandingZone(0.25f);
                break;
        }
    }
    
    public void HandleCollision(GameObject obj, Collision2D collision)
    {
        if (currentState == 2 && collision.gameObject.CompareTag("Player"))
        {
            // Deal damage to the player
            player.GetComponent<Scr_PlayerCtrl>().takeDmg(slamDamage);
            Debug.Log($"Player takes {slamDamage} damage from enemy slam.");
            // Move the enemy slightly above the player to avoid being stuck
            Vector3 newPosition = obj.transform.position;
            newPosition.y = collision.transform.position.y + collision.collider.bounds.extents.y + obj.GetComponent<Collider2D>().bounds.extents.y + 0.1f;
            obj.transform.position = newPosition;

            ApplyKnockbackToNearbyPlayer(obj);
            StartSkillCD(obj);
        }

        if (currentState == 2 && collision.gameObject.CompareTag("Obstacle"))
        {
            ApplyKnockbackToNearbyPlayer(obj);
            StartSkillCD(obj);
        }
    }

    private void ApplyKnockbackToNearbyPlayer(GameObject user)
    {
        // Find the player near the location
        Collider2D playerInRange = Physics2D.OverlapCircle(user.transform.position, knockbackRadius, LayerMask.GetMask("Player"));

        if (playerInRange != null)
        {
            Rigidbody2D playerRigidbody = playerInRange.GetComponent<Rigidbody2D>();
            if (playerRigidbody != null)
            {
                // Deal damage to the player
                playerInRange.GetComponent<Scr_PlayerCtrl>().takeDmg(slamDamage);
                Debug.Log($"Player takes {slamDamage} damage from enemy slam.");

                // Calculate the horizontal knockback direction based on the player's position relative to the enemy
                float horizontalDirection = playerRigidbody.position.x > user.transform.position.x ? 1 : -1;
                Vector2 knockbackDirection = new Vector2(horizontalDirection, 0);

                float collisionDisableDuration = 0.4f;
                user.GetComponent<scr_ShieldBossEnemy>().StartDisableCollisionCoroutine(collisionDisableDuration);

                // Apply the force
                playerRigidbody.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            }
        }

    }
}
