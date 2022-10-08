using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
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

    void Update()
    {
        // if (hitPoints.value <= float.Epsilon)
        // {
        //     KillCharacter();
        // }
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("CanBePickedUp"))
        {
            // if (collision.gameObject.GetComponent<PhotonView>().IsMine == false)
            // {
            //     collision.gameObject.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer.ActorNumber);
            // }

            //if (collision.gameObject.GetComponent<PhotonView>().IsMine)
            //{
            Item hitObject = collision.gameObject.GetComponent<Consumable>().item;

            if (hitObject != null && collision.gameObject != null)
            {
                print("Hit: " + hitObject.objectName);

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
                    RPGGameManager.sharedInstance.DestroyObject(collision.gameObject);
            }
            //}
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
        if (GetComponent<PhotonView>().IsMine)
        {

            Destroy(healthBar.gameObject);
            Destroy(inventory.gameObject);

            base.KillCharacter();
        }
    }

    public override void ResetCharacter()
    {
        if (GetComponent<PhotonView>().IsMine)
        {
            inventory = Instantiate(inventoryPrefab);

            healthBar = Instantiate(healthBarPrefab);
            healthBar.character = this;

            hitPoints.value = startingHitPoints;
        }
    }
}
