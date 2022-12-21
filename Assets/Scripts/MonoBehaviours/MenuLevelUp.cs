using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLevelUp : MonoBehaviour
{
    [HideInInspector]
    public Player player;

    public List<GameObject> prefabToSpawn;


    public void SpawnPrefab(int choice)
    {
        if(prefabToSpawn != null) 
        {
            RPGGameManager.sharedInstance.InstantiateObject(prefabToSpawn[choice], player.gameObject.transform.position, Quaternion.identity);
            print(player.gameObject.transform.position + "");
            gameObject.SetActive(false);
        }
    }




}
