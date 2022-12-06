using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JouerSon : MonoBehaviour
{
    public void JouerSonByIndex(int index) {
        SoundManager.Instance.PlaySfxByIndex(index);
    }
}
