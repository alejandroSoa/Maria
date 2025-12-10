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
            // Determinar qué escena estamos usando
            int currentScene = SceneManager.GetActiveScene().buildIndex;
            
            // Room_level2 = buildIndex 5 (Unidad 2)
            if (currentScene == 5)
            {
                SetupUnit2Problems();
            }
            else
            {
                SetupUnit1Problems();
            }
        }
    }
    
    /// <summary>
    /// Problemas de la Unidad 1 (básicos)
    /// </summary>
    private void SetupUnit1Problems()
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
    
    /// <summary>
    /// Problemas de la Unidad 2 (SELECT, UPDATE, DELETE, INSERT)
    /// </summary>
    private void SetupUnit2Problems()
    {
        // ===== SELECT =====
        
        // 1. Listar usuarios activos
        SQLProblem p1 = new SQLProblem();
        p1.problemDescription = "[SELECT] Listar todos los usuarios activos (Id, Name, LastName, Country, Status).";
        p1.expectedQuery = "SELECT ID, NAME, LASTNAME, COUNTRY, STATUS FROM USERS WHERE STATUS = TRUE";
        p1.successMessage = "[System]: Usuarios activos listados correctamente.";
        p1.errorMessage = "[ERROR]: Usa: SELECT Id, Name, LastName, Country, Status FROM Users WHERE Status = true";
        p1.caseSensitive = false;
        sqlProblems.Add(p1);
        
        // 2. Usuarios ordenados por prioridad
        SQLProblem p2 = new SQLProblem();
        p2.problemDescription = "[SELECT] Lista usuarios ordenados por nivel de prioridad de mayor a menor.";
        p2.expectedQuery = "SELECT NAME, LASTNAME, PRIORITYLEVEL, COUNTRY FROM USERS ORDER BY PRIORITYLEVEL DESC";
        p2.successMessage = "[System]: Usuarios ordenados por prioridad.";
        p2.errorMessage = "[ERROR]: Usa: SELECT Name, LastName, PriorityLevel, Country FROM Users ORDER BY PriorityLevel DESC";
        p2.caseSensitive = false;
        sqlProblems.Add(p2);
        
        // 3. Ver todos los permisos
        SQLProblem p3 = new SQLProblem();
        p3.problemDescription = "[SELECT] Ver todos los permisos disponibles.";
        p3.expectedQuery = "SELECT * FROM PERMISSIONS";
        p3.successMessage = "[System]: Todos los permisos listados.";
        p3.errorMessage = "[ERROR]: Usa: SELECT * FROM Permissions";
        p3.caseSensitive = false;
        sqlProblems.Add(p3);
        
        // 4. Permisos con palabra 'usuarios'
        SQLProblem p4 = new SQLProblem();
        p4.problemDescription = "[SELECT] Permisos que incluyan la palabra 'usuarios' en la descripción.";
        p4.expectedQuery = "SELECT ID, NAME, DESCRIPTION FROM PERMISSIONS WHERE DESCRIPTION LIKE '%USUARIOS%'";
        p4.successMessage = "[System]: Permisos filtrados correctamente.";
        p4.errorMessage = "[ERROR]: Usa LIKE: SELECT Id, Name, Description FROM Permissions WHERE Description LIKE '%usuarios%'";
        p4.caseSensitive = false;
        sqlProblems.Add(p4);
        
        // 5. Documentos con contraseña
        SQLProblem p5 = new SQLProblem();
        p5.problemDescription = "[SELECT] Traer todos los documentos que tienen contraseña.";
        p5.expectedQuery = "SELECT ID, NAME, FILETYPE FROM DOCUMENTS WHERE HASPASSWORD = TRUE";
        p5.successMessage = "[System]: Documentos protegidos listados.";
        p5.errorMessage = "[ERROR]: Usa: SELECT Id, Name, FileType FROM Documents WHERE HasPassword = true";
        p5.caseSensitive = false;
        sqlProblems.Add(p5);
        
        // 6. Contar documentos por tipo
        SQLProblem p6 = new SQLProblem();
        p6.problemDescription = "[SELECT] Contar cuántos documentos hay por tipo de archivo (usa GROUP BY).";
        p6.expectedQuery = "SELECT FILETYPE, COUNT(*) AS CANTIDAD FROM DOCUMENTS GROUP BY FILETYPE";
        p6.successMessage = "[System]: Conteo de documentos completado.";
        p6.errorMessage = "[ERROR]: Usa: SELECT FileType, COUNT(*) AS cantidad FROM Documents GROUP BY FileType";
        p6.caseSensitive = false;
        sqlProblems.Add(p6);
        
        // ===== UPDATE =====
        
        // 7. Cambiar estado de Selbst
        SQLProblem p7 = new SQLProblem();
        p7.problemDescription = "[UPDATE] Cambiar el estado del usuario Selbst (Email: wunderwaffle@yahoo.com) a false.";
        p7.expectedQuery = "UPDATE USERS SET STATUS = FALSE WHERE EMAIL = 'WUNDERWAFFLE@YAHOO.COM'";
        p7.successMessage = "[System]: Estado de Selbst actualizado.";
        p7.errorMessage = "[ERROR]: Usa: UPDATE Users SET Status = false WHERE Email = 'wunderwaffle@yahoo.com'";
        p7.caseSensitive = false;
        sqlProblems.Add(p7);
        
        // 8. Modificar prioridad de usuarios de Japón
        SQLProblem p8 = new SQLProblem();
        p8.problemDescription = "[UPDATE] Modificar la prioridad de todos los usuarios de Japón a nivel 4.";
        p8.expectedQuery = "UPDATE USERS SET PRIORITYLEVEL = 4, UPDATEDAT = '2025-10-21 12:10:00' WHERE COUNTRY = 'JAPÓN'";
        p8.successMessage = "[System]: Prioridad de usuarios japoneses actualizada.";
        p8.errorMessage = "[ERROR]: Usa: UPDATE Users SET PriorityLevel = 4, UpdatedAt = '2025-10-21 12:10:00' WHERE Country = 'Japón'";
        p8.caseSensitive = false;
        sqlProblems.Add(p8);
        
        // 9. Activar todos los permisos
        SQLProblem p9 = new SQLProblem();
        p9.problemDescription = "[UPDATE] Activar todos los permisos (Status = true).";
        p9.expectedQuery = "UPDATE PERMISSIONS SET STATUS = TRUE";
        p9.successMessage = "[System]: Todos los permisos activados.";
        p9.errorMessage = "[ERROR]: Usa: UPDATE Permissions SET Status = true";
        p9.caseSensitive = false;
        sqlProblems.Add(p9);
        
        // ===== DELETE =====
        
        // 10. Eliminar usuario H0peles$0ul
        SQLProblem p10 = new SQLProblem();
        p10.problemDescription = "[DELETE] Eliminar el usuario H0peles$0ul (Email: securityservice@gamefuna.com).";
        p10.expectedQuery = "DELETE FROM USERS WHERE EMAIL = 'SECURITYSERVICE@GAMEFUNA.COM'";
        p10.successMessage = "[System]: Usuario H0peles$0ul eliminado.";
        p10.errorMessage = "[ERROR]: Usa: DELETE FROM Users WHERE Email = 'securityservice@gamefuna.com'";
        p10.caseSensitive = false;
        sqlProblems.Add(p10);
        
        // 11. Eliminar permiso del chiste
        SQLProblem p11 = new SQLProblem();
        p11.problemDescription = "[DELETE] Eliminar el permiso 'Acceso al nivel subterráneo'.";
        p11.expectedQuery = "DELETE FROM PERMISSIONS WHERE NAME = 'ACCESO AL NIVEL SUBTERRÁNEO'";
        p11.successMessage = "[System]: Permiso eliminado correctamente.";
        p11.errorMessage = "[ERROR]: Usa: DELETE FROM Permissions WHERE Name = 'Acceso al nivel subterráneo'";
        p11.caseSensitive = false;
        sqlProblems.Add(p11);
        
        // ===== INSERT =====
        
        // 12. Crear usuario Maria
        SQLProblem p12 = new SQLProblem();
        p12.problemDescription = "[INSERT] Crear usuario llamado MARIA con apellido López Hernández y email maria.lopez@example.com.";
        p12.expectedQuery = "INSERT INTO USERS (NAME, LASTNAME, EMAIL, PHONE, AGE, BIRTHDAY, ADDRESS, CITY, COUNTRY, PRIORITYLEVEL, STATUS, LASTSEEN, CREATEDAT, UPDATEDAT) VALUES ('MARÍA', 'LÓPEZ HERNÁNDEZ', 'MARIA.LOPEZ@EXAMPLE.COM', '+52 55 6789 4321', 26, '1999-07-18', 'CALLE REFORMA 45, COL. CENTRO', 'CIUDAD DE MÉXICO', 'MÉXICO', 2, TRUE, '2025-10-21 13:30:00', '2025-02-10 10:00:00', '2025-09-25 09:00:00')";
        p12.successMessage = "[System]: Usuario MARIA creado. ID asignado y registrado para futuros pasos.";
        p12.errorMessage = "[ERROR]: Usa INSERT INTO con todos los campos requeridos para MARIA";
        p12.caseSensitive = false;
        sqlProblems.Add(p12);
        
        // 13. Crear permiso ADMINISTRADOR
        SQLProblem p13 = new SQLProblem();
        p13.problemDescription = "[INSERT] Crear permiso 'ADMINISTRADOR' con descripción 'Permite acceso y control total del sistema'.";
        p13.expectedQuery = "INSERT INTO PERMISSIONS (NAME, DESCRIPTION, CREATEDAT, UPDATEDAT) VALUES ('ADMINISTRADOR', 'PERMITE ACCESO Y CONTROL TOTAL DEL SISTEMA', '2025-01-05 10:00:00', '2025-09-12 08:30:00')";
        p13.successMessage = "[System]: Permiso ADMINISTRADOR creado. ID registrado para MARIA.";
        p13.errorMessage = "[ERROR]: Usa INSERT INTO Permissions con Name, Description, CreatedAt, UpdatedAt";
        p13.caseSensitive = false;
        sqlProblems.Add(p13);
        
        // 14. Crear documentos de prueba
        SQLProblem p14 = new SQLProblem();
        p14.problemDescription = "[INSERT] Crear documento 'Roblox' tipo docx, tamaño 210, con contraseña 'true'.";
        p14.expectedQuery = "INSERT INTO DOCUMENTS (NAME, HASPASSWORD, PASSWORD, FILETYPE, FILESIZE) VALUES ('ROBLOX', TRUE, 'TRUE', 'DOCX', 210)";
        p14.successMessage = "[System]: Documento Roblox creado correctamente.";
        p14.errorMessage = "[ERROR]: Usa INSERT INTO Documents (Name, HasPassword, Password, FileType, FileSize)";
        p14.caseSensitive = false;
        sqlProblems.Add(p14);
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
            // Si estamos en la escena 1, mostrar solo "Hola"
            if (currentScene == 1)
            {
                consoleOutputText.text = "Hola";
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
