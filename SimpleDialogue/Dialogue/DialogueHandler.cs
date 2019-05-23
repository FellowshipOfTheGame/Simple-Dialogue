using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Fog.Dialogue
{
	using Fog.Editor;
	/// <summary>
	/// 	This is the main, manager class for a dialogue box system
	/// 	The pipeline is:
	/// 		-> StartDialogue() will start the dialogue from the "dialogue" variable and StartDialogue(dialogue) will start the dialogue from the parameter
	/// 		-> Skip() will skip to the next line, this has to be set manually, for example a button that calls this function, or automatically from the "autoSkip" setting
	/// 		-> EndDialogue() will be called when there are no more lines to be shown, but you can call it before to abruptly stop the dialogue
	/// </summary>
	public class DialogueHandler : MonoBehaviour
	{

		[Header("References")]
		[Tooltip("Reference to the TMPro text component of the main dialogue box.")]
		[SerializeField] private TextMeshProUGUI dialogueText = null;
		[Tooltip("Whether or not the dialogue has a title or character name display.")]
		[SerializeField] private bool useTitles = false;
		[Tooltip("Reference to the TMPro text component of the title/name display.")]
		[SerializeField] [HideInInspectorIfNot(nameof(useTitles))] private TextMeshProUGUI titleText = null;
		[Tooltip("Current dialogue script to be displayed. To create a new dialogue, go to Assets->Create->Anathema->Dialogue.")]
		[SerializeField] private Dialogue dialogue;
		[Tooltip("Game object that contains the chat box to be enabled/disabled")]
		[SerializeField] private GameObject dialogueBox = null;

		[Space(10)]

		[Header("Settings")]
		[Tooltip("Whether or not the characters are going to be displayed one at a time.")]
		[SerializeField] private bool useTypingEffect = false;
		[SerializeField] [HideInInspectorIfNot(nameof(useTypingEffect))] [Range(1, 60)] private int framesBetweenCharacters = 0;
		[Tooltip("If true, trying to skip dialogue will first fill in the entire dialogue line and then skip if prompted again, if false it will skip right away.")]
		[SerializeField] [HideInInspectorIfNot(nameof(useTypingEffect))] private bool fillInBeforeSkip = false;
		[Tooltip("Whether or not, after filling in the entire text, the dialogue skips to the next line automatically.")]
		[SerializeField] private bool autoSkip = false;
		[SerializeField] [HideInInspectorIfNot(nameof(autoSkip))] private float timeUntilSkip = 0;
        [Tooltip("Whether or not to pause game during dialogue")]
		[SerializeField] private bool pauseDuringDialogue = false;
        [Tooltip("Advanced setting: If there is only 1 handler/dialogue box (A visual novel for example) you can make this a singleton and call it from DialogueHandler.instance. If unsure, leave it false.")]
        [SerializeField] private bool isSingleton = false;


		private Queue<DialogueLine> dialogueLines = new Queue<DialogueLine>();
		private DialogueLine currentLine;
		private bool isLineDone;
		private bool isActive;

		public delegate void DialogueAction();
		public event DialogueAction OnDialogueStart;
		public event DialogueAction OnDialogueEnd;

		public static DialogueHandler instance;

		private void Awake()
		{
			if(isSingleton)
			{
				if(instance == null)
					instance = this;
				else if(instance != this)
					Destroy(this);
			}
		}

		/// <summary>
		/// 	Starts the dialogue by adding all the dialogue lines from the current dialogue object to a Queue, calls the OnDialogueStart event, pauses the game (if the setting is active) and enables the dialogue box.
		/// 	This overload is supposed to be used when there is a default dialogue sequence, since it uses the last set dialogue as the current dialogue.
		/// 	Otherwise, use the StartDialogue(Dialogue dialogue) overload.
		/// </summary>
		public void StartDialogue()
		{
			OnDialogueStart?.Invoke();

			if(isActive)
				EndDialogue();

            if(pauseDuringDialogue)
			    Time.timeScale = 0f;

			foreach(var line in dialogue.lines)
				dialogueLines.Enqueue(line);

			isActive = true;
			dialogueBox.SetActive(true);
			StartCoroutine("NextLine");
		}

		/// <summary>
		/// 	Starts the dialogue by adding all the dialogue lines from the current dialogue object to a Queue, calls the OnDialogueStart event, pauses the game (if the setting is active) and enables the dialogue box.
		/// 	In case of a default dialogue (that repeats), you can also set the dialogue and use the StartDialogue() overload instead.
		/// </summary>
		/// <param name="dialogue"> The current dialogue scriptable object. </param>
		public void StartDialogue(Dialogue dialogue)
		{
			this.dialogue = dialogue;
			StartDialogue();
		}

		/// <summary>
		/// 	Loads the next line and handles auto skip if on.
		///		If you mean to skip to the next dialogue, use the public method Skip() instead.
		/// </summary>
		/// <returns> IEnumerator for the Coroutine. </returns>
		private IEnumerator NextLine()
		{
			isLineDone = false;

			if(dialogueLines.Count != 0)
			{
				currentLine = dialogueLines.Dequeue();
				dialogueText.text = "";

				if(useTitles)
					titleText.text = currentLine.Title;

				yield return FillInText();

				isLineDone = true;

				if(autoSkip)
				{
					yield return new WaitForSecondsRealtime(timeUntilSkip);
					StartCoroutine("NextLine");
				}
			}
			else
				EndDialogue();
		}

		/// <summary>
		/// 	Call this method to progress dialogue.
		/// 	If the typing effect is on and the Fill in Before Skip setting is also on, fills in the entire line before going to the next line.
		/// </summary>
		public void Skip()
		{
			if(isActive)
			{
				StopAllCoroutines();
				if(fillInBeforeSkip && !isLineDone)
				{
					dialogueText.text = currentLine.Text;
					isLineDone = true;
				}
				else
					StartCoroutine("NextLine");
			}
		}

		/// <summary>
		/// 	Handles the typing effect.
		/// </summary>
		/// <returns> IEnumerator for the Coroutine. </returns>
		private IEnumerator FillInText()
		{
			if(useTypingEffect)
			{
				foreach(var character in currentLine.Text)
				{
					dialogueText.text += character;
					yield return WaitForFrames(framesBetweenCharacters);
				}
			}
			else
				dialogueText.text = currentLine.Text;
		}

		/// <summary>
		/// 	This method is called automatically once the dialogue line queue is empty, but it can be called to end the dialogue abruptly.
		///		calls the OnDialogueEnd event, unpauses the game (if the setting is on) and disables the dialogue box.
		/// </summary>
		public void EndDialogue()
		{
			dialogueBox.SetActive(false);

			dialogueText.text = "";
			if(titleText){
				titleText.text = "";
			}

			StopAllCoroutines();

			currentLine = null;
			isActive = false;

            if(pauseDuringDialogue)
                Time.timeScale = 1f;

			OnDialogueEnd?.Invoke();
		}

		/// <summary>
		/// 	Auxiliary coroutine method to skip a number of frames.
		/// </summary>
		/// <param name="frameCount"> How many frames to skip. </param>
		/// <returns> IEnumerator for the Coroutine. </returns>
		public static IEnumerator WaitForFrames(int frameCount)
		{
			while (frameCount > 0)
			{
				frameCount--;
				yield return null;
			}
		}

	}

}