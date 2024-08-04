using UnityEngine;

public class BirdController : MonoBehaviour//script that controls the bird in the birdgame
{
    public Animator animator; 
    public float moveSpeed = 5.0f; // Speed of the bird
    public float rotationSpeed = 5.0f; // controls the speed of the birds rotation
    public Joystick joystick;  // joystick asset is assigned in unity (gamebutton that controls the movement/navigation of the bird)

    void Update()
    {
        // joystick input for horizontal and vertical movements
        float horizontalInput = joystick.Horizontal;
        float verticalInput = joystick.Vertical;

        // connecton to the animator parameters based on joystick input
        animator.SetFloat("Horizontal", horizontalInput);
        animator.SetFloat("Vertical", verticalInput);

        // direction based on joystick input
        Vector3 direction = new Vector3(horizontalInput, 0, verticalInput).normalized;

        if (direction.magnitude >= 0.1f){
            
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;

            // rotate the bird matching the joystick input
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSpeed, 0.1f);
            transform.rotation = Quaternion.Euler(0, angle, 0);
           
            Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward; // Move the bird forward in the direction it is facing
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
        }
    }
}
