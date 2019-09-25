using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

public class WebCamTextureSender : NetworkBehaviour {

	public WebCamTexture webCamTexture;
    public Color32[] data;
	public Texture2D texture;
	
	
	// Use this for initialization
	void OnEnable () {

		if (hasAuthority == false)
		{
			return;
		}

		
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if (hasAuthority == false)
		{
			return;
		}
		webCamTexture = new WebCamTexture();
		webCamTexture.Play();

		data = new Color32[webCamTexture.width * webCamTexture.height];
		texture = new Texture2D(webCamTexture.width, webCamTexture.height);

		if (webCamTexture.isPlaying)
		{
			webCamTexture.GetPixels32(data);
			texture.SetPixels32(data);
			texture.Apply();
			var bytes = texture.EncodeToJPG();
		}


	}



	void SendTexture(byte [] bytes)
	{

	}


}
