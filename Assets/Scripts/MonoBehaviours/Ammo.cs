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
            GetComponent<PhotonView>().RPC("BufferPhotonView", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void BufferPhotonView()
    {
        viewBuffer = GetComponent<PhotonView>();
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

            viewBuffer.RPC("DamageCoroutine", RpcTarget.All, collision.gameObject.GetComponent<PhotonView>().ViewID);

            //RPGGameManager.sharedInstance.DestroyObject(gameObject);
            gameObject.SetActive(false);
        }
    }

    [PunRPC]
    void DamageCoroutine(int viewID)
    {
        if (gameObject != null && gameObject.activeSelf == true && viewBuffer.IsMine == true)
        {
            Enemy enemy = PhotonView.Find(viewID).gameObject.GetComponent<Enemy>();
            StartCoroutine(enemy.DamageCharacter(damageInflicted, 0.0f));
        }
    }
}
