using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Scr_PlayerCtrl : MonoBehaviour
{
    public GameObject PlayerObj;
    public KeyCode M_left, M_right, M_jump;
    

    private Rigidbody2D rb;
    private Transform playertransform;
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

    public bool isWalking = false, isAir = false, isAttacking = false, isHited = false;
    public bool isGrounded = false;
    //animation
    Animator playerAnimator;
    private string currentState;
    public bool gettingKnocked = false;
    public float MaxKnockTimeMelee = 0.25f;
    public float knockedTime = 0;
    //melee combo
    public int comboNo = 0;
    public bool isFirstMelee = false, isSecondMelee = false, isThirdMelee = false;
    public float meleeMaxInputTimer = 0.3f,meleeTimer = 0;
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
    }
    // Start is called before the first frame update
    void Start()
    {
        jumpCount = MaxJumpNum;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gettingKnocked)
        {
            ReceiveInputFunc();
            attack();
        }else
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

    //fixed update for movement
    private void FixedUpdate()
    {
        //check for ground 
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, jumpCheckRaidus, GroundObj);
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
            meleeTimer = 0;
        }
    }

    void ReceiveInputFunc()
    {
        moveDir = Input.GetAxis("Horizontal");

        //rb.velocity = new Vector2(PlayerSpd * moveDir, rb.velocity.y);
        if(moveDir >0 && toRight == false)
        {
            flipChara();
        }
        else if(moveDir <0 && toRight == true)
        {
            flipChara();
        }

        if(Input.GetButtonDown("Jump") && jumpCount >0)
        {
            isJumping = true;

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

    void attack()
    {
        //isInMeleeRange = Physics2D.OverlapCircle(attackArrow.position, meleeRange, EnemyLayer);
        if(Input.GetKeyDown(KeyCode.Z) && CurrMeleeCD == 0)
        {
            print("Melee Pressed");
            //do melee attack
            attackArrow.GetComponent<scr_attackArrow>().attackEnemyInRange(meleeDmg);
           // playerAnimator.Play("Attack");
            meleeComboFunc();
            isAttacking = true;
            Invoke(nameof(resetAttack), 0.12f);
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

    }

    public void hpbarUpdate()
    {
       
        float hpPercent = hitpoints / maxHp;
        hpBar.fillAmount = hpPercent;
    }

    public void meleeComboFunc()
    {
        //call func when pressed melee key
        if(comboNo == 0)
        {
            //First melee anim
            animationSwitch("Melee1");
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
                comboNo += 1;
                isSecondMelee = false;
                isThirdMelee = true;
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
}

