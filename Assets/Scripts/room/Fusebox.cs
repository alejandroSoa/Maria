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
    
    private string currentFieldBeingAssigned = "";
    private string selectedFuseType = "";
    private bool isInSelectionMode = false;
    
    // Claves para guardar asignaciones
    private const string ASSIGNMENT_KEY_PREFIX = "FuseAssignment_";
    private const string TOLERANCE_MIN_KEY_PREFIX = "FuseTolMin_";
    private const string TOLERANCE_MAX_KEY_PREFIX = "FuseTolMax_";
    
    void Start()
    {
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
    }

    void Update()
    {
        
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
        
        // Obtener datos de los slots y tipos de datos del inventario
        System.Collections.Generic.Dictionary<string, string> slotNames = GetSlotNames();
        System.Collections.Generic.Dictionary<string, string> dataTypes = GetDataTypes();
        
        // Ocultar el template base
        itemBaseTemplate.SetActive(false);
        
        // Crear slots para cada campo
        foreach (var slot in slotNames)
        {
            string fieldName = slot.Key;
            string displayName = slot.Value;
            string assignedType = GetAssignedDataType(fieldName);
            
            CreateSlotFromTemplate(fieldName, displayName, assignedType);
        }
        
        Debug.Log($"Generados {slotNames.Count} slots de fusibles");
    }
    
    /// <summary>
    /// Obtiene los nombres de los slots de la caja de fusibles
    /// </summary>
    private System.Collections.Generic.Dictionary<string, string> GetSlotNames()
    {
        return new System.Collections.Generic.Dictionary<string, string>()
        {
            { "NAME", "NAME" },
            { "LAST_NAME", "LAST NAME" },
            { "EMAIL", "EMAIL" },
            { "PHONE", "PHONE" },
            { "AGE", "AGE" },
            { "BIRTHDAY", "BIRTHDAY" },
            { "ADDRESS", "ADDRESS" },
            { "CITY", "CITY" }
        };
    }
    
    /// <summary>
    /// Obtiene los tipos de datos disponibles del inventario
    /// </summary>
    private System.Collections.Generic.Dictionary<string, string> GetDataTypes()
    {
        return new System.Collections.Generic.Dictionary<string, string>()
        {
            { "INT", "Entero" },
            { "DATE", "Fecha" },
            { "VARCHAR", "Texto Variable" },
            { "BOOL", "Booleano" }
        };
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
    
    /// <summary>
    /// Obtiene la abreviatura de 2 caracteres para cada tipo
    /// </summary>
    private string GetTypeAbbreviation(string type)
    {
        switch (type)
        {
            case "VARCHAR":
                return "VC";
            case "INT":
                return "IN";
            case "DATE":
                return "DA";
            case "BOOL":
                return "BO";
            default:
                return type.Length >= 2 ? type.Substring(0, 2) : type;
        }
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
    /// Obtiene las instrucciones correctas según el campo
    /// </summary>
    private string GetFieldInstructions(string fieldName)
    {
        // Mapeo de campos a tipos de datos requeridos
        System.Collections.Generic.Dictionary<string, string> fieldInstructions = new System.Collections.Generic.Dictionary<string, string>()
        {
            { "NAME", "Use VARCHAR for text data" },
            { "LAST_NAME", "Use VARCHAR for text data" },
            { "EMAIL", "Use VARCHAR for email format" },
            { "PHONE", "Use VARCHAR for phone numbers" },
            { "AGE", "Use INT for numeric age" },
            { "BIRTHDAY", "Use DATE for date format" },
            { "ADDRESS", "Use VARCHAR for address text" },
            { "CITY", "Use VARCHAR for city names" }
        };
        
        if (fieldInstructions.ContainsKey(fieldName))
        {
            return fieldInstructions[fieldName];
        }
        
        return "Select appropriate data type";
    }
    
    /// <summary>
    /// Crea un slot basándose en el template
    /// </summary>
    private void CreateSlotFromTemplate(string fieldName, string displayName, string assignedType)
    {
        GameObject newSlot = Instantiate(itemBaseTemplate, itemsParent);
        newSlot.SetActive(true);
        newSlot.name = $"Slot_{fieldName}";
        
        // Buscar textos y configurarlos
        TextMeshProUGUI[] texts = newSlot.GetComponentsInChildren<TextMeshProUGUI>();
        
        foreach (TextMeshProUGUI text in texts)
        {
            // Column title (título del campo)
            if (text.gameObject.name.ToLower().Contains("title") || text.gameObject.name.ToLower().Contains("column"))
            {
                text.text = displayName;
            }
            // Status (estado del fusible) - ahora muestra FS-TIPO-TAMAÑO si está asignado
            else if (text.gameObject.name.ToLower().Contains("status"))
            {
                string displayText = GetFormattedFuseDisplay(fieldName);
                text.text = displayText;
                
                // Ajustar tamaño de letra si el texto es muy largo
                AdjustTextSize(text, displayText);
            }
        }
        
        // Buscar y configurar botón de asignación
        UnityEngine.UI.Button[] buttons = newSlot.GetComponentsInChildren<UnityEngine.UI.Button>();
        foreach (UnityEngine.UI.Button button in buttons)
        {
            if (button.gameObject.name.ToLower().Contains("assign") || button.gameObject.name.ToLower().Contains("btn"))
            {
                string fieldRef = fieldName; // Captura para closure
                button.onClick.AddListener(() => OnWrenchButtonClicked(fieldRef));
                break;
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
        
        if (selectedFuseType == "BOOL" || selectedFuseType == "DATE")
        {
            // Para BOOL y DATE solo aceptar "confirm"
            return input.ToLower() == "confirm";
        }
        else if (selectedFuseType == "INT" || selectedFuseType == "VARCHAR")
        {
            // Para INT y VARCHAR validar que sea número
            if (!int.TryParse(input, out int number))
            {
                return false;
            }
            return number >= 0 && number <= 9999;
        }
        
        return false;
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
        
        // Cerrar después de un delay
        Invoke("CloseAssignmentProcess", 2f);
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
            
            // Para casos de error, recargar la interfaz después de un delay
            Invoke("ResetInputForRetry", 2f);
        }
    }
}
