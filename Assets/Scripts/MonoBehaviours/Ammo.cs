using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    public int damageInflicted;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision is BoxCollider2D)
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            GetComponent<PhotonView>().RPC("DamageCoroutine", RpcTarget.All, enemy.name);

            //RPGGameManager.sharedInstance.DestroyObject(gameObject);
            gameObject.SetActive(false);
        }
    }

    [PunRPC]
    void DamageCoroutine(string name)
    {
        Enemy enemy = GameObject.Find(name).GetComponent<Enemy>();
        StartCoroutine(enemy.DamageCharacter(damageInflicted, 0.0f));
    }
}
