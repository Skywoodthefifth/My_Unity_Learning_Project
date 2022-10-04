﻿using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Enemy : Character
{
    float hitPoints;
    public int damageStrength;
    Coroutine damageCoroutine;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // if (hitPoints <= float.Epsilon)
        // {
        //     KillCharacter();
        // }
    }

    private void OnEnable()
    {
        ResetCharacter();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(player.DamageCharacter(damageStrength, 1.0f));
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }

    public override IEnumerator DamageCharacter(int damage, float
interval)
    {
        while (true)
        {
            StartCoroutine(FlickerCharacter());

            hitPoints = hitPoints - damage;
            if (hitPoints <= float.Epsilon)
            {
                KillCharacter();
                break;
            }
            if (interval > float.Epsilon)
            {
                yield return new WaitForSeconds(interval);
            }
            else
            {
                break;
            }

        }
    }

    public override void ResetCharacter()
    {
        hitPoints = startingHitPoints;
    }

    public GameObject dropPrefab;

    public override void KillCharacter()
    {

        if (PhotonNetwork.IsMasterClient)
        {
            GameObject drop = RPGGameManager.sharedInstance.InstantiateObject(dropPrefab, transform.position, Quaternion.identity);

            //drop.transform.position = transform.position;

            base.KillCharacter();
        }
    }
}
