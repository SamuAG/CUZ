using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class PauseController : MonoBehaviour
{
    [SerializeField] private InputManagerSO input;
    [SerializeField] private Canvas pauseCanvas;
    [SerializeField] private GameManagerSO gM;

    private void OnEnable()
    {
        input.OnPauseStarted += Pause;
    }

    private void OnDisable()
    {
        input.OnPauseStarted -= Pause;
    }

    private void Start()
    {
        pauseCanvas.enabled = false;
    }

    public void Pause()
    {
        pauseCanvas.enabled = !pauseCanvas.enabled;
        gM.IsPaused = pauseCanvas.enabled;
        if(pauseCanvas.enabled)
        {
            input.Inputs.Gameplay.Disable();
        }
        else
        {
            input.Inputs.Gameplay.Enable();
        }
        Time.timeScale = pauseCanvas.enabled ? 0 : 1;
        Cursor.visible = pauseCanvas.enabled;
        Cursor.lockState = pauseCanvas.enabled ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
