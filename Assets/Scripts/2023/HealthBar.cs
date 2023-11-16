using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthBar;
    public EnemyControllerSplit Health;

    private void Start()
    {
        //healthBar = GetComponent<Slider>();
        healthBar.maxValue = Health.enemyLife;
        healthBar.value = Health.enemyLife;
    }

    private void Update()
    {
        SetHealth(Health.enemyLife);
    }
    public void SetHealth(int hp)
    {
        healthBar.value = hp;
    }
}
