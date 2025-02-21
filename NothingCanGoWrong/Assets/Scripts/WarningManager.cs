using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WarningManager : MonoBehaviour
{
    #region Singleton
    public static WarningManager instance;

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            return;
        }

    }
    #endregion

    public Animator warningAnimator;
    public TextMeshProUGUI resourcesText;
    public TextMeshProUGUI spaceText;
    public TextMeshProUGUI constructionText;

    public void Warning(string warningType)
    {
        switch (warningType)
        {
            case "resources":
                resourcesText.gameObject.SetActive(true);
                spaceText.gameObject.SetActive(false);
                constructionText.gameObject.SetActive(false);
                break;
            case "space":
                resourcesText.gameObject.SetActive(false);
                spaceText.gameObject.SetActive(true);
                constructionText.gameObject.SetActive(false);
                break;
            case "construction":
                resourcesText.gameObject.SetActive(false);
                spaceText.gameObject.SetActive(false);
                constructionText.gameObject.SetActive(true);
                break;
        }
        StartCoroutine(ShowWarning());
    }

    private IEnumerator ShowWarning()
    {
        warningAnimator.SetBool("isOpen", true);
        AudioManager.instance.Play("warning_message");
        yield return new WaitForSeconds(2f);
        warningAnimator.SetBool("isOpen", false);
    }
}
