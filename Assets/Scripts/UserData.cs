using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AllUserData", menuName = "mod-pls/AllUserData", order = 0)]
public class AllUserData : ScriptableObject
{
    [SerializeField] 
    [SerializeReference]
    public List<UserData> m_users = new List<UserData>();
}

[System.Serializable]
public class UserData : ChatscriptCommand
{
    public string m_strName;
    public Sprite m_spriteProfile;
    public bool m_isMod = false;
}