using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Rule : MonoBehaviour
{
    [SerializeField] private string m_description;
    [SerializeField] private string m_channel;

    public abstract bool CheckIfMessageViolatesRule(Message message);

    public string getDescription()
    {
        return m_description;
    }

    public string getChannel()
    {
        return m_channel;
    }
}
