using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] int selectedLevel;
    [SerializeField] Button startGameButton;
    [SerializeField] bool isStartGame;
    [SerializeField] AudioSource Music;
    [SerializeField] GameObject TitleSceenPanel, MainMenuPanel, NewGamePanel, OptionPanel, ControlPanel, VolumePanel, SystemPanel;

    private void Start()
    {
<<<<<<< HEAD
        //startGameButton.interactable = false;
=======
        startGameButton.interactable = false;
        Music.Play();
>>>>>>> 85dc7130318cc792b4ac4341c9b8d83a3d3f7f50
    }

    private void Update()
    {
        if(isStartGame) return;
        if(Input.anyKeyDown)
            isStartGame = true;

        if(isStartGame){
            MainMenuPanel.SetActive(true);
            TitleSceenPanel.SetActive(false);
            OptionPanel.SetActive(false);
            VolumePanel.SetActive(false);
            SystemPanel.SetActive(false);
        }
    }

    public void SelectLevel(int level)
    {
        selectedLevel = level;
        EnableStartGame();
    }

    public void EnableStartGame(){
        startGameButton.interactable = true;
    }

    public void StartGame()
    {
        if(selectedLevel < 1) return;
        SceneManager.LoadScene(selectedLevel);
    }

    public void ChangeStartGame(bool isStartGame) => this.isStartGame = isStartGame;
<<<<<<< HEAD

<<<<<<< HEAD
    public void ChangeScene(int index){
        SceneManager.LoadScene(index);
=======
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

    public void QuitGame()
    {
        Application.Quit();
    }

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
>>>>>>> 85dc7130318cc792b4ac4341c9b8d83a3d3f7f50
    }
=======
>>>>>>> parent of 4dd0943 (bayangan)
    
}
