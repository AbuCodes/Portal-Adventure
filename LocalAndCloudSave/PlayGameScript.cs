using System.Collections;
using System.Collections.Generic;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using System.Text;
using UnityEngine;

public class PlayGameScript : MonoBehaviour {

	public static PlayGameScript Instance {get; private set;}

	const string SAVE_NAME = "Savefile";
	bool isSaving;
	bool isCloudDataLoaded = false;

	// Use this for initialization
	void Start () {
		StartCoroutine(InitCoroutine());
	}

	IEnumerator InitCoroutine(){
        yield return new WaitForEndOfFrame();
		Instance = this;

		if(!PlayerPrefs.HasKey(SAVE_NAME)){
			PlayerPrefs.SetString(SAVE_NAME, string.Empty);
		}

		if(!PlayerPrefs.HasKey("IsFirstTime")){
			PlayerPrefs.SetInt("IsFirstTime", 1);
		}

		LoadLocal();

		if (Application.platform == RuntimePlatform.Android)
        {
            Debug.Log("Do something special here!");
        }

		#if UNITY_ANDROID
			PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();
			PlayGamesPlatform.InitializeInstance(config);
			PlayGamesPlatform.Activate();
		#endif



		SignIn();
    }

	//This method is used to sign in the user
	void SignIn(){
		Social.localUser.Authenticate(success => { LoadData(); }); //This looks for a call back from google play (not doing anything atm)
	}

	#region Saved Games
	string GameDataToString(){
		return JsonUtil.CollectionToJsonString(CloudVariables.Values, "myKey");
	}

	void StringToGameData(string cloudData, string localData){

		if (cloudData == string.Empty){
			StringToGameData(localData);
			isCloudDataLoaded = true;
			return;
		}
		int[] cloudArray = JsonUtil.JsonStringToArray(cloudData, "myKey", str => int.Parse(str));

		if (localData == string.Empty){
			CloudVariables.Values = cloudArray;
			PlayerPrefs.SetString(SAVE_NAME, cloudData);
			isCloudDataLoaded = true;
			return;
		}
		int[] localArray = JsonUtil.JsonStringToArray(localData, "myKey", str => int.Parse(str));

		if(PlayerPrefs.GetInt("IsFirstTime") == 1){
			PlayerPrefs.SetInt("IsFirstTime", 0);
			
			for (int i = 0; i < cloudArray.Length; i++){
				if(cloudArray[i] > localArray[i]){
					PlayerPrefs.SetString(SAVE_NAME, cloudData);
				}
			}
		}
		else{
			for (int i = 0; i < cloudArray.Length; i++){
				if(localArray[i] > cloudArray[i]){
					CloudVariables.Values = localArray;
					isCloudDataLoaded = true;
					SaveData();
					return;
				}
			}
		}
		CloudVariables.Values = cloudArray;
		isCloudDataLoaded = true;
	}

	void StringToGameData(string localData){
		if (localData != string.Empty){
			CloudVariables.Values = JsonUtil.JsonStringToArray(localData, "myKey", str => int.Parse(str));
		}
	}

	public void LoadData(){

		if(Social.localUser.authenticated){
			isSaving = false;
			#if UNITY_ANDROID
			((PlayGamesPlatform)Social.Active).SavedGame.OpenWithManualConflictResolution(SAVE_NAME, DataSource.ReadCacheOrNetwork, true, ResolveConflict, OnSavedGameOpened);
			#endif
		}
		else{
			LoadLocal();
		}
	}

	private void LoadLocal(){
		StringToGameData(PlayerPrefs.GetString(SAVE_NAME));
	}

	public void SaveData(){
		
		if(!isCloudDataLoaded){
			SaveLocal();
			return;
		}
		if(Social.localUser.authenticated){
			isSaving = true;
			#if UNITY_ANDROID
			((PlayGamesPlatform)Social.Active).SavedGame.OpenWithManualConflictResolution(SAVE_NAME, DataSource.ReadCacheOrNetwork, true, ResolveConflict, OnSavedGameOpened);
			#endif
		}
		else{
			SaveLocal();
		}
	}

	private void SaveLocal(){
		PlayerPrefs.SetString(SAVE_NAME, GameDataToString());
	}

	private void ResolveConflict(IConflictResolver resolver, ISavedGameMetadata original, byte[] originalData, ISavedGameMetadata unmerged, byte[] unmergedData){
		if (originalData == null){
			resolver.ChooseMetadata(unmerged);
		}
		else if (unmergedData == null){
			resolver.ChooseMetadata(original);
		}
		else{
			string originalStr = Encoding.ASCII.GetString(originalData);
			string unmergedStr = Encoding.ASCII.GetString(unmergedData);

			int[] originalArray = JsonUtil.JsonStringToArray(originalStr, "myKey", str => int.Parse(str));
			int[] unmergedArray = JsonUtil.JsonStringToArray(unmergedStr, "myKey", str => int.Parse(str));

			for (int i = 0; i < originalArray.Length; i++){

				if (originalArray[i] > unmergedArray[i]){
					resolver.ChooseMetadata(original);
					return;
				}
				else if (unmergedArray[i] > originalArray[i]){
					resolver.ChooseMetadata(unmerged);
					return;
				}
			}
			resolver.ChooseMetadata(original);
		}
	}

	private void OnSavedGameOpened(SavedGameRequestStatus status, ISavedGameMetadata game){
		if (status == SavedGameRequestStatus.Success){
			if(!isSaving){
				LoadGame(game);
			}
			else {
				SaveGame(game);
			}
		}
		else {
			if (!isSaving){
				LoadLocal();
			}
			else{
				SaveLocal();
			}
		}
	}

	private void LoadGame(ISavedGameMetadata game){
		#if UNITY_ANDROID
		((PlayGamesPlatform)Social.Active).SavedGame.ReadBinaryData(game, OnSavedGameDataRead);
		#endif
	}

	private void SaveGame(ISavedGameMetadata game){
		string stringToSave = GameDataToString();
		SaveLocal();

		byte[] dataToSave = Encoding.ASCII.GetBytes(stringToSave);
		SavedGameMetadataUpdate update = new SavedGameMetadataUpdate.Builder().Build();
		#if UNITY_ANDROID
		((PlayGamesPlatform)Social.Active).SavedGame.CommitUpdate(game, update, dataToSave, OnSavedGameDataWritten);
		#endif
	}

	private void OnSavedGameDataRead(SavedGameRequestStatus status, byte[] savedData){
		if(status == SavedGameRequestStatus.Success){
			string cloudDataString;
			if(savedData.Length == 0){
				cloudDataString = string.Empty;
			}
			else{
				cloudDataString = Encoding.ASCII.GetString(savedData);
			}
			string localDataString = PlayerPrefs.GetString(SAVE_NAME);
			StringToGameData(cloudDataString, localDataString);
		}
	}
	private void OnSavedGameDataWritten(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
 
    }
	#endregion

	#region Achievements
	//This method unlocks an achievements with the given id and looks for call back
	public static void UnlockAchievement(string id){
		Social.ReportProgress(id, 100, success => { });
	}

	public static void IncrementAchievement(string id, int stepsToIncrement){
		//PlayGamesPlatform.Instance.IncrementAchievement(id, stepsToIncrement, success => { });
	}

	public static void ShowAchievementsUI(){
		Social.ShowAchievementsUI();
	}
	#endregion

	#region Leaderboards
	public static void AddScoreToLeaderboard(string leaderboardId, long score){
		Social.ReportScore(score, leaderboardId, success => { });
	}

	public static void ShowLeaderboardUI(){
		Social.ShowLeaderboardUI();
	}
	#endregion
}
