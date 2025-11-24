using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class journal : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI leftPageText;
    [SerializeField] private TextMeshProUGUI rightPageText;
    [SerializeField] private Button leftArrow;
    [SerializeField] private Button rightArrow;
    
    [Header("Content")]
    [SerializeField][TextArea(10, 20)] private string journalContent = @"UNIDAD 1

En la creación de tablas, es importante elegir bien los tipos de datos para cada campo. Por ejemplo, INTEGER (o INT) se usa para números enteros como IDs, edades o niveles de prioridad; ocupa 4 bytes y no admite decimales. VARCHAR(n) sirve para texto de longitud variable, hasta n caracteres, ideal para nombres, correos o direcciones, ya que ahorra espacio frente a tipos fijos como CHAR.

DATE guarda solo la fecha (año, mes y día), mientras que DATETIME guarda fecha y hora, útil para campos como createdAt o lastSeen. El tipo BOOLEAN (o BOOL) almacena valores lógicos (TRUE/FALSE), común para estados activos o inactivos.

También se deben definir si los campos aceptan NULL (valores vacíos) o si deben ser obligatorios (NOT NULL), según si la información puede faltar o no. Las restricciones como UNIQUE ayudan a evitar duplicados, por ejemplo en correos electrónicos. En conjunto, elegir correctamente los tipos y restricciones garantiza que la base de datos sea eficiente, clara y confiable.

INTEGER / INT
Números enteros sin decimales.
Uso: IDs, contadores, edad, priorityLevel.
Tamaño típico: 4 bytes (hasta aprox. 2,147,483,647 en signed INT).
Si esperas valores mayores, usa BIGINT.

VARCHAR(n)
Texto de longitud variable hasta n caracteres.
Ahorra espacio comparado con CHAR.
Úsalo para nombres, emails, direcciones.
Evita poner un n exagerado sin necesidad.

DATE
Guarda solo fecha (YYYY-MM-DD).
Ideal para cumpleaños o fechas sin hora.

BOOLEAN / BOOL
Valor lógico TRUE/FALSE.
En algunos motores es alias de TINYINT(1) (MySQL).
Útil para banderas de estado.

DATETIME
Guarda fecha y hora.
Útil para createdAt, updatedAt, lastSeen.
En MySQL puede manejar zona local; si necesitas UTC, úsalo desde la app o emplea TIMESTAMP.

NULL vs NOT NULL

Si un campo puede estar vacío, permite NULL.

Si siempre debe existir (ej. USR_name), declara NOT NULL.
Recomendación: usar NOT NULL por defecto cuando tenga sentido y valores DEFAULT si aplica.

UNIQUE
Garantiza unicidad en una columna (ej. Email).
Evita duplicados.

INSTRUCCIONES
Obtener todos los valores de la tabla Users.

Cableado y fusibles , Tabla Users

Id → integer
Name → varchar(100)
LastName → varchar(100)
Email → varchar(120)
Phone → varchar(20)
Age → int
Birthday → date
Address → varchar(200)
City → varchar(100)
Country → varchar(100)
PriorityLevel → int
Status → bool
LastSeen → datetime
CreatedAt → datetime
UpdatedAt → datetime";
    
    private int currentPage = 1;
    private int maxPages = 1; // Se calculará dinámicamente
    
    void Start()
    {
        SetupButtons();
        StartCoroutine(InitializePages());
    }
    
    void SetupButtons()
    {
        // Configurar los eventos de los botones
        if (leftArrow != null)
        {
            leftArrow.onClick.RemoveAllListeners();
            leftArrow.onClick.AddListener(PreviousPage);
        }
        
        if (rightArrow != null)
        {
            rightArrow.onClick.RemoveAllListeners();
            rightArrow.onClick.AddListener(NextPage);
        }
    }
    
    IEnumerator InitializePages()
    {
        // Asignar el contenido a ambas páginas
        if (leftPageText != null)
        {
            leftPageText.text = journalContent;
        }
        
        if (rightPageText != null)
        {
            rightPageText.text = journalContent;
        }
        
        // Esperar un frame para que el texto se renderice
        yield return null;
        
        // Forzar actualización del mesh
        if (leftPageText != null)
        {
            leftPageText.ForceMeshUpdate();
        }
        
        if (rightPageText != null)
        {
            rightPageText.ForceMeshUpdate();
        }
        
        // Calcular el número máximo de páginas basado en el contenido
        CalculateMaxPages();
        
        // Actualizar la paginación inicial
        UpdatePagination();
    }
    
    public void NextPage()
    {
        if (currentPage < maxPages)
        {
            currentPage++;
            UpdatePagination();
        }
    }
    
    public void PreviousPage()
    {
        if (currentPage > 1)
        {
            currentPage--;
            UpdatePagination();
        }
    }
    
    void CalculateMaxPages()
    {
        // Usar el leftPageText para calcular el número de páginas
        if (leftPageText != null && leftPageText.textInfo != null)
        {
            maxPages = Mathf.Max(1, leftPageText.textInfo.pageCount);
            Debug.Log($"Páginas calculadas: {maxPages}");
        }
        else
        {
            maxPages = 1;
        }
    }
    
    void UpdatePagination()
    {
        // Alternar entre mostrar el contenido en página izquierda o derecha
        if (currentPage % 2 == 1) // Páginas impares en la izquierda
        {
            // Mostrar página actual en leftPageText
            if (leftPageText != null)
            {
                leftPageText.gameObject.SetActive(true);
                leftPageText.text = journalContent;
                leftPageText.pageToDisplay = currentPage;
            }
            
            // Mostrar página siguiente en rightPageText o página en blanco
            if (rightPageText != null)
            {
                rightPageText.gameObject.SetActive(true);
                if (currentPage < maxPages)
                {
                    rightPageText.text = journalContent;
                    rightPageText.pageToDisplay = currentPage + 1;
                }
                else
                {
                    rightPageText.text = ""; // Página en blanco
                }
            }
        }
        else // Páginas pares en la derecha
        {
            // Mostrar página anterior en leftPageText
            if (leftPageText != null)
            {
                leftPageText.gameObject.SetActive(true);
                leftPageText.text = journalContent;
                leftPageText.pageToDisplay = currentPage - 1;
            }
            
            // Mostrar página actual en rightPageText
            if (rightPageText != null)
            {
                rightPageText.gameObject.SetActive(true);
                rightPageText.text = journalContent;
                rightPageText.pageToDisplay = currentPage;
            }
        }
        
        // Actualizar la visibilidad de los botones de navegación
        UpdateNavigationButtons();
        
        // Debug para verificar la página actual
        Debug.Log($"Página actual: {currentPage}/{maxPages}");
    }
    
    void UpdateNavigationButtons()
    {
        // Habilitar/deshabilitar botones según la página actual
        if (leftArrow != null)
        {
            leftArrow.interactable = currentPage > 1;
            // Cambiar la apariencia visual del botón cuando está desactivado
            var colors = leftArrow.colors;
            colors.disabledColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            leftArrow.colors = colors;
        }
        
        if (rightArrow != null)
        {
            rightArrow.interactable = currentPage < maxPages;
            // Cambiar la apariencia visual del botón cuando está desactivado
            var colors = rightArrow.colors;
            colors.disabledColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            rightArrow.colors = colors;
        }
        
        Debug.Log($"Botones actualizados - Izquierda: {(leftArrow?.interactable ?? false)}, Derecha: {(rightArrow?.interactable ?? false)}");
    }
    
    // Función pública para establecer una página específica
    public void SetPage(int pageNumber)
    {
        if (pageNumber >= 1 && pageNumber <= maxPages)
        {
            currentPage = pageNumber;
            UpdatePagination();
        }
    }
    
    // Función para obtener la página actual
    public int GetCurrentPage()
    {
        return currentPage;
    }
    
    // Función para reinicializar el diario
    public void ResetJournal()
    {
        currentPage = 1;
        UpdatePagination();
    }
    
    // Función para actualizar el contenido del diario dinámicamente
    public void UpdateJournalContent(string newContent)
    {
        journalContent = newContent;
        
        // Actualizar el texto en ambos componentes
        if (leftPageText != null)
        {
            leftPageText.text = journalContent;
            leftPageText.ForceMeshUpdate();
        }
        
        if (rightPageText != null)
        {
            rightPageText.text = journalContent;
            rightPageText.ForceMeshUpdate();
        }
        
        // Recalcular las páginas
        CalculateMaxPages();
        
        // Volver a la primera página
        currentPage = 1;
        UpdatePagination();
    }
    
    // Función para obtener el número total de páginas
    public int GetMaxPages()
    {
        return maxPages;
    }
}
