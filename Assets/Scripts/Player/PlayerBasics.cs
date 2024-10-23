using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasics : MonoBehaviour, Damageable
{

    private float maxHealth = 3f;
    private float regenerationRate = 0.5f;
    private float damageCooldown = 5f;
    private float currentHealth;
    private Coroutine regenCoroutine;

    [SerializeField] GameManagerSO gM;
   

    
    void Start()
    {
        currentHealth = maxHealth; 
    }

    public void ApplyDamage(float damage)
    {  
        currentHealth -= damage;

   
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
            yield return null; 
        }

        regenCoroutine = null;
    }

    private void Die()
    {
        Debug.Log("Player has died");
    }

}
