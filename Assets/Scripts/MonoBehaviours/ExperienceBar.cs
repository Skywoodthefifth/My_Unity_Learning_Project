using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceBar : MonoBehaviour
{

    [HideInInspector]
    public Player character;

    public Image meterImage;

    float expLimit;

    void Start()
    {
        expLimit = character.expLimit;
    }

    void Update()
    {
        if (character != null)
        {
            meterImage.fillAmount = character.experience / expLimit;
        }
    }
}
