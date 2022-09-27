﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public HitPoints hitPoints;
    public HealthBar healthBarPrefab;

    HealthBar healthBar;

    public Inventory inventoryPrefab;

    Inventory inventory;


    void Start()
    {
        ResetCharacter();
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("CanBePickedUp"))
        {
            Item hitObject = collision.gameObject.GetComponent<Consumable>().item;

            if (hitObject != null)
            {
                //print("Hit: " + hitObject.objectName);

                bool shouldDisappear = false;

                switch (hitObject.itemType)
                {
                    case Item.ItemType.COIN:

                        shouldDisappear = inventory.AddItem(hitObject);

                        break;

                    case Item.ItemType.HEALTH:


                        shouldDisappear = AdjustHitPoints(hitObject.quantity);
                        break;
                    default:
                        break;
                }

                if (shouldDisappear)
                    Destroy(collision.gameObject);
            }
        }
    }

    public bool AdjustHitPoints(int amount)
    {
        if (hitPoints.value < maxHitPoints)
        {
            hitPoints.value = hitPoints.value + amount;
            print("Adjusted HitPoints by: " + amount + ". New value: " + hitPoints.value);
            return true;
        }
        return false;
    }

    public override IEnumerator DamageCharacter(int damage, float
interval)
    {
        while (true)
        {
            StartCoroutine(FlickerCharacter());

            hitPoints.value = hitPoints.value - damage;

            if (hitPoints.value <= float.Epsilon)
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

    public override void KillCharacter()
    {
        base.KillCharacter();

        Destroy(healthBar.gameObject);
        Destroy(inventory.gameObject);

        RPGGameManager.sharedInstance.SetupScene();
    }

    public override void ResetCharacter()
    {
        inventory = Instantiate(inventoryPrefab);

        healthBar = Instantiate(healthBarPrefab);
        healthBar.character = this;

        hitPoints.value = startingHitPoints;
    }
}
