using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CanvasManager : MonoBehaviour
{
    #region Singleton Pattern
    /*
     * Singleton pattern
     */
    private static CanvasManager _instance = null;
    public static CanvasManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogWarning("CanvasManager has not been instanced in the scene!");

            return _instance;
        }
    }

    private bool InitializeSingleton()
    {
        if (_instance == null)
        {
            _instance = this;
            return true;
        }

        Debug.LogWarning("There is already an instance of CanvasManager, this game object will be removed from the scene automatically.");
        Destroy(gameObject);
        return false;
    }
    #endregion

    [SerializeField] private GameManagerSO gM;
    [SerializeField] private GameObject mainInterface;
    [SerializeField] private TMP_Text grenadeTxt, magazineAmmoTxt, currentAmmoTxt, pointsTxt, roundTxt;

    public TMP_Text GrenadeTMP { get =>  grenadeTxt; }
    public TMP_Text MagazineAmmoTxt { get => magazineAmmoTxt; }
    public TMP_Text CurrentAmmoTxt { get => currentAmmoTxt; }
    public TMP_Text PointsTxt { get => PointsTxt; }
    public TMP_Text RoundTxt { get => RoundTxt; }

    private void Awake()
    {
        InitializeSingleton();

        gM.OnUpdateRounds += (value) => roundTxt.text = "" + value;
        gM.OnUpdatePoints += (value) => pointsTxt.text = "" + value;
        gM.OnGameOver += HandleGameOver;
    }

    private void HandleGameOver()
    {
        HideHUD();
        StartCoroutine(WaitAndLoadScene());
    }

    private void HideHUD()
    {
        foreach (Transform child in mainInterface.transform)
        {
            if (child.name != "HealthBlood") 
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator WaitAndLoadScene()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(0);
    }

    private void Update()
    {

    }


}
