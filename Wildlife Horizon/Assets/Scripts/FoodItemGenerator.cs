using UnityEngine;

public class FoodItemGenerator : MonoBehaviour
{
    [SerializeField] GameObject foodItemPrefab; // Drag and drop the food item prefab in the inspector

    public void GenerateFoodItem()
    {
        // Instantiate the food item prefab at the generator's position
        Instantiate(foodItemPrefab, transform.position, Quaternion.identity);
    }
}