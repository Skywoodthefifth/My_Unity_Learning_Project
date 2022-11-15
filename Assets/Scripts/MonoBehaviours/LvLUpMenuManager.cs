using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvLUpMenuManager : MonoBehaviour
{
    [SerializeField] GameObject panel;

    PauseManager pauseManager;


    private void Awake()
    {
        pauseManager = GetComponent<PauseManager>();
        panel.SetActive(false);
    }
    public void OpenPanel()
    {
        pauseManager.PauseGame();
        panel.SetActive(true);
    }
    public void ClosePanel()
    {

        pauseManager.UnPauseGame();
        panel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (panel.activeInHierarchy == false)
            {
                OpenPanel();
            }
            else
            {
                ClosePanel();
            }
        }
    }


}
