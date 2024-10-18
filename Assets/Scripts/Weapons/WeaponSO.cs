using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Weapon")]
public class WeaponSO : ScriptableObject
{
    public string weaponName;             // Nombre del arma
    public int maxAmmo;                   // Máxima cantidad de munición disponible (reserva)
    public int magazineSize;              // Capacidad del cargador
    public float fireRate;                // Cadencia de disparo
    public float reloadTime;              // Tiempo de recarga
    public bool isAutomatic;
    public float damage;
    public float bulletSpeed; 
}
