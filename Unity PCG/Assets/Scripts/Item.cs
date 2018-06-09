using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public enum itemType
{
    glove, boot
}

public class Item : MonoBehaviour
{
    public Sprite glove;
    public Sprite boot;

    public itemType type;
    public Color level;
    public int attackMod, defenseMod;

    private SpriteRenderer spriteRenderer;

    public void RandomItem()
    {

    }
}
