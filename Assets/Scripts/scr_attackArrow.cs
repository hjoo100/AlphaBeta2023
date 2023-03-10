using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_attackArrow : MonoBehaviour
{
    public string enemyTag;

    [SerializeField]
    public HashSet<GameObject> enemyInRange = new HashSet<GameObject>();

    public float KbackForce = 30;
    public float KbackUp = 0;
    public Scr_PlayerAudioCtrl playerAudio;
    // Start is called before the first frame update

    private void Awake()
    {
        enemyTag = "Enemy";
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
        print("enemy in range");
    }

    void OnTriggerExit2D(Collider2D c) 
    {
        if (c.isTrigger) return;
        if (c.GetType() == typeof(CircleCollider2D)) return;
        enemyInRange.Remove(c.gameObject);
        print("enemy out of range");
    }

    public bool IsInRange(GameObject target)
    {
        return target != null && enemyInRange.Contains(target);
    }

    public void attackEnemyInRange(float dmg)
    {
        foreach (var enemy in enemyInRange)
        {
            
               
                enemy.GetComponent<scr_enemyBase>().receiveDmg(dmg);
                Vector2 attackForce = new Vector2();
                attackForce.y = transform.localPosition.y;
                attackForce.x = 70f;

                Vector2 difference = enemy.transform.position - transform.position;
                difference = difference.normalized * 80f;
                difference.y = 0;
                difference *= 100f;
                if (enemy.GetComponent<scr_enemyBase>().enemyType == 1)
                {
                    enemy.GetComponent<scr_meleeEnemyMove>().tempFreeze();
                    enemy.GetComponent<scr_meleeEnemyMove>().knockBack();
                    print("enemy received dmg");
                    playerAudio.PlayAudio(1);

                }

                if (enemy.GetComponent<scr_enemyBase>().enemyType == 2)
                {

                }
          

        }
        if(enemyInRange.Count <=0)
        {//Melee Not hit
            print("melee not hit");
            playerAudio.PlayAudio(0);
        }
    }
}
