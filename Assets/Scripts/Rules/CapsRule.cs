using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CapsRule : Rule
{
    public override bool CheckIfMessageViolatesRule(Message message)
    {
        string strMessage = message.GetMessage();
        int i = strMessage.IndexOf(strMessage.FirstOrDefault(c => char.IsLetter(c) && char.IsLower(c)));

        return i >= 0;
    }
}
