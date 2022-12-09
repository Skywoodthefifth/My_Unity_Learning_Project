﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public abstract class Character : MonoBehaviour
{

    public float maxHitPoints;

    public float startingHitPoints;

    public int startingExperience;

    public int level;

    [HideInInspector]
    public int expLimit;

    public virtual void KillCharacter()
    {
        RPGGameManager.sharedInstance.DestroyObject(gameObject);
    }

    public abstract void ResetCharacter();
    public abstract IEnumerator DamageCharacter(int damage, float interval);

    public virtual IEnumerator FlickerCharacter()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = Color.white;

    }

}
