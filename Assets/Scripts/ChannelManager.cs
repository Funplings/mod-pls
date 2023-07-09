using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class ChannelManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField]
    public AllUserData m_allUsers;

    [Header("References")]
    [SerializeField]
    [SerializeReference]
    public List<Channel> m_Channels = new List<Channel>();

    [SerializeField]
    private Channel m_selectedChannel;

    [Header("Scene Components")]
    [SerializeField] private TextMeshProUGUI m_CurrentChannelHeader;

    private float m_timeDayStart = 0;   // 0 = day hasn't started
    private int m_day = 0;

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

    void Update()
    {
        // End game at 5:30, give exact time for moderating

        if (CurrentGameTime() >= Utils.HoursLinear(5.5f))
        {
            // Run end of day sequence

            print("DAY COMPLETE");
            m_day++;
            StartDay();
        }
    }

    public void GetPlayerMessage(string strInput)
    {
        if (IsTimeFrozen() && strInput.Contains("yes"))
        {
            ResumeTime();
        }

        m_selectedChannel.PushMessage(GameManager.instance.m_strPlayer, GameManager.instance.m_spritePlayer, strInput);
    }

    public Sprite SpriteForUser(string strUser)
    {
        return m_allUsers.User(strUser).m_spriteProfile;
    }

    public bool IsTimeFrozen()
    {
        return m_timeDayStart == 0;
    }

    public float CurrentGameTime()
    {
        // Start with 13 because we start our days at 1 PM
        float hours = 13;

        if (!IsTimeFrozen())
        {
            float dSec = Time.time - m_timeDayStart;

            hours += dSec / Constants.REAL_SECOND_TO_GAME_HOUR_RATIO;
        }

        return hours;
    }

    public string CurrentGameTimeString()
    {
        float hours = CurrentGameTime();

        if (hours >= 24)
        {
            hours -= 24;
        }

        System.TimeSpan span = System.TimeSpan.FromHours(hours);
        return new System.DateTime().Add(span).ToShortTimeString();
    }

    public void StartDay()
    {
        SelectChannel("announcements");

        // Setup channels

        foreach (Channel channel in m_Channels)
        {
            channel.SetupForDay(m_day);
        }
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

    public void ResumeTime()
    {
        m_timeDayStart = Time.time; // Makes time move again, so messages in other channels will start
    }

    /* GETTERS */
    public Channel GetSelectedChannel()
    {
        return m_selectedChannel;
    }
}
