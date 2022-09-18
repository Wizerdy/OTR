using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffProjectile : MonoBehaviour
{
    [SerializeField] private ScriptableStatusEffect buff;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        //var target = collision.GetComponent<IBuffable>();
        if (collision.tag == "Player") 
            //target.ApplyBuff(buff);

        Destroy(this);
    }
}
