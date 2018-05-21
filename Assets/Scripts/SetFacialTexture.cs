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

	void Update () {
		if (avatar.activeInHierarchy){
			UploadFile ();
		}
	}

	public void UploadFile () {

		Renderer bustRenderer = bust.GetComponent<Renderer> ();
		//Load a preexisting texture for now
		Texture faceTexture = Resources.Load("User_Image/texture", typeof(Texture)) as Texture;
		if (faceTexture == null)
			UnityEngine.Debug.Log ("true");
		bustRenderer.materials [2].mainTexture = faceTexture;

	}
		
}
