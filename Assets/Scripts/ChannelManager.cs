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

    [SerializeField]
    private GameObject m_goodPrefab;
    [SerializeField]
    private GameObject m_badPrefab;

    [SerializeField]
    private Sprite m_spritePlayer;

    [Header("Scene Components")]
    [SerializeField] private TextMeshProUGUI m_CurrentChannelHeader;
    [SerializeField] private TextMeshProUGUI m_CurrentChannelRule;
    [SerializeField] private Canvas m_canvas;
    [SerializeField] private DayEnd m_dayEnd;


    private float m_timeDayStart = 0;   // 0 = day hasn't started
    public int m_day = 0;

    private int m_cMistake = 0;
    private int m_cCorrect = 0;

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
            m_dayEnd.Trigger(GetResults());
            m_day++;
            m_timeDayStart = 0;
            m_cMistake = 0;
            m_cCorrect = 0;
        }
    }

    private Results GetResults()
    {
        Results results = new Results();

        results.cCorrect = m_cCorrect;
        results.cMistake = m_cMistake;

        int totalViolations = 0;

        foreach (Channel channel in m_Channels)
        {
            if (channel.Available())
            {
                totalViolations += channel.CViolations();
            }
        }

        results.cMiss = totalViolations - m_cCorrect;

        return results;
    }

    public void Delete(bool wasSuccess)
    {
        GameObject prefab = wasSuccess ? m_goodPrefab : m_badPrefab;

        if (wasSuccess) m_cCorrect++;
        else m_cMistake++;

        Vector3 mousePos = Input.mousePosition;

        GameObject notif = Instantiate(prefab, mousePos, Quaternion.identity);
        notif.transform.SetParent(m_canvas.transform, false);
        notif.GetComponent<RectTransform>().position = mousePos;
    }

    public void GetPlayerMessage(string strInput)
    {
        if (IsTimeFrozen() && strInput.ToLower().Contains("yes"))
        {
            ResumeTime();
        }

        m_selectedChannel.PushMessage(GameManager.instance.m_strPlayer, m_spritePlayer, strInput);
    }

    public Sprite SpriteForUser(string strUser)
    {
        return m_allUsers.User(strUser).m_spriteProfile;
    }

    public void Reset()
    {
        m_day = 0;
        m_timeDayStart = 0;
        m_cMistake = 0;
        m_cCorrect = 0;
    }

    public bool IsTimeFrozen()
    {
        return m_timeDayStart == 0;
    }

    public float StartHour()
    {
        if(m_day == 0) return 24;
        else return 13;
    }

    public float CurrentGameTime()
    {
        float hours = StartHour();

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


        List<Rule> channelRules = m_selectedChannel.getChannelRules();

        if (channelRules.Count == 1)
        {
            m_CurrentChannelRule.text = channelRules[0].getDescription();
        }
        else if (channelRules.Count == 2)
        {
            throw new System.ArgumentException("More than 1 rule for channel!");
        }
        else
        {
            m_CurrentChannelRule.text = "";
        }


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

    // End of Day

    public struct Results
    {
        public int cCorrect;
        public int cMistake;
        public int cMiss;

        public int Total()
        {
            return cCorrect - cMistake - cMiss;
        }
    }
}
