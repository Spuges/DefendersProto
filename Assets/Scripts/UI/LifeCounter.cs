using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeCounter : MonoBehaviour
{
    public static LifeCounter I { get; set; }
    public int StartsWithLives = 3;

    public GameObject LifeIcon;
    public Transform UIRootForIcons;

    private int currentLives;

    public void LostLife()
    {
        LifeCount--;
    }

    public int LifeCount
    {
        get { return currentLives; }
        set
        {
            currentLives = value;
            UpdateUI();
        }
    }

    private void Awake()
    {
        I = this;
    }

    public void ResetLives()
    {
        LifeCount = StartsWithLives;
    }

    public void UpdateUI()
    {
        if(UIRootForIcons && LifeIcon)
        {
            int count = (UIRootForIcons.childCount > LifeCount ? UIRootForIcons.childCount : LifeCount);
            for (int i = 0; i < count; i++)
            {
                if (i < UIRootForIcons.childCount)
                    UIRootForIcons.GetChild(i).gameObject.SetActive(LifeCount > i);
                else
                    Instantiate(LifeIcon, UIRootForIcons, false);
            }
        }
    }
}
