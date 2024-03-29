using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Animations;
using UnityEngine.InputSystem.Controls;
using System;

public class Scr_PlayerCtrl : MonoBehaviour
{
    public GameObject PlayerObj;
    public KeyCode M_left, M_right, M_jump;


    private Rigidbody2D rb;
    private Transform playertransform;
    [SerializeField]
    private InputAction playerCtrlSystem;
    private float moveDir;
    private bool toRight = true;
    [SerializeField]
    public bool isJumping = false;

    private bool isInMeleeRange;
    public float jumpCount;

    public float PlayerSpd = 4f, basicSpd = 4f;
    public float jumpForce = 5f;
    private float originalJumpForce;
    public float reducedJumpForce = 3.2f;
    
    private Collider2D[] playercolliders;
    public Transform cellingCheck;
    public Transform groundCheck;
    public LayerMask GroundObj;
    public string EnemyTag;
    public float jumpCheckRaidus;
    public float MaxJumpNum;
    private float jumpTimeCounter;
    public float maxJumpTime = 2f;
    public bool isHittingWall = false;

    public float hitpoints = 100;
    public float maxHp = 100;
    public bool isDead = false;
    public GameObject attackArrow;
    public float meleeRange;
    public float CurrMeleeCD = 0;
    public float meleeCD = 0.2f;
    public float ComboEndMeleeCd = 0.6f;
    public float meleeDmg = 10f;

    [SerializeField]
    public float basicMeleeDmg = 40f;

    //for combo
    public bool attackKeyDown = false;
    //for disable and enabling attack
    private bool canAttack = true;
    [SerializeField]
    private StateMachine MeleeStatemachine;

    public bool isWalking = false, isAir = false, isAttacking = false, isHited = false, isAirAttacked = false;
    public bool isGrounded = false;
    //animation
    Animator playerAnimator;
    private string currentState;
    public bool gettingKnocked = false;
    public float MaxKnockTimeMelee = 0.25f;
    public float knockedTime = 0;
    //melee combo
    public int comboNo = 0;
    public bool isFirstMelee = false, isSecondMelee = false, isThirdMelee = false, isFourthMelee = false, isFifthMelee = false;
    public float meleeMaxInputTimer = 0.45f, meleeTimer = 0;
    public Image hpBar;

    //skill bool
    [SerializeField]
    private bool isMoonSlicing = false, isRapidHiting = false;

    [SerializeField]
    public bool isImmune = false;

    //Buff var
    [SerializeField]
    private float BuffMultiPlier = 1;
    private List<scr_PlayerDmgBuff> activeDamageBuffs = new List<scr_PlayerDmgBuff>();
    //debug var for buff
    [SerializeField]
    private float currentAddedBuff;
    scr_GManager Gamemanager;

    private Scr_PauseManager pauseManager;

    [SerializeField]
    private scr_SkillHolder[] skillHolders;

    [SerializeField]
    public bool isDefending = false;

    public ComboSystem comboSystem;
    public event Action OnSuccessfulAttack;
    public event Action<float, bool> OnComboDamage;

    public delegate void HealthChangedDelegate(float currentHealth, float maxHealth);
    public event HealthChangedDelegate OnHealthChanged;

    public bool cancelGroundCheck = false;

    public bool isUsingSkill = false;

    public bool isChoosingSkill = false;

    public bool playerIsOnEnemy = false;
    public PolygonCollider2D playerFeetCollider;

    public SpriteRenderer playerSprite;
    //edge collider stuff
    public int resolution = 20; // the number of points on the arc
    public float radius = 0.7f; // the radius of the arc
    public void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        attackArrow.GetComponent<scr_attackArrow>().enemyTag = EnemyTag;
        playertransform = gameObject.transform;
        playerAnimator = gameObject.GetComponent<Animator>();
        playerAnimator.Play("Idle");
        hitpoints = maxHp;
        Gamemanager = GameObject.FindGameObjectWithTag("GManager").GetComponent<scr_GManager>();
        hpBar = GameObject.FindGameObjectWithTag("UI.Hud.Hp").GetComponent<Image>();

        MeleeStatemachine = GetComponent<StateMachine>();

        comboSystem = GetComponent<ComboSystem>();

        playercolliders = GetComponents<Collider2D>();

        CircleCollider2D[] circleCollider2Ds = GetComponents<CircleCollider2D>();
        playerFeetCollider = GetComponent<PolygonCollider2D>();
       
    }
    // Start is called before the first frame update
    void Start()
    {
        pauseManager = FindObjectOfType<Scr_PauseManager>();
        jumpCount = MaxJumpNum;

        var skillUpgradeMenu = FindObjectOfType<SkillUpgradeMenu>();
        if (skillUpgradeMenu != null)
        {
            skillUpgradeMenu.OnMenuVisibilityChanged += HandleMenuVisibilityChanged;
        }
        originalJumpForce = jumpForce;

        CreateArcPolygonCollider();
    }

    private void HandleMenuVisibilityChanged(bool menuVisible)
    {
        canAttack = !menuVisible;
    }

    private void OnEnable()
    {
        playerCtrlSystem.Enable();
    }

    private void OnDisable()

    {
        playerCtrlSystem.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseManager.IsPaused())
        {
            return; // Do not execute the rest of the Update logic if the game is paused
        }
        playerIsOnEnemy = playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Enemy"));
        if (!gettingKnocked)
        {
            ReceiveInputFunc();
            // attack();
            //attackKeyFuncOld();

            if (isGrounded)
            {

                if (attackKeyDown && canAttack)
                {
                    if (CheckSkillActive())
                    {
                        //dont attack if using skill
                        return;
                    }


                    if (MeleeStatemachine.CurrentState == null)
                    {

                        Debug.Log("statemachine is gone again, ha");
                        //MeleeStatemachine.CurrentState = new Scr_IdleComboState();
                        return;

                    }
                    else
                    {
                        string statemachineName = MeleeStatemachine.CurrentState.GetType().ToString();
                        Debug.Log("In inspector, the name of current state is: " + statemachineName);
                    }
                    if (attackKeyDown && MeleeStatemachine.CurrentState.GetType() == typeof(Scr_IdleComboState))
                    {
                        MeleeStatemachine.SetNextState(new Scr_GroundEntryState());
                    }

                }
                else
                {
                    if (MeleeStatemachine.mainStateType == null)
                    {
                        Debug.Log("ha, main state type is missing!");
                        //MeleeStatemachine.mainStateType = new Scr_IdleComboState();
                        return;
                    }


                }
            }
            else
            {

                if (attackKeyDown && isAirAttacked == false && canAttack)
                {
                    if (CheckSkillActive())
                    {   //dont attack in air if using skill
                        return;
                    }
                    if (MeleeStatemachine.CurrentState == null)
                    {
                        Debug.Log("statemachine is gone again, ha");
                        // MeleeStatemachine.CurrentState = new Scr_IdleComboState();
                        return;
                    }
                    if (MeleeStatemachine.CurrentState.GetType() == typeof(Scr_IdleComboState))

                        MeleeStatemachine.SetNextState(new Scr_AirEntryState());
                }
            }

        }
        else
        {
            if (knockedTime > 0)
            {
                knockedTime -= Time.deltaTime;
                if (knockedTime <= 0)
                {
                    knockedTime = 0;
                    gettingKnocked = false;
                }
            }
        }

        bool isTouchingEnemy = false;
        foreach (Collider2D colliderOnPlayer in playercolliders)
        {
            if (colliderOnPlayer.IsTouchingLayers(LayerMask.NameToLayer("Enemy")))
            {
                isTouchingEnemy = true;
                break;
            }
        }

        if (isTouchingEnemy)
        {
            jumpForce = reducedJumpForce;
        }
        else
        {
            jumpForce = originalJumpForce;
        }




        if (isDead == true)
        {
            print("Player Died!");
            Gamemanager.LoseFunc();
            Destroy(gameObject);

        }

        hpbarUpdate();
    }
    private void LateUpdate()
    {
        releaseAttack();
    }

    //fixed update for movement
    private void FixedUpdate()
    {
        if (pauseManager.IsPaused())
        {
            return; // Do not execute the rest of the Update logic if the game is paused
        }
        //check for ground 
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, jumpCheckRaidus, GroundObj);
        if (isGrounded == true)
        {
            if (isAirAttacked)
            {
                resetAttack();
                isAirAttacked = false;


            }

            if (isJumping == true && !cancelGroundCheck)
            {
                isJumping = false;
            }
        }

        if (isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb.AddForce(new Vector2(0f, jumpForce));
                jumpTimeCounter -= Time.deltaTime; // Reduce the jump time counter
            }
            else
            {
                isJumping = false; // Stop the jump when jump time counter is depleted
            }
        }

        if (playerIsOnEnemy )
        {
            Debug.Log("Player is on enemy's head");
            //rb.AddForce(Vector2.down * 20f);
            if(moveDir >= 0)
            {
                rb.AddForce(Vector2.right * 130f);
            }
            
            else
            {
                rb.AddForce(Vector2.left * 130f);
            }
        }

        if (!gettingKnocked)
        {
            if (isGrounded)
            {
                jumpCount = MaxJumpNum;
            }
            if ((isHittingWall && !isGrounded))
            {
                //charaJumpFunc();
                //isJumping = false;
            }
            else
            {
                if (CheckSkillActive() == false)
                    charaMoveFunc();
            }


            if (CurrMeleeCD > 0)
            {
                CurrMeleeCD -= Time.deltaTime;
                if (CurrMeleeCD < 0)
                {
                    CurrMeleeCD = 0;
                }
            }
        }
        else
        { //getting knocked

        }

        if (comboNo >= 1)
        {
            meleeTimer += Time.fixedDeltaTime;
        }

        if (meleeTimer > meleeMaxInputTimer)
        {
            //reset all melee states
            comboNo = 0;
            isThirdMelee = false;
            isSecondMelee = false;
            isFirstMelee = false;
            isFifthMelee = false;
            isFourthMelee = false;
            meleeTimer = 0;
        }

        CheckForStuckInPlatform();
    }

    void ReceiveInputFunc()
    {


        //rb.velocity = new Vector2(PlayerSpd * moveDir, rb.velocity.y);
        if (moveDir > 0 && toRight == false)
        {
            flipChara();
        }
        else if (moveDir < 0 && toRight == true)
        {
            flipChara();
        }


    }
    public void MoveAxisFunc(InputAction.CallbackContext context)
    {

        if (CheckSkillActive() == true)
        {
            return;
        }
        moveDir = context.ReadValue<float>();
    }
    public void JumpFunc(InputAction.CallbackContext context)
    {
        if (pauseManager.IsPaused())
        {
            return;
        }

        if (context.started) // When space key is pressed
        {
            if (jumpCount > 0)
            {
                isJumping = true;
                charaJumpFunc();
                cancelGroundCheck = true;
                Invoke(nameof(resumeGroundCheck), 0.2f);
            }
        }

        if (context.canceled) // When space key is released
        {
            isJumping = false;
        }
    }

    public void attackKeyFunc(InputAction.CallbackContext context)
    {

        if (context.canceled)
        {
            if (!isImmune)
            {
                attackKeyDown = true;
            }

            // comboNo = 0;
        }
        else
        {
            attackKeyDown = false;

        }
    }

    public void releaseAttack()
    {
        attackKeyDown = false;
    }

    public void attackKeyFuncOld()
    {
        if (!attackKeyDown && Input.GetKeyDown(KeyCode.Z))
        {
            attackKeyDown = true;
        }

        if (Input.GetKeyUp(KeyCode.Z))
        {
            attackKeyDown = false;
        }
    }
    void charaMoveFunc()
    {
        rb.velocity = new Vector2(PlayerSpd * moveDir, rb.velocity.y);
        //charaJumpFunc();
        //isJumping = false;
        if (moveDir == 0 && isGrounded && !isAttacking && !isDefending && !isJumping)
        {
            animationSwitch("Idle");
        }
        else if (isGrounded && !isAttacking && !isDefending && !isJumping)
        {
            animationSwitch("Walk");
        }
    }

    void charaJumpFunc()
    {
        if (isJumping && jumpCount > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Use velocity instead of AddForce
            print("jumped");
            animationSwitch("Jump", true);

            jumpCount--;
            jumpTimeCounter = maxJumpTime;
        }
    }


    void flipChara()
    {
        toRight = !toRight;

        playertransform.localScale = new Vector2(-playertransform.localScale.x, playertransform.localScale.y);
    }

   



    //animation

    void animationSwitch(string animState)
    {
        if (currentState == animState)
        {
            return;
        }

        //play animation
        playerAnimator.Play(animState);
    }

    void animationSwitch(string animState, bool Replay)
    {
        if (Replay == true)
        {
            playerAnimator.Play(animState, -1, 0f);
        }
    }
    public void takeDmg(float dmg)
    {
        if (isImmune)
        {
            return;
        }

        if (isDefending)
        {
            Debug.Log("successfully blocked attack!");
            FindObjectOfType<Scr_PlayerAudioCtrl>().PlayAudio(5);
            GameObject.FindGameObjectWithTag("BlockAnimation").GetComponent<Animator>().Play("BlockAnim");
            return;
        }
        hitpoints -= dmg;
        OnHealthChanged?.Invoke(hitpoints, maxHp);
        FindObjectOfType<scr_camerafollow>().ShakeCamera();
        if (hitpoints < 0)
        {
            isDead = true;
            return;
        }
        if (CheckSkillActive())
        {
            return;
        }
        if (gettingKnocked == false)
        {

            gettingKnocked = true;
            knockedTime = MaxKnockTimeMelee;
        }
        else
        {
            knockedTime = MaxKnockTimeMelee;
        }
        animationSwitch("Knocked");
    }

    void resetAttack()
    {
        isAttacking = false;
        animationSwitch("Idle");


    }

    public void hpbarUpdate()
    {

        float hpPercent = hitpoints / maxHp;
        hpBar.fillAmount = hpPercent;
    }

    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            //stop movement
            isHittingWall = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            //resume movement
            isHittingWall = false;
        }
    }

    public void levelUP(int level)
    {
        meleeDmg += 3;

        maxHp += 10;
        if (hitpoints < maxHp)
        {
            hitpoints += 30;

            if (hitpoints > maxHp)
            {
                hitpoints = maxHp;
            }

            OnHealthChanged?.Invoke(hitpoints, maxHp);
        }



    }

    public void resetVelocity()
    {
        Vector2 zeroVel = Vector2.zero;
        rb.velocity = zeroVel;
        moveDir = 0;
    }

    public void applyAttack()
    {
        attackArrow.GetComponent<scr_attackArrow>().attackEnemyInRange(meleeDmg * BuffMultiPlier * CalculateBuffMultiplier());
    }

    public void applyAttackNoSound()
    {
        attackArrow.GetComponent<scr_attackArrow>().attackEnemyInRangeNoSound(meleeDmg * BuffMultiPlier * CalculateBuffMultiplier());
    }

    public void InvokeAttack(float time)
    {
        Invoke(nameof(applyAttack), time);
    }
    public void StartMoonSlicing(float damage)
    {
        isMoonSlicing = true;
        resetVelocity();
        animationSwitch("MoonSlicingSkill");
        meleeDmg = damage;
        Invoke(nameof(applyAttack), 0.35f);

    }

    public void EndMoonSlicingEntryPoint()
    {
        Invoke(nameof(EndMoonSlicing), 0.4f);
    }
    public void EndMoonSlicing()
    {
        isMoonSlicing = false;
        meleeDmg = basicMeleeDmg;
    }

    public IEnumerator RapidHitComboAttack(int comboCount, float comboDmg, float powerAttackDmg, float knockBackVal, bool isMaxLvl)
    {
        float timePerAttack = 1f / comboCount; // calculate time per attack
        Debug.Log("time between rapid hit: " + timePerAttack + " s");
        resetVelocity();
        for (int i = 0; i < comboCount; i++)
        {
            PoweredKnockHit(comboDmg, 0.08f); // execute Dmg function
            Debug.Log("rapid " + i + " hit");
            yield return new WaitForSeconds(timePerAttack); // wait for timePerAttack seconds
        }
        
    }
    public void StartRapidHitCombo()
    {
        isRapidHiting = true;
        animationSwitch("RapidHitSkill");

    }

    public void EndRapidHitCombo()
    {
        isRapidHiting = false;
        StopAllCoroutines();
    }
    public bool CheckSkillActive()
    {
        return (isMoonSlicing || isRapidHiting);
    }

    public void PoweredKnockHit(float dmg, float forceval)
    {
        attackArrow.GetComponent<scr_attackArrow>().attackEnemyInRangeWithForce(dmg, forceval);
    }

    public void TriggerImmune()
    {
        isImmune = true;
        ImmuneVisual();
    }

    public void StopImmune()
    {
        isImmune = false;
        RestoreVisual();
    }

    public void resumeGroundCheck()
    {
        cancelGroundCheck = false;
    }
    private float CalculateBuffMultiplier()
    {
        float multiplier = 1.0f;
        foreach (var buff in activeDamageBuffs)
        {
            multiplier *= buff.Multiplier;
        }

        currentAddedBuff = multiplier;
        return multiplier;

    }


    public void AddDamageBuff(float multiplier, float duration)
    {
        var buff = new scr_PlayerDmgBuff();
        buff.DamageBuff(multiplier, duration);
        activeDamageBuffs.Add(buff);
        StartCoroutine(RemoveDamageBuffAfterDuration(buff));
    }

    private IEnumerator RemoveDamageBuffAfterDuration(scr_PlayerDmgBuff buff)
    {
        yield return new WaitForSeconds(buff.Duration);
        activeDamageBuffs.Remove(buff);
    }

    public scr_SkillHolder[] getSkillHolders()
    {
        return skillHolders;
    }

    public void ResetVelocity()
    {
        rb.velocity = Vector2.zero;
    }

    public void SetDefend(bool defendBool)
    {
        isDefending = defendBool;
    }

    public bool CanGuard()
    {
        return isGrounded && !isAttacking;

    }

    public StateMachine getStateMachine()
    {
        return MeleeStatemachine;
    }

    public event Action EnemyDefeated;

    public void NotifyEnemyDefeated()
    {
        Debug.Log("OnEnemyDefeated called");
        EnemyDefeated?.Invoke();
    }

    public void SuccessfulAttack()
    {
        OnSuccessfulAttack?.Invoke();
        comboSystem.IncrementCombo();
    }

    public void ApplyComboDamage(float damageAmount, bool shouldPierceArmor)
    {
        OnComboDamage?.Invoke(damageAmount, shouldPierceArmor);
    }

    public void RestoreHp(float hpIncrement)
    {
        hitpoints += hpIncrement;
        if (hitpoints > maxHp)
        {
            hitpoints = maxHp;
        }
        OnHealthChanged?.Invoke(hitpoints, maxHp);
    }

    void CheckForStuckInPlatform()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 1f, 1 << LayerMask.NameToLayer("OneWayPlatform"));
        if (hit.collider != null)
        {
            
            transform.position = new Vector3(transform.position.x, hit.collider.bounds.max.y + 0.45f, transform.position.z);
        }
    }

    public void StartSkill()
    {
        isUsingSkill = true;
    }

    public void EndSkill()
    {
        isUsingSkill = false;

    }

    public void ImmuneVisual()
    {
        Color transparentColor = new Color(Color.white.r, Color.white.g, Color.white.b, 0.27f);
        playerSprite.color = transparentColor;
    }

    public void RestoreVisual()
    {
        playerSprite.color = Color.white;
    }

    public void CreateArcPolygonCollider()
    {


        float angleRad =62f * Mathf.Deg2Rad; // Convert angle to radians.
         // Number of points on the arc.

        Vector2[] points = new Vector2[resolution + 2];

        // First point is the origin (center of the circle)
        points[0] = Vector2.zero;

        // Calculate the radius
        float radius = GetComponent<BoxCollider2D>().size.x;

        // Calculate the rest of the points along the arc
        for (int i = 0; i <= resolution; i++)
        {
            float t = i / (float)resolution;
            float currentAngle = Mathf.Lerp(-angleRad / 2, angleRad / 2, t);
            Vector2 pointOnCircle = new Vector2(Mathf.Cos(currentAngle), Mathf.Sin(currentAngle)) * radius;
            // Rotate the point 90 degrees clockwise
            points[i + 1] = new Vector2(pointOnCircle.y, -pointOnCircle.x);
        }



        playerFeetCollider.points = points;
        playerFeetCollider.transform.position = new Vector2(GetComponent<BoxCollider2D>().transform.position.x, GetComponent<BoxCollider2D>().bounds.min.y);
    }
}

