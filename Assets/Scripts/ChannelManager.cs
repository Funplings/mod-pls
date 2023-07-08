using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChannelManager : MonoBehaviour
{
    [SerializeField]
    [SerializeReference]
    public List<Channel> m_Channels = new List<Channel>();

    [SerializeField]
    public Channel m_selectedChannel;

    public void selectChannel(string channelName)
    {
        foreach (Channel channel in m_Channels)
        {
            if (channel.getChannelName().Equals(channelName))
            {
                m_selectedChannel = channel;
                channel.selectChannel();
                channel.getChannelButtonObject().GetComponent<ChannelButton>().setButtonAsSelected();

            } else
            {
                channel.deselectChannel();
                channel.getChannelButtonObject().GetComponent<ChannelButton>().setButtonAsUnselected();
            }
        }
    }

}
