using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;


public class Gun : Weapon
{
    public Transform firePoint;   // Punto de disparo del arma
    public GameObject bulletPrefab; // Prefab de la bala
    bool isShooting = false;
    [SerializeField]
    private InputManagerSO input;
    [SerializeField]
    private LineRenderer _lineRenderer = null;
    [SerializeField]
    private bool _debugMode = false;

    protected override void Awake()
    {
        base.Awake();

        if (_lineRenderer == null)
            _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (isAutomatic)
        {
            if (isShooting)
            {

                Shoot();
            }
        }
    }
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
        if (!CanShoot()) { 
            //Debug.Log("Can not shoot!");
            return;
        }

        // Disparar una bala
        //Debug.Log($"{weaponName} disparando. Munición en cargador restante: {magazineAmmo}");
        anim.SetTrigger("shoot");
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, Mathf.Infinity)) {

            if (hit.transform.gameObject.CompareTag("Zombie"))
            {
                if (hit.transform.TryGetComponent(out Zombie zombie))
                    zombie.ApplyDamage(damage);
            }     
        }

        DebugLine();
        StartShootCooldown();

        base.Shoot();
    }

    public void ReloadAmmo()
    {
        anim.SetTrigger("reload");
    }

    private void DebugLine()
    {
        if (_debugMode && _lineRenderer != null)
        {
            _lineRenderer.positionCount = 2;
            _lineRenderer.SetPosition(0, transform.position);
            _lineRenderer.SetPosition(1, transform.position + transform.TransformDirection(Vector3.forward) * 100);
        }
    }
}
