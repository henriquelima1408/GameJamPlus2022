using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class LevelButton : MonoBehaviour
{
    [SerializeField]
    Button button;

    [SerializeField]
    Text levelName;

    int index;

    public void Init(int index, Action<int> onClick)
    {
        this.index = index;
        button.onClick.AddListener(() => onClick?.Invoke(index));
        levelName.text = (index + 1).ToString();
    }
}
