using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossBar : MonoBehaviour
{
    [SerializeField] TMP_Text numBar;
    [SerializeField] private int maxNumberOfBar = 10;
    [SerializeField] private Image fillTransition;
    [SerializeField] private Image currentFill;
    [SerializeField] private List<Color> colors = new List<Color>();
    private int numberOfBarLeft;
    // Start is called before the first frame update
    void Start()
    {
        if (numBar && fillTransition && currentFill) {
            numberOfBarLeft = maxNumberOfBar;
            numBar.text = "X" + numberOfBarLeft.ToString();
        }
    }

    public void ReduceNumberOfBar(int numberOfBarToRemove) {
        if (numBar && fillTransition && currentFill) {
            numberOfBarLeft -= numberOfBarToRemove;
            numBar.text = "X" + numberOfBarLeft.ToString();

            currentFill.color = colors[numberOfBarLeft - 1];
            fillTransition.color = colors[numberOfBarLeft - 2];
        }
    }
}
