using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuffable
{
    public void ApplyBuff(ScriptableStatusEffect effect);

    public void RemoveBuff(ScriptableStatusEffect effect);

    public void DoEffect(float time, int index);
}
