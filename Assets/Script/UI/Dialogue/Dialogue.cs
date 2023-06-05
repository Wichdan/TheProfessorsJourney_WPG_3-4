using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Scriptable Objects/New Dialogue", order = 1)]
public class Dialogue : ScriptableObject
{
    public List<Conversation> conversation;
    public List<Sprite> dialogueBackgrounds;
    public bool isCanSkip;
    public bool isGetSomething;
    public GetSomething getSomething;

    [System.Serializable]
    public struct Conversation
    {
        public string charName;
        public Sprite leftPortrait, rightPortrait;
        [TextArea(1, 3)]
        public string sentence;
        public bool isNarrator;
        public int dlgBGIndex;
        public bool dontUseHolderBG;
    }

    [System.Serializable]
    public struct GetSomething
    {
        //public Weapon weapon;
        public int updateMission;
    }
}


