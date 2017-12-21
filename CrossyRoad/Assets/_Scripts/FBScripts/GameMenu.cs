/**
 * Copyright (c) 2014-present, Facebook, Inc. All rights reserved.
 *
 * You are hereby granted a non-exclusive, worldwide, royalty-free license to use,
 * copy, modify, and distribute this software in source code or binary form for use
 * in connection with the web services and APIs provided by Facebook.
 *
 * As with any software that integrates with the Facebook platform, your use of
 * this software is subject to the Facebook Developer Principles and Policies
 * [http://developers.facebook.com/policy/]. This copyright notice shall be
 * included in all copies or substantial portions of the software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
 * FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
 * COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
 * IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
 * CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Facebook.Unity;

public class GameMenu : MonoBehaviour
{
	// UI Element References (Set in Unity Editor)
	//  Header
	public GameObject[] panelUser;
	public GameObject panelSocres;
	public GameObject prefabScore;

	public GameObject panelPersonInfo;
	public GameObject panelAchivemets;
	private bool share, score;
    #region Built-In
    void Awake()
    {
        // Initialize FB SDK
        if (!FB.IsInitialized)
        {
            FB.Init(InitCallback);
        }
    }

    // OnApplicationPause(false) is called when app is resumed from the background
    void OnApplicationPause (bool pauseStatus)
    {
        // Do not do anything in the Unity Editor
        if (Application.isEditor) {
            return;
        }

        // Check the pauseStatus for an app resume
        if (!pauseStatus)
        {
            if (FB.IsInitialized)
            {
                // App Launch events should be logged on app launch & app resume
                // See more: https://developers.facebook.com/docs/app-events/unity#quickstart
                FBAppEvents.LaunchEvent();
            }
            else
            {
                FB.Init(InitCallback);
            }
        }
    }

    // OnLevelWasLoaded is called when we return to the main menu
    void OnLevelWasLoaded(int level)
    {
        Debug.Log("OnLevelWasLoaded");
        if (level == 0 && FB.IsInitialized)
        {
            Debug.Log("Returned to main menu");

            // We've returned to the main menu so let's complete any pending score activity
            if (FB.IsLoggedIn)
            {
                RedrawUI();

                // Post any pending High Score
               // if (GameStateManager.highScorePending)
              //  {
              //      GameStateManager.highScorePending = false;
               //     FBShare.PostScore(GameStateManager.HighScore);
               // }
            }
        }
    }
    #endregion

    #region FB Init
    private void InitCallback()
    {
        Debug.Log("InitCallback");

        // App Launch events should be logged on app launch & app resume
        // See more: https://developers.facebook.com/docs/app-events/unity#quickstart
        FBAppEvents.LaunchEvent();
		

		if (FB.IsLoggedIn) 
        {

            Debug.Log("Already logged in");
            OnLoginComplete();
        }
    }
    #endregion

    #region Login
    public void OnLoginClick ()
    {
        Debug.Log("OnLoginClick");


		// Call Facebook Login for Read permissions of 'public_profile', 'user_friends', and 'email'
		//if(FBLogin)
		FBLogin.PromptForLogin(OnLoginComplete);
		//FB.LogInWithPublishPermissions(new List<string>() { "publish_actions" }, Share);
	
		score = true;
		share = false;
    }

	public void Share(IResult result)
	{
		if (result.Error != null)
		{
			Debug.LogError(result.Error);
			return;
		}
		Debug.Log(result.RawResult);
		FBShare.TakeScreenshot();
	}
	public void CountAchivement()
	{
		if (!FB.IsInitialized)
		{
			FB.Init(InitCallback);
		}
		if (FB.IsLoggedIn)
		{
			panelAchivemets.SetActive(true);
			FBAchievements.ViewAchievement();
		}
		else
		{
			
			FBLogin.PromptForLogin(OnAchivementCom);
		}
	}
	public void OnAchivementCom()
	{
		panelAchivemets.SetActive(true);
		FBAchievements.ViewAchievement();
	}
	public void ViewAchivements()
	{
		if (!FB.IsInitialized)
		{
			FB.Init(InitCallback);
		}
		if (FBLogin.HavePublishActions)
		{
		
			FBAchievements.GiveAchievement("");
		}
		else
		{
			FB.LogInWithPublishPermissions(new List<string>() { "publish_actions" }, Achivements);
		}
	}
	public void Achivements(IResult result)
	{
		if (result.Error != null)
		{
			Debug.LogError(result.Error);
			return;
		}
		Debug.Log(result.RawResult);

		//FBAchievements.GiveAchievement("");
		FBAchievements.DeleteAchievement("");
	}
    private void OnLoginComplete()
    {
        Debug.Log("OnLoginComplete");

        if (!FB.IsLoggedIn)
        {
 
            return;
        }

		// Show loading animations


		// Begin querying the Graph API for Facebook data
	
			FBGraph.GetPlayerInfo();
			FBGraph.GetFriends();
			FBGraph.GetInvitableFriends();
			FBGraph.GetScores();
		

		//FB.LogInWithPublishPermissions(new List<string>() { "publish_actions" }, Share);
		//ShareImage();
		//FBShare.ShareBrag();
		//FBShare.PostScore();
	}
	public void ShareImage()
	{
		if (!FB.IsInitialized)
		{
			FB.Init(InitCallback);
		}

		FB.LogInWithPublishPermissions(new List<string>() { "publish_actions" }, Share);
	
	}
	private void OnShare()
	{
		if (!FB.IsLoggedIn)
		{

			return;
		}
	
		FBShare.TakeScreenshot();
		//FBShare.ShareBrag();
	}
	private void AuthCallback(IResult result)
	{
		Debug.Log("share Hi");
		if (result.Error != null)
		{
			Debug.LogError(result.Error);
			return;
		}
	
		if ((AccessToken.CurrentAccessToken.Permissions as List<string>).Contains("publish_actions"))
		{
			Debug.Log("have publish actions");
		}
		else
		{
			Debug.Log("no publish actions");
		}
		//if(FBLogin)
		//FBShare.TakeScreenshot();
		Debug.Log(result.RawResult);
	}
	#endregion
	public void Logout()
	{
		FB.LogOut();
	}
    #region GUI
    // Method to update the Game Menu User Interface
    public void RedrawUI ()
    {
		if (FB.IsLoggedIn)
		{
			// Swap GUI Header for a player after login
			gameManager.intance.RedrawPanelTopScore();

			UIManager panelAll = GetComponent<UIManager>();
			panelAll.setActivePanel();

			PersonLogin personLogin = panelPersonInfo.GetComponent<PersonLogin>();
			if (GameStateManager.UserTexture != null && !string.IsNullOrEmpty(GameStateManager.Username))
			{
				personLogin.setTxtName(GameStateManager.Username);
				personLogin.setImage(GameStateManager.UserTexture);
			}
			if (GameStateManager.HighScore > 0)
				personLogin.setTxtScore(GameStateManager.HighScore.ToString());

		

			var scores = GameStateManager.Scores;
			if (GameStateManager.ScoresReady && scores.Count > 0)
			{
				//personLogin.setStandings("1", scores.Count.ToString());


				Transform[] childLBElements = panelSocres.GetComponentsInChildren<Transform>();
				foreach (Transform childObject in childLBElements)
				{
					if (!panelSocres.transform.IsChildOf(childObject.transform))
					{
						Destroy(childObject.gameObject);
					}
				}
				
				// Populate leaderboard
				for (int i = 0; i < scores.Count; i++)
				{
					GameObject LBgameObject = Instantiate(prefabScore) as GameObject;
					PersonInfo LBelement = LBgameObject.GetComponent<PersonInfo>();

					var entry = (Dictionary<string, object>)scores[i];
					var user = (Dictionary<string, object>)entry["user"];
					if(((string)user["name"]).Split(new char[] { ' ' })[0].Contains(GameStateManager.Username))
						personLogin.setStandings((i + 1).ToString(), scores.Count.ToString());

					LBelement.SetupElement(i + 1, scores[i]);
					RectTransform rect = LBelement.GetComponent<RectTransform>();
					rect.anchoredPosition = new Vector2(0, i * -140);

					LBelement.transform.SetParent(panelSocres.transform, false);
				}
			
				// Scroll to top
			//	LeaderboardScrollRect.verticalNormalizedPosition = 1f;

				//Debug.Log(GameStateManager.Friends.Count + " " + scores.Count);
			}
		}
		
    }
    #endregion

    #region Menu Buttons
    public void OnPlayClicked()
    {
     
    }

    public void OnBragClicked()
    {
        Debug.Log("OnBragClicked");
        FBShare.ShareBrag();
    }

    public void OnChallengeClicked()
    {
        Debug.Log("OnChallengeClicked");
        FBRequest.RequestChallenge();
    }

    public void OnStoreClicked()
    {
        Debug.Log("OnStoreClicked");

    }
    #endregion
}
