using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChannelManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField]
    [SerializeReference]
    public AllUserData m_allUsers;

    [Header("References")]
    [SerializeField]
    [SerializeReference]
    public List<Channel> m_Channels = new List<Channel>();

    [SerializeField]
    private Channel m_selectedChannel;

    [Header("Scene Components")]
    [SerializeField] private TextMeshProUGUI m_CurrentChannelHeader;

    private bool m_dayStarted = false;
    private float m_timeStarted = 0;

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
        m_timeStarted = Time.time;
    }

    public void GetPlayerMessage(string strInput)
    {
        if (strInput == "yes")
        {
            ChannelManager.Instance.StartDay();
        }

        m_selectedChannel.PushMessage(GameManager.instance.m_strPlayer, GameManager.instance.m_spritePlayer, strInput);
    }

    public Sprite SpriteForUser(string strUser)
    {

        return m_allUsers.m_users.Find(user => user.m_strName == strUser).m_spriteProfile;
    }

    public string CurrentGameTime()
    {
        float dSec = Time.time - m_timeStarted;

        float hours = 13 + (dSec / Constants.REAL_SECOND_TO_GAME_MINUTE_RATIO) / 60; // Add 13 because we start our days at 1 PM

        if (hours >= 24)
        {
            hours -= 24;
        }

        System.TimeSpan span = System.TimeSpan.FromHours(hours);
        return new System.DateTime().Add(span).ToShortTimeString();
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
        m_timeStarted = Time.time;
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
