using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChannelButton : MonoBehaviour
{
    [SerializeField] private string m_ChannelName;
    [SerializeField] private Button m_Button;
	[SerializeField] private TextMeshProUGUI m_tmpText;

    void Start()
    {
        m_Button.onClick.AddListener(onClickChannelButton);
    }

    void onClickChannelButton()
    {
        AudioManager.instance.PlaySFX("Change_Channel");
        ChannelManager.Instance.SelectChannel(m_ChannelName);
    }

    public void setButtonAsSelected()
    {
        m_Button.interactable = false;
		GetComponent<Animator>().SetBool("unread", false);
    }

    public void setButtonAsUnselected()
    {
        m_Button.interactable = true;
    }

    public void SetUnread()
    {
		GetComponent<Animator>().SetBool("unread", true);
    }
}
