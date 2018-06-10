using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    // 현재 무기가 플레이어의 인벤토리 안에 있는지
    public bool inPlayerInventory = false;

    private Player player;
    private WeaponComponents[] weaponsComps;
    private bool weaponUsed = false;
    // 무기를 얻었을 때 호출
    public void AquireWeapon()
    {
        player = GetComponentInParent<Player>();
        weaponsComps = GetComponentsInChildren<WeaponComponents>();
    }

    void Update()
    {
        if(inPlayerInventory)
        {
            transform.position = player.transform.position;
            if(weaponUsed == true)
            {
                float degreeY = 0, degreeZ = -90f, degreeZMax = 275f;
                Vector3 returnVector = Vector3.zero;

                if(Player.isFacingRight)
                {
                    degreeY = 0;
                    returnVector = Vector3.zero;
                }
                else if(!Player.isFacingRight)
                {
                    degreeY = 180;
                    returnVector = new Vector3(0, 180, 0);
                }

                // Quaternion.Slerp 함수!
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, degreeY, degreeZ), Time.deltaTime * 20f);
                if(transform.eulerAngles.z <= degreeZMax)
                {
                    transform.eulerAngles = returnVector;
                    weaponUsed = false;
                    enableSpriteRender(false);
                }
            }
        }
    }

    public void useWeapon()
    {
        enableSpriteRender(true);
        weaponUsed = true;
    }
    // 무기의 Sprite Enable, Disable 설정
    public void enableSpriteRender(bool isEnabled)
    {
        foreach(WeaponComponents comp in weaponsComps)
        {
            comp.getSpriteRenderer().enabled = isEnabled;
        }
    }

    public Sprite getComponentImage(int index)
    {
        return weaponsComps[index].getSpriteRenderer().sprite;
    }
}
