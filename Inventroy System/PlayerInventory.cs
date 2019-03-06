using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerInventory : MonoBehaviour {

	public int[] slotID = new int[9];
	public int[] slotValue = new int[9];
	public string[] slotName = new string[9];
	public string [] slotDesc = new string[9];
	public Sprite[] slotSprite = new Sprite[9];
	public int[] slotType = new int[9];
	public int[] slotStack = new int[9];
	public Image[] slotIcon = new Image[9];
	public Sprite emptySlotSprite;
	public int maxStack = 1;
	public int clickedSlotID;

	//Selected Items UI Refernces
	public int selectedItemID;
	public int selectedItemValue;
	public Text selectedItemName;
	public Text selectedItemDesc;
	public Image selectedItemSprite;
	public int selectedItemType;
	public GameObject SelectMenu;
	public GameObject UseBtn;
	public GameObject EquipBtn;

	//Crafting item variables
	private int stored_item_ID = 0;
	public Text craftedItemName;
	public Text craftedItemDesc;
	public Image craftedItemSprite;
	public GameObject CraftedItemShow;
	public GameObject NotEnoughResources;
	public GameObject NotEnoughCoins;
	public GameObject InventoryIsFull;

	//Cooking item variables
	private int cooked_stored_item_ID = 0;
	public Text cookedItemName;
	public Text cookedItemDesc;
	public Image cookedItemSprite;

	//REEE
	public ParticleSystem test;

	//Reference to how much coins the player has
	public int[] storeItems = new int[3];
	
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            ItemClicked(29);
        }

		//TEST FIND ITEM FUNCTION
		if (Input.GetKeyDown(KeyCode.K)){
			FindItem(8);
		}

		if (Input.GetKeyDown(KeyCode.N)){
			test.Clear();
			test.Play();
		}
    }

	public void ItemClicked(int ID){
		Item item = ItemDatabase.GetItem (ID);
		for(int i = 0; i < slotID.Length; i++)
		{
			if (slotID [i] == item.itemID && slotStack [i] < maxStack) 
			{
				slotStack [i]++;
				break;
			}
			else if (slotID [i] == 0) 
			{
				slotID [i] = item.itemID;
				slotValue [i] = item.itemValue;
				slotName [i] = item.itemName;
				slotDesc [i] = item.itemDesc;
				slotSprite [i] = item.itemSprite;
				slotType [i] = item.itemType;
				slotIcon[i].sprite = item.itemSprite;
				slotStack [i]++;
                EnableItem(ID);
				break;
			}
            else
            {
                //YOUR INVENTORY IS FULL YOU NEED TO DROP SOMETHING
                //TO DO LATER
            }
		}
	}

	/*public void RestoreItems(int[] restoredSlotID, int[] restoredValue, int[] restoredName){
		slotID [indexID] = item.itemID;
		slotValue [indexID] = item.itemValue;
		slotName [indexID] = item.itemName;
		slotDesc [indexID] = item.itemDesc;
		slotSprite [indexID] = item.itemSprite;
		slotType [indexID] = item.itemType;
		slotIcon[indexID].sprite = item.itemSprite;
		slotStack [indexID]++;
		EnableItem(ID);
	}*/
	
	public void PurchaseItem (int ID) //This method allows the player to purchase an item based on value of given item ID
	{
		BarController coinScript = this.gameObject.GetComponent<BarController>();
		if(InventoryFull() == false) {
			Debug.Log(coinScript.GetCoins());
			Item item = ItemDatabase.GetItem (storeItems[ID]);
			Debug.Log(item.itemValue);
			if (coinScript.GetCoins() >= item.itemValue){
				//Subtract the correct amount of coins 
				coinScript.SetCoins(coinScript.GetCoins() - item.itemValue);
				ItemClicked(storeItems[ID]);
				coinScript.InvokeSound(coinScript.craftSound);
				StartCoroutine(coinScript.InvokeMessage("+ITEM!", 1f));
			}
			else 
			{
				//YOU DONT HAVE ENOUGH COINS
				coinScript.InvokeSound(coinScript.errorSound);
				StartCoroutine(coinScript.InvokeMessage("NOT ENOUGH COINS!", 1f));
			}
		}
		else{
			coinScript.InvokeSound(coinScript.errorSound);
			StartCoroutine(coinScript.InvokeMessage("INVENTORY FULL!", 1f));
		}
	}

    public void SlotClicked(int ID)
	{
		if (slotID[ID] != 0) {
			selectedItemID = slotID [ID];
			selectedItemValue = slotValue [ID];
			selectedItemName.text = slotName [ID];
			selectedItemDesc.text = slotDesc [ID];
			selectedItemSprite.sprite = slotSprite [ID];
			selectedItemType = slotType [ID];
			clickedSlotID = ID;
			SelectMenu.gameObject.SetActive (true);
			
			if (selectedItemType == 0) {
				UseBtn.gameObject.SetActive (true);
				EquipBtn.gameObject.SetActive (false);

				if (selectedItemID == 2){
					UseBtn.gameObject.GetComponentInChildren<Text>().text = "Use";
				}
				else if (selectedItemID == 17){
					UseBtn.gameObject.GetComponentInChildren<Text>().text = "Use";
				}
				else if (selectedItemID == 18){
					UseBtn.gameObject.GetComponentInChildren<Text>().text = "Use";
				}
				else if (selectedItemID == 21){
					UseBtn.gameObject.GetComponentInChildren<Text>().text = "Use";
				}
				else if (selectedItemID == 22){
					UseBtn.gameObject.GetComponentInChildren<Text>().text = "Use";
				}
				else if (selectedItemID == 23){
					UseBtn.gameObject.GetComponentInChildren<Text>().text = "Use";
				}
				else if (selectedItemID == 27){
					UseBtn.gameObject.GetComponentInChildren<Text>().text = "Use";
				}
				else{
					UseBtn.gameObject.GetComponentInChildren<Text>().text = "Drop";
				}
			} else if (selectedItemType == 1) {
				UseBtn.gameObject.SetActive (false);
				EquipBtn.gameObject.SetActive (true);
			}
		}
	}

	public void CraftItem(int amount) 
	{
		bool inventory_full = true;

		BarController barcontroller;
		barcontroller = this.gameObject.GetComponent<BarController>();

		for (int i = 0; i < slotID.Length; i++)
		{
			if (slotID[i] == 0)
			{
				inventory_full = false;
				break;
			}
			else
			{
				inventory_full = true;
			}
		}

		if(inventory_full == false) {

			if (barcontroller.resource_points >= barcontroller.craftingCost) 
			{
				barcontroller.resource_points -= barcontroller.craftingCost;
				barcontroller.craftingCost += (int)Mathf.Ceil(barcontroller.craftingCost * 0.10f);
				barcontroller.craftingCostText.text = "-" + barcontroller.craftingCost.ToString();
				stored_item_ID = Random.Range (2, 32);
				//stored_item_ID = 2;
				Item item = ItemDatabase.GetItem (stored_item_ID);
				craftedItemName.text = item.itemName;
				craftedItemDesc.text = item.itemDesc;
				craftedItemSprite.sprite = item.itemSprite;
	
				//CREATE A FUNCTION TO DISPLAY WHAT THE PLAYER CREATED
				CraftedItemShow.SetActive(true);
				//PLAY SOUND
				barcontroller.InvokeSound(barcontroller.craftSound);
			}
			else
			{
				//YOU DONT HAVE ENOUGH RESOURCE POINTS
				//NotEnoughResources.SetActive(true);
				barcontroller.InvokeSound(barcontroller.errorSound);
				StartCoroutine(barcontroller.InvokeMessage("NOT ENOUGH RESOURCES!", 1f));
			}
		}
		else
		{
			//InventoryIsFull.SetActive(true);
			barcontroller.InvokeSound(barcontroller.errorSound);
			StartCoroutine(barcontroller.InvokeMessage("INVENTORY FULL!", 1f));
		}
	}

	public void TakeItem() 
	{
		ItemClicked(stored_item_ID);	
	}

	public void UseItemClicked()
	{
		if (slotStack [clickedSlotID] > 0) {
			//USE THE ITEM then YOU USED ITEM X
			slotStack[clickedSlotID]--;
							//Magic Barries heal
			if (slotID[clickedSlotID] == 2){
				this.gameObject.GetComponent<BarController>().MagicBarriesHeal();
			}

			if (slotID[clickedSlotID] == 17){
				this.gameObject.GetComponent<BarController>().ToilAndTrouble();
			}

			if (slotID[clickedSlotID] == 18){
				this.gameObject.GetComponent<BarController>().Dice();
			}

			if (slotID[clickedSlotID] == 21){
				this.gameObject.GetComponent<BarController>().Apple();
			}

			if (slotID[clickedSlotID] == 22){
				this.gameObject.GetComponent<BarController>().Marmalade();
			}

			if (slotID[clickedSlotID] == 23){
				this.gameObject.GetComponent<BarController>().CraftingScroll();
			}

			if (slotID[clickedSlotID] == 27){
				this.gameObject.GetComponent<BarController>().ResourceSack();
			}

			BarController player = this.gameObject.GetComponent<BarController>();
			player.InvokeSound(player.powerUpSound);

			if (slotStack [clickedSlotID] <= 0) {
				//CLEAR ITEM FROM SLOT
				slotID [clickedSlotID] = 0;
				slotValue [clickedSlotID] = 0;
				slotName [clickedSlotID] = "";
				slotDesc [clickedSlotID] = "";
				slotSprite [clickedSlotID] = null;
				slotType [clickedSlotID] = 0;
				slotIcon[clickedSlotID].sprite = emptySlotSprite;
				DisableItem(selectedItemID);
			}
		}
	}

	public void UseItem(int ID){
		for (int i = 0; i < slotID.Length; i++){
			if(slotID[i] == ID){
				slotID [i] = 0;
				slotValue [i] = 0;
				slotName [i] = "";
				slotDesc [i] = "";
				slotSprite [i] = null;
				slotType [i] = 0;
				slotIcon[i].sprite = emptySlotSprite;
				DisableItem(ID);
				return;
			}
		}
	}

	/*
	
		FROM THIS POINT ON ITS ALL ITEM ENABLE DISABLE CODE THIS IS THE GOOD STUFF
	
	
	
	 */

	public void EnableItem(int ID)
    {
        //IF ITEM ID EQUALS SOMETHING ENABLE THE EFFECTS OF THIS ITEMS TOWARDS WHAT EVER OBJECT IT IS

        if (ID == 3)
        {
            //DO THIS ITEM EFFFECT/STAT INCREASE
			Debug.Log("Item with id 1 enabled");
			GameObject.Find("Captain").GetComponent<BarController>().IronDaggerEnabled();
        }

		if (ID == 4)
		{
			this.gameObject.GetComponent<BarController>().MechanicalHeartEnabled();
		}
		
		if (ID == 5)
		{
			this.gameObject.GetComponent<PlayerController>().AngelFeatherEnabled();
		}

		if (ID == 6)
		{
			this.gameObject.GetComponent<BarController>().SwordOfHateEnabled();
		}

		if(ID == 7) //Platinum Plate
		{
			this.gameObject.GetComponent<BarController>().PlatinumGuardsEnabled();
		}

		if(ID == 8) //Shark tooth
		{
			this.gameObject.GetComponent<BarController>().SharkToothEnabled();
		}

		if(ID == 9) //Necklace of Escaping
		{
			this.gameObject.GetComponent<BarController>().NecklaceOfEscapingEnabled();
		}

		if(ID == 10) //Necklace of Escaping
		{
			this.gameObject.GetComponent<BarController>().ShieldOfImmunityEnabled();
		}

		if(ID == 11) //Cursed Shield
		{
			this.gameObject.GetComponent<BarController>().CursedShieldEnabled();
		}

		if(ID == 12) //Speedy Sabatons
		{
			this.gameObject.GetComponent<PlayerController>().SpeedySabatonsEnabled();
		}
		if(ID == 13) //Pick
		{
			this.gameObject.GetComponent<BarController>().PickEnabled();
		}

		if(ID == 14) //Scythe
		{
			this.gameObject.GetComponent<BarController>().ScytheEnabled();
		}

		if(ID == 15) //Fiery Helm
		{
			this.gameObject.GetComponent<BarController>().FieryHelmEnabled();
		}

		if(ID == 16) //Volcanic Shield
		{
			this.gameObject.GetComponent<BarController>().VolcanicShieldEnabled();
		}

		//ID = 17 Toil and Trouble
		
		//ID = 18 Dice

		if(ID == 19) //Acid Skull
		{
			this.gameObject.GetComponent<BarController>().AcidSkullEnabled();
		}

		if(ID == 20) //Cursed Blade
		{
			this.gameObject.GetComponent<BarController>().CursedBladeEnabled();
		}

		if(ID == 24) //Bomb
		{
			this.gameObject.GetComponent<BarController>().BombEnabled();
		}
		
		if(ID == 25) //Horseshoe
		{
			this.gameObject.GetComponent<BarController>().HorseShoeEnabled();
		}

		if(ID == 26) //Goldtooth
		{
			this.gameObject.GetComponent<BarController>().GoldToothEnabled();
		}
		
		//Sack of resources

		if(ID == 28) //Nightblade
		{
			this.gameObject.GetComponent<BarController>().NightbladeEnabled();
		}

		if(ID == 29) //God helm
		{
			this.gameObject.GetComponent<BarController>().GodHelmEnabled();
		}

		if(ID == 30) //Bloodthirster
		{
			this.gameObject.GetComponent<BarController>().BloodthirsterEnabled();
		}
    }

    public void DisableItem(int ID)
    {	

        //DISABLE THE EFFECT OF THIS ITEM WITH THIS ID
		if (ID == 3)
		{

			this.gameObject.GetComponent<BarController>().IronDaggerDisabled();
		}

		if (ID == 4) //Mechanical Heart
		{
			this.gameObject.GetComponent<BarController>().MechanicalHeartDisabled();
			ReEnableCorutineItems(4);
		}

		if (ID == 5) //Angel Feather
		{
			this.gameObject.GetComponent<PlayerController>().AngelFeatherDisabled();
		}

		if (ID == 6) //Sword of Hate
		{
			this.gameObject.GetComponent<BarController>().SwordOfHateDisabled();
		}

		if(ID == 7) //Platinum Plate
		{
			this.gameObject.GetComponent<BarController>().PlatinumGuardsDisabled();
		}

		if(ID == 8) //Shark Tooth
		{	
			if(FindItem(8) == false){
				this.gameObject.GetComponent<BarController>().SharkToothDisabled();
			}	
		}

		if(ID == 9) //Necklace of Escaping
		{	
			if(FindItem(9) == false){
				this.gameObject.GetComponent<BarController>().NecklaceOfEscapingDisabled();
			}	
		}

		if(ID == 10) //Shield of Immunity
		{	
			if(FindItem(10) == false){
				this.gameObject.GetComponent<BarController>().ShieldOfImmunityDisabled();
			}	
		}

		if(ID == 11) //Cursed Shield
		{	
			if(FindItem(11) == false){
				this.gameObject.GetComponent<BarController>().CursedShieldDisabled();
			}	
		}

		if(ID == 12) //Speedy Sabatons
		{
			this.gameObject.GetComponent<PlayerController>().SpeedySabatonsDisabled();
		}

		if(ID == 13) //Pick
		{	
			if(FindItem(13) == false){
				this.gameObject.GetComponent<BarController>().PickDisabled();
			}	
		}

		if(ID == 14) //Scythe
		{	
			if(FindItem(14) == false){
				this.gameObject.GetComponent<BarController>().ScytheDisabled();
			}	
		}

		if(ID == 15) //Fiery Helm
		{	
			if(FindItem(15) == false){
				this.gameObject.GetComponent<BarController>().FieryHelmDisabled();
			}	
		}

		if(ID == 16) //Volcanic Shield
		{	
			if(FindItem(16) == false){
				this.gameObject.GetComponent<BarController>().VolcanicShieldDisabled();
			}	
		}

		//ID = 17 Toil and Trouble
		
		//ID = 18 Dice

		if(ID == 19) //Acid Skull
		{	
			if(FindItem(19) == false){
				this.gameObject.GetComponent<BarController>().AcidSkullDisabled();
			}	
		}

		if(ID == 20) //Cursed Blade
		{	
			if(FindItem(20) == false){
				this.gameObject.GetComponent<BarController>().CursedBladeDisabled();
			}	
		}

		if(ID == 24) //Bomb
		{	
			if(FindItem(24) == false){
				this.gameObject.GetComponent<BarController>().BombDisabled();
			}	
		}

		if(ID == 25) //Horseshoe
		{	
			if(FindItem(25) == false){
				this.gameObject.GetComponent<BarController>().HorseShoeDisabled();
			}	
		}

		if(ID == 26) //Goldtooth
		{	
			if(FindItem(26) == false){
				this.gameObject.GetComponent<BarController>().GoldToothDisabled();
			}	
		}

		if(ID == 28) //Goldtooth
		{	
			if(FindItem(28) == false){
				this.gameObject.GetComponent<BarController>().NightbladeDisabled();
			}	
		}

		if(ID == 29) //God helm
		{
			this.gameObject.GetComponent<BarController>().GodHelmDisabled();
		}

		if(ID == 30) //Bloodthirster
		{	
			if(FindItem(30) == false){
				this.gameObject.GetComponent<BarController>().BloodthirsterDisabled();
			}	
		}

		if(ID == 31) //Bloodthirster
		{	
			if(FindItem(31) == false){
				this.gameObject.GetComponent<BarController>().PooDisabled();
			}	
		}
    }

	//When an item is disabled for some reason it stops all coruitens so this method reEnables them
	private void ReEnableCorutineItems(int ID){
		for(int i = 0; i < slotID.Length; i++){
			if(ID == slotID[i]){
				EnableItem(ID);
			}
		}
	}

	//This Method finds item in the invetory and returns ture or false
	private bool FindItem(int ID)
	{
		for(int i = 0; i < slotID.Length; i++){
			if(ID == slotID[i]){
				return true;
			}
		}
		return false;
	}

	public bool InventoryFull(){
		for(int i = 0; i < slotID.Length; i++){
			if(slotID[i] == 0){
				return false;
			}
		}
		return true;
	}
}