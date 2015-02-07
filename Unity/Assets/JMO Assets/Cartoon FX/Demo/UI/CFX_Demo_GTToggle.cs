using UnityEngine;
using System.Collections;

// Cartoon FX - (c) 2015 - Jean Moreno
//
// Script for the Demo scene

public class CFX_Demo_GTToggle : MonoBehaviour
{
	public Texture Normal, Hover;
	public Color NormalColor = new Color32(128,128,128,128), DisabledColor = new Color32(128,128,128,48);
	public bool State = true;
	
	public string Callback;
	public GameObject Receiver;
	
	private Rect CollisionRect;
	private bool Over;
	private GUIText Label;
	
	//-------------------------------------------------------------
	
	void Awake()
	{
		CollisionRect = this.guiTexture.GetScreenRect(Camera.main);
		Label = this.GetComponentInChildren<GUIText>();
		
		UpdateTexture();
	}
	
	void Update ()
	{
		if(CollisionRect.Contains(Input.mousePosition))
		{
			Over = true;
			if(Input.GetMouseButtonDown(0))
			{
				OnClick();
			}
		}
		else
		{
			Over = false;
			this.guiTexture.color = NormalColor;
		}
		
		UpdateTexture();
	}
	
	//-------------------------------------------------------------
	
	private void OnClick()
	{
		State = !State;
		
		Receiver.SendMessage(Callback);
	}
	
	private void UpdateTexture()
	{
		Color col = State ? NormalColor : DisabledColor;
		if(Over)
		{
			this.guiTexture.texture = Hover;
		}
		else
			this.guiTexture.texture = Normal;
		
		this.guiTexture.color = col;
		
		if(Label != null)
			Label.color = col * 1.75f;
	}
}
