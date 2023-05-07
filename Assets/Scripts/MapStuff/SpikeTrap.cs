using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [SerializeField]
    private float spikeSpeed = 3f;
    [SerializeField]
    private float damage = 10f;
    [SerializeField]
    private float spikeDelay = 1f;
    [SerializeField]
    private float spikeHeight = 2f;

    private Vector3 initialPosition;
    private bool isActivated = false;
    private bool isEnabled = false;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        if (isEnabled)
        {
            if (isActivated)
            {
                transform.position = Vector3.MoveTowards(transform.position, initialPosition + Vector3.up * spikeHeight, spikeSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, initialPosition, spikeSpeed * Time.deltaTime);
            }
        }
    }

    public IEnumerator ActivateTrap()
    {
        isEnabled = true;

        if (!isActivated)
        {
            isActivated = true;
            yield return new WaitForSeconds(spikeDelay);
            isActivated = false;
            isEnabled = false; // Add this line
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player"))
        {
            other.collider.GetComponent<Scr_PlayerCtrl>().takeDmg(damage);
        }
    }
}