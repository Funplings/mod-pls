using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChannelButton : MonoBehaviour
{
	[SerializeField] private string m_ChannelName;
	[SerializeField] private Button m_Button;
	
	void Start()
	{
		m_Button.onClick.AddListener(onClickChannelButton);
	}

	void onClickChannelButton()
	{
		ChannelManager.Instance.SelectChannel(m_ChannelName);
	}

	public void setButtonAsSelected()
    {
		// Set transparency of button to 255 (opaque)
		Utils.setImageAlpha(m_Button.image, 255);
	}

	public void setButtonAsUnselected()
	{
		// Set transparency of image to 0 (transparent)
		Utils.setImageAlpha(m_Button.image, 0);
	}
}
