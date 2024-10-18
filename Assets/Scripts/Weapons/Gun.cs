using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapon
{
    public Transform firePoint;   // Punto de disparo del arma
    public GameObject bulletPrefab; // Prefab de la bala
    bool isShooting = false;
    [SerializeField] private InputManagerSO input;

    private void OnEnable()
    {
        if(isAutomatic)
        {
            input.OnShootStarted += ShootAuto;
            input.OnShootCanceled += ShootAutoCancel;
        }
        else
        {
            input.OnShootStarted += Shoot;
        }
        
        input.OnReloadStarted += StartReload;
    }

    private void OnDisable()
    {
        if (isAutomatic)
        {
            input.OnShootStarted -= ShootAuto;
            input.OnShootCanceled -= ShootAutoCancel;
        }
        else
        {
            input.OnShootStarted -= Shoot;
        }
        input.OnReloadStarted -= StartReload;
    }

    

    private void Update()
    {
        if(isAutomatic)
        {
            if (isShooting)
            {

                Shoot();
            }
        }
    }

    private void ShootAuto()
    {
        
        isShooting = true;
    }

    private void ShootAutoCancel()
    {
        
        isShooting = false;
    }

    public override void Shoot()
    {
        if (!CanShoot()) return;

        // Disparar una bala
        Debug.Log($"{weaponName} disparando. Munición en cargador restante: {magazineAmmo}");
        Bullet bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation).GetComponent<Bullet>();

        bullet.speed = bulletSpeed;
        bullet.damage = damage;

        // Reducir munición
        ReduceAmmo();

        // Aplicar la cadencia de disparo
        ApplyFireRate();
    }
}
