using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponSO weaponData; // Scriptable Object con los datos del arma
    protected string weaponName;             // Nombre del arma
    protected int maxAmmo;                   // M�xima cantidad de munici�n disponible (reserva)
    protected int magazineSize;              // Capacidad del cargador
    [SerializeField] protected int magazineAmmo;              // Munici�n actual en el cargador
    [SerializeField] private int currentAmmo;               // Munici�n actual en la reserva
    protected float fireRate;                // Cadencia de disparo
    protected float reloadTime;              // Tiempo de recarga
    protected bool isAutomatic;              // Si es autom�tica o semiautom�tica
    protected float damage;                  // Da�o del arma
    protected float bulletSpeed;             // Velocidad de las balas
    protected bool _inCooldown = false;
    protected Animator anim;

    private bool isReloading = false;
    private float nextFireTime = 0f;
    public WeaponSO WeaponData { get => weaponData; set => weaponData = value; }
    public int CurrentAmmo { get => currentAmmo; set => currentAmmo = value; }
    

    // M�todo abstracto para disparar (definido en subclases)
    public abstract void Shoot();


    protected virtual void Awake()
    {
        // Inicializar cargador con munici�n completa al comienzo
        weaponName = weaponData.weaponName;
        maxAmmo = weaponData.maxAmmo;
        magazineSize = weaponData.magazineSize;
        fireRate = weaponData.fireRate;
        reloadTime = weaponData.reloadTime;
        isAutomatic = weaponData.isAutomatic;
        damage = weaponData.damage;
        bulletSpeed = weaponData.bulletSpeed;


        magazineAmmo = magazineSize;
        currentAmmo = maxAmmo - magazineAmmo;

        anim = GetComponent<Animator>();
    }

    public void StartReload()
    {
        if (isReloading || magazineAmmo == magazineSize || currentAmmo <= 0) return;
        StartCoroutine(Reload());
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        //Debug.Log("Recargando...");
        anim.SetTrigger("reload");
        yield return new WaitForSeconds(reloadTime);

        // Calcular la cantidad de balas que se pueden recargar en el cargador
        int ammoNeeded = magazineSize - magazineAmmo;  // Cu�nta munici�n falta en el cargador
        int ammoToReload = Mathf.Min(ammoNeeded, currentAmmo);  // Lo que podemos recargar

        // Recargar el cargador
        magazineAmmo += ammoToReload;
        currentAmmo -= ammoToReload;

        isReloading = false;
       // Debug.Log("Recarga completa. Munici�n en cargador: " + magazineAmmo + " / Munici�n restante: " + currentAmmo);
    }

    protected bool CanShoot()
    {
        return !_inCooldown && magazineAmmo > 0 && !isReloading;
    }

    #region Cooldown
    Coroutine _cooldownRoutine = null;
    
    protected void StartShootCooldown()
    {
        _cooldownRoutine ??= StartCoroutine(ShootCooldown());
    }

    IEnumerator ShootCooldown()
    {
        _inCooldown = true;
        float t = 0;
        while (t < fireRate)
        {
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        _inCooldown = false;
        _cooldownRoutine = null;
    }
    #endregion


    public void ReduceAmmo()
    {
        magazineAmmo--;
    }
}
