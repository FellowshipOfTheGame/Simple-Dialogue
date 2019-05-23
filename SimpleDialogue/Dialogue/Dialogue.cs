using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Malee;

namespace Fog.Dialogue
{
    /// <summary>
    ///     Creates a scriptable object for an array of dialogue lines, so that it can be saved as a file.
    /// </summary>
    [CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue")]
    public class Dialogue : ScriptableObject
    {
        [Reorderable] public ReorderableDialogueList lines;
    }

    [System.Serializable]
    public class ReorderableDialogueList : ReorderableArray<DialogueLine> {}
}