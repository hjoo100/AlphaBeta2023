using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class SkillToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Transform tooltipPrefab;
    private GameObject tooltipInstance;
    public string skillDescription;
    private bool isTooltipActive = false;

    private void Start()
    {
        
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltipInstance = Instantiate(tooltipPrefab, transform.position, Quaternion.identity, transform).gameObject;
        tooltipInstance.GetComponentInChildren<TextMeshProUGUI>().text = skillDescription;

        // change the position of tool tip
        tooltipInstance.transform.position += new Vector3(50, 25, 0);

        if (!isTooltipActive)
        {
            isTooltipActive = true;
            tooltipInstance.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isTooltipActive)
        {
            isTooltipActive = false;
            tooltipInstance.SetActive(false);
        }

        if (tooltipInstance != null)
        {
            Destroy(tooltipInstance);
        }
    }
}
