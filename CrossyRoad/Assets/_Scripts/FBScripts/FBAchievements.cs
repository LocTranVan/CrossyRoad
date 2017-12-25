using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using UnityEngine;
public static class FBAchievements{


	public static void GiveAchievement(string achievementUrl)
	{
		//achievementUrl = "https://loctranvan-89.herokuapp.com/index.htm";
		//achievementUrl = "https://achivements2.herokuapp.com/index.htm";
		//achievementUrl = "https://achivements3.herokuapp.com/index.htm";
		//achievementUrl = "https://achivements4.herokuapp.com/index.htm";
		var data = new Dictionary<string, string>() { { "achievement", achievementUrl } };
		FB.API("/me/achievements",
				HttpMethod.POST,
				AchievementCallback,
				data);
	}
	public static void ViewAchievement()
	{
		//var data = new Dictionary<string, string>() { { "achievement", achievementUrl } };
		FB.API("/me/achievements", HttpMethod.GET,
				getAchit);
	}
	public static void getAchit(IGraphResult ir)
	{
		if (ir.Error != null)
		{
			Debug.LogError(ir.Error);
			return;
		}
		Debug.Log(ir.RawResult);

		var scoresList = new List<object>();
		object scoresh;
		if (ir.ResultDictionary.TryGetValue("data", out scoresh))
		{
			scoresList = (List<object>)scoresh;
			List<string> achive = new List<string>();
			foreach (var ob in scoresList)
			{
				var entry = (Dictionary<string, object>)ob;
				var user = (Dictionary<string, object>)entry["data"];
				var achivemeent = (Dictionary<string, object>)user["achievement"];
				achive.Add((string)achivemeent["id"]);
				Debug.Log((string)achivemeent["id"]);
				//string userId = (string)user["id"];
			}
			GameStateManager.Achivements = achive;
			GameStateManager.gAchivementsRedraw();
		}
		
	}
	public static void AchievementCallback(IResult result)
	{
		if (result.Error != null)
		{
			Debug.LogError(result.Error);
			return;
		}
		Debug.Log(result.RawResult);
	}
	public static void DeleteAchievement(string achievementUrl)
	{
		//achievementUrl = "https://loctranvan-89.herokuapp.com/index.htm";
		//achievementUrl = "https://achivements2.herokuapp.com/index.htm";
		//achievementUrl = "https://achivements3.herokuapp.com/index.htm";
		achievementUrl = "https://achivements4.herokuapp.com/index.htm";
		var data = new Dictionary<string, string>() { { "achievement", achievementUrl } };
		FB.API("/me/achievements",
				HttpMethod.DELETE,
				AchievementCallback,
				data);
	}
}
