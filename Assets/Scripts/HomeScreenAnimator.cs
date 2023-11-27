using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HomeScreenAnimator : MonoBehaviour
{
    [SerializeField] GameObject titulo, prota, characters,button,logo,logotext,logoAE;


    void Start()
    {
        LeanTween.moveLocalY(characters, 0.2f, 6).setEaseOutQuart();
        LeanTween.moveLocalY(prota, 0, 6).setEaseOutQuart().setOnComplete(TitleAlpha);
        


    }

    private void TitleAlpha()
    {
        Image r = titulo.GetComponent<Image>();
        LeanTween.value(gameObject, 0, 1, 1).setOnUpdate((float val) =>
        {
            Color c = r.color;
            c.a = val;
            r.color = c;
        }).setOnComplete(ButtonAlpha);
    }
    private void ButtonAlpha()
    {
        Image r = button.GetComponent<Image>();
        TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();

        LeanTween.value(gameObject, 0, 1, 1).setOnUpdate((float val) =>
        {
            Color c = r.color;
            c.a = val;
            r.color = c;
        });

        LeanTween.value(gameObject, 0, 1, 1).setOnUpdate((float val) =>
        {
            Color c2 = buttonText.color;
            c2.a = val;
            buttonText.color = c2;
        }).setOnComplete(LogoAlpha);
    }

    private void LogoAlpha()
    {
        Image r = logo.GetComponent<Image>();
        Image r2 = logoAE.GetComponent<Image>();
        TextMeshProUGUI logoTextComp = logotext.GetComponentInChildren<TextMeshProUGUI>();

        LeanTween.value(gameObject, 0, 1, 1).setOnUpdate((float val) =>
        {
            Color c = r.color;
            Color c2 = logoTextComp.color;
            Color c3 = r2.color;
            c.a = val;
            c2.a = val;
            c3.a = val;
            r.color = c;
            logoTextComp.color = c2;
            r2.color = c3;  
            
        });

    }

}
