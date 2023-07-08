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
        Debug.Log("Set Text");
        m_Text = "";
        List<Rule> universalRules = RulesManager.Instance.GetUniversalRules();

        List<Rule> channelRules = new List<Rule>();
        Channel selectedChannel = ChannelManager.Instance.GetSelectedChannel();
        if (selectedChannel != null)
        {
            channelRules = selectedChannel.getChannelRules();
        }

        foreach (Rule rule in universalRules)
        {
            m_Text += string.Format("{0} \n\n", rule.getDescription());
        }

        foreach (Rule rule in channelRules)
        {
            m_Text += string.Format("(#{0}) {1} \n\n", rule.getChannel(), rule.getDescription());
        }

        m_TextMesh.text = m_Text;
    }
}
