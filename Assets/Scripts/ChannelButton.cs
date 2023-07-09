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
		m_Button.interactable = false;		
	}

	public void setButtonAsUnselected()
	{
		m_Button.interactable = true;		
	}
}
