using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallInteractive : MonoBehaviour, IInteract
{
    [SerializeField] GameObject gunPrefab;
    [SerializeField] GameManagerSO gM;
    [SerializeField] int price;

    [SerializeField] private Transform weaponDisplay;
    [SerializeField] private TMPro.TMP_Text weaponName;
    [SerializeField] private TMPro.TMP_Text weaponPrice;

    // TODO: implementar cobrar al jugador cuando interact�e con el objeto
    private void Start()
    {
        // Parte visual, cambia los graficos del arma en display, pone el precio y el nombre
        weaponDisplay.GetComponent<MeshFilter>().mesh = gunPrefab.GetComponent<MeshFilter>().sharedMesh;
        weaponDisplay.GetComponent<MeshRenderer>().material = gunPrefab.GetComponent<MeshRenderer>().sharedMaterial;
        weaponName.text = gunPrefab.GetComponent<Weapon>().WeaponData.weaponName;
        weaponPrice.text = price.ToString();

    }

    public void interact()
    {
        WeaponHolder weaponHolder = gM.Player.GetComponent<WeaponHolder>();
        string weaponName = gunPrefab.GetComponent<Weapon>().WeaponData.weaponName;

        // Verifica si el jugador ya tiene el arma
        bool alreadyHasWeapon = weaponHolder.HasWeapon(weaponName);

        if (!alreadyHasWeapon && weaponHolder.CurrentWeapons.Count < 2)
        {
            // Si no tiene el arma y hay espacio, a�adirla
            weaponHolder.AddWeapon(gunPrefab);
        }
        else if (alreadyHasWeapon)
        {
            // Si ya tiene el arma, comprar munici�n
            Debug.Log("Ya tienes esta arma, comprando munici�n...");
            weaponHolder.RefillAmmo(weaponName);
            // Aqu� puedes a�adir l�gica para aumentar la munici�n
        }
        else if (weaponHolder.CurrentWeapons.Count == 2)
        {
            // Si el jugador ya tiene 2 armas, intercambiar
            weaponHolder.ExchangeWeapon(gunPrefab);
        }
    }
}
