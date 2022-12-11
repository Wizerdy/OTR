using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Introduction : MonoBehaviour {
    [SerializeField] private Image audioWorkshopLogo;
    [SerializeField] private Image iimLogo;
    [SerializeField] private Image firstBackGround;
    [SerializeField] private Image studioLogo;
    [SerializeField] private Image studioBackGround;
    // Start is called before the first frame update
    void Start() {
        StartCoroutine(PlayIntroduction());
    }

    private IEnumerator PlayIntroduction() {
        studioLogo.gameObject.SetActive(true);
        studioBackGround.gameObject.SetActive(true);
        audioWorkshopLogo.gameObject.SetActive(true);
        iimLogo.gameObject.SetActive(true);
        firstBackGround.gameObject.SetActive(true);

        StartCoroutine(FadeImage(iimLogo, false));
        StartCoroutine(FadeImage(audioWorkshopLogo, false));
        yield return new WaitForSeconds(2f);

        StartCoroutine(FadeImage(iimLogo, true));
        StartCoroutine(FadeImage(audioWorkshopLogo, true));
        yield return new WaitForSeconds(1f);
        StartCoroutine(FadeImage(firstBackGround, true));


        StartCoroutine(FadeImage(studioLogo, false));

        yield return new WaitForSeconds(2f);
        StartCoroutine(FadeImage(studioLogo, true)); 
        yield return new WaitForSeconds(1f);

        StartCoroutine(FadeImage(studioBackGround, true));


        yield return null;
    }

    IEnumerator FadeImage(Image img, bool fadeAway) {
        if (fadeAway) {
            for (float i = 1; i >= 0; i -= Time.deltaTime) {
                img.color = new Color(img.color.r, img.color.g, img.color.b, i);
                yield return null;
            }
        }
        else {
            for (float i = 0; i <= 1; i += Time.deltaTime) {
                img.color = new Color(img.color.r, img.color.g, img.color.b, i);
                yield return null;
            }
        }
    }
}
