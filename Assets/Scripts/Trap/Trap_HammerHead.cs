using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_HammerHead : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float timer = 0;
    //[SerializeField] private float MaxWaitTimer = 0.4f;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.CompareTag("Player") )
        {
            if (timer > 0.4f)
            {
                if (collision.gameObject.GetComponent<Scr_PlayerCtrl>() != null)
                {
                    collision.gameObject.GetComponent<Scr_PlayerCtrl>().takeDmg(damage);
                    Debug.Log($"Player takes {damage} damage from hammer end.");
                    timer = 0;
                }

            }

        }
    }
   

    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
    }
}
