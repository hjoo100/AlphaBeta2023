using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerTrigger : MonoBehaviour
{
    bool spawnerEnabled = false;
    public scr_EnemySpawner Spawner;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") == false)
        {
            return;
        }
        if(!spawnerEnabled)
        {
            spawnerEnabled = true;
            Spawner.StartSpawning();
        }
    }
}
