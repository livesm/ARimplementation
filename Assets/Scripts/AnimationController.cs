using System.Collections;
using UnityEngine;

public class AnimationController : MonoBehaviour //script to control the animations the dog and its reactions to the buttons. It also controls the animations of the bowls. Have them appear and disapear when the respective buttons are pressed.
{
    public Animator animator; //controls the animations of the dog
    public GameObject bowl; //assigning both the food bowl and waterbowl specifically
    public GameObject waterBowl; 

    void Start()
    {
    
    }

    public void TriggerFoodSequence() //this triggers the animations/reactions of the dog when the food-button is pressed.
    {
        animator.SetTrigger("Eat");
    }

    public void TriggerDrinkSequence() //this is a trigger of the animations connected to the water-button
    {
        animator.SetTrigger("Drink");
    }



    
    public void TriggerBite() //egentlig berre til leiking med animasjona og testing 
    {
        animator.SetTrigger("Bite");
    }




    public void TriggerShake()
    {
        animator.SetTrigger("Shake");
    }

    public void TriggerAirflip() //fjernast?
    {
        animator.SetTrigger("AirFlip");
    }



    public void TriggerWalk()
    {
        animator.SetTrigger("Walk");
    }

    public void ToggleBowlVisibility() //this controls the appearance and disaparance of the food bowl.
    {
        if (bowl != null)
        {
            bowl.SetActive(!bowl.activeSelf);
            if (bowl.activeSelf) // Check if the bowl is now visible
            {
                StartCoroutine(HideAfterDelay(bowl, 10)); // Hide tje bowl after 10 seconds
            }
        }
    }



    public void ToggleWaterBowlVisibility() // same as the food-bowl
    {
        if (waterBowl != null)
        {
            waterBowl.SetActive(!waterBowl.activeSelf);
            if (waterBowl.activeSelf) 
            {
                StartCoroutine(HideAfterDelay(waterBowl, 10)); 
            }
        }
    }

    // hide GameObject after a delay
    private IEnumerator HideAfterDelay(GameObject gameObject, float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}
