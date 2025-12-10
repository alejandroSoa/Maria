using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ConsoleManager : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private TMP_Text instructionText;
    [SerializeField] private TMP_InputField consoleInput;
    [SerializeField] private TMP_Text consoleOutputText;
    [SerializeField] private GameObject consolePanel;
    
    [Header("Referencias del Sistema")]
    [SerializeField] private Fusebox fuseboxReference;
    
    [Header("Configuración de Problemas SQL")]
    [SerializeField] private List<SQLProblem> sqlProblems = new List<SQLProblem>();
    
    // Estados del sistema
    private enum ConsoleState
    {
        AwaitingConfirmation,
        ValidatingTables,
        QueryMode,
        Locked
    }
    
    private ConsoleState currentState = ConsoleState.AwaitingConfirmation;
    private int currentProblemIndex = 0;
    
    [System.Serializable]
    public class SQLProblem
    {
        public string problemDescription; // "Obtener todos los valores de la tabla Users"
        public string expectedQuery; // "SELECT * FROM USERS"
        public string successMessage; // "¡Correcto! Has obtenido todos los usuarios"
        public string errorMessage; // "Query incorrecta. Intenta de nuevo"
        public bool caseSensitive = false; // Si la comparación debe ser sensible a mayúsculas
    }
    
    void Start()
    {
        // Buscar Fusebox si no está asignado
        if (fuseboxReference == null)
        {
            fuseboxReference = FindFirstObjectByType<Fusebox>();
        }
        
        // Configurar problemas SQL por defecto
        SetupDefaultSQLProblems();
        
        // Configurar input de consola
        if (consoleInput != null)
        {
            consoleInput.onSubmit.AddListener(OnConsoleCommandEntered);
        }
        
        // Mostrar mensaje inicial
        ShowWelcomeMessage();
    }
    
    /// <summary>
    /// Configura los problemas SQL por defecto
    /// </summary>
    private void SetupDefaultSQLProblems()
    {
        if (sqlProblems.Count == 0)
        {
            // Problema 1: SELECT básico
            SQLProblem problem1 = new SQLProblem();
            problem1.problemDescription = "Obtener todos los valores de la tabla Users.";
            problem1.expectedQuery = "SELECT * FROM USERS";
            problem1.successMessage = "¡Correcto! Has obtenido todos los registros de la tabla USERS.";
            problem1.errorMessage = "Query incorrecta. Recuerda usar: SELECT * FROM USERS";
            problem1.caseSensitive = false;
            sqlProblems.Add(problem1);
            
            // Problema 2: SELECT con columnas específicas
            SQLProblem problem2 = new SQLProblem();
            problem2.problemDescription = "Obtener el nombre y email de todos los usuarios.";
            problem2.expectedQuery = "SELECT NAME, EMAIL FROM USERS";
            problem2.successMessage = "¡Excelente! Has seleccionado las columnas correctas.";
            problem2.errorMessage = "Query incorrecta. Usa: SELECT NAME, EMAIL FROM USERS";
            problem2.caseSensitive = false;
            sqlProblems.Add(problem2);
            
        }
    }
    
    /// <summary>
    /// Muestra el mensaje de bienvenida y pregunta si desea continuar
    /// </summary>
    private void ShowWelcomeMessage()
    {
        // Verificar en qué escena estamos
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        
        if (instructionText != null)
        {
            instructionText.text = "[System]: Consola Maria";
        }
        
        if (consoleOutputText != null)
        {
            // Si estamos en la escena 1, mostrar que todavía no se puede mover acá
            if (currentScene == 1)
            {
                consoleOutputText.text = "[System]: Iniciando...\n";
                consoleOutputText.text = "[System]: .\n";
                consoleOutputText.text = "[System]: .\n";
                consoleOutputText.text += "[System]: ERROR: Señal no establecida, se requiere primero una conexión estable.\n";
                consoleOutputText.text += "[System]: Restablece la conexión en la caja de fusibles primero para continuar.\n";
                return;
            }
            
            // Si no, mostrar el mensaje original
            consoleOutputText.text = "[System]: Iniciando protocolo de cierre de caja de fusibles...\n";
            consoleOutputText.text += "[System]: Esta acción bloqueará el acceso físico a los fusibles.\n";
            consoleOutputText.text += "[System]: ¿Desea continuar?\n\n";
            consoleOutputText.text += "> CONFIRM: Continuar con el cierre\n";
            consoleOutputText.text += "> DENY: Cancelar operación\n\n";
            consoleOutputText.text += "Esperando respuesta...";
        }
        
        currentState = ConsoleState.AwaitingConfirmation;
    }
    
    /// <summary>
    /// Procesa los comandos ingresados en la consola
    /// </summary>
    private void OnConsoleCommandEntered(string command)
    {
        if (string.IsNullOrEmpty(command)) return;
        
        string cmd = command.Trim().ToLower();
        
        // Comandos globales (funcionan en cualquier estado)
        if (cmd == "clear" || cmd == "limpiar" || cmd == "cls")
        {
            ClearConsole();
            if (consoleInput != null)
            {
                consoleInput.text = "";
                consoleInput.ActivateInputField();
            }
            return;
        }
        
        switch (currentState)
        {
            case ConsoleState.AwaitingConfirmation:
                HandleConfirmationInput(cmd);
                break;
                
            case ConsoleState.ValidatingTables:
                // En este estado no se esperan comandos
                break;
                
            case ConsoleState.QueryMode:
                HandleQueryInput(cmd);
                break;
                
            case ConsoleState.Locked:
                if (cmd == "restart")
                {
                    RetryValidation();
                }
                else if (consoleOutputText != null)
                {
                    consoleOutputText.text += "\n> Sistema bloqueado. Escriba 'restart' después de completar los fusibles.\n";
                }
                break;
        }
        
        // Limpiar input
        if (consoleInput != null)
        {
            consoleInput.text = "";
            consoleInput.ActivateInputField();
        }
    }
    
    /// <summary>
    /// Limpia el texto de salida de la consola
    /// </summary>
    private void ClearConsole()
    {
        if (consoleOutputText != null)
        {
            consoleOutputText.text = "";
        }
        Debug.Log("Consola limpiada");
    }
    
    /// <summary>
    /// Maneja la confirmación inicial
    /// </summary>
    private void HandleConfirmationInput(string input)
    {
        if (input == "confirm")
        {
            if (consoleOutputText != null)
            {
                consoleOutputText.text = "[System]: CONFIRM recibido.\n";
                consoleOutputText.text += "[System]: Iniciando validación de tablas...\n";
            }
            
            // Bloquear la caja de fusibles
            if (fuseboxReference != null)
            {
                fuseboxReference.LockFusebox();
            }
            
            currentState = ConsoleState.ValidatingTables;
            StartTableValidation();
        }
        else if (input == "deny")
        {
            if (consoleOutputText != null)
            {
                consoleOutputText.text = "[System]: DENY recibido.\n";
                consoleOutputText.text += "[System]: Operación cancelada. Hasta luego.";
            }
            Debug.Log("Usuario canceló la operación");
        }
        else
        {
            if (consoleOutputText != null)
            {
                consoleOutputText.text += "\n[ERROR]: Comando no reconocido.\n";
                consoleOutputText.text += "[System]: Por favor escriba 'CONFIRM' o 'DENY'\n";
            }
        }
    }
    
    /// <summary>
    /// Inicia la validación de todas las tablas predefinidas
    /// </summary>
    private void StartTableValidation()
    {
        if (fuseboxReference == null)
        {
            if (consoleOutputText != null)
            {
                consoleOutputText.text += "[ERROR]: No se encontró referencia a Fusebox\n";
            }
            OnTablesInvalid();
            return;
        }
        
        bool allTablesValid = true;
        string validationReport = "";
        
        // Obtener todas las tablas disponibles
        string[] tableNames = fuseboxReference.GetAllTableNames();
        
        // Validar cada tabla
        foreach (string tableName in tableNames)
        {
            string tableReport = fuseboxReference.ValidateTableConfiguration(tableName);
            validationReport += tableReport + "\n";
            
            bool isValid = fuseboxReference.IsTableValid(tableName);
            if (!isValid)
            {
                allTablesValid = false;
            }
        }
        
        if (consoleOutputText != null)
        {
            consoleOutputText.text += validationReport;
        }
        
        if (allTablesValid)
        {
            OnAllTablesValid();
        }
        else
        {
            OnTablesInvalid();
        }
    }
    
    /// <summary>
    /// Se ejecuta cuando todas las tablas son válidas
    /// </summary>
    private void OnAllTablesValid()
    {
        if (consoleOutputText != null)
        {
            consoleOutputText.text += "\n[System]: ===== VALIDATION SUCCESS =====\n";
            consoleOutputText.text += "[System]: Todas las tablas son válidas.\n";
            consoleOutputText.text += "[System]: Caja de fusibles bloqueada.\n";
            consoleOutputText.text += "[System]: Modo de consultas SQL activado.\n\n";
        }
        
        currentState = ConsoleState.QueryMode;
        currentProblemIndex = 0;
        
        // Mostrar el primer problema
        ShowCurrentProblem();
        
        Debug.Log("Validación exitosa. Sistema listo para consultas.");
    }
    
    /// <summary>
    /// Muestra el problema SQL actual
    /// </summary>
    private void ShowCurrentProblem()
    {
        if (currentProblemIndex >= sqlProblems.Count)
        {
            // Terminó todos los problemas
            if (consoleOutputText != null)
            {
                consoleOutputText.text += "\n🎉 ¡FELICIDADES! Has completado todos los problemas.\n";
                consoleOutputText.text += "Puedes cerrar la consola o escribir 'salir'.\n";
            }
            return;
        }
        
        var currentProblem = sqlProblems[currentProblemIndex];
        
        // Actualizar instrucción
        if (instructionText != null)
        {
            instructionText.text = currentProblem.problemDescription;
        }
        
        // Mostrar en output
        if (consoleOutputText != null)
        {
            consoleOutputText.text += $"\n--- PROBLEMA {currentProblemIndex + 1}/{sqlProblems.Count} ---\n";
            consoleOutputText.text += $"{currentProblem.problemDescription}\n";
            consoleOutputText.text += "Escribe tu consulta SQL:\n";
        }
    }
    
    /// <summary>
    /// Se ejecuta cuando hay errores en las tablas
    /// </summary>
    private void OnTablesInvalid()
    {
        if (consoleOutputText != null)
        {
            consoleOutputText.text += "\n[ERROR]: ===== VALIDATION FAILED =====\n";
            consoleOutputText.text += "[System]: Errores detectados en la configuración.\n";
            consoleOutputText.text += "[System]: Por favor verifique la caja de fusibles.\n";
            consoleOutputText.text += "[System]: Escriba 'restart' cuando esté listo:\n";
        }
        
        // NO desbloquear fusebox, mantenerla bloqueada
        currentState = ConsoleState.Locked;
        Debug.Log("Validación fallida. Fusebox permanece bloqueada hasta restart.");
    }
    
    /// <summary>
    /// Maneja las consultas SQL en modo de consulta
    /// </summary>
    private void HandleQueryInput(string query)
    {
        if (currentProblemIndex >= sqlProblems.Count)
        {
            if (consoleOutputText != null)
            {
                consoleOutputText.text += "\n> Ya completaste todos los problemas.\n";
            }
            return;
        }
        
        var currentProblem = sqlProblems[currentProblemIndex];
        
        if (consoleOutputText != null)
        {
            consoleOutputText.text += $"\n> {query}\n";
        }
        
        // Normalizar queries para comparación
        string normalizedInput = query.Trim();
        string normalizedExpected = currentProblem.expectedQuery.Trim();
        
        if (!currentProblem.caseSensitive)
        {
            normalizedInput = normalizedInput.ToUpper();
            normalizedExpected = normalizedExpected.ToUpper();
        }
        
        // Remover espacios múltiples y punto y coma final
        normalizedInput = System.Text.RegularExpressions.Regex.Replace(normalizedInput, @"\s+", " ");
        normalizedExpected = System.Text.RegularExpressions.Regex.Replace(normalizedExpected, @"\s+", " ");
        normalizedInput = normalizedInput.TrimEnd(';');
        normalizedExpected = normalizedExpected.TrimEnd(';');
        
        // Comparar
        if (normalizedInput == normalizedExpected)
        {
            // ¡Correcto!
            if (consoleOutputText != null)
            {
                consoleOutputText.text += $"✓ {currentProblem.successMessage}\n";
            }
            
            // Avanzar al siguiente problema
            currentProblemIndex++;
            
            if (currentProblemIndex < sqlProblems.Count)
            {
                ShowCurrentProblem();
            }
            else
            {
                // Completó todos los problemas
                if (consoleOutputText != null)
                {
                    consoleOutputText.text += "\n🎉 ¡FELICIDADES! Has completado todos los problemas SQL.\n";
                    consoleOutputText.text += "Puedes cerrar la consola.\n";
                }
            }
        }
        else
        {
            // Incorrecto - Penalizar quitando fusibles aleatorios
            if (consoleOutputText != null)
            {
                consoleOutputText.text += $"✗ {currentProblem.errorMessage}\n";
                consoleOutputText.text += "[System]: ERROR - Respuesta incorrecta.\n";
            }
            
            // Desbloquear fusebox y quitar fusibles como penalización
            if (fuseboxReference != null)
            {
                fuseboxReference.UnlockFusebox();
                
                // Determinar cuántos fusibles eliminar (entre 2 y 4)
                int fusesToRemove = Random.Range(2, 5);
                
                if (consoleOutputText != null)
                {
                    consoleOutputText.text += $"[System]: Eliminando {fusesToRemove} fusibles como penalización...\n";
                }
                
                // Remover fusibles aleatoriamente
                fuseboxReference.RemoveRandomFuses(fusesToRemove);
                
                if (consoleOutputText != null)
                {
                    consoleOutputText.text += "[System]: Complete los fusibles faltantes.\n";
                    consoleOutputText.text += "[System]: Escriba 'reintentar' para volver a validar:\n";
                }
                
                Debug.Log("Query incorrecta - Fusibles removidos como penalización");
            }
            
            // Cambiar a estado bloqueado para requerir revalidación
            currentState = ConsoleState.Locked;
        }
    }
    
    /// <summary>
    /// Permite reintentar la validación después de corregir errores
    /// </summary>
    public void RetryValidation()
    {
        if (currentState == ConsoleState.Locked)
        {
            // Desbloquear fusebox al escribir restart
            if (fuseboxReference != null)
            {
                fuseboxReference.UnlockFusebox();
            }
            
            ShowWelcomeMessage();
        }
    }
}
