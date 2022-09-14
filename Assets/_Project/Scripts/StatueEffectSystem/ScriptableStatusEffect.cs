using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effect")]
public class ScriptableStatusEffect : ScriptableObject
{
    public string name;
    public float ammount;
    public float duration;

    //public GameObject particules;
}
