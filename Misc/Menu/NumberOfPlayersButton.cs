using UnityEngine;
using System.Collections;

public class NumberOfPlayersButton : MonoBehaviour {

	public TextMesh Tell;

	public int Operation;
	
	public GameObject Player1GameObjectHolder;
	public MenuPickInput Player1PickInput;
	public GameObject Player2GameObjectHolder;
	public MenuPickInput Player2PickInput;
	public GameObject Player3GameObjectHolder;
	public MenuPickInput Player3PickInput;
	public GameObject Player4GameObjectHolder;
	public MenuPickInput Player4PickInput;
	
	int NumberOfPlayers=1;
	
	public bool Receiver=false;
	
	void Start() {
		if(Receiver)
			UpdateInputChoices(1);
	}
	
	public void ReceiveUpdate (int operation) {
		Debug.Log("Operation "+operation+" NumbeROfPla" + NumberOfPlayers );
		if(NumberOfPlayers==4&&operation==1)
			return;
		if(NumberOfPlayers==1&&operation==0)
			return;
		
		if(operation==0) {
			NumberOfPlayers--;
			StartCoroutine(transform.Shake(0.02f,0.3f));
		}
		
		if(operation==1) {
			NumberOfPlayers++;
			StartCoroutine(transform.Shake(0.02f,0.3f));
		}
		
		Tell.text=""+NumberOfPlayers;
		GameManager.instance.numberOfPlayers=NumberOfPlayers;
		UpdateInputChoices(NumberOfPlayers);
	}
	
	
	public void UpdateInputChoices(int operation)
	{		
		switch (operation)
		{
			case 1:
					Player1GameObjectHolder.SetActive(true);
					Player1PickInput.CheckInput(2);
					Player2GameObjectHolder.SetActive(false);
					Player3GameObjectHolder.SetActive(false);
					Player4GameObjectHolder.SetActive(false);
				break;
			case 2:
					Player1GameObjectHolder.SetActive(true);
					Player1PickInput.CheckInput(3);
					Player2GameObjectHolder.SetActive(true);
					Player2PickInput.CheckInput(4);
					Player3GameObjectHolder.SetActive(false);
					Player4GameObjectHolder.SetActive(false);
				break;
			case 3:
					Player1GameObjectHolder.SetActive(true);
					Player1PickInput.CheckInput(3);
					Player2GameObjectHolder.SetActive(true);
					Player2PickInput.CheckInput(4);
					Player3GameObjectHolder.SetActive(true);
					Player3PickInput.CheckInput(5);
					Player4GameObjectHolder.SetActive(false);
				break;
			case 4:
					Player1GameObjectHolder.SetActive(true);
					Player1PickInput.CheckInput(3);
					Player2GameObjectHolder.SetActive(true);
					Player2PickInput.CheckInput(4);
					Player3GameObjectHolder.SetActive(true);
					Player3PickInput.CheckInput(5);
					Player4GameObjectHolder.SetActive(true);
					Player4PickInput.CheckInput(6);
				break;
		}
	}
	
	
	void ButtonClick(int Operation) {
		Debug.Log("Operation "+Operation);
		Tell.GetComponent<NumberOfPlayersButton>().ReceiveUpdate(Operation);
	}
}
