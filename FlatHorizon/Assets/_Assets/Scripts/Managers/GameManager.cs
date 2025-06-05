using UnityEngine;
using TMPro; // Necesario para TextMeshProUGUI (Text de TextMeshPro)
using UnityEngine.SceneManagement; // Necesario para la función SceneManager.LoadScene

public class GameManager : MonoBehaviour
{
    // Patrón Singleton para acceso fácil desde otros scripts
    public static GameManager Instance { get; private set; }

    [Header("UI References")]
    public TextMeshProUGUI scoreText; // Referencia al texto de la puntuación actual
    public GameObject gameOverPanel; // Referencia al panel de Game Over
    public TextMeshProUGUI finalScoreText; // Referencia al texto de la puntuación final en el panel

    private float score = 0f; // La puntuación actual del jugador
    private bool isGameOver = false; // Estado del juego

    // Awake se llama cuando la instancia del script se carga
    void Awake()
    {
        // Implementación del Singleton: asegura que solo haya una instancia de GameManager
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destruye cualquier instancia duplicada
        }
        else
        {
            Instance = this; // Establece esta instancia como la única
        }
    }

    // Start se llama antes de la primera actualización del frame
    void Start()
    {
        // Asegurarse de que el panel de Game Over esté oculto al inicio
        gameOverPanel.SetActive(false);
        score = 0f; // Reiniciar puntuación
        isGameOver = false; // Reiniciar estado del juego
        Time.timeScale = 1f; // Asegurarse de que el tiempo del juego esté corriendo
    }

    // Update se llama una vez por frame
    void Update()
    {
        // Si el juego no ha terminado, la puntuación aumenta con el tiempo
        if (!isGameOver)
        {
            score += Time.deltaTime * 5f; // Ajusta el multiplicador para cambiar la velocidad de la puntuación
            scoreText.text = "Score: " + Mathf.FloorToInt(score).ToString(); // Actualiza el texto de la puntuación
        }
    }

    // Método llamado cuando el juego termina (por colisión o caída)
    public void GameOver()
    {
        // Evitar que se llame a Game Over múltiples veces si ya ha terminado
        if (isGameOver) return;

        isGameOver = true; // Establecer el estado del juego a Game Over
        gameOverPanel.SetActive(true); // Mostrar el panel de Game Over
        finalScoreText.text = "Tu puntuación final: " + Mathf.FloorToInt(score).ToString(); // Mostrar la puntuación final
        Time.timeScale = 0f; // Pausar el juego para que todo se detenga
    }

    // Método para reiniciar el juego, llamado por el botón de reinicio
    public void RestartGame()
    {
        Time.timeScale = 1f; // Reanudar el tiempo del juego
        // Cargar la escena actual, lo que reinicia todo el juego
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
