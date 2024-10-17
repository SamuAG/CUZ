using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasics : MonoBehaviour
{
    [SerializeField] GameManagerSO gM;
    // Start is called before the first frame update
    void Start()
    {
        gM.Player = this.gameObject;
    }

}
