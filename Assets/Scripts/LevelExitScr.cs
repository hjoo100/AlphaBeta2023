using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExitScr : MonoBehaviour
{
    public scr_GManager GameManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameObject.FindGameObjectWithTag("Enemy") == null)
        {
            switchToClearColor();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player") return;
        GameManager.proceedToNextLevel();
    }

    void switchToClearColor()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.green;
    }
}
