using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    HealthBar health;
    public int MaxHp;
    public int Hp;
    // Start is called before the first frame update
    void Start()
    {
        Hp = MaxHp;
        health = transform.GetChild(1).GetChild(0).GetComponent<HealthBar>();
        health.SetText(Hp);
    }

    public void DealDamage(int dmg, bool isCrit)
    {
        Hp -= dmg;
        
        health.HandleHealthChanged(Hp, MaxHp, dmg, isCrit, false);
        if (Hp <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Attack"))
        {
            PlayerController pc = GameObject.Find("Player").GetComponent<PlayerController>();
            int crit = pc.baseCritChance + (pc.MaxPowerLevel - pc.PowerLevel) * 10;
            int dmg = 35;
            int rnd = Random.Range(0, 99);
            bool isCrit = false;
            if (rnd <= crit)
            {
                Instantiate(GameAssets.I.lightning, transform.GetChild(0));
                
                isCrit = true;
                dmg *= 2;
            }
            DealDamage(dmg, isCrit);
        }
        Destroy(collision.gameObject);
    }
}
