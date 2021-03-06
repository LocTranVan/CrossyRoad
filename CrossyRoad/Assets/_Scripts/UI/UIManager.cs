﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour {

	// Use this for initialization
	public GameObject PanelParent, btNoSadow, btblockRotation;
	// For Bt More
	public GameObject btPet, btMap, btXhang, btAchivements, btShare;
	// Sprite
	public GameObject[] PanelUnActive, PanelActive;
	public Text txtNameAchivements, txtPoint;
	public Sprite imageBt;
	private Sprite currentImageBt;
	private Image image;
	private bool ON = true;
	private GameObject DirectionalLight;

	private string MUTE = "mute", OnValue = "on", OffValue = "off", QUALITY = "quality", SADOW = "sadow";
	public GameObject[] allSettings;
	private void Start()
	{
		image = GetComponent<Image>();
		if (image != null)
			currentImageBt = image.sprite;
	}

	public void setMute()
	{
		string tam;
		if (ON)
		{
			AudioListener.volume = 0;
			tam = OffValue;
		}
		else
		{
			tam = OnValue;
			AudioListener.volume = 1;
		}
		ChangeSpriteWhenPress();
	}
	public void setResetAlls()
	{
		string tam;
		PlayerPrefs.DeleteAll();
		if (ON)
		{
			tam = OffValue;
			foreach (GameObject ob in allSettings)
			{
				UIManager keys = ob.GetComponent<UIManager>();
				if (!keys.getON())
				{
					keys.setON("");
					switch (keys.name)
					{
						case "btMute":
							keys.setMute();
							break;
						case "btNoShadows":
							keys.turnOffShadow();
							break;
						case "btRotationLock":
							keys.LockRotation();
							break;
						case "BtMore":
							keys.setON("");
							keys.setMore();
							Debug.Log("set More");

							//	keys.ChangeSpriteWhenPress();
							break;
						default:
							break;
					}
				}
			}
		}
		else
		{
			tam = OnValue;
		}
		//	ChangeSpriteWhenPress();
	}
	public void setQuality()
	{
		string tam;
		if (ON)
		{
			QualitySettings.SetQualityLevel(1);
			tam = OffValue;
		}
		else
		{
			tam = OnValue;
			QualitySettings.SetQualityLevel(5);
		}
	}
	public void setMore()
	{
		if (ON)
		{
			btAchivements.SetActive(true);
			btPet.SetActive(true);
			btXhang.SetActive(true);
			btMap.SetActive(true);
			btShare.SetActive(true);
		}
		else
		{
			btPet.SetActive(false);
			btXhang.SetActive(false);
			btMap.SetActive(false);
			btAchivements.SetActive(false);
			btShare.SetActive(false);
		}
		ChangeSpriteWhenPress();
	}
	public void setBack()
	{
		PanelParent.SetActive(false);
	}
	public void setActivePanel()
	{
		foreach (GameObject ob in PanelActive)
		{
			ob.SetActive(true);
		}
		foreach (GameObject ob in PanelUnActive)
		{
			ob.SetActive(false);
		}
	}
	public void setUnActivePanel()
	{
		foreach (GameObject ob in PanelActive)
		{
			ob.SetActive(false);
		}
	}
	public void ExitGame()
	{
		Application.Quit();
	}
	public void turnOffShadow()
	{
		DirectionalLight = GameObject.Find("Directional Light");
		Light light = DirectionalLight.GetComponent<Light>();
		if (light != null)
		{
			if (ON)
				light.shadows = LightShadows.None;
			else
				light.shadows = LightShadows.Hard;
		}
		ChangeSpriteWhenPress();
	}
	public void LockRotation()
	{
		if (ON)
		{
			Screen.orientation = ScreenOrientation.LandscapeLeft;
		}
		else
			Screen.orientation = ScreenOrientation.AutoRotation;
		ChangeSpriteWhenPress();
	}
	public void setON(string tam)
	{
		if (tam == OnValue)
			ON = true;
		else ON = false;
	}
	public bool getON()
	{
		return ON;
	}
	public void ChangeSpriteWhenPress()
	{
		if (ON)
		{
			image.sprite = imageBt;
			ON = false;
			return;
		}
		image.sprite = currentImageBt;
		ON = true;
	}
	public void setAchivements(string txtNameAchivements, string txtPoints)
	{
		this.txtNameAchivements.text = txtNameAchivements;
		this.txtPoint.text = txtPoints;
	}
}
