using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class LevelButton : MonoBehaviour
{
    [SerializeField]
    Button button;

    int index;

    public void Init(int index, Action<int> onClick)
    {
        this.index = index;
        button.onClick.AddListener(() => onClick?.Invoke(index));
    }
}
