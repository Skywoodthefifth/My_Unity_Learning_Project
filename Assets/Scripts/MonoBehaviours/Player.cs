using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("CanBePickedUp"))
        {
            Item hitObject = collision.gameObject.GetComponent<Consumable>().item;

            if (hitObject != null)
            {
                print("Hit: " + hitObject.objectName);

                switch (hitObject.itemType)
                {
                    case Item.ItemType.COIN:
                        break;

                    case Item.ItemType.HEALTH:
                        AdjustHitPoints(hitObject.quantity);
                        break;
                }

                collision.gameObject.SetActive(false);
            }
        }
    }

    public void AdjustHitPoints(int amount)
    {
        hitPoints = hitPoints + amount;
        print("Adjusted HitPoints by: " + amount + ". New value: " + hitPoints);
    }
}
