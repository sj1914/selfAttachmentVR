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
//	public GameObject[] skinObjects;
	public GameObject[] eyeObjects;
	public GameObject[] endObjects;

	public Text welcome;

	public GameObject avatar;

	//avatar attibutes
	string genderChosen;
	string hairColourChosen;
//	Color skinToneChosen;
	string eyeColourChosen;

	int cullingMask;

	void Start () {
//		FadeTextToFullAlpha (1.0f, welcome);
//		FadeTextToZeroAlpha (2.0f, welcome);
		stage = (int)Stages.Gender;
		cullingMask = this.gameObject.GetComponent<Camera> ().cullingMask;
		SetStage ();
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.X) || OVRInput.GetDown(OVRInput.Button.Three)) {
			SetEyeColour ("brown");
//			SetSkinColour ("skin1");
			SetHairColour ("brunette");
			SetGender ("male");
			SetStage ();
		} else if (Input.GetKeyDown(KeyCode.B) || OVRInput.GetDown(OVRInput.Button.Two)) {
			Restart ();
			SetEyeColour ("green");
//			SetSkinColour ("skin3");
			SetHairColour ("black");
			SetStage ();
		} else if (Input.GetKeyDown(KeyCode.A) || OVRInput.GetDown(OVRInput.Button.One)) {
			
//			SetSkinColour ("skin4");
			SetHairColour ("ginger");
			SetGender ("female");
			SetStage ();
		} else if (Input.GetKeyDown(KeyCode.Y) || OVRInput.GetDown(OVRInput.Button.Four)) {
//			SetSkinColour ("skin2");
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
			SetupAvatar ();
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
		ChangeSkinColour ();
		ChangeEyeColour (eyeColourChosen);
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

	public void SetSkinColour(string skinType) {
//		if (stage == (int)Stages.Skin) {
//			Color skinColour = new Color (255, 214, 197, 0); 
//			switch (skinType) {
//			case "skin1":
//				skinColour = RGBConvert (255, 214, 197, 255);
//				break;
//			case "skin2":
//				skinColour = RGBConvert (232, 184, 148, 255);
//				break;
//			case "skin3":
//				skinColour = RGBConvert (216, 144, 95, 255);
//				break;
//			case "skin4":
//				skinColour = RGBConvert (123, 73, 52, 255);
//				break;
//			default:
//				UnityEngine.Debug.Log ("Default case");
//				break;
//			}
//			skinToneChosen = skinColour;

//			stage = (int)Stages.Final;
//		}
	}
		

	private void ChangeSkinColour(){
		Renderer armsRenderer = arms.GetComponent<Renderer> ();
		Renderer bustRenderer = bust.GetComponent<Renderer> ();

		//need to load this from the same place as the face.
		Texture2D faceTexture = Resources.Load("User_Image/texture", typeof(Texture)) as Texture2D;
		Color[] colours = new Color[3];
		colours[0] = faceTexture.GetPixel (360, 410);
		colours[1] = faceTexture.GetPixel (590, 410);
		colours[2] = faceTexture.GetPixel (460, 550);
		Color skinColour = averageColor (colours);
		armsRenderer.material.color = skinColour;

		//Change the colour of the head
		bustRenderer.materials [0].color = skinColour;
	}

	//Helper function to convert range of RGB from 0-255 to 0-1
//	private static Vector4 RGBConvert(float r, float g, float b, float a){
//		Vector4 color = new Vector4(r/255, g/255, b/255, a/255);
//		return color;
//	}

	private static Color averageColor(Color[] colors) {
		float r = 0.0f;
		float g = 0.0f;
		float b = 0.0f;
		int numOfColours = colors.Length;
		foreach (Color c in colors){
			r += c.r;
			g += c.g;
			b += c.b;
		}
		Color averageColour = new Color (r/numOfColours, g/numOfColours, b/numOfColours);
		return averageColour;
	}

	private void Restart() {
		if (stage == (int)Stages.Final) {
			stage = (int)Stages.Gender;
			this.gameObject.GetComponent<Camera> ().cullingMask = cullingMask;
		}
	}

//	public IEnumerator FadeTextToFullAlpha(float t, Text i)
//	{
//		i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
//		while (i.color.a < 1.0f)
//		{
//			i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
//			yield return null;
//		}
//	}
//
//	public IEnumerator FadeTextToZeroAlpha(float t, Text i)
//	{
//		i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
//		while (i.color.a > 0.0f)
//		{
//			i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
//			yield return null;
//		}
//	}

}
