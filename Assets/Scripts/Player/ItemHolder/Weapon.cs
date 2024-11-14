using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public abstract class Weapon : MonoBehaviour
{
    #region Events
    public event Action OnStartReloading;
    public event Action OnStopReloading;
    public event Action OnShoot;
    #endregion


    [SerializeField] private WeaponSO weaponData; // Scriptable Object con los datos del arma
    protected string weaponName;             // Nombre del arma
    protected int maxAmmo;                   // Máxima cantidad de munición disponible (reserva)
    protected int magazineSize;              // Capacidad del cargador
    [SerializeField] protected int magazineAmmo;              // Munición actual en el cargador
    [SerializeField] private int currentAmmo;               // Munición actual en la reserva
    [SerializeField] protected ParticleSystem shootingParticles;               // Sistema de partículas de disparo
    protected float fireRate;                // Cadencia de disparo
    protected float reloadTime;              // Tiempo de recarga
    protected bool isAutomatic;              // Si es automática o semiautomática
    protected float damage;                  // Daño del arma
    protected float bulletSpeed;             // Velocidad de las balas
    protected bool _inCooldown = false;
    [SerializeField] protected Animator anim;
    protected AudioSource audioSource;

    protected bool isReloading = false;
    private float nextFireTime = 0f;
    public WeaponSO WeaponData { get => weaponData; set => weaponData = value; }
    public int CurrentAmmo { get => currentAmmo; set => currentAmmo = value; }


    // Método abstracto para disparar (definido en subclases)
    public virtual void Shoot() {
        OnShoot?.Invoke();
    }


    protected virtual void Awake()
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
        currentAmmo = maxAmmo - magazineAmmo;

        if(anim == null) anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        OnShoot += ReduceAmmo;
    }

    private void Start()
    {
        CanvasManager.Instance.CurrentAmmoTxt.text = "" + CurrentAmmo;
        CanvasManager.Instance.MagazineAmmoTxt.text = "" + magazineAmmo;
    }


    virtual protected void OnDisable()
    {
        Debug.Log("Weapon disabled, stopping coroutines.");
        StopAllCoroutines();
        isReloading = false;
        if (_cooldownRoutine != null)
        {
            StopCoroutine(_cooldownRoutine);
            _inCooldown = false;
            _cooldownRoutine = null;
        }
    }


    public void StartReload()
    {
        if (isReloading || magazineAmmo == magazineSize || currentAmmo <= 0) return;
        if(gameObject.activeSelf) StartCoroutine(Reload());
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        //Debug.Log("Recargando...");
        anim.SetTrigger("reload");
        yield return new WaitForSeconds(reloadTime);

        // Calcular la cantidad de balas que se pueden recargar en el cargador
        int ammoNeeded = magazineSize - magazineAmmo;  // Cuánta munición falta en el cargador
        int ammoToReload = Mathf.Min(ammoNeeded, currentAmmo);  // Lo que podemos recargar

        // Recargar el cargador
        magazineAmmo += ammoToReload;
        currentAmmo -= ammoToReload;

        isReloading = false;
        CanvasManager.Instance.CurrentAmmoTxt.text = "" + CurrentAmmo;
        CanvasManager.Instance.MagazineAmmoTxt.text = "" + magazineAmmo;
        // Debug.Log("Recarga completa. Munición en cargador: " + magazineAmmo + " / Munición restante: " + currentAmmo);
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


    private void ReduceAmmo()
    {
        magazineAmmo--;
        CanvasManager.Instance.MagazineAmmoTxt.text = "" + magazineAmmo;
    }
}
