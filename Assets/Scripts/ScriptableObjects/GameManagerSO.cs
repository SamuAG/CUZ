using System;
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
    #endregion

    private PlayerBasics player;
    private bool isPaused = false;

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
    }

    //Es en este metodo cuando buscamos al player u objetos que queramos tener traqueados
    private void SceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        player = FindAnyObjectByType<PlayerBasics>();
    }

    public void AddRound(int round)
    {
        Debug.Log("Ronda: "+ Rounds);
        this.rounds += round;
        OnUpdateRounds?.Invoke(rounds);
        Debug.Log("Pasamos a ronda: " + Rounds);
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
    }
}
