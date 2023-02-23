using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_attackArrow : MonoBehaviour
{
    public string enemyTag;
    public HashSet<GameObject> enemyInRange = new HashSet<GameObject>();
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

        enemyInRange.Add(c.gameObject);
        print("enemy in range");
    }

    void OnTriggerExit2D(Collider2D c) 
    {
        enemyInRange.Remove(c.gameObject);
        print("enemy out of range");
    }

    public bool IsInRange(GameObject target)
    {
        return target != null && enemyInRange.Contains(target);
    }

    public void attackEnemyInRange(float dmg)
    {
        foreach (GameObject enemy in enemyInRange)
        {
            enemy.GetComponent<scr_enemy>().receiveDmg(dmg);
            print("enemy received dmg");
        }    
    }
}
