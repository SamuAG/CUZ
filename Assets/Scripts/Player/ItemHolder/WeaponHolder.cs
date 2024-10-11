using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    public List<GameObject> weaponPrefabs;  // Prefabs de las armas
    public Transform handTransform;         // Transform de la mano donde se spawnearán las armas
    public int selectedWeaponIndex = 0;

    private GameObject currentWeapon;       // Referencia al arma equipada

    private void Start()
    {
        EquipWeapon(selectedWeaponIndex);
    }

    private void Update()
    {
        // Cambiar de arma con la rueda del ratón
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            selectedWeaponIndex = (selectedWeaponIndex + 1) % weaponPrefabs.Count;
            EquipWeapon(selectedWeaponIndex);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            selectedWeaponIndex--;
            if (selectedWeaponIndex < 0)
            {
                selectedWeaponIndex = weaponPrefabs.Count - 1;
            }
            EquipWeapon(selectedWeaponIndex);
        }
    }

    void EquipWeapon(int index)
    {
        // Si ya hay un arma equipada, destruirla
        if (currentWeapon != null)
        {
            Destroy(currentWeapon);
        }

        // Instanciar el nuevo arma en la posición de la mano
        currentWeapon = Instantiate(weaponPrefabs[index], handTransform.position, handTransform.rotation, handTransform);

        // Ajustar la posición y rotación para que coincida con la mano (si es necesario)
        currentWeapon.transform.localPosition = Vector3.zero;  // Ajustar si es necesario
        currentWeapon.transform.localRotation = Quaternion.identity;

        Debug.Log($"Arma equipada: {currentWeapon.name}");
    }
}
