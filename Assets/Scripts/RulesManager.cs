using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RulesManager : MonoBehaviour
{
    [SerializeField] private List<Rule> m_universalRules;
    private Dictionary<string, List<Rule>> m_channelRules;

    public static RulesManager Instance { get; private set; }
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

        // Set up channel rules dictionary
        m_channelRules = new Dictionary<string, List<Rule>>();
        foreach (Channel channel in ChannelManager.Instance.m_Channels)
        {
            m_channelRules.Add(channel.getChannelName(), channel.getChannelRules());
        }
    }

    public bool ValidateMessage(Message message)
    {
        // Check universal rules
        foreach (Rule rule in m_universalRules)
        {
            if (rule.CheckIfMessageViolatesRule(message)) return true;
        }

        // Check channel specific rules
        List<Rule> channelRules;
        if (m_channelRules.TryGetValue(message.GetChannelName(), out channelRules)) {
            foreach (Rule rule in channelRules)
            {
                if (rule.CheckIfMessageViolatesRule(message)) return true;
            }
        }

        // If no rules violated, return false
        return false;
    }

    public List<Rule> GetUniversalRules()
    {
        return m_universalRules;
    }
}
