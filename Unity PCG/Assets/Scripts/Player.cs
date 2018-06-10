using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Player : MovingObject
{
    public static bool isFacingRight;

	public int wallDamage = 1;
	public Text healthText;
	private Animator animator;
	private int health;
    // 플레이어의 현재 위치를 저장하는 static 변수
    public static Vector2 position;
    // 플레이어가 존재하는 곳을 표시하는 플래그
    public bool onWorldBoard;
    // 플레이어가 던전이나 월드로 이동중일때 true
    public bool dungeonTransition;

    public Image glove;
    public Image boot;

    public int attackMod = 0, defenseMod = 0;
    private Dictionary<string, Item> inventory;

    private Weapon weapon;
    public Image weaponComp1, weaponComp2, weaponComp3;
	
	protected override void Start ()
	{
		animator = GetComponent<Animator>();
		
		health = GameManager.instance.healthPoints;
		
		healthText.text = "Health: " + health;

        position.x = position.y = 2;

        onWorldBoard = true;
        dungeonTransition = false;

        inventory = new Dictionary<string, Item>();

		base.Start ();
	}
	
	private void Update ()
	{
        if (!GameManager.instance.playersTurn) return;

        int horizontal = 0;
		int vertical = 0;

        bool canMove = false;

		horizontal = (int) (Input.GetAxisRaw ("Horizontal"));
		
		vertical = (int) (Input.GetAxisRaw ("Vertical"));
		
		if(horizontal != 0)
		{
			vertical = 0;
		}

		if(horizontal != 0 || vertical != 0)
		{
            if (!dungeonTransition)
            {
                if(onWorldBoard)
                {
                    canMove = AttemptMove<Wall>(horizontal, vertical);
                }
                else
                {
                    canMove = AttemptMove<Chest>(horizontal, vertical);
                }
                if (canMove && onWorldBoard)
                {
                    position.x += horizontal;
                    position.y += vertical;
                    GameManager.instance.updateBoard(horizontal, vertical);
                }
            }
		}
	}
	
	protected override bool AttemptMove <T> (int xDir, int yDir)
	{	
		bool hit = base.AttemptMove <T> (xDir, yDir);
		
		GameManager.instance.playersTurn = false;

        if(xDir == 1 && !isFacingRight)
        {
            isFacingRight = true;
        }
        else if(xDir == -1 && isFacingRight)
        {
            isFacingRight = false;
        }

		return hit;
	}
	
	
	protected override void OnCantMove <T> (T component)
	{
        // 벽인 경우
        if(typeof(T) == typeof(Wall))
        {
            Wall blockingObj = component as Wall;
            blockingObj.DamageWall(wallDamage);
        }
        else if(typeof(T) == typeof(Chest))
        {
            Chest blockingObj = component as Chest;
            blockingObj.Open();
        }
		
		//Set the attack trigger of the player's animation controller in order to play the player's attack animation.
		animator.SetTrigger ("playerChop");
        // 무기 애니메이션 호출
        if(weapon)
        {
            weapon.useWeapon();
        }
	}
	
	//LoseHealth is called when an enemy attacks the player.
	//It takes a parameter loss which specifies how many points to lose.
	public void LoseHealth (int loss)
	{
		//Set the trigger for the player animator to transition to the playerHit animation.
		animator.SetTrigger ("playerHit");
		
		//Subtract lost health points from the players total.
		health -= loss;
		
		//Update the health display with the new total.
		healthText.text = "-"+ loss + " Health: " + health;
		
		//Check to see if game has ended.
		CheckIfGameOver ();
	}
	
	
	//CheckIfGameOver checks if the player is out of health points and if so, ends the game.
	private void CheckIfGameOver ()
	{
		//Check if health point total is less than or equal to zero.
		if (health <= 0) 
		{	
			//Call the GameOver function of GameManager.
			GameManager.instance.GameOver ();
		}
	}

    private void GoDungeonPortal()
    {
        // 포탈 진입시 현재 플레이어가 있는 곳을 기준으로 어디로 이동할지 판단.
        if (onWorldBoard)
        {
            onWorldBoard = false;
            GameManager.instance.enterDungeon();
            transform.position = DungeonManager.startPos;
        }
        else
        {
            onWorldBoard = true;
            GameManager.instance.exitDungeon();
            transform.position = position;

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Exit")
        {
            dungeonTransition = true;
            // 0.5초 뒤 GoDungeonPortal 메소드 호출
            Invoke("GoDungeonPortal", 0.5f);
            Destroy(other.gameObject);
        }
        else if(other.tag == "Food" || other.tag == "Soda")
        {
            UpdateHealth(other);
            Destroy(other.gameObject);
        }
        else if(other.tag == "Item")
        {
            UpdateInventory(other);
            Destroy(other.gameObject);
        }
        else if(other.tag == "Weapon")
        {
            if(weapon)
            {
                Destroy(transform.GetChild(0).gameObject);
            }
            // Collider 기능 off
            other.enabled = false;
            other.transform.parent = transform;
            weapon = other.GetComponent<Weapon>();
            weapon.AquireWeapon();
            weapon.inPlayerInventory = true;
            weapon.enableSpriteRender(false);
            wallDamage = attackMod + 3;
            weaponComp1.sprite = weapon.getComponentImage(0);
            weaponComp2.sprite = weapon.getComponentImage(1);
            weaponComp3.sprite = weapon.getComponentImage(2);
        }
    }

    private void UpdateHealth(Collider2D item)
    {
        if(health < 100)
        {
            if (item.tag == "Food")
            {
                health += Random.Range(1, 4);
            }
            else
            {
                health += Random.Range(4, 11);
            }
            GameManager.instance.healthPoints = health;
            healthText.text = "Health: " + health;
        }
    }

    private void UpdateInventory(Collider2D item)
    {
        Item itemData = item.GetComponent<Item>();
        switch(itemData.type)
        {
            case itemType.glove:
                if (!inventory.ContainsKey("glove"))
                {
                    inventory.Add("glove", itemData);
                }
                else
                {
                    inventory["glove"] = itemData;
                }

                glove.color = itemData.level;
                break;
            case itemType.boot:
                if(!inventory.ContainsKey("boot"))
                {
                    inventory.Add("boot", itemData);
                }
                else
                {
                    inventory["boot"] = itemData;
                }

                boot.color = itemData.level;
                break;
        }

        attackMod = 0;
        defenseMod = 0;

        foreach(KeyValuePair<string, Item> gear in inventory)
        {
            attackMod += gear.Value.attackMod;
            defenseMod += gear.Value.defenseMod;
        }

        if(weapon)
        {
            wallDamage = attackMod + 3;
        }
    }
}

