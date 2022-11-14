using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Player : Character
{
    //public HitPoints hitPoints;
    [HideInInspector]
    public float hitPoints;

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
        if (collision.gameObject.CompareTag("CanBePickedUp") && GetComponent<PhotonView>().IsMine)
        {

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

        }
    }

    public bool AdjustHitPoints(int amount)
    {
        if (GetComponent<PhotonView>().IsMine)
        {

            if (hitPoints < maxHitPoints)
            {
                hitPoints = hitPoints + amount;
                print("Adjusted HitPoints by: " + amount + ". New value: " + hitPoints);
                return true;
            }
            return false;
        }

        return false;
    }

    public override IEnumerator DamageCharacter(int damage, float
interval)
    {

        while (true)
        {
            StartCoroutine(FlickerCharacter());

            if (GetComponent<PhotonView>().IsMine)
            {

                hitPoints = hitPoints - damage;

                if (hitPoints <= float.Epsilon)
                {
                    KillCharacter();
                    break;
                }

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

            hitPoints = startingHitPoints;
        }
    }
}
