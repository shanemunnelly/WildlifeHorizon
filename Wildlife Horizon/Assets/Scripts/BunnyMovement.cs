using UnityEngine;
using UnityEngine.AI;

public class BunnyMovement : MonoBehaviour
{
    GameObject player;
    NavMeshAgent agent;

    [SerializeField] LayerMask groundLayer, playerLayer, foodLayer;
    [SerializeField] float range;
    [SerializeField] float sightRange = 10f;
    [SerializeField] float foodDetectionRange = 5f;
    [SerializeField] float animalSpeed = 5f;
    [SerializeField] float carryingFoodSpeed = 2f;
    [SerializeField] float RunningSpeed = 8f;
    [SerializeField] float stoppingDistance = 1.5f;

    bool playerInSight;
    bool foodInSight;
    bool hasFood = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");

        agent.speed = animalSpeed;
    }

    void Update()
    {
        playerInSight = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        foodInSight = Physics.CheckSphere(transform.position, foodDetectionRange, foodLayer);

        if (hasFood && playerInSight)
        {
            
            FollowPlayerWithFood();
        }
        else
        {
            if (!playerInSight && !foodInSight) Patrol();
            if (playerInSight) Flee();
          

        }
        if (Input.GetKeyDown(KeyCode.E)) // Change the key as needed
        {
            InteractWithObject();
        }
    }

  

    void FollowPlayerWithFood()
    {
        // Set the destination to the player's position
        agent.SetDestination(player.transform.position);

        // Check the distance between the animal and the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // If the distance is less than the stopping distance, stop the animal
        if (distanceToPlayer <= stoppingDistance)
        {
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
        }
        else
        {
            // Adjust the speed when the player has food
            agent.isStopped = false;
            agent.speed = carryingFoodSpeed;
        }
       ;
    }
    void Flee()
    {
        Vector3 dirToPlayer = transform.position - player.transform.position;

        Vector3 newPos = transform.position + dirToPlayer;
        agent.speed = RunningSpeed;
        agent.SetDestination(newPos);
        /*    Debug.Log("Fleeing from player");
            agent.SetDestination(player.transform.position)*/
        ;
    }

    void Patrol()
    {

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            SearchForDest();
        }

    }

    void SearchForDest()
    {
        agent.speed = animalSpeed;
        float z = Random.Range(-range, range);
        float x = Random.Range(-range, range);

        Vector3 destPoint = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(destPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    void InteractWithObject()
    {
        // Assuming there is an object with a collider and interactable script
        // This can be adjusted based on your specific game design
        Collider interactableCollider = null;

        // Example: Raycast to check if the player is interacting with an object
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            interactableCollider = hit.collider;
        }

        if (interactableCollider != null)
        {
            // Check if the interactable object is a food item generator
            FoodItemGenerator foodGenerator = interactableCollider.GetComponent<FoodItemGenerator>();

            if (foodGenerator != null)
            {
                // Generate the food item
                foodGenerator.GenerateFoodItem();

                // Set the hasFood flag to true

                hasFood = true;

                // You can add additional logic here if needed
            }
        }
    }
}