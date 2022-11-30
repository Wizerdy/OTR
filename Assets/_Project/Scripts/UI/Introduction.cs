using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Introduction : MonoBehaviour {
    [SerializeField] private Image studioLogo;
    [SerializeField] private Image introBackground;
    // Start is called before the first frame update
    void Start() {
        StartCoroutine(PlayIntroduction());
    }

    private void Intro() {
    }
    private IEnumerator PlayIntroduction() {
        StartCoroutine(FadeImage(studioLogo, false));

        yield return new WaitForSeconds(2f);
        StartCoroutine(FadeImage(studioLogo, true)); 
        yield return new WaitForSeconds(1f);

        StartCoroutine(FadeImage(introBackground, true));


        yield return null;
    }

    IEnumerator FadeImage(Image img, bool fadeAway) {
        // fade from opaque to transparent
        if (fadeAway) {
            // loop over 1 second backwards
            for (float i = 1; i >= 0; i -= Time.deltaTime) {
                // set color with i as alpha
                img.color = new Color(img.color.r, img.color.g, img.color.b, i);
                yield return null;
            }
        }
        // fade from transparent to opaque
        else {
            // loop over 1 second
            for (float i = 0; i <= 1; i += Time.deltaTime) {
                // set color with i as alpha
                img.color = new Color(img.color.r, img.color.g, img.color.b, i);
                yield return null;
            }
        }
    }
}
