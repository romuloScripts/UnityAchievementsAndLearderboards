using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if STEAM
using Steamworks;
#endif

[CreateAssetMenu(fileName = "SteamLeaderboard", menuName = "Shieldnator/SteamLeaderboard", order = 0)]
public class SteamLeaderboard : Leaderboard {

	public enum RequestType{
		Friends,
		Global,
		GlobalAroundUser,
		Users
	}

	public enum UploadScoreMethod{
		ForceUpdate,
		KeepBest,
		None
	}

	public RequestType requestType;
	public UploadScoreMethod uploadMethod = UploadScoreMethod.KeepBest;
	public int  maxEntries=30, uploadScore;
	[Tooltip("Use manual entries instead of download")]
	public bool test;
	public bool releaseMemory=true;

	#if STEAM
	private int type;

	[NonSerialized]
	private SteamLeaderboard_t leaderboard_T;

	private CallResult<LeaderboardFindResult_t> OnLeaderboardFindResultCallResult = new CallResult<LeaderboardFindResult_t>();
	private CallResult<LeaderboardScoresDownloaded_t> OnLeaderboardScoresDownloadedCallResult = new CallResult<LeaderboardScoresDownloaded_t>();
	private CallResult<LeaderboardScoreUploaded_t> OnLeaderboardScoreUploadedCallResult = new CallResult<LeaderboardScoreUploaded_t>();

	public void Find(Action action){
	 	SteamAPICall_t steamAPICall = SteamUserStats.FindLeaderboard(boardName);
		OnLeaderboardFindResultCallResult.Set(steamAPICall,(
			LeaderboardFindResult_t pCallback, bool bIOFailure) => FindResult(pCallback, bIOFailure, action));
	}

	public void FindResult(LeaderboardFindResult_t pCallback, bool bIOFailure, Action action){
		if (pCallback.m_bLeaderboardFound != 1 || bIOFailure) {
			Debug.Log("There was an error retrieving the leaderboard.");
		}else {
			leaderboard_T = pCallback.m_hSteamLeaderboard;
			action.Invoke();
		}
	}

	public override void Upload(int score){
		if (!IsSteam())
			return;
		uploadScore = score;
		if(leaderboard_T == new SteamLeaderboard_t(0)){
			Find(OnUpload);
		}else{
			OnUpload();
		}
	}

	private void OnUpload(){	
		if(leaderboard_T == new SteamLeaderboard_t(0))
			return;
		SteamAPICall_t steamAPICall = SteamUserStats.UploadLeaderboardScore(
			leaderboard_T,(ELeaderboardUploadScoreMethod)uploadMethod,uploadScore,null,0);
		OnLeaderboardScoreUploadedCallResult.Set(steamAPICall,UploadResult);
	} 

	private void UploadResult(LeaderboardScoreUploaded_t pCallback, bool bIOFailure){
		if (pCallback.m_bSuccess != 1 || bIOFailure) {
			//debug.Log("Not uploaded." + pCallback.m_bSuccess+" " +bIOFailure);
		}else{
			//debug.Log("Uploaded");
		}
	}

	public override void Download(int type){
		if(test){
			onDownloaded.Invoke(entries);
			return;
		}
		if (!IsSteam())
			return;
		requestType = (RequestType)type;
		if(leaderboard_T == new SteamLeaderboard_t(0)){
			Find(OnDownload);
		}else{
			OnDownload();
		}
	}

	public void Download(){
		if(test){
			onDownloaded.Invoke(entries);
			return;
		}
		if (!IsSteam())
			return;
		if(leaderboard_T == new SteamLeaderboard_t(0)){
			Find(OnDownload);
		}else{
			OnDownload();
		}
	}

	private void OnDownload(){
	 	if(leaderboard_T == new SteamLeaderboard_t(0))
			return;
		SteamAPICall_t steamAPICall = SteamUserStats.DownloadLeaderboardEntries(leaderboard_T, (ELeaderboardDataRequest)requestType, 1, 30);
		OnLeaderboardScoresDownloadedCallResult.Set(steamAPICall,DownloadResult);
	}

	private void DownloadResult(LeaderboardScoresDownloaded_t pCallback, bool bIOFailure){
		if (!bIOFailure) {
			int n = Mathf.Min(pCallback.m_cEntryCount, maxEntries);
			LeaderboardEntry_t[] m_leaderboardEntries = new LeaderboardEntry_t[n];
			entries = new LeaderboardEntrie[n];
			for (int i = 0; i < n; i++){
				SteamUserStats.GetDownloadedLeaderboardEntry(pCallback.m_hSteamLeaderboardEntries,i,out m_leaderboardEntries[i],null,0);
				LeaderboardEntrie entrie  = new LeaderboardEntrie();
				entrie.name = SteamFriends.GetFriendPersonaName(m_leaderboardEntries[i].m_steamIDUser);
				entrie.score = m_leaderboardEntries[i].m_nScore;
				entrie.user = (SteamUser.GetSteamID().m_SteamID.Equals(m_leaderboardEntries[i].m_steamIDUser.m_SteamID));
				entrie.pos = i;
				entries[i] = entrie;
			}
			onDownloaded.Invoke(entries);
			if(releaseMemory)
				entries = null;
		}
	}
	#endif

	public static bool IsSteam(){
		#if STEAM
		return true;
		#endif
		return false;
	}
}
