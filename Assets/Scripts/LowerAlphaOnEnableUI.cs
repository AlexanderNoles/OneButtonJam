using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LowerAlphaOnEnableUI : MonoBehaviour
{
    private Image image;
    private Color cachedColour;
    public bool raiseInsteadOfLower = false;
    public float speed = 1f;

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
        image.color = new Color(cachedColour.r, cachedColour.g, cachedColour.b, Mathf.Lerp(image.color.a, raiseOrLower(), Time.deltaTime * speed));
    }

    private int raiseOrLower()
    {
        if (raiseInsteadOfLower)
        {
            return 1;
        }
        return 0;
    }
}
