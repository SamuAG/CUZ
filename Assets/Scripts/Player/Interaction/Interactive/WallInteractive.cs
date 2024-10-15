using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallInteractive : MonoBehaviour, IInteract
{
    [SerializeField] GameObject gunPrefab;
    [SerializeField] GameManagerSO gM;

    public void interact()
    {
        // TODO: esto se podria deshardcodear un poco o se pueden tener armas repetidas o que te compre municion si ya la tienes
        // eso se podria comprobar xq la clase Weapon tiene un campo llamado name
        if (gM.Player.GetComponent<WeaponHolder>().CurrentWeapons.Count < 2)
        {
            gM.Player.GetComponent<WeaponHolder>().AddWeapon(gunPrefab);
        }
        else if (gM.Player.GetComponent<WeaponHolder>().CurrentWeapons.Count == 2)
        {
            gM.Player.GetComponent<WeaponHolder>().ExchangeWeapon(gunPrefab);
        }
    }
}
