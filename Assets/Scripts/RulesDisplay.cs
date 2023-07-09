using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RulesDisplay : MonoBehaviour
{
    public static RulesDisplay Instance { get; private set; }

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

    [SerializeField] private TextMeshProUGUI m_TextMesh;
    private string m_Text;

    public void SetText()
    {
        m_Text = "";
        List<Rule> universalRules = RulesManager.Instance.GetUniversalRules();

        // List<Rule> channelRules = new List<Rule>();
        // Channel selectedChannel = ChannelManager.Instance.GetSelectedChannel();
        // if (selectedChannel != null)
        // {
        //     channelRules = selectedChannel.getChannelRules();
        // }

        for (int iRule = 0; iRule < universalRules.Count; iRule++)
        {
            if (iRule > ChannelManager.Instance.m_day) break;

            m_Text += string.Format("{0} \n\n", universalRules[iRule].getDescription());
        }

        // foreach (Rule rule in channelRules)
        // {
        //     m_Text += string.Format("(#{0}) {1} \n\n", selectedChannel.getChannelName(), rule.getDescription());
        // }

        m_TextMesh.text = m_Text;
    }
}
