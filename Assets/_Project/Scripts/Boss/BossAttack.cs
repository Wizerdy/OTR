using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class  BossAttack : MonoBehaviour {
    [SerializeField] protected float weight;
    public float Weight => weight;

    public abstract void Activate(EntityAbilities ea, Transform target);

    public abstract void Activate(EntityAbilities ea, Transform[] targets);
}
