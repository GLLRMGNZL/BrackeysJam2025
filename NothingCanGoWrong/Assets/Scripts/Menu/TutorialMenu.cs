using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMenu : MonoBehaviour
{
    public Animator mainMenuAnim;
    public Animator tutorialAnim;

    public void BackToMainMenu()
    {
        tutorialAnim.SetBool("isOpen", false);
        mainMenuAnim.SetBool("isOpen", true);
    }
}
