using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour {
    [SerializeField] protected float weight;
    public float Weight => weight;

    public virtual void Activate(EntityAbilities ea, Transform target) {

    }

    public virtual void Activate(EntityAbilities ea, Transform[] targets) {

    }
}
