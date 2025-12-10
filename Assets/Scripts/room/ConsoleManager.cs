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
            // Room_level3 = buildIndex 6 (Unidad 3)
            if (currentScene == 6)
            {
                SetupUnit3Problems();
            }
            // Room_level4 = buildIndex 7 (Unidad 4)
            if (currentScene == 7)
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
        // Problema 18: JOIN para usuarios con documentos solicitados
        SQLProblem p18 = new SQLProblem();
        p18.problemDescription = "[JOIN] Usuarios con los documentos que solicitaron - mostrar Name, LastName del usuario, Name del documento y TimesRequested.";
        p18.expectedQuery = "SELECT U.NAME, U.LASTNAME, D.NAME, R.TIMESREQUESTED FROM RECORDS R INNER JOIN USERS U ON R.VISITOR = U.ID INNER JOIN DOCUMENTS D ON R.REQUESTED = D.ID";
        p18.successMessage = "[System]: ¡Perfecto! Has obtenido la relación usuarios-documentos correctamente.";
        p18.errorMessage = "[ERROR]: Usa INNER JOIN: SELECT U.Name, U.LastName, D.Name, R.TimesRequested FROM Records R INNER JOIN Users U ON R.Visitor = U.Id INNER JOIN Documents D ON R.Requested = D.Id";
        p18.caseSensitive = false;
        sqlProblems.Add(p18);

        // Problema 19: JOIN con WHERE - usuarios prioridad alta con documentos protegidos
        SQLProblem p19 = new SQLProblem();
        p19.problemDescription = "[JOIN + WHERE] Usuarios con prioridad alta que solicitaron documentos con contraseña - mostrar Name, PriorityLevel del usuario y Name, HasPassword del documento.";
        p19.expectedQuery = "SELECT U.NAME, U.PRIORITYLEVEL, D.NAME, D.HASPASSWORD FROM RECORDS R INNER JOIN USERS U ON R.VISITOR = U.ID INNER JOIN DOCUMENTS D ON R.REQUESTED = D.ID WHERE U.PRIORITYLEVEL <= 2 AND D.HASPASSWORD = TRUE";
        p19.successMessage = "[System]: ¡Excelente! Has filtrado usuarios VIP con documentos protegidos correctamente.";
        p19.errorMessage = "[ERROR]: Usa WHERE con condiciones: SELECT U.Name, U.PriorityLevel, D.Name, D.HasPassword FROM Records R INNER JOIN Users U ON R.Visitor = U.Id INNER JOIN Documents D ON R.Requested = D.Id WHERE U.PriorityLevel <= 2 AND D.HasPassword = true";
        p19.caseSensitive = false;
        sqlProblems.Add(p19);

        // Problema 20: JOIN con GROUP BY y COUNT
        SQLProblem p20 = new SQLProblem();
        p20.problemDescription = "[JOIN + GROUP BY] Cuántas solicitudes hizo cada usuario - mostrar Name del usuario y total de solicitudes.";
        p20.expectedQuery = "SELECT U.NAME, COUNT(R.ID) AS TOTALSOLICITUDES FROM RECORDS R INNER JOIN USERS U ON R.VISITOR = U.ID GROUP BY U.NAME";
        p20.successMessage = "[System]: ¡Perfecto! Has contado las solicitudes por usuario correctamente.";
        p20.errorMessage = "[ERROR]: Usa GROUP BY con COUNT: SELECT U.Name, COUNT(R.Id) AS totalSolicitudes FROM Records R INNER JOIN Users U ON R.Visitor = U.Id GROUP BY U.Name";
        p20.caseSensitive = false;
        sqlProblems.Add(p20);

        // Problema 21: LEFT JOIN para incluir usuarios sin solicitudes
        SQLProblem p21 = new SQLProblem();
        p21.problemDescription = "[LEFT JOIN] Todos los usuarios y los documentos que pidieron (aunque no pidan nada) - mostrar Name del usuario, Name del documento y LastAccess.";
        p21.expectedQuery = "SELECT U.NAME, D.NAME, R.LASTACCESS FROM USERS U LEFT JOIN RECORDS R ON U.ID = R.VISITOR LEFT JOIN DOCUMENTS D ON R.REQUESTED = D.ID";
        p21.successMessage = "[System]: ¡Excelente! Has incluido todos los usuarios, incluso los que no tienen solicitudes.";
        p21.errorMessage = "[ERROR]: Usa LEFT JOIN: SELECT U.Name, D.Name, R.LastAccess FROM Users U LEFT JOIN Records R ON U.Id = R.Visitor LEFT JOIN Documents D ON R.Requested = D.Id";
        p21.caseSensitive = false;
        sqlProblems.Add(p21);

        // Problema 22: LEFT JOIN inverso - documentos con sus solicitantes
        SQLProblem p22 = new SQLProblem();
        p22.problemDescription = "[LEFT JOIN] Todos los documentos y quién los pidió (si alguien los pidió) - mostrar Name del documento, solicitadoPor (alias para Name del usuario) y LastAccess.";
        p22.expectedQuery = "SELECT D.NAME, U.NAME AS SOLICITADOPOR, R.LASTACCESS FROM DOCUMENTS D LEFT JOIN RECORDS R ON D.ID = R.REQUESTED LEFT JOIN USERS U ON R.VISITOR = U.ID";
        p22.successMessage = "[System]: ¡Perfecto! Has mostrado todos los documentos, incluso los que nadie ha solicitado.";
        p22.errorMessage = "[ERROR]: Usa LEFT JOIN con alias: SELECT D.Name, U.Name AS solicitadoPor, R.LastAccess FROM Documents D LEFT JOIN Records R ON D.Id = R.Requested LEFT JOIN Users U ON R.Visitor = U.Id";
        p22.caseSensitive = false;
        sqlProblems.Add(p22);

        // Problema 23: LEFT JOIN + GROUP BY + COALESCE
        SQLProblem p23 = new SQLProblem();
        p23.problemDescription = "[LEFT JOIN + COALESCE] Usuarios y cantidad total de accesos (aunque no tengan registros) - mostrar Name del usuario y totalAccesos usando COALESCE para manejar NULLs.";
        p23.expectedQuery = "SELECT U.NAME, COALESCE(SUM(R.TIMESREQUESTED), 0) AS TOTALACCESOS FROM USERS U LEFT JOIN RECORDS R ON U.ID = R.VISITOR GROUP BY U.NAME";
        p23.successMessage = "[System]: ¡Excelente! Has manejado correctamente los valores NULL con COALESCE y calculado totales por usuario.";
        p23.errorMessage = "[ERROR]: Usa COALESCE para NULLs: SELECT U.Name, COALESCE(SUM(R.TimesRequested), 0) AS totalAccesos FROM Users U LEFT JOIN Records R ON U.Id = R.Visitor GROUP BY U.Name";
        p23.caseSensitive = false;
        sqlProblems.Add(p23);

        // Problema 24: RIGHT JOIN para preservar registros huérfanos
        SQLProblem p24 = new SQLProblem();
        p24.problemDescription = "[RIGHT JOIN] Todos los registros de acceso, incluso si el usuario fue eliminado - mostrar Id del registro, Name del usuario, Name del documento y LastAccess.";
        p24.expectedQuery = "SELECT R.ID, U.NAME, D.NAME, R.LASTACCESS FROM USERS U RIGHT JOIN RECORDS R ON U.ID = R.VISITOR INNER JOIN DOCUMENTS D ON R.REQUESTED = D.ID";
        p24.successMessage = "[System]: ¡Perfecto! Has preservado todos los registros, incluso aquellos con usuarios eliminados.";
        p24.errorMessage = "[ERROR]: Usa RIGHT JOIN: SELECT R.Id, U.Name, D.Name, R.LastAccess FROM Users U RIGHT JOIN Records R ON U.Id = R.Visitor INNER JOIN Documents D ON R.Requested = D.Id";
        p24.caseSensitive = false;
        sqlProblems.Add(p24);

        // Problema 25: RIGHT JOIN doble con WHERE específico
        SQLProblem p25 = new SQLProblem();
        p25.problemDescription = "[RIGHT JOIN + WHERE] Usuarios japoneses (o sus registros si existieran) - mostrar Name del usuario, Country, Name del documento y LastAccess.";
        p25.expectedQuery = "SELECT U.NAME, U.COUNTRY, D.NAME, R.LASTACCESS FROM USERS U RIGHT JOIN RECORDS R ON U.ID = R.VISITOR RIGHT JOIN DOCUMENTS D ON R.REQUESTED = D.ID WHERE U.COUNTRY = 'JAPÓN'";
        p25.successMessage = "[System]: ¡Excelente! Has filtrado usuarios japoneses preservando sus registros de acceso.";
        p25.errorMessage = "[ERROR]: Usa RIGHT JOIN doble con filtro: SELECT U.Name, U.Country, D.Name, R.LastAccess FROM Users U RIGHT JOIN Records R ON U.Id = R.Visitor RIGHT JOIN Documents D ON R.Requested = D.Id WHERE U.Country = 'Japón'";
        p25.caseSensitive = false;
        sqlProblems.Add(p25);
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
                consoleOutputText.text += "[System]: .\n";
                consoleOutputText.text += "[System]: .\n";
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
