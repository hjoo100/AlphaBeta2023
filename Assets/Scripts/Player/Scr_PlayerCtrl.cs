using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Animations;
using UnityEngine.InputSystem.Controls;

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
    private bool isJumping = false;
    
    private bool isInMeleeRange;
    public float jumpCount;

    public float PlayerSpd = 4f, basicSpd = 4f;
    public float jumpForce = 5f;
    public Transform cellingCheck;
    public Transform groundCheck;
    public LayerMask GroundObj;
    public string EnemyTag;
    public float jumpCheckRaidus;
    public float MaxJumpNum;
    public bool isHittingWall = false  ;

    public float hitpoints = 100;
    public float maxHp = 100;
    public bool isDead = false;
    public GameObject attackArrow;
    public float meleeRange;
    public float CurrMeleeCD = 0;
    public float meleeCD = 0.2f;
    public float ComboEndMeleeCd = 0.6f;
    public float meleeDmg = 10f;

    //for combo
    public bool attackKeyDown = false;

    [SerializeField]
    private StateMachine MeleeStatemachine;

    public bool isWalking = false, isAir = false, isAttacking = false, isHited = false,isAirAttacked = false;
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
    public float meleeMaxInputTimer = 0.45f,meleeTimer = 0;
    public Image hpBar;
    
    scr_GManager Gamemanager;
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
        
    }
    // Start is called before the first frame update
    void Start()
    {
        jumpCount = MaxJumpNum;
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
        if (!gettingKnocked)
        {
            ReceiveInputFunc();
            attack();
            //attackKeyFuncOld();
            /*
            if(isGrounded)
            {
              
               if (attackKeyDown )
                {
                    if(MeleeStatemachine.CurrentState == null)
                    {
                        
                        Debug.Log("statemachine is gone again, ha");
                        MeleeStatemachine.CurrentState = new Scr_IdleComboState();

                    }else
                    {
                        string statemachineName = MeleeStatemachine.CurrentState.GetType().ToString();
                        Debug.Log("In inspector, the name of current state is: " + statemachineName);
                    }
                    if(MeleeStatemachine.CurrentState.GetType() == typeof(Scr_IdleComboState))
                    MeleeStatemachine.SetNextState(new Scr_GroundEntryState());
                }else
                {
                    if(MeleeStatemachine.mainStateType == null)
                    {
                        Debug.Log("ha, main state type is missing!");
                        MeleeStatemachine.mainStateType = new Scr_IdleComboState();
                    }
                    
                   
                }
            }
            else
            {
                
                 if (attackKeyDown && isAirAttacked == false)
                {
                    if (MeleeStatemachine.CurrentState == null)
                    {
                        Debug.Log("statemachine is gone again, ha");
                        MeleeStatemachine.CurrentState = new Scr_IdleComboState();

                    }
                    if (MeleeStatemachine.CurrentState.GetType() == typeof(Scr_IdleComboState))
                    
                    MeleeStatemachine.SetNextState(new Scr_AirEntryState());
                }
            }*/
            
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

        if(isDead == true)
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
        //check for ground 
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, jumpCheckRaidus, GroundObj);
        if(isGrounded == true)
        {
            if(isAirAttacked)
            {
                resetAttack();
                isAirAttacked = false;
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
                charaJumpFunc();
                isJumping = false;
            }
            else
            {
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

        if(comboNo >= 1)
        {
            meleeTimer += Time.fixedDeltaTime;
        }
        
        if(meleeTimer > meleeMaxInputTimer)
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
    }

    void ReceiveInputFunc()
    {
        

        //rb.velocity = new Vector2(PlayerSpd * moveDir, rb.velocity.y);
        if(moveDir >0 && toRight == false)
        {
            flipChara();
        }
        else if(moveDir <0 && toRight == true)
        {
            flipChara();
        }

      
    }
    public void MoveAxisFunc(InputAction.CallbackContext context)
    {
        moveDir = context.ReadValue<float>();
    }
    public void JumpFunc(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if (jumpCount > 0)
            {
                isJumping = true;

            }
        }
        
    }

    public void attackKeyFunc(InputAction.CallbackContext context)
    {
       /* if(context.started && !attackKeyDown)
        {
           // attackKeyDown = true;
           // comboNo = 1;
            Debug.Log("Attack enabled by context started");
        }

        if(context.performed )
        {
            attackKeyDown = true;
            Debug.Log("Attack enabled by context performed");
        }*/
        if(context.canceled)
        {
            attackKeyDown = true;
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
        if(!attackKeyDown && Input.GetKeyDown(KeyCode.Z))
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
        charaJumpFunc();
        isJumping = false;
        if(moveDir == 0 && isGrounded && !isAttacking)
        {
            animationSwitch("Idle");
        }
        else if(isGrounded && !isAttacking)
        {
            animationSwitch("Walk");
        }
    }

    void charaJumpFunc()
    {
        if (isJumping && jumpCount > 0)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(0f, jumpForce));
            print("jumped");
            animationSwitch("Jump");
            jumpCount--;
        }
    }

    void flipChara()
    {
        toRight = !toRight;

        playertransform.localScale = new Vector2(-playertransform.localScale.x,playertransform.localScale.y);
    }

    void attack() //replaced by new system
    {
        //isInMeleeRange = Physics2D.OverlapCircle(attackArrow.position, meleeRange, EnemyLayer);
        if(Input.GetKeyDown(KeyCode.Mouse0) && CurrMeleeCD == 0)
        {
            print("Melee Pressed");
            //do melee attack
            //attackArrow.GetComponent<scr_attackArrow>().attackEnemyInRange(meleeDmg);
           // playerAnimator.Play("Attack");
            meleeComboFunc();
            isAttacking = true;
            if(isGrounded)
            {
                Invoke(nameof(resetAttack), 0.45f);
            }
            
            //CurrMeleeCD = meleeCD;
        }
    }



    //animation

    void animationSwitch(string animState)
    {
        if(currentState == animState)
        {
            return;
        }

        //play animation
        playerAnimator.Play(animState);
    }

    public void takeDmg(float dmg)
    {
        hitpoints -= dmg;
        if(hitpoints < 0)
        {
            isDead = true;
            return;
        }
        if(gettingKnocked == false)
        {
            
            gettingKnocked = true;
            knockedTime = MaxKnockTimeMelee;
        }else
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

    //Melee Combo func is replaced by a new combosystem
    public void meleeComboFunc()
    {
        if (isGrounded == false && isAirAttacked == false)
        {
            animationSwitch("AirMelee");
            attackArrow.GetComponent<scr_attackArrow>().attackEnemyInRange(meleeDmg);
            meleeTimer = 0;
            CurrMeleeCD = meleeCD;
            isAirAttacked = true;
            return;
        }else if(isGrounded == false && isAirAttacked == true)
        {
            return;
        }

        //call func when pressed melee key
        if (comboNo == 0)
        {
            //First melee anim
            animationSwitch("Melee1");
            attackArrow.GetComponent<scr_attackArrow>().attackEnemyInRange(meleeDmg);
            comboNo += 1;
            isFirstMelee = true;
            meleeTimer = 0;
            CurrMeleeCD = meleeCD;
            return;
        }
        else if(comboNo == 1 && meleeTimer < meleeMaxInputTimer)
        {
            if(isFirstMelee == true)
            {
                animationSwitch("Melee2");
                attackArrow.GetComponent<scr_attackArrow>().attackEnemyInRange(meleeDmg);
                comboNo += 1;
                isFirstMelee =false;
                isSecondMelee = true;
                meleeTimer = 0;
                CurrMeleeCD = meleeCD;
            }
            return;
        }else if(comboNo == 2 && meleeTimer <meleeMaxInputTimer)
        {
            if(isSecondMelee == true)
            {
                animationSwitch("Melee3");
                attackArrow.GetComponent<scr_attackArrow>().attackEnemyInRange(meleeDmg);
                comboNo += 1;
                isSecondMelee = false;
                isThirdMelee = true;
                meleeTimer = 0;
                CurrMeleeCD = meleeCD;
            }
        }else if(comboNo == 3 && meleeTimer <meleeMaxInputTimer)

        {

            if (isThirdMelee == true)
            {
                animationSwitch("Melee4");
                attackArrow.GetComponent<scr_attackArrow>().attackEnemyInRange(meleeDmg);
                comboNo += 1;
                isThirdMelee = false;
                isFourthMelee = true;
                meleeTimer = 0;
                CurrMeleeCD = meleeCD;
            }

        }else if(comboNo == 4 && meleeTimer < meleeMaxInputTimer)
        {
            if (isFourthMelee == true)
            {
                animationSwitch("Melee5");
                attackArrow.GetComponent<scr_attackArrow>().attackEnemyInRange(meleeDmg);
                comboNo += 1;
                isFourthMelee = false;
                isFifthMelee = true;
                meleeTimer = 0;
                CurrMeleeCD = ComboEndMeleeCd;
            }
        }

       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Obstacle")
        {
            //stop movement
            isHittingWall = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Obstacle")
        {
            //resume movement
            isHittingWall = false;
        }
    }

    public void levelUP(int level)
    {
        meleeDmg += 3;

        if(hitpoints < maxHp)
        {
            hitpoints += 30;
            if(hitpoints > maxHp)
            {
                hitpoints = maxHp;
            }
        }
        

        //improve skills
        
    }

    public void resetVelocity()
    {
        Vector2 zeroVel = Vector2.zero;
        rb.velocity = zeroVel;
    }

    public void applyAttack()
    {
        attackArrow.GetComponent<scr_attackArrow>().attackEnemyInRange(meleeDmg);
    }
}

