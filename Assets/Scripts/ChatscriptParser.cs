using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;

public static class ChatscriptParser
{
    const string s_strUnparsed = "/Unparsed Chatscripts/";
    const string s_strUsers = "Assets/AllUserData.asset";

    [MenuItem("mod-pls/ParseChats")]
    public static void ParseChats()
    {
        AllUserData allUserData = AssetDatabase.LoadAssetAtPath<AllUserData>(s_strUsers);

        if (!allUserData)
            throw new System.ArgumentException("No user data defined!");

        string[] strFiles = Utils.DirFiles(s_strUnparsed);

        foreach (string strFile in strFiles)
        {
            ParseChatscript(allUserData, strFile);
        }

        // Updated the asset database

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static bool IsUserReal(AllUserData allUserData, string strUser)
    {
        return allUserData.m_users.Find(user => user.m_strName == strUser) != null;
    }

    private static void ParseChatscript(AllUserData allUserData, string strFile)
    {
        const string extension = ".txt";
        string strTrimmed = strFile.Substring(0, strFile.Length - extension.Length);

        Regex commandRegex = new Regex(@"(?m)^((\S+):|\[(.*?)\])");

        string strParsed = string.Format("Assets/Parsed Chatscripts/{0}.asset", strTrimmed);

        ChatscriptData chatscriptData;
        chatscriptData = AssetDatabase.LoadAssetAtPath<ChatscriptData>(strParsed);

        if (!chatscriptData)
        {
            chatscriptData = ScriptableObject.CreateInstance<ChatscriptData>();
            AssetDatabase.CreateAsset(chatscriptData, strParsed);
        }

        string strText = Utils.ReadFile(s_strUnparsed + strFile);

        // Parse to a temporary list of commands

        chatscriptData.m_messages.Clear();

        List<ChatscriptCommand> commands = new List<ChatscriptCommand>();

        MatchCollection matches = commandRegex.Matches(strText);

        for (int iMatch = 0; iMatch < matches.Count; iMatch++)
        {
            Match match = matches[iMatch];
            bool isMessage = match.Groups[2].Success;

            if (isMessage)
            {
                Group group = match.Groups[1];
                int iStart = group.Index + group.Length;
                int iEnd = (iMatch == matches.Count - 1) ? strText.Length : matches[iMatch + 1].Index;

                string strMessage = strText.Substring(iStart, iEnd - iStart).Trim();

                string strUser = match.Groups[2].ToString();

                if (!IsUserReal(allUserData, strUser))
                    throw new System.ArgumentException("Undefined user: " + strUser);

                MessageCommand messageCommand = new MessageCommand(strUser, strMessage);
                commands.Add(messageCommand);
            }
            else
            {
                string[] strWords = match.Groups[3].ToString().Split(' ');

                string strCommand = strWords[0];

                switch (strCommand)
                {
                    case "Hour":

                        float hour = Utils.HoursLinear(float.Parse(strWords[1]));

                        HourCommand hourCommand = new HourCommand(hour);
                        commands.Add(hourCommand);
                        break;

                    default:
                        throw new System.ArgumentException();
                }
            }
        }

        // Do a second pass to write hours down for messages

        float hourNow = 13;
        float hourLater = Utils.HoursLinear(5); // Work in linear time range
        int iNow = 0;
        int iLater = commands.Count - 1;

        for (int iCommand = 0; iCommand < commands.Count; iCommand++)
        {
            ChatscriptCommand command = commands[iCommand];

            switch (command.m_type)
            {
                case ChatscriptCommand.TYPE.MESSAGE:
                    MessageCommand messageCommand = (MessageCommand)command;
                    float hour = Utils.Map(iNow, iLater, hourNow, hourLater, iCommand);
                    messageCommand.m_hourSent = Utils.HoursLinear(hour);
                    chatscriptData.m_messages.Add(messageCommand);
                    break;

                case ChatscriptCommand.TYPE.HOUR:
                    HourCommand hourCommand = (HourCommand)command;
                    hourNow = Utils.HoursLinear(hourCommand.m_hour);
                    iNow = iCommand;

                    // Look for next

                    hourLater = Utils.HoursLinear(5);
                    iLater = commands.Count - 1;

                    for (int i = iNow + 1; i < commands.Count; i++)
                    {
                        if (commands[i].m_type == ChatscriptCommand.TYPE.HOUR)
                        {
                            HourCommand laterCommand = (HourCommand)commands[i];
                            hourLater = Utils.HoursLinear(laterCommand.m_hour);
                            iLater = i;
                            break;
                        }
                    }

                    break;

                default:
                    throw new System.ArgumentException();
            }

        }

        // Set data dirty

        EditorUtility.SetDirty(chatscriptData);
    }
}
