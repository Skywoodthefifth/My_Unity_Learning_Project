﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RPGGameManager : MonoBehaviourPunCallbacks
{
    public RPGCameraManager cameraManager;
    public SpawnPoint playerSpawnPoint;

    public static RPGGameManager sharedInstance = null;
    void Awake()
    {
        if (sharedInstance != null && sharedInstance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            sharedInstance = this;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        SetupScene();

        //DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }


    }

    public void SetupScene()
    {
        if (RPGPlayerManager.LocalPlayerInstance == null)
        {
            SpawnPlayer();
        }
    }

    public void SpawnPlayer()
    {
        if (playerSpawnPoint != null)
        {
            RPGPlayerManager.LocalPlayerInstance = playerSpawnPoint.SpawnPlayerObject();
            cameraManager.virtualCamera.Follow = RPGPlayerManager.LocalPlayerInstance.transform;
        }
    }

    public void DestroyObject(GameObject gameObjectToDestroy)
    {

        if (gameObjectToDestroy.GetComponent<PhotonView>().IsMine == false)
            GetComponent<PhotonView>().RPC("OthersDestroyObject", RpcTarget.Others, gameObjectToDestroy.GetComponent<PhotonView>().ViewID);
        else
            PhotonNetwork.Destroy(gameObjectToDestroy);
    }

    public GameObject InstantiateObject(GameObject gameObjectToInstantiate, Vector3 position, Quaternion rotation)
    {
        return PhotonNetwork.Instantiate(gameObjectToInstantiate.name, position, rotation);
    }

    [PunRPC]
    void OthersDestroyObject(int viewID)
    {
        if (PhotonView.Find(viewID).IsMine)
        {
            PhotonNetwork.Destroy(PhotonView.Find(viewID).gameObject);
        }
    }

    void OpenMenu()
    {
        
    }


}
