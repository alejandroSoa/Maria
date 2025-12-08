using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ConsoleManager : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private TMP_Text instructionText;
    [SerializeField] private TMP_InputField consoleInput;
    [SerializeField] private TMP_Text consoleOutputText;
    [SerializeField] private GameObject consolePanel;
    
    [Header("Referencias del Sistema")]
    [SerializeField] private Fusebox fuseboxReference;
    
    [Header("Configuración de Tablas Predefinidas")]
    [SerializeField] private List<PredefinedTable> predefinedTables = new List<PredefinedTable>();
    
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
    private int currentTableIndex = 0;
    private int currentProblemIndex = 0;
    
    [System.Serializable]
    public class PredefinedTable
    {
        public string tableName;
        public List<ColumnDefinition> columns = new List<ColumnDefinition>();
    }
    
    [System.Serializable]
    public class ColumnDefinition
    {
        public string columnName;
        public string dataType; // INT, VARCHAR, DATE, BOOL
        public int maxLength; // Para VARCHAR
    }
    
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
        
        // Configurar tablas predefinidas por defecto
        SetupDefaultTables();
        
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
    /// Configura las tablas predefinidas por defecto
    /// </summary>
    private void SetupDefaultTables()
    {
        if (predefinedTables.Count == 0)
        {
            // Tabla USERS
            PredefinedTable usersTable = new PredefinedTable();
            usersTable.tableName = "USERS";
            usersTable.columns.Add(new ColumnDefinition { columnName = "NAME", dataType = "VARCHAR", maxLength = 50 });
            usersTable.columns.Add(new ColumnDefinition { columnName = "LAST_NAME", dataType = "VARCHAR", maxLength = 50 });
            usersTable.columns.Add(new ColumnDefinition { columnName = "EMAIL", dataType = "VARCHAR", maxLength = 100 });
            usersTable.columns.Add(new ColumnDefinition { columnName = "PHONE", dataType = "VARCHAR", maxLength = 15 });
            usersTable.columns.Add(new ColumnDefinition { columnName = "AGE", dataType = "INT", maxLength = 0 });
            usersTable.columns.Add(new ColumnDefinition { columnName = "BIRTHDAY", dataType = "DATE", maxLength = 0 });
            usersTable.columns.Add(new ColumnDefinition { columnName = "ADDRESS", dataType = "VARCHAR", maxLength = 200 });
            usersTable.columns.Add(new ColumnDefinition { columnName = "CITY", dataType = "VARCHAR", maxLength = 50 });
            predefinedTables.Add(usersTable);
            
            // Tabla PRODUCTS
            PredefinedTable productsTable = new PredefinedTable();
            productsTable.tableName = "PRODUCTS";
            productsTable.columns.Add(new ColumnDefinition { columnName = "PRODUCT_ID", dataType = "INT", maxLength = 0 });
            productsTable.columns.Add(new ColumnDefinition { columnName = "NAME", dataType = "VARCHAR", maxLength = 100 });
            productsTable.columns.Add(new ColumnDefinition { columnName = "PRICE", dataType = "INT", maxLength = 0 });
            productsTable.columns.Add(new ColumnDefinition { columnName = "CATEGORY", dataType = "VARCHAR", maxLength = 50 });
            productsTable.columns.Add(new ColumnDefinition { columnName = "STOCK", dataType = "INT", maxLength = 0 });
            productsTable.columns.Add(new ColumnDefinition { columnName = "DESCRIPTION", dataType = "VARCHAR", maxLength = 255 });
            predefinedTables.Add(productsTable);
            
            // Tabla ORDERS
            PredefinedTable ordersTable = new PredefinedTable();
            ordersTable.tableName = "ORDERS";
            ordersTable.columns.Add(new ColumnDefinition { columnName = "ORDER_ID", dataType = "INT", maxLength = 0 });
            ordersTable.columns.Add(new ColumnDefinition { columnName = "USER_ID", dataType = "INT", maxLength = 0 });
            ordersTable.columns.Add(new ColumnDefinition { columnName = "PRODUCT_ID", dataType = "INT", maxLength = 0 });
            ordersTable.columns.Add(new ColumnDefinition { columnName = "QUANTITY", dataType = "INT", maxLength = 0 });
            ordersTable.columns.Add(new ColumnDefinition { columnName = "ORDER_DATE", dataType = "DATE", maxLength = 0 });
            ordersTable.columns.Add(new ColumnDefinition { columnName = "STATUS", dataType = "VARCHAR", maxLength = 20 });
            predefinedTables.Add(ordersTable);
        }
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
            
            // Problema 3: SELECT de otra tabla
            SQLProblem problem3 = new SQLProblem();
            problem3.problemDescription = "Obtener todos los productos.";
            problem3.expectedQuery = "SELECT * FROM PRODUCTS";
            problem3.successMessage = "¡Perfecto! Has obtenido todos los productos.";
            problem3.errorMessage = "Query incorrecta. Usa: SELECT * FROM PRODUCTS";
            problem3.caseSensitive = false;
            sqlProblems.Add(problem3);
        }
    }
    
    /// <summary>
    /// Muestra el mensaje de bienvenida y pregunta si desea continuar
    /// </summary>
    private void ShowWelcomeMessage()
    {
        if (instructionText != null)
        {
            instructionText.text = "Sistema de Validación de Base de Datos";
        }
        
        if (consoleOutputText != null)
        {
            consoleOutputText.text = "¿Desea continuar con la validación de la base de datos?\nEscriba 'si' o 'no':";
        }
        
        currentState = ConsoleState.AwaitingConfirmation;
        Debug.Log("Esperando confirmación del usuario...");
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
                if (cmd == "reintentar" || cmd == "retry")
                {
                    RetryValidation();
                }
                else if (consoleOutputText != null)
                {
                    consoleOutputText.text += "\n> Sistema bloqueado. Escriba 'reintentar' después de completar los fusibles.\n";
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
        if (input == "si" || input == "yes" || input == "s" || input == "y")
        {
            if (consoleOutputText != null)
            {
                consoleOutputText.text = "Iniciando validación de tablas...\n";
            }
            
            // Bloquear la caja de fusibles
            if (fuseboxReference != null)
            {
                fuseboxReference.LockFusebox();
                Debug.Log("Caja de fusibles bloqueada");
            }
            
            // Iniciar validación de tablas
            currentState = ConsoleState.ValidatingTables;
            currentTableIndex = 0;
            StartTableValidation();
        }
        else if (input == "no" || input == "n")
        {
            if (consoleOutputText != null)
            {
                consoleOutputText.text = "Operación cancelada. Hasta luego.";
            }
            Debug.Log("Usuario canceló la operación");
        }
        else
        {
            if (consoleOutputText != null)
            {
                consoleOutputText.text += "\nRespuesta no válida. Escriba 'si' o 'no':";
            }
        }
    }
    
    /// <summary>
    /// Inicia la validación de todas las tablas predefinidas
    /// </summary>
    private void StartTableValidation()
    {
        bool allTablesValid = true;
        string validationReport = "";
        
        foreach (var predefinedTable in predefinedTables)
        {
            bool isValid = ValidateTable(predefinedTable, out string tableReport);
            validationReport += tableReport + "\n";
            
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
    /// Valida una tabla contra su definición predefinida
    /// </summary>
    private bool ValidateTable(PredefinedTable predefinedTable, out string report)
    {
        report = $"\n=== Validando tabla {predefinedTable.tableName} ===\n";
        bool isValid = true;
        
        // Obtener la configuración actual del Fusebox para esta tabla
        if (fuseboxReference == null)
        {
            report += "ERROR: No se encontró referencia a Fusebox\n";
            return false;
        }
        
        // Verificar cada columna
        foreach (var expectedColumn in predefinedTable.columns)
        {
            string fieldKey = $"{predefinedTable.tableName}_{expectedColumn.columnName}";
            string assignedType = PlayerPrefs.GetString($"FuseAssignment_{fieldKey}", "Unassigned");
            
            if (assignedType == "Unassigned")
            {
                report += $"✗ {expectedColumn.columnName}: SIN FUSIBLE ASIGNADO\n";
                isValid = false;
                continue;
            }
            
            // Validar tipo de dato
            if (assignedType != expectedColumn.dataType)
            {
                report += $"✗ {expectedColumn.columnName}: Tipo incorrecto (Esperado: {expectedColumn.dataType}, Actual: {assignedType})\n";
                isValid = false;
                continue;
            }
            
            // Validar tamaño para VARCHAR e INT
            if (expectedColumn.dataType == "VARCHAR" || expectedColumn.dataType == "INT")
            {
                int assignedSize = PlayerPrefs.GetInt($"FuseTolMin_{fieldKey}", 0);
                
                if (expectedColumn.maxLength > 0 && assignedSize != expectedColumn.maxLength)
                {
                    report += $"✗ {expectedColumn.columnName}: Tamaño incorrecto (Esperado: {expectedColumn.maxLength}, Actual: {assignedSize})\n";
                    isValid = false;
                    continue;
                }
            }
            
            report += $"✓ {expectedColumn.columnName}: {assignedType} - CORRECTO\n";
        }
        
        return isValid;
    }
    
    /// <summary>
    /// Se ejecuta cuando todas las tablas son válidas
    /// </summary>
    private void OnAllTablesValid()
    {
        if (consoleOutputText != null)
        {
            consoleOutputText.text += "\n✓✓✓ TODAS LAS TABLAS SON VÁLIDAS ✓✓✓\n";
            consoleOutputText.text += "La caja de fusibles permanecerá cerrada.\n";
            consoleOutputText.text += "Puede comenzar a realizar consultas SQL.\n\n";
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
            consoleOutputText.text += "\nSYSTEM ERROR\n";
            consoleOutputText.text += "Desbloqueando caja de fusibles...\n";
        }
        
        // Desbloquear la caja de fusibles
        if (fuseboxReference != null)
        {
            fuseboxReference.UnlockFusebox();
            Debug.Log("Caja de fusibles desbloqueada");
            
            // Determinar cuántos fusibles eliminar (ejemplo: entre 2 y 4)
            int fusesToRemove = Random.Range(2, 5);
            
            if (consoleOutputText != null)
            {
                consoleOutputText.text += $"Eliminando {fusesToRemove} fusibles al azar...\n";
            }
            
            // Remover fusibles aleatoriamente
            fuseboxReference.RemoveRandomFuses(fusesToRemove);
            
            if (consoleOutputText != null)
            {
                consoleOutputText.text += "Complete los fusibles faltantes y vuelva a intentar.\n";
                consoleOutputText.text += "Escriba 'reintentar' cuando esté listo:";
            }
        }
        
        currentState = ConsoleState.Locked;
        Debug.Log("Validación fallida. Se requiere reconfiguración.");
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
            // Incorrecto
            if (consoleOutputText != null)
            {
                consoleOutputText.text += $"✗ {currentProblem.errorMessage}\n";
                consoleOutputText.text += "Intenta de nuevo:\n";
            }
        }
    }
    
    /// <summary>
    /// Permite reintentar la validación después de corregir errores
    /// </summary>
    public void RetryValidation()
    {
        if (currentState == ConsoleState.Locked)
        {
            ShowWelcomeMessage();
        }
    }
}
