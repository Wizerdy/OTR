using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effect")]
public class ScriptableStatusEffect : ScriptableObject
{
    public string effectName;
    public float amount;
    public float duration;
    public bool isDOT;
    public float DOTfrequency;

    //public GameObject particules;
}
