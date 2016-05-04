/// <summary>
/// Created by Shaun Landy - 22/04/16
/// </summary>

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace ArmadHeroes
{
    //Enum types to represent characters and their permanent position in any arrays
    public enum CharacterType
    {
        SARGE = 0,
        RAPH,
        CIPHER,
        SLICK,
        ALT_SARGE,
        ALT_RAPH,
        ALT_CIPHER,
        ALT_SLICK,
        MAX_TYPES
    }
    //Struct containing all details relating to a character
    [System.Serializable]
    public struct CharacterProfile
    {
        public CharacterType character;
        public string characterName;
		public Sprite characterFileImage, characterFileImage_left, characterFileImage_right, characterFileImage_Debrief, characterFileImage_head;
    }
    //Class containing profiles for each character - ultimately makes it easier to add new characters also
    public class CharacterProfiles : MonoBehaviour
    {
        static private CharacterProfiles m_instance = null;
        static public CharacterProfiles instance { get { return m_instance; } }

        [SerializeField] private CharacterProfile[] profiles;

        void Awake()
        {
            m_instance = this;
        }

        public CharacterProfile GetProfile(CharacterType _type)
        {
            return profiles[(int)_type];
        }

        public string TypeToString(CharacterType _type)
        {
            return profiles[(int)_type].characterName;
        }
    }
}
