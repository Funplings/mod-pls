using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellingRule : Rule
{
    public override bool CheckIfMessageViolatesRule(Message message)
    {
        return (message.GetMessage() != message.GetMessage().ToUpper());
    }
}
