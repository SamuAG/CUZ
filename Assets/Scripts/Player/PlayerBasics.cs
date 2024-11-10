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
    private Coroutine regenCoroutine;

    [SerializeField] Image damagedFrame;
    [SerializeField] GameManagerSO gM;
    
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
        Debug.Log("Player has died");
    }

}
