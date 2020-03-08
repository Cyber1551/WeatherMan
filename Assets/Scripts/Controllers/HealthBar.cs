using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private const float DAMAGED_FADE_TIMER_MAX = 0.75f;
    [SerializeField] private Image foreground;
    [SerializeField] private Image DamagedBar;
    [SerializeField] private Text healthText;
    private float damageFadeTimer;
    private Color damagedColor;
    [SerializeField] private float positionOffset;
    [SerializeField] private float updateSpeedSeconds = 0.2f;

    private void Awake()
    {
        damagedColor = DamagedBar.color;
        damagedColor.a = 0f;
        DamagedBar.color = damagedColor;

        
    }
    public void SetText(int hp)
    {
        healthText.text = "" + hp;
    }
    public  void HandleHealthChanged(int Hp, int MaxHealth, int amount, bool isCrit, bool isHeal)
    {

        float hpPercent = (float)Hp / (float)MaxHealth;
        Vector3 randomPos = new Vector2(Random.Range(-0.3f, 0.3f), Random.Range(0.5f, 0.75f));
        DamagePopup.Create(transform.position + randomPos, transform.parent, amount, isCrit, isHeal);  
        if ( damagedColor.a <= 0)
        {
            DamagedBar.fillAmount = foreground.fillAmount;
        }

        damagedColor.a = 0.7f;
        DamagedBar.color = damagedColor;
        damageFadeTimer = DAMAGED_FADE_TIMER_MAX;
        healthText.text = "" + Hp;
        StartCoroutine(ChangeToPct(hpPercent));
    }
    private IEnumerator ChangeToPct(float pc)
    {
       
        float preChangePct = foreground.fillAmount;
        float elapsed = 0f;
        while (elapsed < updateSpeedSeconds)
        {
            elapsed += Time.deltaTime;
            foreground.fillAmount = Mathf.Lerp(preChangePct, pc, elapsed / updateSpeedSeconds);
            yield return null;  
        }
        foreground.fillAmount = pc;
    }
    private void Update()
    {
      if ( damagedColor.a > 0)
        {
            damageFadeTimer -= Time.deltaTime; 
            if (damageFadeTimer < 0)
            {   
                float fadeAmount = 5f;
                damagedColor.a -= fadeAmount * Time.deltaTime;
                DamagedBar.color = damagedColor;
            }
        }

    }

}
