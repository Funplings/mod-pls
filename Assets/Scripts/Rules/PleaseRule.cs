using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PleaseRule : Rule
{
    public override bool CheckIfMessageViolatesRule(Message message)
    {
        return (message.GetMessage().ToLower().Contains("please"));
    }
}
