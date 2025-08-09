using TMPro;
using UnityEngine;
/// <summary>
/// Script de Control de vida, TextMeshpro de Vidas y oleada actual; ademas de controlar el panel de victoria o derrota.
/// Autor: Juan Jose Acosta
/// Fecha: 2025-08-09
/// Uso: Añadir este script en un emptyobject además de poner sus respectivos canvas.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Config Vidas")]
    public int vidas = 3;
    public TextMeshProUGUI textoVidas;

    [Header("UI Contadores y Mensajes")]
    public TextMeshProUGUI textoCuentaRegresiva;
    public GameObject panelGanaste;
    public GameObject panelPerdiste;

    [Header("Oleadas")]
    public Oleadas oleadas;

    [HideInInspector] public bool juegoIniciado = false;

    private float tiempoCuenta;
    private bool contando = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        ActualizarVidasUI();
        textoCuentaRegresiva.gameObject.SetActive(false);
        panelGanaste.SetActive(false);
        panelPerdiste.SetActive(false);

        // Arranca con cuenta regresiva inicial
        IniciarCuentaRegresiva();
    }

    void Update()
    {
        // Manejo de cuenta regresiva
        if (contando)
        {
            tiempoCuenta -= Time.deltaTime;
            textoCuentaRegresiva.text = Mathf.Ceil(tiempoCuenta).ToString();

            if (tiempoCuenta <= 0)
            {
                contando = false;
                juegoIniciado = true;
                textoCuentaRegresiva.gameObject.SetActive(false);
            }
        }

        // Comprobar victoria
        if (oleadas.OleadaActual > oleadas.MaxOleadas)
        {
            Victoria();
        }
    }

    public void PerderVida()
    {
        vidas--;
        ActualizarVidasUI();

        if (vidas <= 0)
        {
            Derrota();
            return;
        }

        // Pausar y reiniciar con cuenta regresiva
        juegoIniciado = false;
        IniciarCuentaRegresiva();
    }

    public void IniciarCuentaRegresiva()
    {
        tiempoCuenta = 5f;
        contando = true;
        textoCuentaRegresiva.gameObject.SetActive(true);
    }

    private void ActualizarVidasUI()
    {
        textoVidas.text = "Vidas: " + vidas;
    }

    private void Derrota()
    {
        juegoIniciado = false;
        panelPerdiste.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Victoria()
    {
        juegoIniciado = false;
        panelGanaste.SetActive(true);
        Time.timeScale = 0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Bala enemiga golpea al jugador
        if (collision.CompareTag("bala"))
        {
            Bala bala = collision.GetComponent<Bala>();
            if (bala != null && bala.esBalaJugador == false)
            {
                collision.gameObject.SetActive(false);
                PerderVida();
            }
        }

        // Enemigo cruza límite inferior
        if (collision.CompareTag("EnemigoLimite"))
        {
            PerderVida();
        }
    }
}