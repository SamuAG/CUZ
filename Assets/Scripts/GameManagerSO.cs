using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "GameManagerSO", menuName = "ScriptableObjects/GameManagerSO", order = 1)]
public class GameManagerSO : ScriptableObject
{
    private PlayerBasics player;

    [NonSerialized] private int rounds = 1;

    public event Action<int> OnUpdateRounds;


    //Recicaldo de otro proyecto, se podria usar para añadir objetos y udpatear rondas
    //public event Action<ItemSO> OnNewItem;
    //public event Action<ItemSO> OnRemoveItem;


    public PlayerBasics Player { get => player; }
    public int Rounds { get => rounds; }

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
        rounds += round;
        OnUpdateRounds?.Invoke(rounds);
        Debug.Log("Pasamos a ronda: " + Rounds);
    }

    /*
    //Evento para añadir un item
    public void NewItem(ItemSO itemSO)
    {
        OnNewItem?.Invoke(itemSO);
    }

    //Evento para eliminar un item
    public void RemoveItem(ItemSO itemSO)
    {
        OnRemoveItem?.Invoke(itemSO);
    }
    */

}
