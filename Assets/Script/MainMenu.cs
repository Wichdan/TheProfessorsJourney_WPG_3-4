using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] bool isStartGame;
    //[SerializeField] AudioSource Music;
    //[SerializeField] GameObject NewGamePanel, OptionPanel, ControlPanel, VolumePanel, SystemPanel;
    [SerializeField] GameObject TitleSceenPanel, MainMenuPanel;
    [SerializeField] Button startGame;
    [SerializeField] int sceneIndex;

    private void Start()
    {
        if (MusicManager.instance != null)
            MusicManager.instance.SetAndPlayBGM(0);
        
        startGame.onClick.AddListener(() =>
        {
            SceneChanger.instance.ChangeScene(sceneIndex);
        });
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

    public void ChangeIndexScene(int index)
    {
        sceneIndex = index;
    }

    public void ChangeStartGame(bool isStartGame) => this.isStartGame = isStartGame;

    public void QuitGame()
    {
        Application.Quit();
    }
}
