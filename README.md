# Simple Dialogue

This is a simple, versatile dialogue system for Unity.

This system features:
* **Highly customizable** options, such as:
* * Text "filling" effect - letters are filled in, one by one. Speed is adjustable.
* * Auto skip dialogue lines after a set time.
* * Titles - Special extra field of text that can be used for character names, for example.
* * Fill in entire text before skipping.
* Text skip prompt is fully custumizable, you setup what calls the skip line method.
* All text settings (Fonts, size, formatting) are setup whichever way you want.
* **Scriptable Objects** for dialogue events, to save and protect data from external changes.
* Whether or not to pause the game when displaying dialogue.
* Option for the dialogue system to be a **Singleton**! In case you only have 1 dialogue going on at a time, this lets you call any dialogue event at any given time.
* **Event based**!

## Dependencies

There are a couple of dependencies in this modulem, note that they are already in this repository
* [Reorderable List](https://github.com/cfoulston/Unity-Reorderable-List), made by [cfoulston](https://github.com/cfoulston)
* [Hide in Inspector If](https://github.com/FellowshipOfTheGame/Hide-In-Inspector-If)
* Installing the **Text Mesh Pro** package (Default on newer versions of Unity)

## Installing

Just download and move this module's folder to your project's folder!


### Try the test scene!
* [Test Scene](https://github.com/FellowshipOfTheGame/Simple-Dialogue-Test)

## Instructions

* Add the Dialogue Handler component to a game object
* Set up all the references and settings, including a dialogue box object to be enabled/disabled and a text object to display the text.
* Create a dialogue scriptable object (Assets->Create)
* Call *StartDialogue(Dialogue dialogueObject)*
* Skip to the next line by setting auto skip to true or by calling *SkipDialogue()*
* To end dialogue abruptly, call *EndDialogue()*!

**Notes:**
* By using multiple assets, check for duplicated dependencies(remove the duplicates)!
* Be aware of the namespace: Fog.Dialogue! Just add _using Fog.Dialogue_ or type the namespace when referencing the classes (Fog.Dialogue.DialogueHandler for ex.)
* Scripting runtime version **must** be set to: ".NET 4.x"
*  * This setting can be found under: Edit->Project Settings->Player->Other Settings->Configurations
*  * This is the default setting in Unity 2018.3 and above

The code is really simple and self-explanatory, so if you have any questions, read the code!

## Authors

* **Abner Santos** - *For the project Anathema* - [Anathema](https://github.com/FellowshipOfTheGame/anathema)

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details
