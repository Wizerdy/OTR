using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IHealth {
    bool CanTakeDamage { get; set; }
    int CurrentHealth { get; set; }
    event UnityAction<int> OnHit;
    event UnityAction<int> OnHeal;
    event UnityAction OnDeath;

    void TakeDamage(int damage, string damageType = "");
    void TakeHeal(int damage);
    void Die();
}
