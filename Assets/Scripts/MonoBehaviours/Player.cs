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

    Weapon weapon;

    int level = 1;
    int experience = 0;


    PhotonView viewBuffer;

    void Start()
    {
        if (viewBuffer == null)
            GetComponent<PhotonView>().RPC("BufferPhotonView", RpcTarget.AllBuffered);

        ResetCharacter();

    }

    [PunRPC]
    void BufferPhotonView()
    {
        viewBuffer = GetComponent<PhotonView>();
        print("Done: " + gameObject.name + ", viewID: " + viewBuffer.ViewID);
    }
    void Update()
    {
        // if (hitPoints.value <= float.Epsilon)
        // {
        //     KillCharacter();
        // }
        if (viewBuffer.IsMine)
        {
            UpdateInventory();
        }

    }

    void UpdateInventory()
    {
        if (inventory.items != null)
            foreach (Item item in inventory.items)
            {
                if (item != null)
                    if (item.itemType == Item.ItemType.ATTACK)
                    {
                        weapon._attackDamage = weapon._baseAttackDamage + item.quantity;
                    }
            }
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("CanBePickedUp") && viewBuffer.IsMine)
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

                        print("_attackDamage before adjusted: " + weapon._attackDamage);

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
        if (viewBuffer.IsMine)
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

    int TO_LEVEL_UP
    {
        get
        {
            return level * 1000;
        }
    }
    public bool AddExperience(int amount)
    {
        if (viewBuffer.IsMine)
        {

            if (experience <= TO_LEVEL_UP)
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

            if (viewBuffer.IsMine)
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
        if (viewBuffer.IsMine)
        {

            Destroy(healthBar.gameObject);
            Destroy(inventory.gameObject);

            base.KillCharacter();
        }
    }

    public override void ResetCharacter()
    {
        if (viewBuffer.IsMine)
        {
            inventory = Instantiate(inventoryPrefab);

            healthBar = Instantiate(healthBarPrefab);

            healthBar.character = this;

            hitPoints = startingHitPoints;

            weapon = GetComponent<Weapon>();
        }
    }

    //TO ADD EXPERIENCE



    public void CheckLevelUp()
    {
        if (viewBuffer.IsMine)
        {

            if (experience >= TO_LEVEL_UP)
            {
                experience -= TO_LEVEL_UP;
                level += 1;
            }
        }

    }


}
