using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Script de Manejo de balas para tener mejor control.
/// Autor: Juan Jose Acosta
/// Fecha: 2025-08-09
/// Uso: Añadir al Prefab de "bala".
/// </summary>
public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    // Diccionario: prefab -> lista de objetos
    private Dictionary<GameObject, List<GameObject>> pools = new Dictionary<GameObject, List<GameObject>>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    /// <summary>
    /// Obtiene un objeto del pool correspondiente a un prefab.  
    /// Si no existe, se crea un nuevo pool para ese prefab.
    /// </summary>
    public GameObject ObtenerObjeto(GameObject prefab, int cantidadInicial = 5)
    {
        // Si el pool de este prefab no existe, crearlo
        if (!pools.ContainsKey(prefab))
        {
            pools[prefab] = new List<GameObject>();

            for (int i = 0; i < cantidadInicial; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                pools[prefab].Add(obj);
            }
        }

        // Buscar uno libre
        foreach (var obj in pools[prefab])
        {
            if (!obj.activeInHierarchy)
            {
                return obj;
            }
        }

        // Si no hay libres, instanciar uno nuevo
        GameObject nuevo = Instantiate(prefab);
        nuevo.SetActive(false);
        pools[prefab].Add(nuevo);
        return nuevo;
    }
}