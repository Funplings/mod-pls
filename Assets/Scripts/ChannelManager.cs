using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChannelManager : MonoBehaviour
{
    [SerializeField]
    [SerializeReference]
    public List<Channel> m_Channels = new List<Channel>();

    [SerializeField]
    private Channel m_selectedChannel;

    [Header("Scene Components")]
    [SerializeField] private TextMeshProUGUI m_CurrentChannelHeader;

    private bool m_dayStarted = false;

    public static ChannelManager Instance { get; private set; }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void Start()
    {
        SelectChannel(m_selectedChannel.getChannelName());
    }

    public void SelectChannel(string channelName)
    {
        foreach (Channel channel in m_Channels)
        {
            if (channel.getChannelName().Equals(channelName))
            {
                m_selectedChannel = channel;
                channel.selectChannel();
                channel.getChannelButtonObject().GetComponent<ChannelButton>().setButtonAsSelected();

            }
            else
            {
                channel.deselectChannel();
                channel.getChannelButtonObject().GetComponent<ChannelButton>().setButtonAsUnselected();
            }
        }
        m_CurrentChannelHeader.text = string.Format("#{0}", m_selectedChannel.getChannelName());
        RulesDisplay.Instance.SetText();
    }

    public void StartDay()
    {
        this.m_dayStarted = true;
    }

    /* GETTERS */
    public Channel GetSelectedChannel()
    {
        return m_selectedChannel;
    }

    public bool GetDayStarted()
    {
        return m_dayStarted;
    }

}
