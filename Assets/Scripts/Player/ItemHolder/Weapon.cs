using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponSO weaponData; // Scriptable Object con los datos del arma
    protected string weaponName;             // Nombre del arma
    protected int maxAmmo;                   // Máxima cantidad de munición disponible (reserva)
    protected int magazineSize;              // Capacidad del cargador
    [SerializeField] protected int magazineAmmo;              // Munición actual en el cargador
    [SerializeField] private int currentAmmo;               // Munición actual en la reserva
    protected float fireRate;                // Cadencia de disparo
    protected float reloadTime;              // Tiempo de recarga
    protected bool isAutomatic;              // Si es automática o semiautomática
    protected float damage;                  // Daño del arma
    protected float bulletSpeed;             // Velocidad de las balas


    private bool isReloading = false;
    private float nextFireTime = 0f;
    public WeaponSO WeaponData { get => weaponData; set => weaponData = value; }
    public int CurrentAmmo { get => currentAmmo; set => currentAmmo = value; }

    // Método abstracto para disparar (definido en subclases)
    public abstract void Shoot();


    void Awake()
    {
        // Inicializar cargador con munición completa al comienzo
        weaponName = weaponData.weaponName;
        maxAmmo = weaponData.maxAmmo;
        magazineSize = weaponData.magazineSize;
        fireRate = weaponData.fireRate;
        reloadTime = weaponData.reloadTime;
        isAutomatic = weaponData.isAutomatic;
        damage = weaponData.damage;
        bulletSpeed = weaponData.bulletSpeed;


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
