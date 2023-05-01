using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_swordWaveObj : MonoBehaviour
{
    public float dmg;
    public float waveSpd;
    public float lifeTime, maxLife;
    public GameObject player;
    public GameObject launcherObj;
    private Rigidbody2D rb;

    public int level = 0;

    Scr_PauseManager pauseManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        launcherObj = player.transform.Find("Arrow").gameObject;

        pauseManager = FindObjectOfType<Scr_PauseManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        Vector3 direction = launcherObj.transform.position - player.transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * waveSpd;

        float rot = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot);
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseManager.IsPaused())
        {
            return; // Do not execute the rest of the Update logic if the game is paused
        }

        lifeTime = Time.deltaTime;
        if (lifeTime > maxLife)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            //do impact scr here
            if(level == 2)
            {
                collision.gameObject.GetComponent<scr_enemyBase>().receiveDmg(dmg,true);
            }else
            {
                collision.gameObject.GetComponent<scr_enemyBase>().receiveDmg(dmg);
            }
            
            if(collision.gameObject.GetComponent<scr_MeleeEnemy>() != null)
            {
                collision.gameObject.GetComponent<scr_meleeEnemyMove>().knockBack();
            }

            if(collision.gameObject.GetComponent<scr_shieldEnemy>()!= null)
            {
                if(level==2)
                {
                    //shield percing
                    collision.gameObject.GetComponent<scr_shieldEnemyMove>().PoweredKnockBack(2f);
                }
                else
                {
                    //detect shield up or not
                    if(collision.gameObject.GetComponent<scr_enemyBase>().shieldVal <= 0)
                    {
                        collision.gameObject.GetComponent<scr_shieldEnemyMove>().PoweredKnockBack(2f);
                    }
                }
            }
            
            Destroy(gameObject);
        }
        else if(collision.gameObject.tag == "Obstacle")
        {
            Destroy(gameObject);
        }
    }
}
