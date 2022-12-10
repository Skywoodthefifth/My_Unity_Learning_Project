using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceBar : MonoBehaviour
{

    [HideInInspector]
    public Player player;

    public Image meterImage;

    float expToGain;

    void Start()
    {
        expToGain = player.expToGain;
    }

    void Update()
    {
        if (player != null)
        {
           
           
            meterImage.fillAmount = player.experience / expToGain;
            
        }
    }
}
