using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPoint : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public float repeatInterval;

    // Start is called before the first frame update
    public void Start()
    {
        if (repeatInterval > 0)
        {
            InvokeRepeating("SpawnObject", 0.0f, repeatInterval);
        }
    }

    public GameObject SpawnObject()
    {
        if (prefabToSpawn != null)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                return RPGGameManager.sharedInstance.InstantiateObject(prefabToSpawn, transform.position, Quaternion.identity);
            }
        }
        return null;
    }

    public GameObject SpawnPlayerObject()
    {
        if (prefabToSpawn != null)
        {
            //return PhotonNetwork.Instantiate(prefabToSpawn.name, transform.position, Quaternion.identity);
            return RPGGameManager.sharedInstance.InstantiateObject(prefabToSpawn, transform.position, Quaternion.identity);
        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
