using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_enemyAttackArrow : MonoBehaviour
{
    public string enemyTag;
    public HashSet<GameObject> enemyInRange = new HashSet<GameObject>();
    // Start is called before the first frame update
    private Scr_PauseManager pauseManager;
    private void Awake()
    {
        enemyTag = "Player";

        pauseManager = FindObjectOfType<Scr_PauseManager>();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (pauseManager.IsPaused())
        {
            return; // Do not execute the rest of the Update logic if the game is paused
        }
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (!c.gameObject.CompareTag(this.enemyTag)) return;
        if (c.isTrigger) return;

        enemyInRange.Add(c.gameObject);
        print("Player in range");
    }

    void OnTriggerExit2D(Collider2D c)
    {
        if (!c.gameObject.CompareTag(this.enemyTag)) return;
        if (c.isTrigger && c.tag!="Player") return;

        enemyInRange.Remove(c.gameObject);
        print("Player out of range");
    }

    public bool IsInRange(GameObject target)
    {
        return target != null && enemyInRange.Contains(target);
    }

    public void attackEnemyInRange(float dmg)
    {
        foreach (GameObject enemy in enemyInRange)
        {
            if(enemy != null)

            {
                if(enemy.GetComponent<BoxCollider2D>().isTrigger)
                {
                    return;
                }
                enemy.GetComponent<Scr_PlayerCtrl>().takeDmg(dmg);
                print("player received dmg");
            }
            
        }
    }

    public void enableStompCollider()
    {
        CapsuleCollider2D stompCollider = GetComponent<CapsuleCollider2D>();
        stompCollider.enabled = true;
    }

    public void disableStompCollider()
    {

        CapsuleCollider2D stompCollider = GetComponent<CapsuleCollider2D>();
        if(GameObject.FindGameObjectWithTag("Player")!= null)
        enemyInRange.Remove(GameObject.FindGameObjectWithTag("Player"));
        stompCollider.enabled = false;
    }

    public void enableHitGroundCollider()
    {
        BoxCollider2D hitGroundCollider = GetComponent<BoxCollider2D>();
        hitGroundCollider.enabled = true;
    }

    public void disableHitGroundCollider()
    {
        BoxCollider2D hitGroundCollider = GetComponent<BoxCollider2D>();
        if (GameObject.FindGameObjectWithTag("Player") != null)
            enemyInRange.Remove(GameObject.FindGameObjectWithTag("Player"));
        hitGroundCollider.enabled = false;
    }
}
