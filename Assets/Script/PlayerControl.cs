using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// Script de control de personaje principal, movimiento y disparo.
/// Autor: Juan Jose Acosta
/// Fecha: 2025-08-09
/// Uso: Añadir a "player".
/// </summary>
public class PlayerControl : MonoBehaviour
{
    PlayerInput playerInput;
    public float velocidad = 5f;
    public GameObject balaPrefab; // Prefab de la bala del jugador
    private GameManager cuentaRegresiva;
    [SerializeField]
    private Transform spawnpoint; // Punto de aparición de las balas

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        cuentaRegresiva = FindAnyObjectByType<GameManager>();
    }

    void Update()
    {
        if (cuentaRegresiva.juegoIniciado)
        {
            movimiento();
            Disparar();
        }
    }

    void movimiento()
    {
        Vector2 moveInput = playerInput.actions["Move"].ReadValue<Vector2>();
        if (moveInput != Vector2.zero)
        {
            Vector3 movimiento = new Vector3(moveInput.x, 0, 0).normalized;
            transform.Translate(movimiento * velocidad * Time.deltaTime);
        }
    }

    void Disparar()
    {
        if (playerInput.actions["Attack"].WasPressedThisFrame())
        {
            GameObject bala = PoolManager.Instance.ObtenerObjeto(balaPrefab, 10);
            bala.transform.position = spawnpoint.position + Vector3.up * 0.5f;
            bala.transform.rotation = Quaternion.identity;
            bala.SetActive(true);

            // Configuramos que es del jugador
            Bala scriptBala = bala.GetComponent<Bala>();
            scriptBala.esBalaJugador = true;

            Rigidbody2D rb = bala.GetComponent<Rigidbody2D>();
            rb.linearVelocity = Vector2.up * 10f;
        }
    }
}