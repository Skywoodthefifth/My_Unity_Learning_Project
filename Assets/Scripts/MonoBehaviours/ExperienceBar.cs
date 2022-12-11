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

    float experience;

    void Update()
    {
        expToGain = player.expToGain;
        experience = player.experience; 
        if (player != null)
        {
            meterImage.fillAmount = player.experience / expToGain;
            print("exp to gain " + expToGain);  
            
        }
    }
}
