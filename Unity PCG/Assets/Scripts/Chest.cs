using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{

    public Sprite openSprite;
    //public Item randomItem;
    private SpriteRenderer spriteRenderer;
    public Item randomItem;
    public Weapon weapon;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Open()
    {
        spriteRenderer.sprite = openSprite;

        GameObject toInstantiate;

        if(Random.Range(0, 2) == 1)
        {// 아이템을 생성하는 경우
            randomItem.RandomItem();
            toInstantiate = randomItem.gameObject;
        }
        else
        {// 무기를 생성하는 경우
            toInstantiate = weapon.gameObject;
        }
        // 무기 또는 아이템을 생성 후, Chest를 열린 스프라이트로 변경
        GameObject instance = Instantiate(toInstantiate, new Vector3(transform.position.x, transform.position.y), Quaternion.identity) as GameObject;
        instance.transform.SetParent(transform.parent);
        gameObject.layer = 10;
        spriteRenderer.sortingLayerName = "Items";
    }
}
