using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallInteractive : MonoBehaviour, IInteract
{
    [SerializeField] GameObject gunPrefab;
    [SerializeField] GameManagerSO gM;
    [SerializeField] int price;

    // TODO: implementar cobrar al jugador cuando interactúe con el objeto

    public void interact()
    {
        WeaponHolder weaponHolder = gM.Player.GetComponent<WeaponHolder>();
        string weaponName = gunPrefab.GetComponent<Weapon>().WeaponData.weaponName;

        // Verifica si el jugador ya tiene el arma
        bool alreadyHasWeapon = weaponHolder.HasWeapon(weaponName);

        if (!alreadyHasWeapon && weaponHolder.CurrentWeapons.Count < 2)
        {
            // Si no tiene el arma y hay espacio, añadirla
            weaponHolder.AddWeapon(gunPrefab);
        }
        else if (alreadyHasWeapon)
        {
            // Si ya tiene el arma, comprar munición
            Debug.Log("Ya tienes esta arma, comprando munición...");
            weaponHolder.RefillAmmo(weaponName);
            // Aquí puedes añadir lógica para aumentar la munición
        }
        else if (weaponHolder.CurrentWeapons.Count == 2)
        {
            // Si el jugador ya tiene 2 armas, intercambiar
            weaponHolder.ExchangeWeapon(gunPrefab);
        }
    }
}
