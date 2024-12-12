using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class HintManager : MonoBehaviour
{
    public static HintManager instance { get; private set; }

    [SerializeField] private GameObject hintBox;
    [SerializeField] private TextMeshProUGUI hintText;

    private List<string> hintsGiven;


    private Animator _animator;


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
        _animator = hintBox.GetComponent<Animator>();
        hintsGiven = new List<string>();
    }

    public void DisplayHint(string message)
    {
        if (!hintsGiven.Contains(message))
        {
            hintText.text = message;
            hintsGiven.Add(message);
            _animator.Play("Show Hint");
        }
    }

  
}
