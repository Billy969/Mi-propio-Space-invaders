using UnityEngine;
/// <summary>
/// Script sencillo para quitar vida cuando los enemigos lleguen a la línea de abajo.
/// Autor: Juan Jose Acosta
/// Fecha: 2025-08-09
/// Uso: Añadir a límite.
/// </summary>
public class LimiteInferior : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemigo"))
        {
            collision.gameObject.SetActive(false);
            GameManager.Instance.PerderVida();
        }
    }
}