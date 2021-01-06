using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadMask : MonoBehaviour
{
    public Slider slider;

    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Start()
    {
        canvasGroup.alpha = 1;
    }

    void Update()
    {
        if (canvasGroup.alpha>0)
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 0, 4 * Time.deltaTime);
        }
    }
}
