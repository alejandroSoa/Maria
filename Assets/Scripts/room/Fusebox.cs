using UnityEngine;
using TMPro;

public class Fusebox : MonoBehaviour
{
    [Header("Item Generation")]
    [SerializeField] public GameObject itemBaseTemplate; // Template base para copiar
    [SerializeField] public Transform itemsParent; // Padre donde se crean los items
    
    [Header("UI References")]
    [SerializeField] private GameObject inventoryNormalUI; // Inventario modo normal
    [SerializeField] private GameObject inventoryEditUI; // Inventario modo edición
    [SerializeField] private GameObject messageConsole; // Consola de comandos
    [SerializeField] private TMP_InputField consoleInput; // Input de la consola (después de FUSE>)

    [Header("Validation Messages")]
    public GameObject wrongCaseMessage; // Wrong_case GameObject
    public GameObject successCaseMessage; // Success_case GameObject
    public GameObject buttonBack; // Button_back GameObject
    
    [Header("Lock System")]
    public bool isFuseboxLocked = false; // Estado de bloqueo de la caja
    
    [Header("Multi-Table System")]
    [SerializeField] private int currentActiveTable = 0; // Índice de la tabla activa
    [SerializeField] private UnityEngine.UI.Button previousTableButton; // Botón para tabla anterior
    [SerializeField] private UnityEngine.UI.Button nextTableButton; // Botón para tabla siguiente
    [SerializeField] private TextMeshProUGUI tableInfoText; // Texto informativo de la tabla actual
    
    private string currentFieldBeingAssigned = "";
    private string selectedFuseType = "";
    private bool isInSelectionMode = false;
    
    // Claves para guardar asignaciones
    private const string ASSIGNMENT_KEY_PREFIX = "FuseAssignment_";
    private const string TOLERANCE_MIN_KEY_PREFIX = "FuseTolMin_";
    private const string TOLERANCE_MAX_KEY_PREFIX = "FuseTolMax_";
    
    void Start()
    {
        int currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        if (currentScene == 2)
        {
            // En escena 2, limpiar todos los fusibles para que el jugador los configure
            ClearAllFuses();
        }
        else
        {
            // En otras escenas, cargar fusibles completos automáticamente
            InitializeDefaultFuses();
        }
        
        GenerateFuseboxSlots();
        SetupUI();
    }
    
    void OnEnable()
    {
        // Generar slots de fusibles
        Invoke("GenerateFuseboxSlots", 0.1f); // Pequeño delay
    }
    
    void SetupUI()
    {
        // Configurar input de consola
        if (consoleInput != null)
        {
            consoleInput.onSubmit.AddListener(OnConsoleCommandEntered);
        }
        
        // Ocultar consola al inicio
        if (messageConsole != null)
        {
            messageConsole.SetActive(false);
        }
        
        // Asegurar modo normal del inventario
        SetInventoryMode(false);
        
        // Configurar botones de navegación de tablas
        SetupTableNavigationButtons();
        
        // Actualizar información de tabla
        UpdateTableInfoDisplay();
    }


    
    /// <summary>
    /// Genera los slots de la caja de fusibles basándose en el template
    /// </summary>
    public void GenerateFuseboxSlots()
    {
        if (itemBaseTemplate == null || itemsParent == null)
        {
            Debug.LogWarning("Template o parent no asignados en Fusebox");
            return;
        }
        
        Debug.Log("=== Generando slots de fusibles ===");
        
        // Limpiar slots existentes
        ClearExistingSlots();
        
        // Obtener datos de la tabla activa
        var currentTable = GetCurrentTable();
        if (currentTable == null)
        {
            Debug.LogError($"No se pudo obtener la tabla en el índice {currentActiveTable}");
            return;
        }
        
        // Ocultar el template base
        itemBaseTemplate.SetActive(false);
        
        // Crear slots para cada campo de la tabla activa
        foreach (var slot in currentTable.columns)
        {
            string fieldName = GetFieldKey(currentTable.tableName, slot.Key);
            string displayName = slot.Value;
            string assignedType = GetAssignedDataType(fieldName);
            
            CreateSlotFromTemplate(fieldName, displayName, assignedType);
        }
        
        Debug.Log($"Generados {currentTable.columns.Count} slots de fusibles para tabla {currentTable.tableName}");
    }
    
    // Sistema de múltiples tablas
    [System.Serializable]
    public class DatabaseTable
    {
        public string tableName;
        public System.Collections.Generic.Dictionary<string, string> columns; // columna -> display name
        public System.Collections.Generic.Dictionary<string, string> expectedTypes; // columna -> tipo esperado
        public System.Collections.Generic.Dictionary<string, int> expectedSizes; // columna -> tamaño esperado
        
        public DatabaseTable(string name, System.Collections.Generic.Dictionary<string, string> cols, 
                            System.Collections.Generic.Dictionary<string, string> types, 
                            System.Collections.Generic.Dictionary<string, int> sizes)
        {
            tableName = name;
            columns = cols;
            expectedTypes = types;
            expectedSizes = sizes;
        }
    }
    
    private static readonly DatabaseTable[] DatabaseTables = new DatabaseTable[]
    {
        #region USERS
        new DatabaseTable(
            "USERS",
            // Columnas (clave -> nombre display)
            new System.Collections.Generic.Dictionary<string, string>()
            {
                { "Id", "Id" },
                { "Name", "Name" },
                { "LastName", "LastName" },
                { "Email", "Email" },
                { "Phone", "Phone" },
                { "Age", "Age" },
                { "Birthday", "Birthday" },
                { "Address", "Address" },
                { "City", "City" },
                { "Country", "Country" },
                { "PriorityLevel", "PriorityLevel" },
                { "Status", "Status" },
                { "LastSeen", "LastSeen" },
                { "CreatedAt", "CreatedAt" },
                { "UpdatedAt", "UpdatedAt" }
            },
            // Tipos esperados (clave -> tipo de dato)
            new System.Collections.Generic.Dictionary<string, string>()
            {
                { "Id", "INT" },
                { "Name", "VARCHAR" },
                { "LastName", "VARCHAR" },
                { "Email", "VARCHAR" },
                { "Phone", "INT" },
                { "Age", "INT" },
                { "Birthday", "DATE" },
                { "Address", "VARCHAR" },
                { "City", "VARCHAR" },
                { "Country", "VARCHAR" },
                { "PriorityLevel", "INT" },
                { "Status", "BOOL" },
                { "LastSeen", "DATE" },
                { "CreatedAt", "DATE" },
                { "UpdatedAt", "DATE" }
            },
            // Tamaños esperados (clave -> tamaño/tolerancia)
            new System.Collections.Generic.Dictionary<string, int>()
            {
                { "Id", 2000 },
                { "Name", 100 },
                { "LastName", 100 },
                { "Email", 120 },
                { "Phone", 15 },
                { "Age", 2 },
                { "Birthday", 0 },
                { "Address", 200 },
                { "City", 100 },
                { "Country", 100 },
                { "PriorityLevel", 2 },
                { "Status", 1 },
                { "LastSeen", 0 },
                { "CreatedAt", 0 },
                { "UpdatedAt", 0 }
            }
        ),
        #endregion

        #region PERMISSIONS
        new DatabaseTable(
            "PERMISSIONS",
            // Columnas (clave -> nombre display)
            new System.Collections.Generic.Dictionary<string, string>()
            {
                { "Id", "Id" },
                { "Name", "Name" },
                { "Description", "Description" },
                { "CreatedAt", "CreatedAt" },
                { "UpdatedAt", "UpdatedAt" }
            },
            // Tipos esperados (clave -> tipo de dato)
            new System.Collections.Generic.Dictionary<string, string>()
            {
                { "Id", "INT" },
                { "Name", "VARCHAR" },
                { "Description", "VARCHAR" },
                { "CreatedAt", "DATE" },
                { "UpdatedAt", "DATE" }
            },
            // Tamaños esperados (clave -> tamaño/tolerancia)
            new System.Collections.Generic.Dictionary<string, int>()
            {
                { "Id", 2000 },
                { "Name", 100 },
                { "Description", 255 },
                { "CreatedAt", 0 },
                { "UpdatedAt", 0 }
            }
        ),
        #endregion

        #region DOCUMENTS
        new DatabaseTable(
            "DOCUMENTS",
            // Columnas (clave -> nombre display)
            new System.Collections.Generic.Dictionary<string, string>()
            {
                { "Id", "Id" },
                { "Name", "Name" },
                { "HasPassword", "HasPassword" },
                { "Password", "Password" },
                { "FileType", "FileType" },
                { "FileSize", "FileSize" }
            },
            // Tipos esperados (clave -> tipo de dato)
            new System.Collections.Generic.Dictionary<string, string>()
            {
                { "Id", "INT" },
                { "Name", "VARCHAR" },
                { "HasPassword", "BOOL" },
                { "Password", "VARCHAR" },
                { "FileType", "VARCHAR" },
                { "FileSize", "INT" }
            },
            // Tamaños esperados (clave -> tamaño/tolerancia)
            new System.Collections.Generic.Dictionary<string, int>()
            {
                { "Id", 2000 },
                { "Name", 100 },
                { "HasPassword", 0 },
                { "Password", 100 },
                { "FileType", 20 },
                { "FileSize", 500 }
            }
        )
        #endregion
    };
    
    private static readonly System.Collections.Generic.Dictionary<string, string> DataTypes = new System.Collections.Generic.Dictionary<string, string>()
    {
        { "INT", "Entero" },
        { "DATE", "Fecha" },
        { "VARCHAR", "Texto Variable" },
        { "BOOL", "Booleano" }
    };
    
    /// <summary>
    /// Obtiene la tabla activa actual
    /// </summary>
    private DatabaseTable GetCurrentTable()
    {
        if (currentActiveTable >= 0 && currentActiveTable < DatabaseTables.Length)
            return DatabaseTables[currentActiveTable];
        return null;
    }
    
    /// <summary>
    /// Genera una clave única para un campo considerando la tabla
    /// </summary>
    private string GetFieldKey(string tableName, string fieldName)
    {
        return $"{tableName}_{fieldName}";
    }
    
    /// <summary>
    /// Cambia la tabla activa
    /// </summary>
    public void SetActiveTable(int tableIndex)
    {
        if (tableIndex >= 0 && tableIndex < DatabaseTables.Length)
        {
            currentActiveTable = tableIndex;
            Debug.Log($"Tabla activa cambiada a: {DatabaseTables[tableIndex].tableName}");
            GenerateFuseboxSlots(); // Regenerar slots
            UpdateTableInfoDisplay(); // Actualizar display
        }
        else
        {
            Debug.LogError($"Índice de tabla inválido: {tableIndex}");
        }
    }
    
    /// <summary>
    /// Obtiene el nombre de la tabla activa
    /// </summary>
    public string GetActiveTableName()
    {
        var currentTable = GetCurrentTable();
        return currentTable?.tableName ?? "UNKNOWN";
    }
    
    /// <summary>
    /// Obtiene la cantidad total de tablas
    /// </summary>
    public int GetTableCount()
    {
        return DatabaseTables.Length;
    }
    
    /// <summary>
    /// Obtiene los nombres de todas las tablas
    /// </summary>
    public string[] GetAllTableNames()
    {
        string[] names = new string[DatabaseTables.Length];
        for (int i = 0; i < DatabaseTables.Length; i++)
        {
            names[i] = DatabaseTables[i].tableName;
        }
        return names;
    }
    
    /// <summary>
    /// Configura los eventos de los botones de navegación entre tablas
    /// </summary>
    private void SetupTableNavigationButtons()
    {
        // Configurar botón anterior
        if (previousTableButton != null)
        {
            previousTableButton.onClick.RemoveAllListeners();
            previousTableButton.onClick.AddListener(() => {
                PreviousTable();
            });
        }
        else
        {
            Debug.LogWarning("Previous Table Button no está asignado en el inspector");
        }
        
        // Configurar botón siguiente
        if (nextTableButton != null)
        {
            nextTableButton.onClick.RemoveAllListeners();
            nextTableButton.onClick.AddListener(() => {
                NextTable();
            });
        }
        else
        {
            Debug.LogWarning("Next Table Button no está asignado en el inspector");
        }
        
        // Configurar estado inicial de botones
        UpdateNavigationButtonsState();
    }
    
    /// <summary>
    /// Actualiza el estado de los botones de navegación
    /// </summary>
    private void UpdateNavigationButtonsState()
    {
        // Los botones siempre están habilitados ya que la navegación es cíclica
        if (previousTableButton != null)
        {
            previousTableButton.interactable = !isFuseboxLocked;
        }
        
        if (nextTableButton != null)
        {
            nextTableButton.interactable = !isFuseboxLocked;
        }
    }
    
    /// <summary>
    /// Actualiza la información mostrada de la tabla actual
    /// </summary>
    private void UpdateTableInfoDisplay()
    {
        if (tableInfoText != null)
        {
            var currentTable = GetCurrentTable();
            if (currentTable != null)
            {
                string info = $"Table: {currentTable.tableName} ({currentTable.columns.Count} fields) [{currentActiveTable + 1}/{DatabaseTables.Length}]";
                tableInfoText.text = info;
            }
            else
            {
                tableInfoText.text = "Error: No table selected";
            }
        }
        else
        {
            Debug.LogWarning("Table Info Text no está asignado en el inspector");
        }
    }
    
    /// <summary>
    /// Obtiene el tipo de dato asignado a un campo
    /// </summary>
    private string GetAssignedDataType(string fieldName)
    {
        string assignmentKey = ASSIGNMENT_KEY_PREFIX + fieldName;
        return PlayerPrefs.GetString(assignmentKey, "Unassigned");
    }
    
    /// <summary>
    /// Obtiene la tolerancia configurada para un campo
    /// </summary>
    private string GetToleranceDisplay(string fieldName)
    {
        string assignment = GetAssignedDataType(fieldName);
        if (assignment == "Unassigned") return "";
        
        int minTol = PlayerPrefs.GetInt(TOLERANCE_MIN_KEY_PREFIX + fieldName, 0);
        int maxTol = PlayerPrefs.GetInt(TOLERANCE_MAX_KEY_PREFIX + fieldName, 0);
        
        if (minTol > 0 || maxTol > 0)
        {
            return $" ({minTol}-{maxTol})";
        }
        return "";
    }
    
    /// <summary>
    /// Obtiene el status en inglés para un campo
    /// </summary>
    private string GetFieldStatus(string fieldName)
    {
        string assignment = GetAssignedDataType(fieldName);
        
        if (assignment == "Unassigned")
        {
            return "Unassigned";
        }
        
        // Si tiene asignación pero está en proceso de configuración
        if (currentFieldBeingAssigned == fieldName && isInSelectionMode)
        {
            return "Processing";
        }
        
        return "Assigned";
    }
    
    /// <summary>
    /// Obtiene el formato abreviado del fusible asignado (FS-TIPO-TAMAÑO)
    /// </summary>
    private string GetFormattedFuseDisplay(string fieldName)
    {
        string assignment = GetAssignedDataType(fieldName);
        
        if (assignment == "Unassigned")
        {
            return "Unassigned";
        }
        
        // Si tiene asignación pero está en proceso de configuración
        if (currentFieldBeingAssigned == fieldName && isInSelectionMode)
        {
            return "Processing";
        }
        
        // Convertir a abreviatura de 2 caracteres
        string typeAbbrev = GetTypeAbbreviation(assignment);
        
        // Formatear fusible asignado como FS-TIPO-TAMAÑO
        string fuseDisplay = $"FS-{typeAbbrev}";
        
        // Agregar tamaño/tolerancia si es INT o VARCHAR
        if (assignment == "INT" || assignment == "VARCHAR")
        {
            int tolerance = PlayerPrefs.GetInt(TOLERANCE_MIN_KEY_PREFIX + fieldName, 0);
            if (tolerance > 0)
            {
                fuseDisplay += $"-{tolerance}";
            }
        }
        
        return fuseDisplay;
    }
    
    private static readonly System.Collections.Generic.Dictionary<string, string> TypeAbbreviations = new System.Collections.Generic.Dictionary<string, string>()
    {
        { "VARCHAR", "VC" },
        { "INT", "IN" },
        { "DATE", "DA" },
        { "BOOL", "BO" }
    };
    
    private string GetTypeAbbreviation(string type)
    {
        return TypeAbbreviations.TryGetValue(type, out string abbrev) ? abbrev : 
               (type.Length >= 2 ? type.Substring(0, 2) : type);
    }
    
    /// <summary>
    /// Ajusta el tamaño del texto según su longitud
    /// </summary>
    private void AdjustTextSize(TextMeshProUGUI textComponent, string displayText)
    {
        if (textComponent == null) return;
        
        // Tamaño base (puedes ajustar este valor según tu UI)
        float baseSize = textComponent.fontSize;
        
        // Si el texto es muy largo, reducir el tamaño
        if (displayText.Length > 10)
        {
            // Para textos muy largos (ej: FS-VC-1234)
            textComponent.fontSize = baseSize * 0.7f;
        }
        else if (displayText.Length > 8)
        {
            // Para textos largos (ej: FS-IN-123)
            textComponent.fontSize = baseSize * 0.85f;
        }
        else
        {
            // Para textos cortos mantener tamaño normal
            textComponent.fontSize = baseSize;
        }
    }
    

    
    /// <summary>
    /// Crea un slot basándose en el template
    /// </summary>
    private void CreateSlotFromTemplate(string fieldName, string displayName, string assignedType)
    {
        GameObject newSlot = Instantiate(itemBaseTemplate, itemsParent);
        newSlot.SetActive(true);
        newSlot.name = $"Slot_{fieldName}";
        
        // Configurar textos y botones
        var allComponents = newSlot.GetComponentsInChildren<Transform>();
        string displayText = GetFormattedFuseDisplay(fieldName);
        
        foreach (Transform child in allComponents)
        {
            string childName = child.name.ToLower();
            
            // Configurar textos
            if (child.TryGetComponent<TextMeshProUGUI>(out var text))
            {
                if (childName.Contains("title") || childName.Contains("column"))
                    text.text = displayName;
                else if (childName.Contains("status"))
                {
                    text.text = displayText;
                    AdjustTextSize(text, displayText);
                }
            }
            
            // Configurar botón
            if (child.TryGetComponent<UnityEngine.UI.Button>(out var button) && 
                (childName.Contains("assign") || childName.Contains("btn")))
            {
                string fieldRef = fieldName;
                button.onClick.AddListener(() => OnWrenchButtonClicked(fieldRef));
                button.interactable = !isFuseboxLocked;
            }
        }
        
        // Configurar visibilidad de Icon y Block según el estado actual
        Transform[] children = newSlot.GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
        {
            if (child.name == "Icon")
            {
                child.gameObject.SetActive(!isFuseboxLocked); // Icon visible cuando NO está bloqueado
            }
            else if (child.name == "Block")
            {
                child.gameObject.SetActive(isFuseboxLocked); // Block visible cuando ESTÁ bloqueado
            }
        }
        
        Debug.Log($"Slot creado: {displayName} -> {assignedType}");
    }
    
    /// <summary>
    /// Limpia slots existentes
    /// </summary>
    private void ClearExistingSlots()
    {
        if (itemsParent == null) return;
        
        for (int i = itemsParent.childCount - 1; i >= 0; i--)
        {
            Transform child = itemsParent.GetChild(i);
            if (child.gameObject != itemBaseTemplate && child.gameObject.name.StartsWith("Slot_"))
            {
                DestroyImmediate(child.gameObject);
            }
        }
    }
    
    /// <summary>
    /// Se llama cuando se hace clic en el botón wrench de un slot
    /// </summary>
    public void OnWrenchButtonClicked(string fieldName)
    {
        Debug.Log($"Asignando fusible para campo: {fieldName}");
        currentFieldBeingAssigned = fieldName;
        
        // Cambiar a modo selección del inventario
        SetInventoryMode(true);
        isInSelectionMode = true;
        
        // Regenerar slots para actualizar status a "Processing"
        GenerateFuseboxSlots();
    }
    
    /// <summary>
    /// Cambia entre modo normal y modo edición del inventario
    /// </summary>
    private void SetInventoryMode(bool editMode)
    {
        if (inventoryNormalUI != null && inventoryEditUI != null)
        {
            inventoryNormalUI.SetActive(!editMode);
            inventoryEditUI.SetActive(editMode);
        }
    }
    
    /// <summary>
    /// Se llama cuando se selecciona un fusible del inventario en modo edición
    /// </summary>
    public void OnFuseSelected(string fuseType)
    {
        if (!isInSelectionMode) return;
        
        Debug.Log($"Fusible seleccionado: {fuseType}");
        selectedFuseType = fuseType;
        
        // Mostrar consola de mensajes para configurar tolerancia
        ShowToleranceConfiguration();
    }
    
    /// <summary>
    /// Muestra la consola de comandos para configurar el fusible
    /// </summary>
    private void ShowToleranceConfiguration()
    {
        if (messageConsole == null) return;
        
        messageConsole.SetActive(true);
        
        // Configurar input según el tipo de fusible
        ConfigureInputForFuseType();
        
        // Mostrar instrucciones según el tipo
        ShowInstructionsForFuseType();
        
        // Ocultar casos de éxito/error
        HideValidationMessages();
        
        // Limpiar y enfocar input de consola
        if (consoleInput != null)
        {
            consoleInput.text = "";
            consoleInput.ActivateInputField();
        }
    }
    
    /// <summary>
    /// Se llama cuando se ingresa un comando en la consola
    /// </summary>
    public void OnConsoleCommandEntered(string command)
    {
        if (string.IsNullOrEmpty(command)) return;
        
        Debug.Log($"Comando ingresado: {command}");
        
        // Si el comando es "exit", cancelar la selección
        if (command.ToLower() == "exit")
        {
            CancelAssignmentProcess();
            return;
        }
        
        // Validar según el tipo de fusible
        bool isValid = ValidateCommandForFuseType(command);
        
        if (!isValid)
        {
            ShowValidationMessage(false, GetErrorMessageForFuseType());
            return;
        }
        
        // Si llegamos aquí, el comando es válido
        ProcessValidCommand(command);
    }
    
    /// <summary>
    /// Valida el comando según el tipo de fusible
    /// </summary>
    private bool ValidateCommandForFuseType(string input)
    {
        if (string.IsNullOrEmpty(selectedFuseType)) return false;
        
        return selectedFuseType == "BOOL" || selectedFuseType == "DATE" ? 
               input.ToLower() == "confirm" : 
               int.TryParse(input, out int number) && number >= 0 && number <= 9999;
    }
    
    /// <summary>
    /// Obtiene el mensaje de error apropiado según el tipo de fusible
    /// </summary>
    private string GetErrorMessageForFuseType()
    {
        if (string.IsNullOrEmpty(selectedFuseType)) return "Error de validación";
        
        if (selectedFuseType == "BOOL" || selectedFuseType == "DATE")
        {
            return "[System]: Escriba 'confirm'...";
        }
        else if (selectedFuseType == "INT" || selectedFuseType == "VARCHAR")
        {
            return "[System]: Solo rangos entre 0 y 9999...";
        }
        
        return "[System]: Comando no valido";
    }
    
    /// <summary>
    /// Procesa un comando válido
    /// </summary>
    private void ProcessValidCommand(string command)
    {
        if (string.IsNullOrEmpty(currentFieldBeingAssigned) || string.IsNullOrEmpty(selectedFuseType))
        {
            ShowValidationMessage(false, "Error interno del sistema");
            return;
        }
        
        // Verificar que el jugador tenga el fusible (o permitir modificar existente)
        int availableQuantity = Inventory.GetItemQuantity(selectedFuseType);
        string currentAssignment = GetAssignedDataType(currentFieldBeingAssigned);
        
        // Si no tiene el fusible Y no está modificando una asignación existente
        if (availableQuantity <= 0 && currentAssignment == "Unassigned")
        {
            ShowValidationMessage(false, "No tienes este fusible en el inventario");
            return;
        }
        
        // Determinar valor de tolerancia
        int tolerance = 0;
        if (selectedFuseType == "INT" || selectedFuseType == "VARCHAR")
        {
            tolerance = int.Parse(command);
        }
        
        // Realizar asignación
        AssignFuseToField(currentFieldBeingAssigned, selectedFuseType, tolerance);
        ShowValidationMessage(true, "Fusible asignado correctamente");
        
        // Cerrar inmediatamente
        CloseAssignmentProcess();
    }
    
    /// <summary>
    /// Asigna un fusible a un campo específico
    /// </summary>
    private void AssignFuseToField(string fieldName, string fuseType, int tolerance)
    {
        string currentAssignment = GetAssignedDataType(fieldName);
        
        // Si ya tenía un fusible asignado, devolverlo al inventario
        if (currentAssignment != "Unassigned")
        {
            string oldItemKey = "Item_" + currentAssignment;
            int oldQuantity = PlayerPrefs.GetInt(oldItemKey, 0);
            PlayerPrefs.SetInt(oldItemKey, oldQuantity + 1);
            Debug.Log($"Fusible anterior {currentAssignment} devuelto al inventario");
        }
        
        // Guardar nueva asignación
        PlayerPrefs.SetString(ASSIGNMENT_KEY_PREFIX + fieldName, fuseType);
        
        // Solo guardar tolerancia si el tipo la requiere
        if (fuseType == "INT" || fuseType == "VARCHAR")
        {
            PlayerPrefs.SetInt(TOLERANCE_MIN_KEY_PREFIX + fieldName, tolerance);
            PlayerPrefs.SetInt(TOLERANCE_MAX_KEY_PREFIX + fieldName, tolerance);
        }
        else
        {
            // Para BOOL y DATE, limpiar tolerancias previas
            PlayerPrefs.DeleteKey(TOLERANCE_MIN_KEY_PREFIX + fieldName);
            PlayerPrefs.DeleteKey(TOLERANCE_MAX_KEY_PREFIX + fieldName);
        }
        
        // Reducir cantidad en inventario del nuevo fusible
        string newItemKey = "Item_" + fuseType;
        int newCurrentQuantity = PlayerPrefs.GetInt(newItemKey, 0);
        PlayerPrefs.SetInt(newItemKey, newCurrentQuantity - 1);
        
        PlayerPrefs.Save();
        
        Debug.Log($"Fusible {fuseType} asignado a {fieldName} con tolerancia {tolerance}");
        
        // Regenerar slots para mostrar cambios
        GenerateFuseboxSlots();
    }
    
    /// <summary>
    /// Cierra el proceso de asignación
    /// </summary>
    private void CloseAssignmentProcess()
    {
        // Ocultar consola
        if (messageConsole != null) messageConsole.SetActive(false);
        
        // Volver a modo normal del inventario
        SetInventoryMode(false);
        
        // Resetear variables
        isInSelectionMode = false;
        currentFieldBeingAssigned = "";
        selectedFuseType = "";
        
        // Regenerar slots para actualizar status final
        GenerateFuseboxSlots();
    }
    
    /// <summary>
    /// Cancela el proceso de asignación (comando "exit")
    /// </summary>
    private void CancelAssignmentProcess()
    {
        Debug.Log("Proceso de asignación cancelado");
        
        // Mostrar button_back cuando se cancela
        if (buttonBack != null)
        {
            buttonBack.SetActive(true);
        }
        
        CloseAssignmentProcess();
    }
    
    /// <summary>
    /// Resetea el input para permitir reintento después de un error
    /// </summary>
    private void ResetInputForRetry()
    {
        // Ocultar mensajes de validación
        HideValidationMessages();
        
        // Limpiar y enfocar input para permitir reintento
        if (consoleInput != null)
        {
            consoleInput.text = "";
            consoleInput.ActivateInputField();
        }
        
        Debug.Log("Input reseteado para reintento");
    }
    
    /// <summary>
    /// Configura el input según el tipo de fusible seleccionado
    /// </summary>
    private void ConfigureInputForFuseType()
    {
        if (consoleInput == null || string.IsNullOrEmpty(selectedFuseType)) return;
        
        if (selectedFuseType == "BOOL" || selectedFuseType == "DATE")
        {
            consoleInput.placeholder.GetComponent<TextMeshProUGUI>().text = "[System]: Write confirm...";
        }
        else
        {
            consoleInput.placeholder.GetComponent<TextMeshProUGUI>().text = "[System]: Enter limit...";
        }
    }
    
    /// <summary>
    /// Muestra las instrucciones apropiadas según el tipo de fusible
    /// </summary>
    private void ShowInstructionsForFuseType()
    {
        TextMeshProUGUI instructionsText = GetInstructionsText();
        if (instructionsText == null || string.IsNullOrEmpty(selectedFuseType)) return;
        
        if (selectedFuseType == "BOOL" || selectedFuseType == "DATE")
        {
            instructionsText.text = "[SYSTEM] Escribe confirm...";
        }
        else if (selectedFuseType == "INT" || selectedFuseType == "VARCHAR")
        {
            instructionsText.text = "[SYSTEM] Agrega un limite...";
        }
        else
        {
            instructionsText.text = "[SYSTEM] Configure el fusible...";
        }
    }
    
    /// <summary>
    /// Obtiene el componente de texto de instrucciones
    /// </summary>
    private TextMeshProUGUI GetInstructionsText()
    {
        if (messageConsole == null) return null;
        
        TextMeshProUGUI[] texts = messageConsole.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (TextMeshProUGUI text in texts)
        {
            if (text.gameObject.name.ToLower().Contains("instruction"))
            {
                return text;
            }
        }
        return null;
    }
    
    /// <summary>
    /// Oculta los mensajes de validación
    /// </summary>
    private void HideValidationMessages()
    {
        if (wrongCaseMessage != null)
            wrongCaseMessage.SetActive(false);
            
        if (successCaseMessage != null)
            successCaseMessage.SetActive(false);
    }
    
    /// <summary>
    /// Muestra mensaje de caso correcto o incorrecto
    /// </summary>
    private void ShowValidationMessage(bool isSuccess, string message = "")
    {
        // Ocultar ambos primero
        HideValidationMessages();
        
        if (isSuccess)
        {
            // Mostrar caso de éxito
            if (successCaseMessage != null)
            {
                successCaseMessage.SetActive(true);
            }
            
            // Mostrar button_back cuando es exitoso
            if (buttonBack != null)
            {
                buttonBack.SetActive(true);
            }
        }
        else
        {
            // Mostrar caso de error
            if (wrongCaseMessage != null)
            {
                wrongCaseMessage.SetActive(true);
            }
            
            // Para casos de error, recargar la interfaz inmediatamente
            ResetInputForRetry();
        }
    }
    
    /// <summary>
    /// Borra al azar cierto número de fusibles de TODAS las tablas y los deja desasignados
    /// </summary>
    public void RemoveRandomFuses(int numberOfFusesToRemove)
    {
        System.Collections.Generic.List<string> assignedFields = new System.Collections.Generic.List<string>();
        
        // Obtener todos los campos que tienen fusibles asignados de TODAS las tablas
        foreach (var table in DatabaseTables)
        {
            foreach (var slot in table.columns)
            {
                string fieldName = GetFieldKey(table.tableName, slot.Key);
                string assignment = GetAssignedDataType(fieldName);
                
                if (assignment != "Unassigned")
                {
                    assignedFields.Add(fieldName);
                }
            }
        }
        
        // Limitar el número a remover a los disponibles
        int fusesToRemove = Mathf.Min(numberOfFusesToRemove, assignedFields.Count);
        
        // Seleccionar campos al azar de todas las tablas y desasignar
        for (int i = 0; i < fusesToRemove; i++)
        {
            int randomIndex = Random.Range(0, assignedFields.Count);
            string fieldToUnassign = assignedFields[randomIndex];
            assignedFields.RemoveAt(randomIndex);
            
            // Eliminar fusible completamente (no devolverlo al inventario)
            string currentAssignment = GetAssignedDataType(fieldToUnassign);
            if (currentAssignment != "Unassigned")
            {
                // Solo limpiar asignación sin devolver al inventario
                PlayerPrefs.DeleteKey(ASSIGNMENT_KEY_PREFIX + fieldToUnassign);
                PlayerPrefs.DeleteKey(TOLERANCE_MIN_KEY_PREFIX + fieldToUnassign);
                PlayerPrefs.DeleteKey(TOLERANCE_MAX_KEY_PREFIX + fieldToUnassign);
                
                Debug.Log($"Fusible {currentAssignment} eliminado permanentemente del campo {fieldToUnassign}");
            }
        }
        
        PlayerPrefs.Save();
        
        // Regenerar slots para mostrar cambios
        GenerateFuseboxSlots();
        
        Debug.Log($"Removidos {fusesToRemove} fusibles al azar");
    }
    
    /// <summary>
    /// Cierra la caja de fusibles y no permite editarla (modo lock)
    /// </summary>
    public void LockFusebox()
    {
        if (itemsParent == null) return;
        
        // Establecer estado de bloqueo
        isFuseboxLocked = true;
        
        // Actualizar estado de botones de navegación
        UpdateNavigationButtonsState();
        
        // Recorrer todos los slots y desactivar botones + mostrar imagen de lock
        for (int i = 0; i < itemsParent.childCount; i++)
        {
            Transform slot = itemsParent.GetChild(i);
            if (slot.gameObject != itemBaseTemplate && slot.name.StartsWith("Slot_"))
            {
                // Desactivar botones de asignación
                UnityEngine.UI.Button[] buttons = slot.GetComponentsInChildren<UnityEngine.UI.Button>();
                foreach (UnityEngine.UI.Button button in buttons)
                {
                    if (button.gameObject.name.ToLower().Contains("assign") || button.gameObject.name.ToLower().Contains("btn"))
                    {
                        button.interactable = false;
                    }
                }
                
                // Configurar visibilidad: ocultar Icon y mostrar Text
                Transform[] children = slot.GetComponentsInChildren<Transform>();
                foreach (Transform child in children)
                {
                    if (child.name == "Icon")
                    {
                        child.gameObject.SetActive(false); // Ocultar Icon
                    }
                    else if (child.name == "Text")
                    {
                        child.gameObject.SetActive(true); // Mostrar Text
                    }
                }
            }
        }
        
        Debug.Log("Caja de fusibles bloqueada - no se puede editar");
    }
    
    /// <summary>
    /// Rehabilita la edición de la caja de fusibles (modo unlock)
    /// </summary>
    public void UnlockFusebox()
    {
        if (itemsParent == null) return;
        
        // Establecer estado de desbloqueado
        isFuseboxLocked = false;
        
        // Actualizar estado de botones de navegación
        UpdateNavigationButtonsState();
        
        // Recorrer todos los slots y activar botones + ocultar imagen de lock
        for (int i = 0; i < itemsParent.childCount; i++)
        {
            Transform slot = itemsParent.GetChild(i);
            if (slot.gameObject != itemBaseTemplate && slot.name.StartsWith("Slot_"))
            {
                // Activar botones de asignación
                UnityEngine.UI.Button[] buttons = slot.GetComponentsInChildren<UnityEngine.UI.Button>();
                foreach (UnityEngine.UI.Button button in buttons)
                {
                    if (button.gameObject.name.ToLower().Contains("assign") || button.gameObject.name.ToLower().Contains("btn"))
                    {
                        button.interactable = true;
                    }
                }
                
                // Configurar visibilidad: mostrar Icon y ocultar Block
                Transform[] children = slot.GetComponentsInChildren<Transform>();
                foreach (Transform child in children)
                {
                    if (child.name == "Icon")
                    {
                        child.gameObject.SetActive(true); // Mostrar Icon
                    }
                    else if (child.name == "Block")
                    {
                        child.gameObject.SetActive(false); // Ocultar Block
                    }
                }
            }
        }
        
        Debug.Log("Caja de fusibles desbloqueada - se puede editar");
    }
    
    /// <summary>
    /// Retorna el contenido de la caja de fusibles como tabla de base de datos
    /// </summary>
    public void Get(){GetFuseboxDatabaseTable();}

    public string GetFuseboxDatabaseTable()
    {
        return GetFuseboxDatabaseTable(-1); // -1 significa tabla activa
    }
    
    /// <summary>
    /// Obtiene la tabla de base de datos para una tabla específica o todas
    /// </summary>
    /// <param name="tableIndex">Índice de la tabla (-1 para tabla activa, -2 para todas las tablas)</param>
    public string GetFuseboxDatabaseTable(int tableIndex)
    {
        System.Text.StringBuilder tableBuilder = new System.Text.StringBuilder();
        
        if (tableIndex == -2) // Todas las tablas
        {
            tableBuilder.AppendLine("=== FUSEBOX DATABASE - ALL TABLES ===");
            
            for (int i = 0; i < DatabaseTables.Length; i++)
            {
                tableBuilder.AppendLine();
                tableBuilder.AppendLine($"=== TABLE: {DatabaseTables[i].tableName} ===");
                tableBuilder.AppendLine("FIELD_NAME | DATA_TYPE | TOLERANCE_MIN | TOLERANCE_MAX | STATUS");
                tableBuilder.AppendLine("------------------------------------------------------------");
                
                AppendTableData(tableBuilder, DatabaseTables[i]);
                
                tableBuilder.AppendLine("------------------------------------------------------------");
            }
        }
        else
        {
            // Tabla específica o activa
            DatabaseTable targetTable;
            if (tableIndex == -1)
            {
                targetTable = GetCurrentTable();
            }
            else if (tableIndex >= 0 && tableIndex < DatabaseTables.Length)
            {
                targetTable = DatabaseTables[tableIndex];
            }
            else
            {
                return "ERROR: Índice de tabla inválido";
            }
            
            if (targetTable == null) return "ERROR: No se pudo obtener la tabla";
            
            tableBuilder.AppendLine($"=== FUSEBOX DATABASE TABLE: {targetTable.tableName} ===");
            tableBuilder.AppendLine("FIELD_NAME | DATA_TYPE | TOLERANCE_MIN | TOLERANCE_MAX | STATUS");
            tableBuilder.AppendLine("------------------------------------------------------------");
            
            AppendTableData(tableBuilder, targetTable);
            
            tableBuilder.AppendLine("------------------------------------------------------------");
        }
        
        string tableResult = tableBuilder.ToString();
        Debug.Log("Tabla(s) de base de datos generada(s):");
        Debug.Log(tableResult);
        
        return tableResult;
    }
    
    /// <summary>
    /// Agrega los datos de una tabla específica al StringBuilder
    /// </summary>
    private void AppendTableData(System.Text.StringBuilder tableBuilder, DatabaseTable table)
    {
        foreach (var slot in table.columns)
        {
            string fieldKey = GetFieldKey(table.tableName, slot.Key);
            string fieldName = slot.Key;
            string assignment = GetAssignedDataType(fieldKey);
            string status;
            string toleranceMin = "NULL";
            string toleranceMax = "NULL";
            
            if (assignment == "Unassigned")
            {
                status = "UNASSIGNED";
                assignment = "NULL";
            }
            else
            {
                status = "ASSIGNED";
                
                // Obtener tolerancias si aplica
                if (assignment == "INT" || assignment == "VARCHAR")
                {
                    int minTol = PlayerPrefs.GetInt(TOLERANCE_MIN_KEY_PREFIX + fieldKey, 0);
                    int maxTol = PlayerPrefs.GetInt(TOLERANCE_MAX_KEY_PREFIX + fieldKey, 0);
                    
                    toleranceMin = minTol.ToString();
                    toleranceMax = maxTol.ToString();
                }
            }
            
            // Formatear fila de la tabla
            string row = $"{fieldName.PadRight(12)} | {assignment.PadRight(9)} | {toleranceMin.PadRight(13)} | {toleranceMax.PadRight(13)} | {status}";
            tableBuilder.AppendLine(row);
        }
    }
    
    /// <summary>
    /// Obtiene todas las tablas de la base de datos
    /// </summary>
    public string GetAllFuseboxDatabaseTables()
    {
        return GetFuseboxDatabaseTable(-2);
    }
    
    /// <summary>
    /// Cambia a la siguiente tabla (modo cíclico)
    /// </summary>
    public void NextTable()
    {
        int nextIndex = (currentActiveTable + 1) % DatabaseTables.Length;
        SetActiveTable(nextIndex);
        Debug.Log($"Navegando a tabla siguiente: {GetActiveTableName()}");
    }
    
    /// <summary>
    /// Cambia a la tabla anterior (modo cíclico)
    /// </summary>
    public void PreviousTable()
    {
        int prevIndex = currentActiveTable - 1;
        if (prevIndex < 0) prevIndex = DatabaseTables.Length - 1;
        SetActiveTable(prevIndex);
        Debug.Log($"Navegando a tabla anterior: {GetActiveTableName()}");
    }
    
    /// <summary>
    /// Obtiene información resumida de todas las tablas en formato CSV
    /// Incluye la validación de la tabla activa para debug
    /// </summary>
    public string GetTablesInfo()
    {
        System.Text.StringBuilder info = new System.Text.StringBuilder();
        info.AppendLine("=== FUSEBOX TABLES INFORMATION ===");
        info.AppendLine("TABLE_INDEX,TABLE_NAME,COLUMNS_COUNT,STATUS");
        
        for (int i = 0; i < DatabaseTables.Length; i++)
        {
            var table = DatabaseTables[i];
            string status = i == currentActiveTable ? "ACTIVE" : "INACTIVE";
            info.AppendLine($"{i},{table.tableName},{table.columns.Count},{status}");
        }
        
        // Agregar validación de la tabla activa para debug
        info.AppendLine("\n=== CURRENT ACTIVE TABLE VALIDATION ===");
        var currentTable = GetCurrentTable();
        if (currentTable != null)
        {
            string validation = ValidateTableConfiguration(currentTable.tableName);
            info.AppendLine(validation);
        }
        else
        {
            info.AppendLine("[ERROR]: No active table");
        }
        
        return info.ToString();
    }
    
    /// <summary>
    /// Valida la configuración de fusibles de una tabla específica
    /// Retorna CSV con columnas: COLUMN_NAME,EXPECTED_TYPE,EXPECTED_SIZE,ACTUAL_TYPE,ACTUAL_SIZE,VALIDATION
    /// </summary>
    /// <param name="tableName">Nombre de la tabla a validar</param>
    public string ValidateTableConfiguration(string tableName)
    {
        // Buscar la tabla en DatabaseTables
        DatabaseTable targetTable = null;
        foreach (var table in DatabaseTables)
        {
            if (table.tableName == tableName)
            {
                targetTable = table;
                break;
            }
        }
        
        if (targetTable == null)
        {
            return $"[ERROR]: Tabla '{tableName}' no encontrada";
        }
        
        System.Text.StringBuilder report = new System.Text.StringBuilder();
        report.AppendLine($"[System]: Validando tabla {tableName}");
        
        foreach (var column in targetTable.columns)
        {
            string columnName = column.Key;
            string expectedType = targetTable.expectedTypes[columnName];
            int expectedSize = targetTable.expectedSizes[columnName];
            
            // Obtener configuración actual
            string fieldKey = GetFieldKey(tableName, columnName);
            string actualType = PlayerPrefs.GetString($"FuseAssignment_{fieldKey}", "Unassigned");
            int actualSize = PlayerPrefs.GetInt($"FuseTolMin_{fieldKey}", 0);
            
            string actualSizeStr = actualSize > 0 ? actualSize.ToString() : "N/A";
            string validation = "GOOD";
            
            // Validar
            if (actualType == "Unassigned")
            {
                validation = "BAD";
                actualType = "NULL";
            }
            else if (actualType != expectedType)
            {
                validation = "BAD";
            }
            else if ((expectedType == "VARCHAR" || expectedType == "INT") && expectedSize > 0 && actualSize != expectedSize)
            {
                validation = "BAD";
            }
            
            report.AppendLine($"{columnName},{expectedType},{actualType},{actualSizeStr},{validation}");
        }
        
        return report.ToString();
    }
    
    /// <summary>
    /// Verifica si una tabla está completamente válida
    /// </summary>
    public bool IsTableValid(string tableName)
    {
        // Buscar la tabla en DatabaseTables
        DatabaseTable targetTable = null;
        foreach (var table in DatabaseTables)
        {
            if (table.tableName == tableName)
            {
                targetTable = table;
                break;
            }
        }
        
        if (targetTable == null)
        {
            return false;
        }
        
        foreach (var column in targetTable.columns)
        {
            string columnName = column.Key;
            string expectedType = targetTable.expectedTypes[columnName];
            int expectedSize = targetTable.expectedSizes[columnName];
            
            string fieldKey = GetFieldKey(tableName, columnName);
            string actualType = PlayerPrefs.GetString($"FuseAssignment_{fieldKey}", "Unassigned");
            int actualSize = PlayerPrefs.GetInt($"FuseTolMin_{fieldKey}", 0);
            
            if (actualType == "Unassigned" || actualType != expectedType)
            {
                return false;
            }
            
            if ((expectedType == "VARCHAR" || expectedType == "INT") && expectedSize > 0 && actualSize != expectedSize)
            {
                return false;
            }
        }
        
        return true;
    }
    
    /// <summary>
    /// Inicializa todos los fusibles con sus valores por defecto para pruebas
    /// </summary>
    private void InitializeDefaultFuses()
    {
        Debug.Log("=== Inicializando fusibles por defecto ===");
        
        // Recorrer todas las tablas
        foreach (var table in DatabaseTables)
        {
            foreach (var column in table.columns)
            {
                string fieldKey = GetFieldKey(table.tableName, column.Key);
                string expectedType = table.expectedTypes[column.Key];
                int expectedSize = table.expectedSizes[column.Key];
                
                // Asignar tipo de dato
                PlayerPrefs.SetString(ASSIGNMENT_KEY_PREFIX + fieldKey, expectedType);
                
                // Asignar tolerancia si aplica
                if (expectedType == "INT" || expectedType == "VARCHAR")
                {
                    PlayerPrefs.SetInt(TOLERANCE_MIN_KEY_PREFIX + fieldKey, expectedSize);
                    PlayerPrefs.SetInt(TOLERANCE_MAX_KEY_PREFIX + fieldKey, expectedSize);
                }
                
                Debug.Log($"Fusible inicializado: {fieldKey} -> {expectedType} ({expectedSize})");
            }
        }
        
        PlayerPrefs.Save();
        Debug.Log("=== Todos los fusibles inicializados correctamente ===");
    }
    
    /// <summary>
    /// Limpia todos los fusibles de todas las tablas (los deja sin asignar)
    /// </summary>
    private void ClearAllFuses()
    {
        
        // Recorrer todas las tablas
        foreach (var table in DatabaseTables)
        {
            foreach (var column in table.columns)
            {
                string fieldKey = GetFieldKey(table.tableName, column.Key);
                
                // Eliminar asignaciones
                PlayerPrefs.DeleteKey(ASSIGNMENT_KEY_PREFIX + fieldKey);
                PlayerPrefs.DeleteKey(TOLERANCE_MIN_KEY_PREFIX + fieldKey);
                PlayerPrefs.DeleteKey(TOLERANCE_MAX_KEY_PREFIX + fieldKey);
                
            }
        }
        
        PlayerPrefs.Save();
    }

}

