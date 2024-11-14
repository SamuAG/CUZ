using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBasics : MonoBehaviour, Damageable
{

    private float maxHealth = 100f;
    private float regenerationRate = 10f;
    private float damageCooldown = 5f;
    private float currentHealth;
    private bool instaKillEnabled = false;
    private Coroutine regenCoroutine;

    [SerializeField] Image damagedFrame;
    [SerializeField] GameManagerSO gM;
    [SerializeField] InputManagerSO input;
    [SerializeField] GameObject instaKillUI;

    public bool InstaKillEnabled { get => instaKillEnabled; }

    void Start()
    {
        currentHealth = maxHealth; 
    }

    public void ApplyDamage(float damage)
    {
        currentHealth -= damage;

        UpdateDamageUI();

        if (currentHealth <= 0)
        {
            Die();
        }

        if (regenCoroutine != null)
        {
            StopCoroutine(regenCoroutine);
        }

        regenCoroutine = StartCoroutine(StartRegen());
    }


    private IEnumerator StartRegen()
    {
        yield return new WaitForSeconds(damageCooldown);

        while (currentHealth < maxHealth)
        {
            currentHealth += regenerationRate * Time.deltaTime;
            currentHealth = Mathf.Min(currentHealth, maxHealth);
            UpdateDamageUI();
            yield return null; 
        }

        regenCoroutine = null;
    }

    private void UpdateDamageUI()
    {
        Color damagedUI = damagedFrame.color;
        damagedUI.a = 1 - (currentHealth/maxHealth);

        damagedFrame.color = damagedUI;
    }

    private void Die()
    {
        input.Inputs.Gameplay.Disable();
        Camera.main.transform.SetParent(null);
        gM.GameOver();
        AudioManager.Instance.PlaySFX("GameOver");
        Destroy(gameObject);
    }


    Coroutine _instaKill = null;
    public void StartInstaKill(float secs)
    {
        StopInstaKill();
        StartCoroutine(InstaKillRoutine(secs));
    }
    public void StopInstaKill()
    {
        if (_instaKill != null)
            StopCoroutine(_instaKill);

        _instaKill = null;
    }

    private IEnumerator InstaKillRoutine(float secs)
    {
        instaKillEnabled = true;
        instaKillUI.SetActive(true);
        yield return new WaitForSeconds(secs);
        instaKillEnabled = false;
        instaKillUI.SetActive(false);
    }
}
