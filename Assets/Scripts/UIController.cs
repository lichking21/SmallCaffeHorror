using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Text textHint;

    void Start()
    {
        HideHint();
    }

    public void ShowHint(string text)
    {
        textHint.text = text;
        textHint.enabled = true;
    }

    public void HideHint()
    {
        textHint.enabled = false;
    }
}
