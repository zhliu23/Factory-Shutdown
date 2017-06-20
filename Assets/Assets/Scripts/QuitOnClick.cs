/***************************************************************
* file: QuitOnClick.cs
* author: David Silvan
* class: CS 470.01 - Game Development
*
* assignment: Quarter Project
* date last modified: 5/29/2017
*
* purpose: Quits the game
*
****************************************************************/

using UnityEngine;
using System.Collections;

public class QuitOnClick : MonoBehaviour
{

	//method: Quit
	//purpose: Quits the game
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit ();
#endif
    }

}