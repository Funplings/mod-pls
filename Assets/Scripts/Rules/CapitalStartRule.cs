using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CapitalStartRule : Rule
{
    public override bool CheckIfMessageViolatesRule(Message message)
    {
        string strMessage = message.ToString();
        int i = strMessage.IndexOf(strMessage.FirstOrDefault(char.IsLetter));

        if (i < 0) return false;

        return !System.Char.IsUpper(strMessage[i]);
    }
}
