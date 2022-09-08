using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public float playerHP;
    public float playerStamina;
    public int swordLevel;
    public int spearLevel;
    public int bowLevel;
    public int shieldLevel;

    void Start()
    {
        DontDestroyOnLoad(this);
    }
}
