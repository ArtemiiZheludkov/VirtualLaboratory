using UnityEngine;

public class FollowCursorAndScale : MonoBehaviour
{
    public float moveSpeed = 5f; // швидкість руху
    public float scaleSpeed = 0.1f; // швидкість зміни масштабу

    private bool isScaling = false; // чи відбувається зміна масштабу

    void LateUpdate()
    {
        // Отримати позицію миші у вікні
        Vector2 mousePosition = Input.mousePosition;

        // Оновити позицію руки відповідно до позиції миші
        transform.position = mousePosition;

        // Зміна масштабу при натисканні кнопки миші
        if (Input.GetMouseButtonDown(0))
        {
            isScaling = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isScaling = false;
        }

        if (isScaling)
        {
            float scaleFactor = 1f + scaleSpeed * Time.deltaTime;

            // Змінити масштаб об'єкта
            transform.localScale *= scaleFactor;
        }
    }
}

