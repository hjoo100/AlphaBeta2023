using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrigger : MonoBehaviour
{
    [SerializeField]
    private SpikeTrap spikeTrap;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            spikeTrap.gameObject.SetActive(true);
            StartCoroutine(spikeTrap.ActivateTrap(other)); 
        }
    }
}
