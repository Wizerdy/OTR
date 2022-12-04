using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanternFlicker : MonoBehaviour
{
    [SerializeField] private Image lanternLightOn;
    [SerializeField] private float flickerFrequency = 5f;
    [SerializeField] private float flickerDuration = 0.01f;
    private bool isFlickering = true;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Flicker());
    }

    private IEnumerator Flicker() {
        while (isFlickering) {
            yield return new WaitForSeconds(Random.Range(0.001f, flickerFrequency));
            lanternLightOn.enabled = false;
            yield return new WaitForSeconds(flickerDuration);
            lanternLightOn.enabled = true;
        }
    }
}
