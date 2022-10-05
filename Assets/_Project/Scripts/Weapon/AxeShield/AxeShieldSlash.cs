using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeShieldSlash :Weapon 
{
    [SerializeField] private AxeShield axeShield;
    private int damage;
    // Start is called before the first frame update
    void Start()
    {
        damage = axeShield.axeShieldSlashDamage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void DealDamage() {
        Debug.Log("test");
    }

    protected override void _OnAttackHit(Collider2D collider) {
        if (collider.tag == "Boss" || collider.tag == "Enemy") {
            collider.gameObject.GetComponent<IHealth>().TakeDamage(damage, gameObject);
        }
    }
}
