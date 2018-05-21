using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class Customisation : MonoBehaviour {

	public EventSystem eventSystem;

	//GameObjects to be customised
	public GameObject boyHair;
	public GameObject girlHair;

	public GameObject arms;
	public GameObject bust;

	//Materials
	public Material blonde;
	public Material brunette;
	public Material black;
	public Material ginger;

	//Elements for controlling the menu flow in stages
	enum Stages { Gender, Hair, Skin, Final };
	private int stage;

	public GameObject[] genderObjects;
	public GameObject[] hairObjects;
	public GameObject[] skinObjects;
	public GameObject restart;

	public GameObject avatar;

	//avatar attibutes
	string genderChosen;
	string hairColourChosen;
	Color skinToneChosen;

	int cullingMask;

	void Start () {
		stage = (int)Stages.Gender;
		cullingMask = this.gameObject.GetComponent<Camera> ().cullingMask;
		SetStage ();
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.X) || OVRInput.GetDown(OVRInput.Button.Three)) {
			SetSkinColour ("skin1");
			SetHairColour ("brunette");
			SetGender ("male");
			SetStage ();
		} else if (Input.GetKeyDown(KeyCode.B) || OVRInput.GetDown(OVRInput.Button.Two)) {
			Restart ();
			SetSkinColour ("skin3");
			SetHairColour ("black");
			SetStage ();
		} else if (Input.GetKeyDown(KeyCode.A) || OVRInput.GetDown(OVRInput.Button.One)) {
			SetSkinColour ("skin4");
			SetHairColour ("ginger");
			SetGender ("female");
			SetStage ();
		} else if (Input.GetKeyDown(KeyCode.Y) || OVRInput.GetDown(OVRInput.Button.Four)) {
			SetSkinColour ("skin2");
			SetHairColour ("blonde");
			SetStage ();
		}
	}

	private void SetStage() {
		if (stage == (int)Stages.Gender) {
			foreach (GameObject obj in hairObjects) {
				obj.SetActive (false);
			}
			foreach (GameObject obj in skinObjects) {
				obj.SetActive (false);
			}
			foreach (GameObject obj in genderObjects) {
				obj.SetActive (true);
			}
			restart.SetActive (false);
		} else if (stage == (int)Stages.Hair) {
			foreach (GameObject obj in hairObjects) {
				obj.SetActive (true);
			}
			foreach (GameObject obj in skinObjects) {
				obj.SetActive (false);
			}
			foreach (GameObject obj in genderObjects) {
				obj.SetActive (false);
			}
			restart.SetActive(false);
			avatar.SetActive (true);
		} else if (stage == (int)Stages.Skin) {
			foreach (GameObject obj in hairObjects) {
				obj.SetActive (false);
			}
			foreach (GameObject obj in skinObjects) {
				obj.SetActive (true);
			}
			foreach (GameObject obj in genderObjects) {
				obj.SetActive (false);
			}
			restart.SetActive(false);
		} else if (stage == (int)Stages.Final) {
			SetupAvatar ();
			foreach (GameObject obj in hairObjects) {
				obj.SetActive (false);
			}
			foreach (GameObject obj in skinObjects) {
				obj.SetActive (false);
			}
			foreach (GameObject obj in genderObjects) {
				obj.SetActive (false);
			}
			restart.SetActive(true);
		}
	}

	public void SetGender(string gender) {
		if (stage==(int)Stages.Gender) {
			genderChosen = gender;
			stage = (int)Stages.Hair;
		}
	}

	private void ChangeGender(string gender) {
		if (gender == "female") {
			girlHair.SetActive (true);
			boyHair.SetActive (false);
		} else if (gender == "male"){
			boyHair.SetActive (true);
			girlHair.SetActive (false);
		}
	}

	private void SetupAvatar() {
		this.gameObject.GetComponent<Camera>().cullingMask |= 1 << LayerMask.NameToLayer("HiddenChild");
		ChangeGender (genderChosen);
		ChangeHairColour (hairColourChosen);
		ChangeSkinColour (skinToneChosen);
	}

	public void SetHairColour(string colour) {
		if (stage == (int)Stages.Hair) {
			hairColourChosen = colour;
			stage = (int)Stages.Skin;
		}
	}

	private void ChangeHairColour(string colour){
		Renderer boyRenderer = boyHair.GetComponent<Renderer> ();
		Renderer girlRenderer = girlHair.GetComponent<Renderer> ();
		Renderer bustRenderer = bust.GetComponent<Renderer> ();
		switch (colour) {
		case "brunette":
			boyRenderer.material = brunette;
			girlRenderer.material = brunette;
			bustRenderer.material = brunette;
			break;
		case "blonde":
			boyRenderer.material = blonde;
			girlRenderer.material = blonde;
			bustRenderer.material = blonde;
			break;
		case "ginger":
			boyRenderer.material = ginger;
			girlRenderer.material = ginger;
			bustRenderer.material = ginger;
			break;
		case "black":
			boyRenderer.material = black;
			girlRenderer.material = black;
			bustRenderer.material = black;
			break;
		default:
			UnityEngine.Debug.Log ("Default case");
			break;
		}
	}

	public void SetSkinColour(string skinType) {
		if (stage == (int)Stages.Skin) {
			Color skinColour = new Color (255, 214, 197, 0); 
			switch (skinType) {
			case "skin1":
				skinColour = RGBConvert (255, 214, 197, 255);
				break;
			case "skin2":
				skinColour = RGBConvert (232, 184, 148, 255);
				break;
			case "skin3":
				skinColour = RGBConvert (216, 144, 95, 255);
				break;
			case "skin4":
				skinColour = RGBConvert (123, 73, 52, 255);
				break;
			default:
				UnityEngine.Debug.Log ("Default case");
				break;
			}
			skinToneChosen = skinColour;

			stage = (int)Stages.Final;
		}
	}

	private void ChangeSkinColour(Color colour){
		Renderer armsRenderer = arms.GetComponent<Renderer> ();
		Renderer bustRenderer = bust.GetComponent<Renderer> ();
		armsRenderer.material.color = colour;

		//Change the colour of the head
		bustRenderer.materials [1].color = colour;
		//Temporarily change the colour of the face too so that it doesn't look weird
		bustRenderer.materials [2].color = colour;
	}

	//Helper function to convert range of RGB from 0-255 to 0-1
	private static Vector4 RGBConvert(float r, float g, float b, float a){
		Vector4 color = new Vector4(r/255, g/255, b/255, a/255);
		return color;
	}

	private void Restart() {
		if (stage == (int)Stages.Final) {
			stage = (int)Stages.Gender;
			this.gameObject.GetComponent<Camera> ().cullingMask = cullingMask;
		}
	}

}
