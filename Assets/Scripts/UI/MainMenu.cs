using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string levelName = "Zombies map";
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject optionsPanel;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        optionsPanel.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(levelName);
    }

    public void ToggleOptions()
    {
        optionsPanel.SetActive(!optionsPanel.activeSelf);
    }

    public void ToggleMainPanel()
    {
        mainPanel.SetActive(!mainPanel.activeSelf);
    }

    public void Quit()
    {
        Application.Quit();
    }
}