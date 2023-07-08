using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChannelManager : MonoBehaviour
{
    [SerializeField]
    [SerializeReference]
    public List<Channel> m_Channels = new List<Channel>();

    [SerializeField]
    private Channel m_selectedChannel;

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

    void Start()
    {
        SelectChannel("general");
    }

    public void SelectChannel(string channelName)
    {
        Debug.Log("Selecting channel");
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
        Debug.Log("Bout to Set Text");
        RulesDisplay.Instance.SetText();
    }

    public Channel GetSelectedChannel()
    {
        return m_selectedChannel;
    }

}
