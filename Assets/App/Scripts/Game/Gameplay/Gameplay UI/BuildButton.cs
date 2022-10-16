using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BuildButton : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] Image image;
    int index;

    public void Init(int index, Sprite sprite, Action<int> onClick)
    {
        this.index = index;
        image.sprite = sprite;
        button.onClick.AddListener(() => onClick?.Invoke(index));
    }
}
