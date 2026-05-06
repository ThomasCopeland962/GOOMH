using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ButtonUI : MonoBehaviour
{

    [SerializeField] private GameObject levelSelectPanel;
    [SerializeField] private GameObject instructionsPanel;    
    public void NewGameButton()
    {
        levelSelectPanel.SetActive(true);
    }

    public void HowToPlay()
    {
        if (levelSelectPanel.activeSelf)
        {
            return;
        }
        instructionsPanel.SetActive(true);
    }

    public void QuitGame()
    {
        if (levelSelectPanel.activeSelf)
        {
            return;
        }
        Application.Quit();
    }

    public void Back()
    {
        levelSelectPanel.SetActive(false);
        instructionsPanel.SetActive(false);
    }

    public void FrontLawn()
    {
        SceneManager.LoadSceneAsync("lvl_frontLawn");
    }
    public void Garage()
    {
        SceneManager.LoadSceneAsync("lvl_garage");
    }
    public void Kitchen()
    {
        SceneManager.LoadSceneAsync("lvl_kitchen");
    }
    public void LivingRoom()
    {
        SceneManager.LoadSceneAsync("lvl_livingroom");
    }
    public void Bedroom()
    {
        SceneManager.LoadSceneAsync("lvl_bedroom");
    }
    public void Bathroom()
    {
        SceneManager.LoadSceneAsync("lvl_bathroom");
    }
}
