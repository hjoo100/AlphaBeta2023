using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_enemyAttackArrow : MonoBehaviour
{
    public string enemyTag;
    public HashSet<GameObject> enemyInRange = new HashSet<GameObject>();
    // Start is called before the first frame update

    private void Awake()
    {
        enemyTag = "Player";
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
        if (c.isTrigger) return;

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
                enemy.GetComponent<Scr_PlayerCtrl>().takeDmg(dmg);
                print("player received dmg");
            }
            
        }
    }
}
