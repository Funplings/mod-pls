using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;

public static class ChatscriptParser
{
    const string s_strUnparsed = "/Unparsed Chatscripts/";

    [MenuItem("mod-pls/ParseChats")]
    public static void ParseChats()
    {
        string[] strFiles = Utils.DirFiles(s_strUnparsed);

        foreach (string strFile in strFiles)
        {
            ParseChatscript(strFile);
        }

        // Updated the asset database

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static void ParseChatscript(string strFile)
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

        // Parse

        chatscriptData.m_commands.Clear();

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

                MessageCommand messageCommand = new MessageCommand(match.Groups[2].ToString(), strMessage);
                chatscriptData.m_commands.Add(messageCommand);
            }
            else
            {
                string[] strWords = match.Groups[3].ToString().Split(' ');

                string strCommand = strWords[0];

                switch (strCommand)
                {
                    case "Pause":

                        float secPause = float.Parse(strWords[1]);
                        PauseCommand pauseCommand = new PauseCommand(secPause);
                        chatscriptData.m_commands.Add(pauseCommand);
                        break;

                    default:
                        throw new System.ArgumentException();
                }
            }
        }

        // Set data dirty

        EditorUtility.SetDirty(chatscriptData);
    }
}
