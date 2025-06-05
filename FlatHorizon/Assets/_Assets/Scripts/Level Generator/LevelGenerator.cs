using UnityEngine;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    public List<GameObject> levelSections;
    public float spawnXOffset = 20f;
    public int initialSections = 3;
    public float destroyDistance = 30f;

    private Transform playerTransform;
    private float lastSectionX;
    private List<GameObject> activeSections = new List<GameObject>();

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (playerTransform == null)
        {
            Debug.LogError("No se encontró el Player con el tag 'Player'.");
            enabled = false;
            return;
        }

        for (int i = 0; i < initialSections; i++)
        {
            SpawnSection();
        }
    }

    void Update()
    {
        if (playerTransform.position.x > lastSectionX - spawnXOffset)
        {
            SpawnSection();
        }

        CleanupOldSections();
    }

    void SpawnSection()
    {
        if (levelSections == null || levelSections.Count == 0)
        {
            Debug.LogError("No hay secciones de nivel asignadas.");
            return;
        }

        int randomIndex = Random.Range(0, levelSections.Count);
        GameObject chosenSection = levelSections[randomIndex];

        Vector3 spawnPos = new Vector3(lastSectionX, 0, 0);
        GameObject newSection = Instantiate(chosenSection, spawnPos, Quaternion.identity);

        // Obtener el ancho desde el prefab instanciado
        LevelSection sectionData = newSection.GetComponent<LevelSection>();
        float sectionWidth = (sectionData != null) ? sectionData.sectionWidth : 20f;

        lastSectionX += sectionWidth;
        activeSections.Add(newSection);
    }

    void CleanupOldSections()
    {
        if (activeSections.Count == 0) return;

        GameObject oldest = activeSections[0];
        if (playerTransform.position.x - oldest.transform.position.x > destroyDistance)
        {
            Destroy(oldest);
            activeSections.RemoveAt(0);
        }
    }
}
