using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RPGPlayerManager : MonoBehaviour
{
    public static GameObject LocalPlayerInstance;

    // Start is called before the first frame update
    void Start()
    {

    }

    void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
