using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "GameManagerSO", menuName = "ScriptableObjects/GameManagerSO", order = 1)]
public class GameManagerSO : ScriptableObject
{
    #region Events
    public event Action<int> OnUpdateRounds;
    public event Action<int> OnUpdatePoints;
    public event Action OnGameOver;
    public event Action OnWinGame;
    public event Action OnStopZombies;
    #endregion

    private PlayerBasics player;
    private bool isPaused = false;
    private bool instaKillEnabled = false;

    [NonSerialized] private int rounds = 1;
    [NonSerialized] private int points = 0;

    public PlayerBasics Player { get => player; }
    public int Rounds { get => rounds; }
    public int Points { get => points; }
    public bool IsPaused { get => isPaused; set => isPaused = value; }

    private void OnEnable()
    {
        //El game manager se suscribe al evento de la carga de una nueva escena
        SceneManager.sceneLoaded += SceneLoaded;
        rounds = 0;
        points = 0;
    }

    //Es en este metodo cuando buscamos al player u objetos que queramos tener traqueados
    private void SceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        player = FindAnyObjectByType<PlayerBasics>();
    }

    public void AddRound(int round)
    {
        this.rounds += round;
        OnUpdateRounds?.Invoke(rounds);
    }

    public void AddPoints(int points)
    {
        this.points += points;
        OnUpdatePoints?.Invoke(this.points);
    }

    public void GameOver()
    {
        OnGameOver?.Invoke();
    }

    public void WinGame()
    {
        OnWinGame?.Invoke();
        OnStopZombies?.Invoke();
    }




}
