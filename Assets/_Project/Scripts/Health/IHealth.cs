using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IHealth {
    bool CanTakeDamage { get; set; }
    int CurrentHealth { get; }
    bool IsDead { get; }
    GameObject GameObject { get; }

    event UnityAction<int> OnHit;
    event UnityAction<int> OnHeal;
    event UnityAction OnDeath;

    int TakeDamage(int damage, GameObject source);
    void TakeHeal(int damage);
    void Die();
}
