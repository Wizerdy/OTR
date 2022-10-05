using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakPointManager : MonoBehaviour
{
    [SerializeField] GameObject weakPoint;
    private GameObject weakPointInstance;

    public void InstantiateWeakPoint(Transform parent, Vector3 pos) {
        if (weakPointInstance != null) {
            Destroy(weakPointInstance);
        }
        weakPointInstance = Instantiate(weakPoint, pos, Quaternion.identity, parent);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
