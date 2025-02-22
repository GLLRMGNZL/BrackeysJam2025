using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialMenu : MonoBehaviour
{
    public Animator mainMenuAnim;
    public Animator tutorialAnim;
    public TextMeshProUGUI tutorialText;
    public TextMeshProUGUI lineShownText;
    public List<string> tutorialLines = new List<string>();

    private int lineShown = 0;

    private void Start()
    {
        populateTutorialList();
        tutorialText.text = tutorialLines[lineShown];
        lineShownText.text = (lineShown + 1) + " / " + tutorialLines.Count;
    }

    public void BackToMainMenu()
    {
        tutorialAnim.SetBool("isOpen", false);
        mainMenuAnim.SetBool("isOpen", true);
    }

    private void populateTutorialList()
    {
        tutorialLines.Add("in this game, every world is your world. you start in your home planet, nereo");
        tutorialLines.Add("build cities, factories, labs and make the population grow in order to settle the entire system");
        tutorialLines.Add("once you reach technology level 1, you will produce resources to travel other planets");
        tutorialLines.Add("you will also be able to make more simultaneous buildings");
        tutorialLines.Add("once you travel to a new planet, you will be able to build on it");
        tutorialLines.Add("just select a planet with your mouse and use your screen interface to make it yours. you are the boss!");
        tutorialLines.Add("so, sit back, relax, make our species grow and remember:");
        tutorialLines.Add("nothing can go wrong");
    }

    public void NextLine()
    {
        if (lineShown < tutorialLines.Count -1)
        {
            lineShown++;
        }
        else
        {
            lineShown = 0;
        }

        lineShownText.text = (lineShown + 1) + " / " + tutorialLines.Count;
        tutorialText.text = tutorialLines[lineShown];
    }

    public void previousLine()
    {
        if (lineShown >= 1)
        {
            lineShown--;
        }
        else
        {
            lineShown = tutorialLines.Count - 1;
        }

        lineShownText.text = (lineShown + 1) + " / " + tutorialLines.Count;
        tutorialText.text = tutorialLines[lineShown];
    }

    public void ResetTutorial()
    {
        lineShown = 0;
        lineShownText.text = (lineShown + 1) + " / " + tutorialLines.Count;
        tutorialText.text = tutorialLines[lineShown];
    }
}
