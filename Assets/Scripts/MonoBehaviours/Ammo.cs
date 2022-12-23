using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    public int damageInflicted;
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

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision is BoxCollider2D && gameObject != null)
        {
            //Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            viewBuffer.RPC("DamageCoroutine", RpcTarget.All, collision.gameObject.GetPhotonView().ViewID);

            //RPGGameManager.sharedInstance.DestroyObject(gameObject);
            gameObject.SetActive(false);
        }
    }

    [PunRPC]
    void DamageCoroutine(int viewID)
    {
        PhotonView enemyViewBuffer = PhotonView.Find(viewID);
        if (gameObject != null && gameObject.activeSelf == true && enemyViewBuffer != null)
        {
            Enemy enemy = enemyViewBuffer.gameObject.GetComponent<Enemy>();
            StartCoroutine(enemy.DamageCharacter(damageInflicted, 0.0f));
        }
    }
}
