using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Player : Character
{
    //public HitPoints hitPoints;
    [HideInInspector]
    public float hitPoints;

    [HideInInspector]
    public float experience;

    public HealthBar healthBarPrefab;

    HealthBar healthBar;

    public ExperienceBar experienceBarPrefab;

    ExperienceBar experienceBar;

    [HideInInspector]
    public float expToGain;

    public Inventory inventoryPrefab;

    Inventory inventory;

    Weapon weapon;


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
        if (GetComponent<PhotonView>().IsMine)
        {
            UpdateInventory();
        }

    }

    void UpdateInventory()
    {
        if (inventory.items != null && GetComponent<PhotonView>().IsMine)
            foreach (Item item in inventory.items)
            {
                if (item != null)
                    if (item.itemType == Item.ItemType.ATTACK)
                    {
                        weapon._attackDamage = weapon._baseAttackDamage + item.quantity;
                        print("_attackDamage: " + weapon._attackDamage);
                    }
            }
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

                    case Item.ItemType.ATTACK:

                        shouldDisappear = inventory.AddItem(hitObject);

                        break;

                    case Item.ItemType.HEALTH:


                        shouldDisappear = AdjustHitPoints(hitObject.quantity);
                        break;
                    case Item.ItemType.EXPERIENCE:

                        shouldDisappear = AddExperience(hitObject.quantity);
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



    
    public bool AddExperience(int amount)
    {
        if (GetComponent<PhotonView>().IsMine)
        {
            if (experience <= expToGain)
            {

                experience = experience + amount;
                print("Adjusted experience by: " + amount + ". New value: " + experience);
                
                return true;
            }
            else
            {
                level += 1;
                print("Level increased by 1: " + level);
            }
        }

        return false;

       

    }

    public override IEnumerator DamageCharacter(int damage, float interval)
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

            experienceBar = Instantiate(experienceBarPrefab);

            experience = startingExperience;

            expToGain = level * 1000;

            experienceBar.player = this;

            weapon = GetComponent<Weapon>();
        }
    }

   

   



}
