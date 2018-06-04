using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
#if STEAM
using System.ComponentModel;
using Steamworks;
#endif

public class SteamStatsAndAchievements : MonoBehaviour {
	public SaveStatsAndAchievements statsAndAchievements;
#if STEAM
	// Our GameID
	private CGameID m_GameID;

	// Did we get the stats from Steam?
	private bool m_bRequestedStats;
	private bool m_bStatsValid;

// Should we store stats this frame?
	private bool m_bStoreStats;

	protected Callback<UserStatsReceived_t> m_UserStatsReceived;
	protected Callback<UserStatsStored_t> m_UserStatsStored;
	protected Callback<UserAchievementStored_t> m_UserAchievementStored;

	private static SteamStatsAndAchievements singleton;

	private void Awake() {
		if (singleton != null || !IsSteam()) {
			Destroy(this);
			return;
		}
		//SceneManager.sceneLoaded += DebugStats;
		singleton = this;
		DontDestroyOnLoad(gameObject);
	}

	/* void DebugStats(Scene scene, LoadSceneMode mode){
		foreach (var item in stats){
			Debug.Log(item.name + " " + item.data);
		}
	} */

	void OnEnable() {

		if(singleton != this){
			Destroy(this);
			return;
		}

		if (!SteamManager.Initialized)
			return;
		if(Application.isEditor){
			SteamUserStats.ResetAllStats(true);
		}	
		// Cache the GameID for use in the Callbacks
		m_GameID = new CGameID(SteamUtils.GetAppID());

		m_UserStatsReceived = Callback<UserStatsReceived_t>.Create(OnUserStatsReceived);
		m_UserStatsStored = Callback<UserStatsStored_t>.Create(OnUserStatsStored);
		m_UserAchievementStored = Callback<UserAchievementStored_t>.Create(OnAchievementStored);

		// These need to be reset to get the stats upon an Assembly reload in the Editor.
		m_bRequestedStats = false;
		m_bStatsValid = false;
	}

	private void Update() {
		if (!SteamManager.Initialized)
			return;
		if (!m_bRequestedStats) {
			// Is Steam Loaded? if no, can't get stats, done
			if (!SteamManager.Initialized) {
				m_bRequestedStats = true;
				return;
			}
			
			// If yes, request our stats
			bool bSuccess = SteamUserStats.RequestCurrentStats();

			// This function should only return false if we weren't logged in, and we already checked that.
			// But handle it being false again anyway, just ask again later.
			m_bRequestedStats = bSuccess;
		}

		if (!m_bStatsValid)
			return;

		// Get info from sources

		//Store stats in the Steam database if necessary
		if (m_bStoreStats) {
			// already set any achievements in UnlockAchievement
			// set stats
			foreach (var s in statsAndAchievements.stats){
					SteamUserStats.SetStat(s.apiName,s.data);		
			}
			bool bSuccess = SteamUserStats.StoreStats();
			// If this failed, we never sent anything to the server, try
			// again later.
			m_bStoreStats = !bSuccess;
		}
	}

	private void OnUserStatsReceived(UserStatsReceived_t pCallback) {
		if (!SteamManager.Initialized)
			return;

		// we may get callbacks for other games' stats arriving, ignore them
		if ((ulong)m_GameID == pCallback.m_nGameID) {
			if (EResult.k_EResultOK == pCallback.m_eResult) {
				Debug.Log("Received stats and achievements from Steam\n");
				m_bStatsValid = true;
				// load achievements
				foreach (SteamAchievement ach in statsAndAchievements.achievements) {
					bool ret = SteamUserStats.GetAchievement(ach.apiName, out ach.achieved);
					if (!ret) {
						Debug.LogWarning("SteamUserStats.GetAchievement failed for Achievement " + ach.apiName + "\nIs it registered in the Steam Partner site?");
					}
					else {
						//ach.m_strName = SteamUserStats.GetAchievementDisplayAttribute(ach.m_eAchievementID.ToString(), "name");
						//ach.m_strDescription = SteamUserStats.GetAchievementDisplayAttribute(ach.m_eAchievementID.ToString(), "desc");
					}
				}

				// load stats
				foreach (var s in statsAndAchievements.stats)
				{
                	SteamUserStats.GetStat(s.apiName, out s.data);
					s.CheckAchievements();
				}
			}
			else {
				Debug.Log("RequestStats - failed, " + pCallback.m_eResult);
			}
		}
	}

	public static void UnlockAchievement(SteamAchievement achievement) {
		if(!singleton) return;
		achievement.achieved = true;
		// mark it down
		SteamUserStats.SetAchievement(achievement.apiName);
		// Store stats end of frame
		singleton.m_bStoreStats = true;
	}

	private void OnUserStatsStored(UserStatsStored_t pCallback) {
		// we may get callbacks for other games' stats arriving, ignore them
		if ((ulong)m_GameID == pCallback.m_nGameID) {
			if (EResult.k_EResultOK == pCallback.m_eResult) {
				Debug.Log("StoreStats - success");
			}
			else if (EResult.k_EResultInvalidParam == pCallback.m_eResult) {
				// One or more stats we set broke a constraint. They've been reverted,
				// and we should re-iterate the values now to keep in sync.
				Debug.Log("StoreStats - some failed to validate");
				// Fake up a callback here so that we re-load the values.
				UserStatsReceived_t callback = new UserStatsReceived_t();
				callback.m_eResult = EResult.k_EResultOK;
				callback.m_nGameID = (ulong)m_GameID;
				OnUserStatsReceived(callback);
			}
			else {
				Debug.Log("StoreStats - failed, " + pCallback.m_eResult);
			}
		}
	}

	private void OnAchievementStored(UserAchievementStored_t pCallback) {
		// We may get callbacks for other games' stats arriving, ignore them
		if ((ulong)m_GameID == pCallback.m_nGameID) {
			if (0 == pCallback.m_nMaxProgress) {
				Debug.Log("Achievement '" + pCallback.m_rgchAchievementName + "' unlocked!");
			}
			else {
				Debug.Log("Achievement '" + pCallback.m_rgchAchievementName + "' progress callback, (" + pCallback.m_nCurProgress + "," + pCallback.m_nMaxProgress + ")");
			}
		}
	}

	public static void SaveStats(){
		if(singleton){
			singleton.m_bStoreStats = true;
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
