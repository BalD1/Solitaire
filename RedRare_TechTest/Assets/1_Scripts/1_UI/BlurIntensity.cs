using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class BlurIntensity : MonoBehaviour
{
    [SerializeField] private Image image;

    [SerializeField] private float fadeTime = .25f;

    private int intensityId;

    private void Reset()
    {
        image = this.GetComponent<Image>();
    }

    private void Start()
    {
        intensityId = Shader.PropertyToID("_Intensity");
    }

    public void Show()
    {
        LeanTween.value(0, 1, fadeTime).setOnUpdate((float val) =>
        {
            image.material.SetFloat(intensityId, val);
        });
    }

    public void Hide()
    {
        LeanTween.value(1,0, fadeTime).setOnUpdate((float val) =>
        {
            image.material.SetFloat(intensityId, val);
        });
    }
}
