using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ToolsBoxEngine;

public class WeaponUI : MonoBehaviour {
    [SerializeField] EntityWeaponryReference _entityWeaponryReference;
    [SerializeField] Image weaponImage;
    [SerializeField] Slider slider;
    [SerializeField] Sprite fist;
    [SerializeField] Sprite shield;
    [SerializeField] Sprite instrument;
    [SerializeField] EntityArmor entityArmor;
    [SerializeField] EntityStorePoint entityStorePoint;
    int dirty = 0;
    private void Start() {
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
        weaponImage.gameObject.SetActive(true);
        switch (weapon.Type) {
            case WeaponType.BLOODFIST:
                slider.gameObject.SetActive(true);
                dirty = 1;
                weaponImage.sprite = fist;
                break;
            case WeaponType.SHIELDAXE:
                slider.gameObject.SetActive(true);
                weaponImage.sprite = shield;
                dirty = 2;
                slider.maxValue = ((AxeShield)weapon).entityArmor.MaxArmor;
                break;
            case WeaponType.CROSSGUN:
                weaponImage.sprite = instrument;
                break;
        }
    }

    void NoWeapon(Weapon _) {
        weaponImage.sprite = null;
        weaponImage.gameObject.SetActive(false);
        slider.gameObject.SetActive(false);
        dirty = 0;
    }
}
