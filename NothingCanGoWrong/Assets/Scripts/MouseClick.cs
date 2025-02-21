using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseClick : MonoBehaviour
{
    private Transform highlight;
    private Transform selection;
    private RaycastHit raycastHit;

    public Animator WorldUIAnim;
    public Animator camAnim;

    private void Update()
    {
        CheckSelectionOnClick();
    }

    private void CheckSelectionOnClick()
    {
        if (highlight != null && highlight != selection)
        {
            highlight.gameObject.GetComponent<Outline>().enabled = false;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out raycastHit))
        {
            highlight = raycastHit.transform;
            if (highlight.CompareTag("Selectable") && highlight != selection)
            {
                if (highlight.gameObject.GetComponent<Outline>() != null)
                {
                    highlight.gameObject.GetComponent<Outline>().enabled = true;
                }
            }
            else
            {
                highlight = null;
            }
        }

        // Mouse Selection
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (highlight)
            {
                if (selection != null)
                {
                    selection.gameObject.GetComponent<Outline>().enabled = false;
                    camAnim.SetBool("isOpen", false);
                    WorldUIAnim.SetBool("isOpen", false);
                    TravelManager.instance.animator.SetBool("isOpen", false);
                }
                selection = raycastHit.transform;
                if (selection != null)
                {
                    Debug.Log(selection);
                    selection.gameObject.GetComponent<Outline>().enabled = true;

                    TravelManager.instance.animator.SetBool("isOpen", false);
                    camAnim.SetBool("isOpen", false);

                    Debug.Log("Objeto seleccionado: " + selection.gameObject.name);
                    AudioManager.instance.Play("button_click");
                    World world = selection.gameObject.GetComponent<World>();
                    Player.instance.selectedWorld = world;
                    Player.instance.selectedWorld.isSelected = true;
                    UpdateConstructionSliders();

                    if (!world.isSettled)
                    {
                        TravelManager.instance.animator.SetBool("isOpen", true);
                        TravelManager.instance.PlanetName.text = world.worldName;
                        camAnim.SetBool("isOpen", true);
                    }

                    WorldUIAnim.SetBool("isOpen", true);
                    WorldStatsUI.instance.ShowWorldStats(world);
                    camAnim.SetBool("isOpen", true);
                }
            }
            else
            {
                if (selection != null && !EventSystem.current.IsPointerOverGameObject())
                {
                    WorldUIAnim.SetBool("isOpen", false);
                    TravelManager.instance.animator.SetBool("isOpen", false);
                    camAnim.SetBool("isOpen", false);
                    selection.gameObject.GetComponent<Outline>().enabled = false;
                    selection = null;
                }
            }
        }
    }

    private void UpdateConstructionSliders()
    {
        foreach (World w in FindObjectsOfType<World>())
        {
            bool isActive = (w == Player.instance.selectedWorld);
            w.SetSlidersVisibility(isActive);
        }
    }
}