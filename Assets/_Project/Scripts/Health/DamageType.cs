using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageType : MonoBehaviour {
    [SerializeField] string _damageType;

    public string Type => _damageType;
}
