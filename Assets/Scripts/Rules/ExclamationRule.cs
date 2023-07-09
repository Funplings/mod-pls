using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExclamationRule : Rule
{
    public override bool CheckIfMessageViolatesRule(Message message)
    {
        return (message.GetMessage().Contains("!"));
    }
}
