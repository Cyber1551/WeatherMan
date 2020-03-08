using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    public static GameAssets I;
    public GameObject damagePopupPrefab;
    public List<GameObject> enemies;
    public GameObject lightning;
    private void Awake()
    {
        I = this;
    }

}
