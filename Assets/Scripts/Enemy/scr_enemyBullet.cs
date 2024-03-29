using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class scr_enemyBullet : MonoBehaviour
{
    public float dmg;
    public float bulletSpd;
    public float lifeTime,maxLife;
    public GameObject player;
    private Rigidbody2D rb;

    

    Scr_PauseManager pauseManager;

   
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        pauseManager = FindObjectOfType<Scr_PauseManager>();

        
    }
    


    // Update is called once per frame
    void Update()
    {
        if (pauseManager.IsPaused())
        {
            return; // Do not execute the rest of the Update logic if the game is paused
        }

        lifeTime = Time.deltaTime;
        if(lifeTime > maxLife)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //do impact scr here

            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //do impact scr here
            player.GetComponent<Scr_PlayerCtrl>().takeDmg(dmg);
            Destroy(gameObject);
        }
    }
}
