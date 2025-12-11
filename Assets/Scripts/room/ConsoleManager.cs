using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ConsoleManager : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private TMP_Text instructionText;
    [SerializeField] private TMP_InputField consoleInput;
    [SerializeField] private TMP_Text consoleOutputText;
    [SerializeField] private GameObject consolePanel;
    [SerializeField] private GameObject fadePanel; 
    
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
            if (currentScene == 6)
            {
                SetupUnit2Problems();
            }
            // Room_level3 = buildIndex 6 (Unidad 3)
            if (currentScene == 7)
            {
                SetupUnit3Problems();
            }
            // Room_level4 = buildIndex 7 (Unidad 4)
            if (currentScene == 8)
            {
                SetupUnit4Problems();
            }
        }
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
    /// Problemas de la Unidad 3 (CREACIÓN DE TABLAS)
    /// </summary>
    private void SetupUnit3Problems()
    {
        // Problema 15: CREATE TABLE Records
        SQLProblem p15 = new SQLProblem();
        p15.problemDescription = "Crear la tabla Records con la estructura especificada.";
        p15.expectedQuery = "CREATE TABLE Records (Id SERIAL PRIMARY KEY, Visitor INTEGER NOT NULL, Requested INTEGER NOT NULL, TimesRequested INTEGER NOT NULL DEFAULT 1, LastAccess DATETIME NOT NULL, IpAddress VARCHAR(45) NOT NULL, CONSTRAINT fk_usr_visitor FOREIGN KEY (Visitor) REFERENCES Users(Id), CONSTRAINT fk_doc_requested FOREIGN KEY (Requested) REFERENCES Documents(Id))";
        p15.successMessage = "¡Perfecto! Has creado correctamente la tabla Records.";
        p15.errorMessage = "Query incorrecta. Revisa la sintaxis de CREATE TABLE para Records";
        p15.caseSensitive = false;
        sqlProblems.Add(p15);
                
        // Problema 16: CREATE TABLE UserPermission
        SQLProblem p16 = new SQLProblem();
        p16.problemDescription = "Crear la tabla UserPermission con la estructura especificada.";
        p16.expectedQuery = "CREATE TABLE UserPermission (Id SERIAL PRIMARY KEY, User INTEGER NOT NULL, Permission INTEGER NOT NULL, AssignedAt DATETIME NOT NULL, RevokedAt DATETIME NULL, CONSTRAINT fk_user FOREIGN KEY (User) REFERENCES Users(Id), CONSTRAINT fk_permission FOREIGN KEY (Permission) REFERENCES Permissions(Id), CONSTRAINT unique_user_permission UNIQUE (User, Permission))";
        p16.successMessage = "¡Perfecto! Has creado correctamente la tabla UserPermission.";
        p16.errorMessage = "Query incorrecta. Revisa la sintaxis de CREATE TABLE para UserPermission";
        p16.caseSensitive = false;
        sqlProblems.Add(p16);
                
        // Problema 17: INSERT INTO UserPermission para Maria CHECAR ESTE PARA QUE QUEDE BIEN
        SQLProblem p17 = new SQLProblem();
        p17.problemDescription = "Maria pedirá agregar su usuario en el registro de UserPermission.";
        p17.expectedQuery = "INSERT INTO UserPermission (User, Permission) VALUES ('ID DE MARIA', 'ID PERMISO ABSOLUTO')";
        p17.successMessage = "¡Excelente! Has agregado a Maria en el registro de permisos.";
        p17.errorMessage = "Query incorrecta. Usa: INSERT INTO UserPermission (User, Permission) VALUES ('ID DE MARIA', 'ID PERMISO ABSOLUTO')";
        p17.caseSensitive = false;
        sqlProblems.Add(p17);
                
        // VER DE QUÉ FORMA HACER PARA QUE LA TABLA DE RECORDS
    }

    /// <summary>
    /// Problemas de la Unidad 4 
    /// </summary>
    private void SetupUnit4Problems()
    {
        // Problema 18: INNER JOIN básico - Usuarios con documentos solicitados
        SQLProblem p18 = new SQLProblem();
        p18.problemDescription = "[INNER JOIN] Mostrar usuarios con los documentos que solicitaron (Name, LastName de usuario, Name del documento, TimesRequested).";
        p18.expectedQuery = "SELECT U.NAME, U.LASTNAME, D.NAME, R.TIMESREQUESTED FROM RECORDS R INNER JOIN USERS U ON R.VISITOR = U.ID INNER JOIN DOCUMENTS D ON R.REQUESTED = D.ID";
        p18.successMessage = "[System]: ¡Perfecto! INNER JOIN ejecutado correctamente.";
        p18.errorMessage = "[ERROR]: Usa: SELECT U.Name, U.LastName, D.Name, R.TimesRequested FROM Records R INNER JOIN Users U ON R.Visitor = U.Id INNER JOIN Documents D ON R.Requested = D.Id";
        p18.caseSensitive = false;
        sqlProblems.Add(p18);


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
            // Si estamos en la escena 2 (Room), preguntar si desea conectar la caja de fusibles
            if (currentScene == 2)
            {
                consoleOutputText.text = "[System]: Iniciando protocolo de conexión de caja de fusibles...\n";
                consoleOutputText.text += "[System]: Esta acción validará la configuración de fusibles.\n";
                consoleOutputText.text += "[System]: ¿Desea conectar la caja de fusibles?\n\n";
                consoleOutputText.text += "> CONFIRM: Continuar con la conexión\n";
                consoleOutputText.text += "> DENY: Cancelar operación\n\n";
                consoleOutputText.text += "Esperando respuesta...";
                currentState = ConsoleState.AwaitingConfirmation;
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
    /// Maneja la confirmación inicial o después de correcciones
    /// </summary>
    private void HandleConfirmationInput(string input)
    {
        if (input == "confirm")
        {
            // Verificar si estamos en la escena 2
            int currentScene = SceneManager.GetActiveScene().buildIndex;
            
            if (consoleOutputText != null)
            {
                consoleOutputText.text = "[System]: CONFIRM recibido.\n";
                consoleOutputText.text += "[System]: Verificando configuración de fusibles...\n";
            }
            
            // Validar fusibles antes de bloquear
            if (fuseboxReference != null)
            {
                bool allTablesValid = true;
                string[] tableNames = fuseboxReference.GetAllTableNames();
                
                foreach (string tableName in tableNames)
                {
                    if (!fuseboxReference.IsTableValid(tableName))
                    {
                        allTablesValid = false;
                        break;
                    }
                }
                
                if (!allTablesValid)
                {
                    // Fusibles incorrectos - no continuar
                    if (consoleOutputText != null)
                    {
                        consoleOutputText.text += "[ERROR]: Configuración de fusibles incorrecta.\n";
                        consoleOutputText.text += "[System]: Corrige los fusibles antes de confirmar.\n";
                    }
                    return;
                }
                
                // Fusibles correctos - bloquear la caja
                fuseboxReference.LockFusebox();
            }
            
            if (consoleOutputText != null)
            {
                consoleOutputText.text += "[System]: ✓ Fusibles correctos. Caja bloqueada.\n";
            }
            
            // Si veníamos de un estado bloqueado (corrección), volver al problema
            if (currentProblemIndex > 0 || currentScene != 2)
            {
                currentState = ConsoleState.QueryMode;
                if (consoleOutputText != null)
                {
                    consoleOutputText.text += "[System]: Continuando desde el problema actual...\n\n";
                }
                ShowCurrentProblem();
                return;
            }
            
            // Validación inicial
            currentState = ConsoleState.ValidatingTables;
            
            // Si estamos en escena 2 y las tablas están bien, terminar el nivel
            if (currentScene == 2)
            {
                StartTableValidationForScene2();
            }
            else
            {
                StartTableValidation();
            }
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
    /// Validación especial para la escena 2 que termina el nivel si todo está correcto
    /// </summary>
    private void StartTableValidationForScene2()
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
            bool isValid = fuseboxReference.IsTableValid(tableName);
            validationReport += $"[System]: Tabla {tableName}: {(isValid ? "VÁLIDA" : "INVÁLIDA")}\n";
            
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
            // Si todas las tablas son válidas en escena 2, completar el nivel
            if (consoleOutputText != null)
            {
                consoleOutputText.text += "\n[System]: ¡VALIDACIÓN EXITOSA!\n";
                consoleOutputText.text += "[System]: Todas las tablas configuradas correctamente.\n";
                consoleOutputText.text += "[System]: Conexión de caja de fusibles establecida.\n";
                consoleOutputText.text += "[System]: Nivel completado. Preparando transición...\n";
            }
            
            // Marcar Level_1 como completado
            PlayerPrefs.SetString("Level_1", "done");
            PlayerPrefs.Save();
            
            // Desbloquear fusebox para futuros niveles
            if (fuseboxReference != null)
            {
                fuseboxReference.UnlockFusebox();
            }
            
            // Terminar nivel con fade
            FadeToBlackAndLoadScene(2f);
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
                    consoleOutputText.text += "\n[System]: ¡FELICIDADES! Has resuelto los problemas.\n";
                    consoleOutputText.text += "Apagando luces...\n";
                }
                
                // Marcar el nivel actual como completado
                int currentScene = SceneManager.GetActiveScene().buildIndex;
                string levelKey = "";
                
                if (currentScene == 6) levelKey = "Level_2";
                else if (currentScene == 7) levelKey = "Level_3";
                else if (currentScene == 8) levelKey = "Level_4";
                
                if (!string.IsNullOrEmpty(levelKey))
                {
                    PlayerPrefs.SetString(levelKey, "done");
                    PlayerPrefs.Save();
                }
                                
                // Iniciar el fade a negro y cargar la escena 0
                FadeToBlackAndLoadScene(3f);
            }
        }
        else
        {
            // Incorrecto - Penalizar quitando fusibles aleatorios
            if (consoleOutputText != null)
            {
                consoleOutputText.text += $"✗ {currentProblem.errorMessage}\n";
                consoleOutputText.text += "[System]: ERROR - Respuesta incorrecta.\n";
                
                // Mostrar respuesta encriptada (solo algunos caracteres visibles)
                string encryptedQuery = EncryptQuery(currentProblem.expectedQuery);
                consoleOutputText.text += $"[System]: Prueba: {encryptedQuery}\n";
            }
            
            // Aplicar ERROR: remover fusibles aleatorios de TODAS las tablas
            if (fuseboxReference != null)
            {
                int fusesToRemove = Random.Range(1, 6); // Remover entre 1 y 5 fusibles en total
                fuseboxReference.RemoveRandomFuses(fusesToRemove);
                
                if (consoleOutputText != null)
                {
                    consoleOutputText.text += $"\n[ERROR]: Sobrecarga detectada. {fusesToRemove} fusibles desconectados.\n";
                    consoleOutputText.text += "[System]: Debes reconfigurar los fusibles faltantes.\n";
                    consoleOutputText.text += "[System]: Escribe 'restart' para desbloquear la caja y reintentar.\n";
                    consoleOutputText.text += $"[System]: Volverás al PROBLEMA {currentProblemIndex + 1}.\n";
                }
                
                // NO desbloquear la caja - el jugador debe escribir restart
            }
            
            // Cambiar a estado bloqueado pero NO avanzar el índice del problema
            // El jugador se quedará en el mismo problema después de corregir
            currentState = ConsoleState.Locked;
        }
    }
    
    /// <summary>
    /// Permite reintentar la validación después de corregir errores
    /// Desbloquea la caja de fusibles para que el jugador pueda corregir
    /// </summary>
    public void RetryValidation()
    {
        if (currentState == ConsoleState.Locked)
        {
            // PRIMERO desbloquear la caja cuando el usuario escribe restart
            if (fuseboxReference != null)
            {
                fuseboxReference.UnlockFusebox();
            }
            
            if (consoleOutputText != null)
            {
                consoleOutputText.text = "[System]: Caja de fusibles desbloqueada.\n";
                consoleOutputText.text += "[System]: Corrige los fusibles faltantes.\n";
                consoleOutputText.text += "[System]: Escribe 'confirm' cuando hayas terminado de corregir.\n";
            }
            
            // Cambiar a estado de espera de confirmación
            currentState = ConsoleState.AwaitingConfirmation;
        }
    }
    
    /// <summary>
    /// Encripta una query mostrando solo algunos caracteres aleatorios
    /// </summary>
    private string EncryptQuery(string query)
    {
        if (string.IsNullOrEmpty(query)) return "[ENCRYPTED]";
        
        int visibleChars = Mathf.Max(3, (query.Length * 70) / 100); // Mostrar ~70% de caracteres
        char[] encrypted = new char[query.Length];
        
        // Llenar con caracteres de encriptación
        for (int i = 0; i < encrypted.Length; i++)
        {
            encrypted[i] = '█';
        }
        
        // Revelar algunos caracteres aleatorios
        System.Collections.Generic.HashSet<int> revealedIndices = new System.Collections.Generic.HashSet<int>();
        while (revealedIndices.Count < visibleChars)
        {
            int randomIndex = Random.Range(0, query.Length);
            if (!revealedIndices.Contains(randomIndex))
            {
                encrypted[randomIndex] = query[randomIndex];
                revealedIndices.Add(randomIndex);
            }
        }
        
        return new string(encrypted);
    }
    
    /// <summary>
    /// Activa un panel transparente y lo hace aparecer gradualmente hasta negro, luego carga la escena 0
    /// </summary>
    /// <param name="fadeDuration">Duración del fade en segundos</param>
    public void FadeToBlackAndLoadScene(float fadeDuration = 2f)
    {
        StartCoroutine(FadeToBlackCoroutine(fadeDuration));
    }
    
    private IEnumerator FadeToBlackCoroutine(float duration)
    {
        Debug.Log($"FadeToBlackCoroutine iniciado. Duración: {duration}s");
        
        if (fadePanel == null)
        {
            Debug.LogError("fadePanel no está asignado en el Inspector!");
            // Intentar cargar la escena de todas formas
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene("Title");
            yield break;
        }
        
        // Activar el panel
        fadePanel.SetActive(true);
        Debug.Log("fadePanel activado");
        
        // Obtener el componente Image del panel
        Image panelImage = fadePanel.GetComponent<Image>();
        if (panelImage == null)
        {
            Debug.LogError("fadePanel no tiene un componente Image!");
            // Intentar cargar la escena de todas formas
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene("Title");
            yield break;
        }
        
        Color startColor = new Color(0, 0, 0, 0);
        Color endColor = new Color(0, 0, 0, 1); 
        
        panelImage.color = startColor;
        
        float elapsed = 0f;
        
        // Gradualmente incrementar el alpha de 0 a 1
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / duration;
            
            // Interpolar entre transparente y opaco
            panelImage.color = Color.Lerp(startColor, endColor, progress);
            
            yield return null;
        }
        
        // Asegurar que el color final sea completamente opaco
        panelImage.color = endColor;
        
        Debug.Log("Fade completado. Cargando escena Title (Menú Principal)...");
        
        // Cargar la escena Title
        SceneManager.LoadScene("Title");
    }
}
