
using UnityEngine;
[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class WebCamTextureReceiver : MonoBehaviour
{
	public GameObject Sender;
	private WebCamTextureSender sender;
	Texture2D texture;
	void Start()
	{
		Sender = GameObject.Find("/Sender");
		sender = Sender.GetComponent<WebCamTextureSender>();
		texture = sender.texture;
		GetComponent<MeshRenderer>().material.mainTexture = texture;
	}

	void ReceiveTexture()
	{

	}
}