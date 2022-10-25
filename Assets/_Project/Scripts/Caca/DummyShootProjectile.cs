using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyShootProjectile : MonoBehaviour
{
    [SerializeField] private GameObject _projectile;
    [SerializeField] private float shootRate = 3.0f;

    private float time = 0;

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0) {
            var porjectile = Instantiate(_projectile, transform.position, transform.rotation);
            time = shootRate;
        }
    }
}
