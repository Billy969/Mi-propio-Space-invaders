using UnityEngine;
/// <summary>
/// Script de Control del enemigo para que dispare y se mueva hacia abajo.
/// Autor: Juan Jose Acosta
/// Fecha: 2025-08-09
/// Uso: Añadir al Prefab de "Enemigo".
/// </summary>
public class Enemigos : MonoBehaviour
{
    public float velocidad = 4f; // Controla la velocidad de movimiento del enemigo.
    public float frecuenciaDisparo = 2f; // Define el intervalo de tiempo entre cada disparo (en segundos).
    public GameObject balaEnemigoPrefab; // El prefab de la bala que el enemigo disparará.
    public float velocidadDisparo = 0.5f; // La velocidad con la que se moverá la bala.

    private GameManager cuentaRegresiva; 
    
    // 'spawnpoint' es el punto desde donde se dispararán las balas.
    [SerializeField]
    private Transform spawnpoint;
    void Start()
    {
        // Busca el objeto 'GameManager' en la escena y obtiene su componente.
        cuentaRegresiva = FindAnyObjectByType<GameManager>();
        
        // El primer disparo se realiza después de 1 segundo, y luego se repite.
        InvokeRepeating(nameof(Disparar), 1f, frecuenciaDisparo);
    }

    void Update()
    {
        // El enemigo solo se moverá si el juego ha iniciado.
        if (cuentaRegresiva.juegoIniciado)
        {
            // Define la dirección de movimiento como hacia abajo.
            Vector2 direccion = Vector2.down.normalized;
            // Mueve el enemigo en la dirección y velocidad definidas. 
            transform.Translate(direccion * velocidad * Time.deltaTime);
        }
    }

    void Disparar()
    {
        // Si el juego no ha iniciado, no se dispara.
        if (!cuentaRegresiva.juegoIniciado) return;

        // Verifica si el enemigo está activo en la jerarquía.
        if (gameObject.activeInHierarchy == true)
        {
            // Esto es más eficiente que instanciar y destruir objetos constantemente.
            GameObject bala = PoolManager.Instance.ObtenerObjeto(balaEnemigoPrefab, 15);
            
            // Configura la posición inicial de la bala en el 'spawnpoint'.
            bala.transform.position = spawnpoint.position + Vector3.down * velocidadDisparo;
            // Mantiene la rotación de la bala sin cambios.
            bala.transform.rotation = Quaternion.identity;
            // Activa el objeto de la bala para que sea visible y funcional.
            bala.SetActive(true);

            // Obtiene el script 'Bala' de la bala para configurarla.
            Bala scriptBala = bala.GetComponent<Bala>();
            // Le indica al script que esta bala es de un enemigo.
            scriptBala.esBalaJugador = false;

            // Obtiene el 'Rigidbody2D' de la bala para aplicarle un movimiento.
            Rigidbody2D rb = bala.GetComponent<Rigidbody2D>();
            // Aplica una velocidad lineal hacia abajo a la bala.
            rb.linearVelocity = Vector2.down * 5f;
        }
    }
    
    // Este método se activa cuando el enemigo colisiona con otro objeto.
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Comprueba si el objeto con el que colisionó tiene la etiqueta "bala".
        if (collision.CompareTag("bala"))
        {
            // Obtiene el componente 'Bala' del objeto colisionado.
            Bala bala = collision.GetComponent<Bala>();
            
            if (bala != null && bala.esBalaJugador)
            {
                gameObject.SetActive(false);
            }
        }
    }
}