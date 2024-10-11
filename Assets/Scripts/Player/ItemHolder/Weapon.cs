using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public string weaponName;             // Nombre del arma
    public int maxAmmo;                   // Máxima cantidad de munición disponible (reserva)
    public int magazineSize;              // Capacidad del cargador
    public int magazineAmmo;              // Munición actual en el cargador
    public int currentAmmo;               // Munición actual en la reserva
    public float fireRate;                // Cadencia de disparo
    public float reloadTime;              // Tiempo de recarga
    public bool isAutomatic;              // Si es automática o semiautomática

    private bool isReloading = false;
    private float nextFireTime = 0f;

    // Método abstracto para disparar (definido en subclases)
    public abstract void Shoot();

    private void Start()
    {
        // Inicializar cargador con munición completa al comienzo
        magazineAmmo = magazineSize;
        currentAmmo = maxAmmo;
    }

    public void StartReload()
    {
        if (isReloading || magazineAmmo == magazineSize || currentAmmo <= 0) return;
        StartCoroutine(Reload());
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Recargando...");
        yield return new WaitForSeconds(reloadTime);

        // Calcular la cantidad de balas que se pueden recargar en el cargador
        int ammoNeeded = magazineSize - magazineAmmo;  // Cuánta munición falta en el cargador
        int ammoToReload = Mathf.Min(ammoNeeded, currentAmmo);  // Lo que podemos recargar

        // Recargar el cargador
        magazineAmmo += ammoToReload;
        currentAmmo -= ammoToReload;

        isReloading = false;
        Debug.Log("Recarga completa. Munición en cargador: " + magazineAmmo + " / Munición restante: " + currentAmmo);
    }

    protected bool CanShoot()
    {
        return Time.time >= nextFireTime && magazineAmmo > 0 && !isReloading;
    }

    protected void ApplyFireRate()
    {
        nextFireTime = Time.time + 1f / fireRate;
    }

    public void ReduceAmmo()
    {
        magazineAmmo--;
    }
}
