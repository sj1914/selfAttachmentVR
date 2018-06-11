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
	public GameObject eyeL;
	public GameObject eyeR;

	//Materials
	public Material blonde;
	public Material brunette;
	public Material black;
	public Material ginger;
	public Material blueEyes;
	public Material brownEyes;
	public Material greenEyes;

	//Elements for controlling the menu flow in stages
	enum Stages { Gender, Hair, Eyes, Final };
	private int stage;

	public GameObject[] genderObjects;
	public GameObject[] hairObjects;
	public GameObject[] eyeObjects;
	public GameObject[] endObjects;

	public Text welcome;

	public GameObject avatar;

	//avatar attributes
	string genderChosen;
	string hairColourChosen;
	string eyeColourChosen;

	int cullingMask;

	void Start () {
		stage = (int)Stages.Gender;
		cullingMask = this.gameObject.GetComponent<Camera> ().cullingMask;
		SetStage ();
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.X) || OVRInput.GetDown(OVRInput.Button.Three)) {
			SetEyeColour ("brown");
			SetHairColour ("brunette");
			SetGender ("male");
			SetStage ();
		} else if (Input.GetKeyDown(KeyCode.B) || OVRInput.GetDown(OVRInput.Button.Two)) {
			Restart ();
			SetEyeColour ("green");
			SetHairColour ("black");
			SetStage ();
		} else if (Input.GetKeyDown(KeyCode.A) || OVRInput.GetDown(OVRInput.Button.One)) {
			SetHairColour ("ginger");
			SetGender ("female");
			SetStage ();
		} else if (Input.GetKeyDown(KeyCode.Y) || OVRInput.GetDown(OVRInput.Button.Four)) {
			SetEyeColour ("blue");
			SetHairColour ("blonde");
			SetStage ();
		}
	}

	private void SetStage() {
		if (stage == (int)Stages.Gender) {
			foreach (GameObject obj in hairObjects) {
				obj.SetActive (false);
			}
			foreach (GameObject obj in eyeObjects) {
				obj.SetActive (false);
			}
			foreach (GameObject obj in endObjects) {
				obj.SetActive (false);
			}
			foreach (GameObject obj in genderObjects) {
				obj.SetActive (true);
			}
		} else if (stage == (int)Stages.Hair) {
			foreach (GameObject obj in hairObjects) {
				obj.SetActive (true);
			}
			foreach (GameObject obj in genderObjects) {
				obj.SetActive (false);
			}
			foreach (GameObject obj in endObjects) {
				obj.SetActive (false);
			}
			foreach (GameObject obj in eyeObjects) {
				obj.SetActive (false);
			}
		} else if (stage == (int)Stages.Eyes) {
			foreach (GameObject obj in hairObjects) {
				obj.SetActive (false);
			}
			foreach (GameObject obj in genderObjects) {
				obj.SetActive (false);
			}
			foreach (GameObject obj in endObjects) {
				obj.SetActive (false);
			}
			foreach (GameObject obj in eyeObjects) {
				obj.SetActive (true);
			}
		} else if (stage == (int)Stages.Final) {
			
			foreach (GameObject obj in hairObjects) {
				obj.SetActive (false);
			}
			foreach (GameObject obj in genderObjects) {
				obj.SetActive (false);
			}
			foreach (GameObject obj in eyeObjects) {
				obj.SetActive (false);
			}
			foreach (GameObject obj in endObjects) {
				obj.SetActive (true);
			}
			SetupAvatar ();
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
		ChangeEyeColour (eyeColourChosen);
		ChangeGender (genderChosen);
		ChangeHairColour (hairColourChosen);

	}

	public void SetHairColour(string colour) {
		if (stage == (int)Stages.Hair) {
			hairColourChosen = colour;
			stage = (int)Stages.Eyes;
		}
	}

	public void SetEyeColour(string colour) {
		if (stage == (int)Stages.Eyes) {
			eyeColourChosen = colour;
			stage = (int)Stages.Final;
		}
	}

	private void ChangeEyeColour(string colour){
		Renderer eyeLRenderer = eyeL.GetComponent<Renderer> ();
		Renderer eyeRRenderer = eyeR.GetComponent<Renderer> ();
		switch (colour) {
		case "blue":
			eyeLRenderer.material = blueEyes;
			eyeRRenderer.material = blueEyes;
			break;
		case "brown":
			eyeLRenderer.material = brownEyes;
			eyeRRenderer.material = brownEyes;
			break;
		case "green":
			eyeLRenderer.material = greenEyes;
			eyeRRenderer.material = greenEyes;
			break;
		default:
			UnityEngine.Debug.Log ("Default case");
			break;
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
			bustRenderer.materials[2] = brunette;
			break;
		case "blonde":
			boyRenderer.material = blonde;
			girlRenderer.material = blonde;
			bustRenderer.materials[2] = blonde;
			break;
		case "ginger":
			boyRenderer.material = ginger;
			girlRenderer.material = ginger;
			bustRenderer.materials[2] = ginger;
			break;
		case "black":
			boyRenderer.material = black;
			girlRenderer.material = black;
			bustRenderer.materials[2] = black;
			break;
		default:
			UnityEngine.Debug.Log ("Default case");
			break;
		}
	}

	private void Restart() {
		if (stage == (int)Stages.Final) {
			stage = (int)Stages.Gender;
			this.gameObject.GetComponent<Camera> ().cullingMask = cullingMask;
		}
	}

}
