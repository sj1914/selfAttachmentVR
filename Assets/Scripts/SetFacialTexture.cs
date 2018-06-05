using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Diagnostics;
using System.IO;
using UnityEngine.UI;
using System.Linq;

public class SetFacialTexture : MonoBehaviour {
	
	private string imageLocation;
	public GameObject bust;
	public GameObject avatar;

	//for debugging to screen when in VR
	public Text welcome; 

	void Update () {
		if (avatar.activeInHierarchy){
			StartCoroutine(LoadImageAsTexture ());
		}
	}

//	public void UploadFile () {
//		//can upload a file from within unity resources during development
//		Renderer bustRenderer = bust.GetComponent<Renderer> ();
//		//Load a preexisting texture for now
//		Texture faceTexture = Resources.Load("User_Image/texture", typeof(Texture)) as Texture;
//		if (faceTexture == null)
//			UnityEngine.Debug.Log ("true");
//		bustRenderer.materials [1].mainTexture = faceTexture;
//
//	}
//
	IEnumerator LoadImageAsTexture(){
		//can upload file external from built app
		//works but crashes with access violation
		string directory = "file://" + Application.persistentDataPath + "/User_Image/texture.jpg";

		//DEBUG
		welcome.text = directory;
		UnityEngine.Debug.Log ("HERE: " + Application.persistentDataPath);
		UnityEngine.Debug.Log ("HERE: " + directory);
		//END DEBUG 

		WWW www = new WWW (directory);
		yield return www;
		Texture2D texture = new Texture2D (1024,512);
		www.LoadImageIntoTexture(texture);
		Renderer bustRenderer = bust.GetComponent<Renderer> ();
		bustRenderer.materials [1].mainTexture = www.texture;
	}
		
}
