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
    [SerializeField]
    private LayerMask layerMask;

    protected override void Awake()
    {
        base.Awake();

        if (_lineRenderer == null)
            _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        // Actualizar UI
        CanvasManager.Instance.MagazineAmmoTxt.text = "" + magazineAmmo;
        CanvasManager.Instance.CurrentAmmoTxt.text = "" + CurrentAmmo;

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
        CanvasManager.Instance.MagazineAmmoTxt.text = "" + magazineAmmo;
        CanvasManager.Instance.CurrentAmmoTxt.text = "" + CurrentAmmo;

        if (isAutomatic)
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

    override protected void OnDisable()
    {
        base.OnDisable();
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
        if (!CanShoot() || IsReloading) { 
            //Debug.Log("Can not shoot!");
            return;
        }

        // Disparar una bala
        //Debug.Log($"{weaponName} disparando. Munici�n en cargador restante: {magazineAmmo}");
        anim?.SetTrigger("shoot");
        audioSource.Play();
        shootingParticles.Play();
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward), out RaycastHit hit, Mathf.Infinity, layerMask)) {
 
            if (hit.transform.TryGetComponent(out ZombiePart zombiePart))
                zombiePart.Hit(damage);                   
        }

        DebugLine();
        StartShootCooldown();

        base.Shoot();
    }

    public void ReloadAmmo()
    {
        CanvasManager.Instance.MagazineAmmoTxt.text = "" + magazineAmmo;
        CanvasManager.Instance.CurrentAmmoTxt.text = "" + CurrentAmmo;
        anim?.SetTrigger("reload");
    }

    private void DebugLine()
    {
        if (_debugMode && _lineRenderer != null)
        {
            _lineRenderer.positionCount = 2;
            _lineRenderer.SetPosition(0, Camera.main.transform.position);
            _lineRenderer.SetPosition(1, Camera.main.transform.position + Camera.main.transform.TransformDirection(Vector3.forward) * 100);
        }
    }
}
