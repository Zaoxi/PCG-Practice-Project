﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponComponents : MonoBehaviour {
    // 무기 모듈
    public Sprite[] modules;

    private Weapon parent;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        parent = GetComponentInParent<Weapon>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = modules[Random.Range(0, modules.Length)];
    }

    void Update()
    {
        transform.eulerAngles = parent.transform.eulerAngles;
    }

    public SpriteRenderer getSpriteRenderer()
    {
        return spriteRenderer;
    }
}
