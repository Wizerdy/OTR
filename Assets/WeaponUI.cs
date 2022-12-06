using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ToolsBoxEngine;

public class WeaponUI : MonoBehaviour {
    [SerializeField] EntityWeaponryReference _entityWeaponryReference;
    [SerializeField] Slider slider;
    [SerializeField] GameObject fist;
    [SerializeField] GameObject shield;
    [SerializeField] GameObject lyre;
    [SerializeField] GameObject trapped;
    [SerializeField] EntityAbilities ability;
    EntityArmor entityArmor;
    EntityStorePoint entityStorePoint;
    int dirty = 0;
    private void Start() {
        entityArmor = ability.Get<EntityArmor>();
        entityStorePoint = ability.Get<EntityStorePoint>();
        trapped.gameObject.SetActive(false);
        NoWeapon(null);
        StartCoroutine(Tools.DelayOneFrame(() => _entityWeaponryReference.Instance.OnPickup += ChangeWeapon));
        StartCoroutine(Tools.DelayOneFrame(() => _entityWeaponryReference.Instance.OnDrop += NoWeapon));
    }

    private void Update() {
        if (dirty == 1) {
            slider.value = entityStorePoint.CurrentValue;
            slider.maxValue = entityStorePoint.MaxValue;
        } else if (dirty == 2) {
            //Debug.Log(entityArmor.CurrentArmor + " .. " + slider.value);
            slider.value = entityArmor.CurrentArmor;
        }
    }

    void ChangeWeapon(Weapon weapon) {
        switch (weapon.Type) {
            case WeaponType.BLOODFIST:
                slider.gameObject.SetActive(true);
                dirty = 1;
                fist.SetActive(true);
                shield.SetActive(false);
                lyre.SetActive(false);
                break;
            case WeaponType.SHIELDAXE:
                slider.gameObject.SetActive(true);
                fist.SetActive(false);
                shield.SetActive(true);
                lyre.SetActive(false);
                dirty = 2;
                slider.maxValue = ((AxeShield)weapon).entityArmor.MaxArmor;
                break;
            case WeaponType.CROSSGUN:
                fist.SetActive(false);
                shield.SetActive(false);
                lyre.SetActive(true);
                break;
        }
    }

    void NoWeapon(Weapon _) {
        fist.SetActive(false);
        shield.SetActive(false);
        lyre.SetActive(false);
        slider.gameObject.SetActive(false);
        dirty = 0;
    }
}
