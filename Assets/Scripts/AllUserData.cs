using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AllUserData", menuName = "mod-pls/AllUserData", order = 0)]
public class AllUserData : ScriptableObject
{
    [SerializeField] 
    public List<UserData> m_users = new List<UserData>();

    public UserData User(string strUser)
    {
        return m_users.Find(user => user.m_strName.ToLower() == strUser.ToLower());
    }
}

[System.Serializable]
public class UserData
{
    public string m_strName;
    public Sprite m_spriteProfile;
    public bool m_isMod = false;
}