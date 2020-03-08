using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum WeaponStance
{
    Ranged = 1,
    Melee = 0
}

public class PlayerController : MonoBehaviour
{
    Animator anim;
    public float speed;
    float moveVelocity;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float jump;
    private bool IsGrounded;
    private float asTimer = float.MaxValue;
    [SerializeField] private AnimationClip meleeAttackClip;
    bool IsAttacking;
    SpriteRenderer sr;
    Rigidbody2D rb;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private AnimatorOverrideController rangedController;
    [SerializeField] private RuntimeAnimatorController meleeController;
    [SerializeField] private WeaponStance weapon = WeaponStance.Melee;
    [SerializeField] private GameObject muzzleFlash;
    [SerializeField] private Transform[] bulletSpawn;
    [SerializeField] private GameObject Bullet;
    [SerializeField] private Image PowerLevelBar;
    [SerializeField] private Text PowerLevelText;
    [SerializeField] private float radius;
    [SerializeField] private GameObject windObj;
    [SerializeField] private TextMeshProUGUI stats;
    public int baseCritChance = 20;
    public int PowerLevel = 0;
    public int MaxPowerLevel = 8;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent <Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        attackSpeed = (meleeAttackClip.length / 0.6f) + 0.5f;
        anim = GetComponent<Animator>();
        UpdatePowerLevelUI();
    }

 
    // Update is called once per frame
    void Update()
    {
        asTimer += Time.deltaTime;
        if (Input.GetKey(KeyCode.P))
        {
            if(asTimer >= attackSpeed)
            {
                
                if (weapon == WeaponStance.Melee)
                {
                    anim.Play("AttackMelee");

                    IsAttacking = true;
                }
                else
                {
                    RangedAttack();
                    DescreasePL();
                }

                asTimer = 0;
            }

        }
        UpdateStats();
        if (Input.GetKeyUp(KeyCode.Space) && IsGrounded)
        {
            rb.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
            IsGrounded = false;
        }
        moveVelocity = 0;
        if (IsAttacking)
        {
            rb.velocity = Vector3.zero;
        }
        else
        {
            
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                moveVelocity = -speed;
                sr.flipX = true;

            }
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                moveVelocity = speed;
                sr.flipX = false;
            }
            anim.SetBool("IsRunning", moveVelocity != 0);
            rb.velocity = new Vector2(moveVelocity, GetComponent<Rigidbody2D>().velocity.y);
        }

        
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            IsGrounded = true;
        }

    }
    private void SwitchWeapon(WeaponStance stance)
    {
        weapon = stance;
        if (weapon == WeaponStance.Melee) anim.runtimeAnimatorController = meleeController;
        else anim.runtimeAnimatorController = rangedController;

    }
    private void UpdatePowerLevelUI()
    {
        PowerLevelText.text = ""+PowerLevel;
        PowerLevelBar.fillAmount = (float)PowerLevel / (float)MaxPowerLevel;
    }
    private void IncreasePL()
    {
        PowerLevel = Mathf.Min(MaxPowerLevel, PowerLevel + 1);
        if (PowerLevel == MaxPowerLevel) SwitchWeapon(WeaponStance.Ranged);
        UpdatePowerLevelUI();
    }
    private void DescreasePL()
    {
        PowerLevel = Mathf.Max(0, PowerLevel - 1);
        if (PowerLevel == 0) SwitchWeapon(WeaponStance.Melee);
        UpdatePowerLevelUI();
    }
    public void MeleeAttack()
    {
        Collider2D[] damage = Physics2D.OverlapCircleAll(bulletSpawn[sr.flipX ? 1 : 0].position, radius, LayerMask.GetMask("Enemy"));

        for (int i = 0; i < damage.Length; i++)
        {
            Enemy e = damage[i].GetComponent<Enemy>();
            if (e != null)
            {
                GameObject wnd = Instantiate(windObj, bulletSpawn[sr.flipX ? 1 : 0].position, Quaternion.identity);
                wnd.GetComponent<SpriteRenderer>().flipX = sr.flipX;
                e.GetComponent<Rigidbody2D>().AddForce((sr.flipX ? -1 : 1) * Vector2.right * 100);
                int dmg = 3 + (4 * PowerLevel);
                int rnd = Random.Range(0, 99);
                bool isCrit = false;
                
                if(rnd <= baseCritChance)
                {
                    Instantiate(GameAssets.I.lightning, e.transform.GetChild(0));
                    dmg *= 2;
                    isCrit = true;
                }
                e.DealDamage(dmg, isCrit);
                IncreasePL();
            }
        }
        IsAttacking = false;   
    }
    public void UpdateStats()
    {
        int dmg = (weapon == WeaponStance.Melee) ? 3 + (4 * PowerLevel) : 35;
        int critChance = 20 + ((weapon == WeaponStance.Melee) ? 0 : (MaxPowerLevel - PowerLevel) * 10);
        stats.text = "Damage: " + dmg + "\n" + "CritChance: " + critChance + "%";
    }
    public void RangedAttack()
    {
        StartCoroutine(Shoot());
        GameObject go = Instantiate(muzzleFlash, bulletSpawn[sr.flipX ? 1 : 0]);
        go.GetComponent<SpriteRenderer>().flipX = sr.flipX;
        Destroy(go, 0.05f);
    }
    IEnumerator Shoot()
    {
        GameObject b = Instantiate(Bullet, bulletSpawn[sr.flipX ? 1 : 0].position, bulletSpawn[sr.flipX ? 1 : 0].rotation);
        b.GetComponent<Rigidbody2D>().AddForce((sr.flipX ? -1 : 1) * Vector2.right * bulletSpeed);
        yield return new WaitForSeconds(attackSpeed);
        
    }
    private void OnDrawGizmosSelected()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(bulletSpawn[sr.flipX ? 1 : 0].position, radius);
    }
}
