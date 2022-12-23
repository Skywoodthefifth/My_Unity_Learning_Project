using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Enemy : Character
{
    private float hitPoints;
    public int damageStrength;
    public int experienceReward;
    Coroutine damageCoroutine;

    PhotonView viewBuffer;
    // Start is called before the first frame update
    void Start()
    {
        if (viewBuffer == null)
            gameObject.GetPhotonView().RPC("BufferPhotonView", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void BufferPhotonView()
    {
        viewBuffer = gameObject.GetPhotonView();
        print("Done: " + gameObject.name + ", viewID: " + viewBuffer.ViewID);
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

    public override void ResetCharacter()
    {
        hitPoints = startingHitPoints;
    }

    public GameObject coinDropPrefab;
    public GameObject ExperienceDropPrefab;

    public override void KillCharacter()
    {

        if (PhotonNetwork.IsMasterClient)
        {
            GameObject dropCoin = RPGGameManager.sharedInstance.InstantiateObject(coinDropPrefab, transform.position, Quaternion.identity);
            GameObject dropExperience = RPGGameManager.sharedInstance.InstantiateObject(ExperienceDropPrefab, transform.position, Quaternion.identity);

            //drop.transform.position = transform.position;

            base.KillCharacter();
        }
    }
}
