using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallInteractive : MonoBehaviour, IInteract
{
    [SerializeField] GameObject gunPrefab;
    [SerializeField] GameObject Graphics;
    [SerializeField] GameManagerSO gM;
    [SerializeField] int price;

    [SerializeField] private Transform weaponDisplay;
    [SerializeField] private TMPro.TMP_Text weaponName;
    [SerializeField] private TMPro.TMP_Text weaponPrice;

    // TODO: implementar cobrar al jugador cuando interactúe con el objeto
    private void Start()
    {
        // Parte visual, cambia los graficos del arma en display, pone el precio y el nombre
        weaponDisplay.GetComponent<MeshFilter>().mesh = Graphics.GetComponent<MeshFilter>().sharedMesh;
        weaponDisplay.GetComponent<MeshRenderer>().material = Graphics.GetComponent<MeshRenderer>().sharedMaterial;
        weaponName.text = gunPrefab.GetComponent<Weapon>().WeaponData.weaponName;
        weaponPrice.text = price.ToString();

    }

    public void interact()
    {
        if (gM.Points < price)
            return;

        WeaponHolder weaponHolder = gM.Player.GetComponent<WeaponHolder>();
        string weaponName = gunPrefab.GetComponent<Weapon>().WeaponData.weaponName;

        // Verifica si el jugador ya tiene el arma
        bool alreadyHasWeapon = weaponHolder.HasWeapon(weaponName);

        if (!alreadyHasWeapon && weaponHolder.CurrentWeapons.Count < 2)
        {
            // Si no tiene el arma y hay espacio, añadirla
            weaponHolder.AddWeapon(gunPrefab);
            gM.AddPoints(-price);
        }
        else if (alreadyHasWeapon)
        {
            // Si ya tiene el arma, comprar munición
            Debug.Log("Ya tienes esta arma, comprando munición...");
            weaponHolder.RefillAmmo(weaponName);
            gM.AddPoints(-price);
            // Aquí puedes añadir lógica para aumentar la munición
        }
        else if (weaponHolder.CurrentWeapons.Count == 2)
        {
            // Si el jugador ya tiene 2 armas, intercambiar
            weaponHolder.ExchangeWeapon(gunPrefab);
            gM.AddPoints(-price);
        }
    }
}
