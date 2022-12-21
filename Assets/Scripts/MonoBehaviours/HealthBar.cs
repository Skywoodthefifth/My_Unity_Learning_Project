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

    float maxHitPoints;

    void Start()
    {
        maxHitPoints = character.maxHitPoints;
        
    }

    void Update()
    {
      
        if (character != null)
        {
            maxHitPoints = character.maxHitPoints;
            meterImage.fillAmount = character.hitPoints / maxHitPoints;
            hpText.text = "HP: " + character.hitPoints;
          
        }
    }
}
