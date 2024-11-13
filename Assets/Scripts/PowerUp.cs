using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private PowerUpType powerUpType;

    private enum PowerUpType { MaxAmmo, InstaKill }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ActivatePowerUp();
            Destroy(gameObject);
        }
    }

    private void ActivatePowerUp()
    {
        switch (powerUpType)
        {
            case PowerUpType.MaxAmmo:
                MaxAmmo();
                break;
            case PowerUpType.InstaKill:
                InstaKill();
                break;
        }
    }

    private void MaxAmmo()
    {
        Debug.Log("Max Ammo activated!");
    }

    private void InstaKill()
    {
        Debug.Log("Insta Kill activated!");
    }
}
