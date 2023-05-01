using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_shieldBarScr : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    scr_enemyBase enemyStats;
    [SerializeField]
    GameObject shieldBar;
    [SerializeField]
    float shieldMax, shieldCurr;
    [SerializeField]
    GameObject shieldBorder;

    [SerializeField]
    Sprite NoShieldBorderImage;

    bool shieldActive = true;
    void Start()
    {
        shieldMax = enemyStats.shieldVal;
    }

    // Update is called once per frame
    void Update()
    {

        shieldCurr = enemyStats.shieldVal;
        shieldMax = enemyStats.shieldMaxVal;
        shieldBar.transform.localScale = new Vector3(shieldCurr / shieldMax, 1f);
        if(shieldCurr <= 0 && !shieldActive)
        {
            disaleShieldBar();
        }
    }

    void disaleShieldBar()
    {
        if(NoShieldBorderImage != null)
        shieldBorder.GetComponent<SpriteRenderer>().sprite = NoShieldBorderImage;
    }
}
