using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    public List<GameObject> baseWeaponsPrefabs;  // Prefabs de las armas
    public Transform handTransform;         // Transform de la mano donde se spawnearán las armas
    public int selectedWeaponIndex = 0;
    [SerializeField] private InputManagerSO input;

    private List<GameObject> currentWeapons = new List<GameObject>();  // Armas equipadas

    public List<GameObject> CurrentWeapons { get => currentWeapons; }

    private void OnEnable()
    {
        input.OnChangeWeaponStarted += ChangeWeapon;
    }

    private void OnDisable()
    {
        input.OnChangeWeaponStarted -= ChangeWeapon;
    }

    private void Start()
    {
        // Inicializa las armas (activando solo la seleccionada)
        InitializeWeapons();
        EquipWeapon(selectedWeaponIndex);
        
    }

    private void InitializeWeapons()
    {
        // Instanciar todas las armas desde la lista de prefabs, pero desactivarlas inicialmente
        for (int i = 0; i < baseWeaponsPrefabs.Count; i++)
        {
            GameObject newWeapon = Instantiate(baseWeaponsPrefabs[i], handTransform.position, handTransform.rotation, handTransform);
            newWeapon.SetActive(false);  // Desactivar inicialmente todas las armas
            currentWeapons.Add(newWeapon);
        }
        
        if(currentWeapons.Count >= 0)
        {
            EquipWeapon(0);
        }
    }

    private void ChangeWeapon()
    {
        selectedWeaponIndex = (selectedWeaponIndex + 1) % currentWeapons.Count;
        EquipWeapon(selectedWeaponIndex);
    }

    void EquipWeapon(int index)
    {
        // Desactivar todas las armas
        for (int i = 0; i < currentWeapons.Count; i++)
        {
            currentWeapons[i].SetActive(false);
        }

        // Activar la nueva arma seleccionada
        if (currentWeapons[index] != null)
        {
            currentWeapons[index].SetActive(true);
            //Debug.Log($"Arma equipada: {currentWeapons[index].name}");
        }
        else
        {
            //Debug.LogError("No se encontró el arma seleccionada.");
        }
    }

    public void AddWeapon(GameObject weaponPrefab)
    {
        // Instanciar el arma y annadirla a la lista de armas
        GameObject newWeapon = Instantiate(weaponPrefab, handTransform.position, handTransform.rotation, handTransform);
        currentWeapons[selectedWeaponIndex].SetActive(false);  
        newWeapon.SetActive(true);  
        currentWeapons.Add(newWeapon);
        selectedWeaponIndex = currentWeapons.Count - 1;
    }

    public void RemoveCurrentWeapon()
    {
        // Eliminar el arma actual y equipar la siguiente
        if (currentWeapons.Count > 1)
        {
            Destroy(currentWeapons[selectedWeaponIndex]);
            currentWeapons.RemoveAt(selectedWeaponIndex);
            selectedWeaponIndex = (selectedWeaponIndex + 1) % currentWeapons.Count;
            EquipWeapon(selectedWeaponIndex);
        }
    }

    public void ExchangeWeapon(GameObject weaponPrefab)
    {
        // 1. Eliminar el arma actual
        if (currentWeapons.Count > 0)
        {
            Destroy(currentWeapons[selectedWeaponIndex]);
            currentWeapons.RemoveAt(selectedWeaponIndex);
        }

        // 2. Añadir la nueva arma
        GameObject newWeapon = Instantiate(weaponPrefab, handTransform.position, handTransform.rotation, handTransform);
        newWeapon.SetActive(true); // Activar el arma recién intercambiada
        currentWeapons.Insert(selectedWeaponIndex, newWeapon);  // Añadir en el mismo índice donde estaba la anterior

        // 3. Equipar la nueva arma
        EquipWeapon(selectedWeaponIndex);
    }

    public bool HasWeapon(string weaponName)
    {
        foreach (GameObject weapon in currentWeapons)
        {
            if (weapon.GetComponent<Weapon>().WeaponData.weaponName == weaponName)
            {
                return true;
            }
        }
        return false;
    }

    public void RefillAmmo(string weaponName)
    {
        foreach (GameObject weapon in currentWeapons)
        {
            Weapon weaponScript = weapon.GetComponent<Weapon>();
            if (weaponScript.WeaponData.weaponName == weaponName)
            {
                weaponScript.CurrentAmmo = weaponScript.WeaponData.maxAmmo;
            }
        }
    }
}
