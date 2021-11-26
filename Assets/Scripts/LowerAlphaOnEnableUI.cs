using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LowerAlphaOnEnableUI : MonoBehaviour
{
    private Image image;
    private Color cachedColour;

    private void OnEnable()
    {
        if(image == null)
        {
            image = GetComponent<Image>();
            cachedColour = image.color;
        }
        image.color = cachedColour;
    }

    private void Update()
    {
        image.color = new Color(cachedColour.r, cachedColour.g, cachedColour.b, Mathf.Lerp(image.color.a, 0, Time.deltaTime));
    }
}
