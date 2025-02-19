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

    private void Update()
    {
        CheckSelectionOnClick();
    }

    private void CheckSelectionOnClick()
    {
        if (highlight != null && highlight != selection)
        {
            TravelManager.instance.animator.SetBool("isOpen", false);
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
        if (Input.GetMouseButtonDown(0))
        {
            if (highlight)
            {
                if (selection != null)
                {
                    selection.gameObject.GetComponent<Outline>().enabled = false;
                }
                selection = raycastHit.transform;
                if (selection != null)
                {
                    Debug.Log(selection);
                    selection.gameObject.GetComponent<Outline>().enabled = true;

                    Debug.Log("Objeto seleccionado: " + selection.gameObject.name);
                    World world = selection.gameObject.GetComponent<World>();
                    Player.instance.selectedWorld = world;

                    if (!world.isSettled)
                    {
                        TravelManager.instance.animator.SetBool("isOpen", true);
                        TravelManager.instance.PlanetName.text = world.worldName;
                    }

                    WorldUIAnim.SetBool("isOpen", true);
                    WorldStatsUI.instance.ShowWorldStats(world);
                    
                }
            }
            else
            {
                if (selection != null && !EventSystem.current.IsPointerOverGameObject())
                {
                    WorldUIAnim.SetBool("isOpen", false);
                    TravelManager.instance.animator.SetBool("isOpen", false);
                    selection.gameObject.GetComponent<Outline>().enabled = false;
                    selection = null;
                }
            }
        }
    }
}