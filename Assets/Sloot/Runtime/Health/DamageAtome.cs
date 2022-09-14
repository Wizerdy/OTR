using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sloot {
    public class DamageAtome : MonoBehaviour {
        [SerializeField] string _damageTypeName;
        [SerializeField] int _value;

        private void OnCollisionEnter(Collision collision) {

        }

        private void OnCollisionEnter2D(Collision2D collision) {

        }
    }
}
