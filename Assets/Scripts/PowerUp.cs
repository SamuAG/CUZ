using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private PowerUpType powerUpType;
    [SerializeField] private GameManagerSO gM;
    [SerializeField] private float seconds;
    [SerializeField] private float rotationSpeed = 100f; 
    [SerializeField] private GameObject visualModel; 

    private enum PowerUpType { MaxAmmo, InstaKill }
    private bool isFlashing = false; 

    private void Start()
    {
        Destroy(gameObject, 15f);

        StartCoroutine(BlinkDestruction());
    }

    private void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    private IEnumerator BlinkDestruction()
    {
        yield return new WaitForSeconds(10f);

        isFlashing = true;

        float flashInterval = 0.2f; 
        while (isFlashing && visualModel != null)
        {
            visualModel.SetActive(!visualModel.activeSelf);
            yield return new WaitForSeconds(flashInterval);
        }
    }

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
        AudioManager.Instance.PlaySFX("MaxAmmo");
        WeaponHolder wh = gM.Player.GetComponent<WeaponHolder>();
        for (int i = 0; i < wh.CurrentWeapons.Count; ++i)
        {
            Weapon w = wh.CurrentWeapons[i].GetComponent<Weapon>();
            wh.RefillAmmo(w.WeaponData.weaponName, false);
        }
    }

    private void InstaKill()
    {
        AudioManager.Instance.PlaySFX("InstaKill");
        gM.Player.StartInstaKill(seconds);
    }

    private void OnDestroy()
    {
        isFlashing = false;
        if (visualModel != null) visualModel.SetActive(true); 
    }
}
