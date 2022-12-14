using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ToolsBoxEngine;

public class WeaponUI : MonoBehaviour {
    [SerializeField] Slider slider;
    [SerializeField] Image Banner;
    [SerializeField] GameObject fist;
    [SerializeField] GameObject shield;
    [SerializeField] GameObject lyre;
    [SerializeField] GameObject trapped;
    [SerializeField] EntityAbilitiesReference ability;
    EntityArmor entityArmor;
    EntityStorePoint entityStorePoint;
    int dirty = 0;
    private void Start() {
        entityArmor = ability.Instance.Get<EntityArmor>();
        entityStorePoint = ability.Instance.Get<EntityStorePoint>();
        trapped.SetActive(false);
        NoWeapon(null);
        StartCoroutine(Tools.DelayOneFrame(() => ability.Instance.Get<EntityWeaponry>().OnPickup += ChangeWeapon));
        StartCoroutine(Tools.DelayOneFrame(() => ability.Instance.Get<EntityWeaponry>().OnDrop += NoWeapon));
        StartCoroutine(Tools.DelayOneFrame(() => ability.Instance.Get<EntityPhysicMovement>().Trapped += Trapped));
        StartCoroutine(Tools.DelayOneFrame(() => ability.Instance.Get<EntityPhysicMovement>().UnTrapped += UnTrapped));
    }

    private void Update() {
        if (dirty == 1) {
            //slider.value = entityStorePoint.CurrentValue;
            //slider.maxValue = entityStorePoint.MaxValue;
        } else if (dirty == 2) {
            //Debug.Log(entityArmor.CurrentArmor + " .. " + slider.value);
            slider.value = entityArmor.CurrentArmor;
        }
    }

    void ChangeWeapon(Weapon weapon) {
        switch (weapon.Type) {
            case WeaponType.BLOODFIST:
                Banner.color = new Color(1, 1, 1, 1);
                //slider.gameObject.SetActive(true);
                dirty = 1;
                fist?.SetActive(true);
                shield?.SetActive(false);
                lyre?.SetActive(false);
                break;
            case WeaponType.SHIELDAXE:
                Banner.color = new Color(1, 1, 1, 1);
                slider.gameObject.SetActive(true);
                fist?.SetActive(false);
                shield?.SetActive(true);
                lyre?.SetActive(false);
                dirty = 2;
                slider.maxValue = ((AxeShield)weapon).entityArmor.MaxArmor;
                break;
            case WeaponType.CROSSGUN:
                Banner.color = new Color(1, 1, 1, 1);
                fist?.SetActive(false);
                shield?.SetActive(false);
                lyre?.SetActive(true);
                break;
        }
    }

    void NoWeapon(Weapon _) {
        fist.SetActive(false);
        shield.SetActive(false);
        lyre.SetActive(false);
        Banner.color = new Color(1, 1, 1, 0);
        slider.gameObject.SetActive(false);
        dirty = 0;
    }

    void Trapped() {
        trapped.SetActive(true);
    }

    void UnTrapped() {
        trapped.SetActive(false);
    }
}
