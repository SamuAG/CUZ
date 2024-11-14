using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private PowerUpType powerUpType;
    [SerializeField] private GameManagerSO gM;
    [SerializeField] private float seconds;

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
        WeaponHolder wh = gM.Player.GetComponent<WeaponHolder>();
        for (int i = 0; i < wh.CurrentWeapons.Count; ++i)
        {
            Weapon w = wh.CurrentWeapons[i].GetComponent<Weapon>();
            wh.RefillAmmo(w.WeaponData.weaponName);
        }

        Debug.Log("Max Ammo activated!");
    }

    private void InstaKill()
    {
        gM.Player.StartInstaKill(seconds);
    }
}
