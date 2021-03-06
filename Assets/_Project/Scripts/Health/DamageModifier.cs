using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ToolsBoxEngine;

public abstract class DamageModifier : MonoBehaviour {
    public enum ResistanceType {
        RESISTANCE,
        WEAKNESS,
        FIX,
        IMMUNITY,
        NOMODIFIER
    }

    [SerializeField] ResistanceType _type;
    [SerializeField] int _value;

    [Space]
    [SerializeField, HideInInspector] BetterEvent<int, GameObject> _onUse = new BetterEvent<int, GameObject>();

    public ResistanceType Resistance { get => _type; set => _type = value; }

    public event UnityAction<int, GameObject> OnUse { add => _onUse += value; remove => _onUse -= value; }

    public int Use(int value, GameObject source) {
        if (!Usable(value, source)) { return value; }

        _onUse.Invoke(value, source);
        return Modify(value);
    }

    protected int Modify(int amount) {
        switch (_type) {
            case ResistanceType.WEAKNESS:
                amount += _value;
                break;
            case ResistanceType.FIX:
                amount = _value;
                break;
            case ResistanceType.RESISTANCE:
                amount = Mathf.Min(0, amount - _value);
                break;
            case ResistanceType.IMMUNITY:
                amount = 0;
                break;
        }
        return amount;
    }

    protected abstract bool Usable(int value, GameObject source);
}
