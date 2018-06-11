using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Diagnostics;
using System.IO;
using UnityEngine.UI;

public class SetFacialTexture : MonoBehaviour {
	
	public GameObject bust;
    public GameObject arms;
    public GameObject avatar;

    Texture2D texture;

	string pathPreFix;
	bool complete = false;

	void Update () {
		if (avatar.activeInHierarchy){
			//StartCoroutine(WaitTillDone());
            //ChangeSkinColour();
        }
	}

	IEnumerator LoadImageAsTexture(){

        string directory = Application.dataPath + "/../User_Image/texture.jpg";

        pathPreFix = @"file:///";

        string path = pathPreFix + directory;
		WWW www = new WWW (path);
		yield return www;
		texture = new Texture2D (1024,512);
		www.LoadImageIntoTexture(texture);

		Renderer bustRenderer = bust.GetComponent<Renderer> ();
		bustRenderer.materials [1].mainTexture = www.texture;

		complete = true;
    }

    private void ChangeSkinColour()
    {
        Renderer armsRenderer = arms.GetComponent<Renderer>();
        Renderer bustRenderer = bust.GetComponent<Renderer>();

        Color[] colours = new Color[3];
        colours[0] = texture.GetPixel(360, 410);
        colours[1] = texture.GetPixel(590, 410);
        colours[2] = texture.GetPixel(460, 550);
        Color skinColour = averageColor(colours);

        armsRenderer.materials[0].color = skinColour;
        bustRenderer.materials[0].color = skinColour;
    }

    private IEnumerator WaitTillDone(){
		StartCoroutine (LoadImageAsTexture ());
		yield return new WaitUntil(() => complete == true);
		DynamicGI.UpdateEnvironment ();
	}

    private static Color averageColor(Color[] colors)
    {
        float r = 0.0f;
        float g = 0.0f;
        float b = 0.0f;
        int numOfColours = colors.Length;
        foreach (Color c in colors)
        {
            r += c.r;
            g += c.g;
            b += c.b;
        }
        Color averageColour = new Color(r / numOfColours, g / numOfColours, b / numOfColours);
        return averageColour;
    }

}
