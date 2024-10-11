using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapon
{
    public Transform firePoint;   // Punto de disparo del arma
    public GameObject bulletPrefab; // Prefab de la bala

    private void Update()
    {
        if (isAutomatic)
        {
            if (Input.GetButton("Fire1")) // Mantener presionado el botón
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1")) // Disparar una sola vez al presionar
            {
                Shoot();
            }
        }

        // Recargar
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartReload();
        }
    }

    public override void Shoot()
    {
        if (!CanShoot()) return;

        // Disparar una bala
        Debug.Log($"{weaponName} disparando. Munición en cargador restante: {magazineAmmo}");
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Reducir munición
        ReduceAmmo();

        // Aplicar la cadencia de disparo
        ApplyFireRate();
    }
}
