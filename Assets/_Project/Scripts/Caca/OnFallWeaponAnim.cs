using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnFallWeaponAnim : MonoBehaviour
{
    public void ResetOrientation(GameObject sender) {
        sender.transform.rotation = Quaternion.identity;
    }

    private void SetAnimBool(GameObject sender, string animName ,bool goIdle) {
        Animator anim = sender.GetComponent<Animator>();
        anim.SetBool(animName, goIdle);
    }

    public void ActiveAnim(GameObject sender) {
        SetAnimBool(sender, "Idle", true);
    }

    public void DesactiveAnim(GameObject sender) {
        SetAnimBool(sender, "Idle", false);
    }

    public void ActivateGameObj(GameObject sender) {
        sender.SetActive(true);
    }

    public void DesactivateGameObj(GameObject sender) {
        sender.SetActive(false);
    }
}
