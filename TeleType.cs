using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TeleType : MonoBehaviour
{
    private enum States
    {
        ShowText,
        Delay,
        End
    }

    private int currentText = 0;
    private int currentTextPosition = 0;
    private States currentState;
    private float passedTime = 0.0f;

    public List<TextDisplayConfig> Configuration = new List<TextDisplayConfig>();

    private TMP_Text textMeshPro;

    void Awake()
    {
        textMeshPro = GetComponent<TMP_Text>();
        textMeshPro.enableWordWrapping = true;
        textMeshPro.alignment = TextAlignmentOptions.Top;
    }

    public void Start()
    {
        currentText = 0;
        currentTextPosition = 0;
        passedTime = 0;
        currentState = States.ShowText;
        textMeshPro.ForceMeshUpdate();
    }

    public void Update()
    {
        if (currentText > Configuration.Count)
            return;

        bool stateChanged = false;

        switch (currentState)
        {
            case States.ShowText:
                if (passedTime == 0.0f)
                    break;

                string text = Configuration[currentText].Text;
                float secondsForEachChar = Configuration[currentText].TimeToShowText / text.Length;
                int charsToShow = (int)(passedTime / secondsForEachChar);
                charsToShow = Math.Min(charsToShow, text.Length);

                textMeshPro.text = text.Substring(0, charsToShow);

                if (charsToShow == text.Length)
                {
                    currentState = States.Delay;
                    stateChanged = true;
                }

                break;
            case States.Delay:
                if (passedTime > Configuration[currentText].DelayAfterText)
                {
                    currentText++;
                    currentState = currentText >= Configuration.Count ? States.End : States.ShowText;
                    stateChanged = true;
                }
                break;
            case States.End:
                textMeshPro.text = string.Empty;
                break;
        }

        if (stateChanged)
        {
            passedTime = 0;
        }
        else if(currentState != States.End)
        {
            passedTime += Time.deltaTime;
        }
    }

}