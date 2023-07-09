using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Rule : MonoBehaviour
{
    [SerializeField] private string m_description;

    public abstract bool CheckIfMessageViolatesRule(Message message);

    public string getDescription()
    {
        return m_description;
    }
}
