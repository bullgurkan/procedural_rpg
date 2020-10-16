using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarManager : MonoBehaviour
{
    public float maxHealthPerTick = 5;
    public RectTransform barFill;
    public Text healthNumber;
    float baseSize;

    RectTransform rectTrans;

    void Start()
    {
        rectTrans = GetComponent<RectTransform>();
        baseSize = rectTrans.sizeDelta.x;
        SetHealth(20, 5);
    }

    public void SetHealth(int maxHealth, int health)
    {

        barFill.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, baseSize * health/ maxHealth);
        healthNumber.text = $"{health}/{maxHealth}";

        //barFill.localPosition =  health / maxHealthPerUnit * Vector2.right;
        //barFill.localScale = new Vector3(health /maxHealthPerUnit, barFill.localScale.y, barFill.localScale.z);


    }


}
