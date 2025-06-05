using UnityEngine;
using TMPro; // Necesario para TextMeshProUGUI (Text de TextMeshPro)
using UnityEngine.SceneManagement; // Necesario para la funci�n SceneManager.LoadScene

public class GameManager : MonoBehaviour
{
    // Patr�n Singleton para acceso f�cil desde otros scripts
    public static GameManager Instance { get; private set; }

    [Header("UI References")]
    public TextMeshProUGUI scoreText; // Referencia al texto de la puntuaci�n actual
    public GameObject gameOverPanel; // Referencia al panel de Game Over
    public TextMeshProUGUI finalScoreText; // Referencia al texto de la puntuaci�n final en el panel

    private float score = 0f; // La puntuaci�n actual del jugador
    private bool isGameOver = false; // Estado del juego

    // Awake se llama cuando la instancia del script se carga
    void Awake()
    {
        // Implementaci�n del Singleton: asegura que solo haya una instancia de GameManager
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destruye cualquier instancia duplicada
        }
        else
        {
            Instance = this; // Establece esta instancia como la �nica
        }
    }

    // Start se llama antes de la primera actualizaci�n del frame
    void Start()
    {
        // Asegurarse de que el panel de Game Over est� oculto al inicio
        gameOverPanel.SetActive(false);
        score = 0f; // Reiniciar puntuaci�n
        isGameOver = false; // Reiniciar estado del juego
        Time.timeScale = 1f; // Asegurarse de que el tiempo del juego est� corriendo
    }

    // Update se llama una vez por frame
    void Update()
    {
        // Si el juego no ha terminado, la puntuaci�n aumenta con el tiempo
        if (!isGameOver)
        {
            score += Time.deltaTime * 5f; // Ajusta el multiplicador para cambiar la velocidad de la puntuaci�n
            scoreText.text = "Score: " + Mathf.FloorToInt(score).ToString(); // Actualiza el texto de la puntuaci�n
        }
    }

    // M�todo llamado cuando el juego termina (por colisi�n o ca�da)
    public void GameOver()
    {
        // Evitar que se llame a Game Over m�ltiples veces si ya ha terminado
        if (isGameOver) return;

        isGameOver = true; // Establecer el estado del juego a Game Over
        gameOverPanel.SetActive(true); // Mostrar el panel de Game Over
        finalScoreText.text = "Tu puntuaci�n final: " + Mathf.FloorToInt(score).ToString(); // Mostrar la puntuaci�n final
        Time.timeScale = 0f; // Pausar el juego para que todo se detenga
    }

    // M�todo para reiniciar el juego, llamado por el bot�n de reinicio
    public void RestartGame()
    {
        Time.timeScale = 1f; // Reanudar el tiempo del juego
        // Cargar la escena actual, lo que reinicia todo el juego
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
