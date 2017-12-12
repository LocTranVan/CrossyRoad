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
        FBLogin.PromptForLogin(OnLoginComplete);
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
    }
    #endregion

    #region GUI
    // Method to update the Game Menu User Interface
    public void RedrawUI ()
    {
		if (FB.IsLoggedIn)
		{
			// Swap GUI Header for a player after login
			/*
			GameObject ob = panelUser[0];
			PersonInfo person = ob.GetComponent<PersonInfo>();
			if (GameStateManager.UserTexture != null && !string.IsNullOrEmpty(GameStateManager.Username))
			{
				person.setTxtName(GameStateManager.Username);
				person.setImage(GameStateManager.UserTexture);
			}
			if (GameStateManager.HighScore > 0)
				person.setTxtScore(GameStateManager.HighScore.ToString());

		*/

			var scores = GameStateManager.Scores;
			if (GameStateManager.ScoresReady && scores.Count > 0)
			{

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
