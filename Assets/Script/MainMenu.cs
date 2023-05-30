using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] bool isStartGame;
    //[SerializeField] AudioSource Music;
    //[SerializeField] GameObject NewGamePanel, OptionPanel, ControlPanel, VolumePanel, SystemPanel;
    [SerializeField] GameObject TitleSceenPanel, MainMenuPanel;

    private void Start()
    {
        MusicManager.singleton.SetAndPlayBGM(0);
    }

    private void Update()
    {
        if (isStartGame) return;
        if (Input.anyKeyDown)
            isStartGame = true;


        if (isStartGame)
        {
            MainMenuPanel.SetActive(isStartGame);
            //OptionPanel.SetActive(false);
            //VolumePanel.SetActive(false);
            //SystemPanel.SetActive(false);
        }


        TitleSceenPanel.SetActive(!isStartGame);
    }

    public void ToggleStartGame()
    {
        isStartGame = !isStartGame;
    }

    public void ChangeStartGame(bool isStartGame) => this.isStartGame = isStartGame;

    public void ChangeScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }

    /*
    public void BackToMainMenu()
    {
        NewGamePanel.SetActive(false);
        OptionPanel.SetActive(false);
        ControlPanel.SetActive(false);
        VolumePanel.SetActive(false);
        SystemPanel.SetActive(false);
    }
    public void OpenNewGame()
    {
        NewGamePanel.SetActive(true);
    }

    public void OpenOptions()
    {
        OptionPanel.SetActive(true);
        ControlPanel.SetActive(true);
        VolumePanel.SetActive(false);
        SystemPanel.SetActive(false);
    }
    */

    public void QuitGame()
    {
        Application.Quit();
    }

    /*
    public void ControlOptions()
    {
        ControlPanel.SetActive(true);
        VolumePanel.SetActive(false);
        SystemPanel.SetActive(false);
    }
    public void VolumeOptions()
    {
        ControlPanel.SetActive(false);
        VolumePanel.SetActive(true);
        SystemPanel.SetActive(false);
    }
    public void SystemOptions()
    {
        ControlPanel.SetActive(false);
        VolumePanel.SetActive(false);
        SystemPanel.SetActive(true);
    }
    */

}
