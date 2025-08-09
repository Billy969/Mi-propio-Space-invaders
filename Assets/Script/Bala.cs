using UnityEngine;
/// <summary>
/// Script de Interacción de la bala con el jugador y el enmigo.
/// Autor: Juan Jose Acosta
/// Fecha: 2025-08-09
/// Uso: Añadir al Prefab de "bala".
/// </summary>
public class Bala : MonoBehaviour
{
    // Define cuánto tiempo permanecerá activa la bala.
    public float tiempoVida = 2f;

    //Almacena el tiempo que ha transcurrido desde que la bala se activó.
    private float tiempoActivo;

    // Indica si la bala fue disparada por el jugador o un enemigo.
    [HideInInspector] public bool esBalaJugador; // true = jugador, false = enemigo

    // Ideal para reiniciar el temporizador de la bala.
    private void OnEnable()
    {
        // Reinicia el contador de tiempo activo a 0.
        tiempoActivo = 0f;
    }

    void Update()
    {
        // Incrementa el tiempo activo con el tiempo transcurrido desde el último frame.
        tiempoActivo += Time.deltaTime;

        // Comprueba si el tiempo activo ha superado el tiempo de vida de la bala.
        if (tiempoActivo >= tiempoVida)
        {
            // Si es así, desactiva el objeto para que pueda ser reutilizada.
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Comprueba si la bala es del jugador y si colisionó con un objeto con la etiqueta "Enemigo".
        if (esBalaJugador && collision.CompareTag("Enemigo"))
        {
            Debug.Log("Bala del jugador golpeó enemigo");

            // Desactiva la bala.
            gameObject.SetActive(false);
        }
        // Comprueba si la bala es de un enemigo y si colisionó con el jugador.
        else if (!esBalaJugador && collision.CompareTag("Player"))
        {
            Debug.Log("Bala del enemigo golpeó jugador");

            // Llama a un método en el GameManager para restar una vida al jugador.
            GameManager.Instance.PerderVida();
            gameObject.SetActive(false);
        }
    }
}