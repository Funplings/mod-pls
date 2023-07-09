using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class LeetRule : Rule
{
    public override bool CheckIfMessageViolatesRule(Message message)
    {
        Regex regex = new Regex("e|a|E|A");
        return regex.IsMatch(message.GetMessage());
    }
}
