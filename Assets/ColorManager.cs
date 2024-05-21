using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ColorManager : MonoBehaviour
{
    public static ColorManager i;
    
    public Color startingColor = Color.red;
    public float colorStep = 0.1f;

    private Color currentColor;
    
    private void Awake()
    {
        if (i)
        {
            Destroy(this);
        }
        else
        {
            i = this;
        }
    }

    private void Start()
    {
        currentColor = startingColor;
    }

    public Color NextColor()
    {
        currentColor = ChangeColor(currentColor, colorStep);
        return currentColor;
    }

    private Color ChangeColor(Color color, float step)
    {
        float h, s, v;
        Color.RGBToHSV(color, out h, out s, out v);
        h = (h + step) % 1f;
        return Color.HSVToRGB(h, s, v);
    }

   
}