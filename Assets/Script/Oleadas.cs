using System.Collections.Generic;
using TMPro;
using UnityEngine;
/// <summary>
/// Script de Control de Oleada como enemigos activos y activar la siguiente oleada.
/// Autor: Juan Jose Acosta
/// Fecha: 2025-08-09
/// Uso: Añadir a GameManager.
/// </summary>
public class Oleadas : MonoBehaviour
{
    // 'OleadaActual': el número de la oleada actual, comienza en 1.
    [SerializeField] public int OleadaActual = 1;
    // 'MaxOleadas': el número total de oleadas en el juego.
    public int MaxOleadas = 10;
    
    [SerializeField] List<GameObject> Enemigos = new List<GameObject>();
    
    // referencia al componente de texto para mostrar el número de la oleada.
    [SerializeField] TextMeshProUGUI TextoOleada;

    private bool esperandoNuevaOleada = false;

    void Start()
    {
        // Actualiza el texto de la interfaz de usuario con la oleada actual.
        TextoOleada.text = "Oleada: " + OleadaActual;
    }

    void Update()
    {
        // El script revisa constantemente si ya no hay enemigos activos.
        if (!esperandoNuevaOleada && EnemigosActivosOleada() == 0)
        {
            esperandoNuevaOleada = true;
            // Se llama al método 'SiguienteOleada' después de 2 segundos.
            Invoke(nameof(SiguienteOleada), 2f);
        }
    }

    public int EnemigosActivosOleada()
    {
        int enemigosActivos = 0;
        // Recorre cada GameObject en la lista de enemigos.
        foreach (GameObject enemigo in Enemigos)
        {
           
            if (enemigo.activeInHierarchy)
            {
             
                enemigosActivos++;
            }
        }
        return enemigosActivos; // Devuelve el número total de enemigos activos.
    }

    
    public void SiguienteOleada()
    {
        // Si la oleada actual ha alcanzado el máximo, el juego ha sido ganado.
        if (OleadaActual >= MaxOleadas)
        {
            // Llama a un método en el GameManager para manejar la victoria.
            GameManager.Instance.Victoria();
            return; // Sale del método para no continuar.
        }

        // Incrementa el número de oleada y actualiza el texto de la UI.
        OleadaActual++;
        TextoOleada.text = "Oleada: " + OleadaActual;

        // Recorre todos los enemigos en la lista.
        foreach (GameObject enemigo in Enemigos)
        {
            // Reposiciona a cada enemigo en un lugar aleatorio en la parte superior de la pantalla.
            enemigo.transform.position = new Vector3(Random.Range(-5f, 5f), 6f, 0f);
            // Activa el objeto del enemigo para que aparezca en el juego.
            enemigo.SetActive(true);

            // Obtiene el script 'Enemigos' de cada enemigo.
            Enemigos scriptEnemigo = enemigo.GetComponent<Enemigos>();
            // Si el script existe, aumenta la dificultad de la oleada.
            if (scriptEnemigo != null)
            {
                // Aumenta la velocidad, la frecuencia de disparo y la velocidad de la bala.
                // Esto hace que cada oleada sea progresivamente más difícil.
                scriptEnemigo.velocidad += OleadaActual * 0.2f;
                scriptEnemigo.frecuenciaDisparo += OleadaActual * 0.1f;
                scriptEnemigo.velocidadDisparo += OleadaActual * 0.2f;
            }
        }
        
        esperandoNuevaOleada = false;
    }
}