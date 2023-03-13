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

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        launcherObj = player.transform.Find("Arrow").gameObject;
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
            collision.gameObject.GetComponent<scr_enemyBase>().receiveDmg(dmg);
            if(collision.gameObject.GetComponent<scr_MeleeEnemy>() != null)
            {
                collision.gameObject.GetComponent<scr_meleeEnemyMove>().knockBack();
            }
            
            Destroy(gameObject);
        }
        else if(collision.gameObject.tag == "Obstacle")
        {
            Destroy(gameObject);
        }
    }
}
