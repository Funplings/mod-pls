using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChannelButton : MonoBehaviour
{
	[SerializeField] private string m_ChannelName;
	[SerializeField] private Button m_Button;
	[SerializeField] private ChannelManager m_ChannelManager;
	
	void Start()
	{
		m_Button.onClick.AddListener(onClickChannelButton);
	}

	void onClickChannelButton()
	{
		m_ChannelManager.selectChannel(m_ChannelName);
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
