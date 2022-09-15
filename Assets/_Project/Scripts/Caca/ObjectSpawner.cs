using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public void SpawnObj(GameObject obj, float duration) {
        if (duration <= 0) {
            duration = 1;
        }
        GameObject go = Instantiate(obj, transform.position, Quaternion.identity);
        Destroy(go, duration);
    }

    public void SpawnPs(GameObject ps) {
        GameObject go = Instantiate(ps, transform.position, Quaternion.identity);
        Destroy(go, go.GetComponent<ParticleSystem>().main.duration + 0.1f);
    }
}
