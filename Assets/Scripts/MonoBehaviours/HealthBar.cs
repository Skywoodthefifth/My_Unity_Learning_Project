using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    //public HitPoints hitPoints;

    [HideInInspector]
    public Player character;

    public Image meterImage;

    public Text hpText;

    void Start()
    {
    }

    void Update()
    {
        if (character != null)
        {
            meterImage.fillAmount = character.hitPoints / character.maxHitPoints;
            hpText.text = "HP: " + Mathf.Round(meterImage.fillAmount * 100.0f);
        }
    }
}
