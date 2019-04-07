using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Fog.Dialogue
{
    /// <summary>
    ///     This is the dialogue instance, which will be in a list in the inspector
    ///     There are getters but no setter - To prevent edit from outside scripts, overwriting dialogue made by the writers
    ///     The only way to edit dialogue is from the inspector, if you want to change this, just add a setter to the property
    /// </summary>
    [System.Serializable]
    public class DialogueLine
    {
        [Header("Dialogue Properties")]
        [SerializeField] private string title = null;
        [SerializeField] [TextArea(3, 5)] private string text = null;

        public string Title { get { return title; } }
        public string Text { get { return text; } }
    }
}
