using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class journal : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI leftPageText;
    [SerializeField] private TextMeshProUGUI rightPageText;
    [SerializeField] private Button leftArrow;
    [SerializeField] private Button rightArrow;
    
    [Header("Content")]
    [SerializeField][TextArea(10, 20)] private string unit1Content = @"UNIDAD 1

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

    [SerializeField][TextArea(10, 20)] private string unit2Content = @"

==================================================
UNIDAD 2 - CONSULTAS Y MANIPULACIÓN DE DATOS

Se trabaja con manipulación de datos dentro de las tablas ya creadas, utilizando los
comandos SELECT, UPDATE, DELETE e INSERT. Estos comandos permiten consultar,
modificar, eliminar o agregar información dentro de una base de datos según las
necesidades del sistema.

El comando SELECT se usa para buscar y mostrar información específica. Por ejemplo, se
puede listar todos los usuarios activos filtrando con WHERE Status = true, o mostrar los
permisos que contienen ciertas palabras usando LIKE '%usuarios%'. También permite
ordenar los resultados con ORDER BY o agruparlos con GROUP BY, como cuando se
cuentan los documentos por tipo de archivo.

UPDATE se utiliza para modificar registros existentes. Puede cambiar un valor en una sola
fila o en varias al mismo tiempo, dependiendo de la condición WHERE. Por ejemplo, se
puede actualizar el estado de un usuario, cambiar la prioridad de todos los usuarios de un
país o registrar la última fecha de actualización con NOW() o una fecha específica.

DELETE elimina registros de una tabla según la condición dada. Es importante incluir un
WHERE para evitar borrar toda la tabla por error. Un ejemplo sería eliminar un usuario con
un correo específico o quitar un permiso que ya no es necesario.

Finalmente, INSERT sirve para agregar nuevos registros en las tablas. Se deben indicar las
columnas y los valores correspondientes en el mismo orden. Por ejemplo, se puede crear
un nuevo usuario llamado María o registrar nuevos permisos y documentos de prueba.

En conjunto, estos comandos permiten mantener actualizada, limpia y funcional la base de
datos, asegurando que la información refleje correctamente los cambios dentro del sistema
y sirva de base para futuros procesos o consultas.

SELECT - Consultar información
Sintaxis básica:
SELECT columnas FROM tabla WHERE condiciones ORDER BY campo;

Ejemplos prácticos:

SELECT * FROM Users;
-- Obtiene todos los registros de Users

SELECT Id, Name, LastName, Country, Status FROM Users WHERE Status = true;
-- Lista usuarios activos con campos específicos

SELECT Name, LastName, PriorityLevel, Country FROM Users ORDER BY PriorityLevel DESC;
-- Ordena usuarios por prioridad de mayor a menor

SELECT * FROM Permissions;
-- Muestra todos los permisos disponibles

SELECT Name FROM Permissions WHERE Name LIKE '%usuarios%';
-- Busca permisos que contengan la palabra 'usuarios'

SELECT Country, COUNT(*) as TotalUsuarios FROM Users GROUP BY Country;
-- Cuenta usuarios agrupados por país

WHERE - Filtros y condiciones
Operadores de comparación: =, !=, <, >, <=, >=
Operadores lógicos: AND, OR, NOT
Búsqueda de patrones: LIKE '%texto%', LIKE 'texto%', LIKE '%texto'
Valores nulos: IS NULL, IS NOT NULL

ORDER BY - Ordenamiento
ASC: orden ascendente (por defecto)
DESC: orden descendente

Ejemplos:
ORDER BY Name ASC
ORDER BY PriorityLevel DESC, Age ASC

GROUP BY - Agrupación
Se usa con funciones agregadas:
COUNT() - Cuenta registros
SUM() - Suma valores
AVG() - Promedio
MAX() - Valor máximo
MIN() - Valor mínimo

INSERT - Agregar registros
Sintaxis:
INSERT INTO tabla (columna1, columna2, ...) VALUES (valor1, valor2, ...);

Ejemplos:
INSERT INTO Users (Name, LastName, Email, Age, Status) 
VALUES ('María', 'González', 'maria@email.com', 25, true);

INSERT INTO Permissions (Name, Description) 
VALUES ('Administrador', 'Acceso completo al sistema');

UPDATE - Modificar registros
Sintaxis:
UPDATE tabla SET columna1 = valor1, columna2 = valor2 WHERE condición;

¡IMPORTANTE! Siempre incluye WHERE para evitar actualizar toda la tabla.

Ejemplos:
UPDATE Users SET Status = false WHERE Email = 'usuario@example.com';
UPDATE Users SET PriorityLevel = 1 WHERE Country = 'México';
UPDATE Users SET UpdatedAt = NOW() WHERE Id = 5;

DELETE - Eliminar registros
Sintaxis:
DELETE FROM tabla WHERE condición;

¡CUIDADO! Sin WHERE eliminas TODA la tabla.

Ejemplos:
DELETE FROM Users WHERE Status = false AND LastSeen < '2024-01-01';
DELETE FROM Permissions WHERE Name = 'PermisoTemporal';

BUENAS PRÁCTICAS:
- Usa SELECT específico en lugar de SELECT * en producción
- Siempre incluye WHERE en UPDATE y DELETE
- Prueba consultas complejas paso a paso
- Usa índices en columnas frecuentemente consultadas
- Valida datos antes de INSERT o UPDATE";

    [SerializeField][TextArea(10, 20)] private string unit3Content = @"

==================================================
UNIDAD 3 - CREACIÓN DE TABLAS RELACIONADAS

Se crearán tablas relacionadas en SQL, aplicando reglas más avanzadas como el uso de
claves foráneas, restricciones de unicidad y valores por defecto. Estas características
permiten construir una base de datos más completa y coherente, donde las tablas no
funcionan de manera aislada, sino que se comunican entre sí.

Por ejemplo, al crear la tabla Records, el objetivo es registrar las interacciones entre los
usuarios y los documentos. Para eso, se usa una clave primaria (Id) que identifica cada
registro de forma única, y se agregan campos como Visitor y Requested, que son claves
foráneas (FOREIGN KEY). Esto significa que esos campos hacen referencia a registros de
otras tablas: 'visitor' apunta a un usuario existente en Users y 'requested' a un documento
en Documents. Así, se garantiza que no existan registros huérfanos o con datos que no
correspondan a nada real en la base.

Otros campos como TimesRequested, LastAccess o IpAddress ayudan a registrar
información detallada. Por ejemplo, el número de veces que un documento ha sido
consultado, la fecha y hora del último acceso (usando el tipo DATETIME), y la dirección IP
del usuario (almacenada como VARCHAR(45), lo suficiente para direcciones IPv6).
Además, se puede establecer un valor por defecto con DEFAULT, como TimesRequested =
1, para inicializar el campo automáticamente si no se indica otro valor.

Por otro lado, la tabla UserPermission representa una relación entre los usuarios y los
permisos del sistema. Aquí también se aplica el concepto de clave foránea, vinculando los
campos User y Permission con las tablas Users y Permissions respectivamente. Esta tabla
incluye además un campo AssignedAt para saber cuándo se otorgó el permiso, y
RevokedAt (que puede ser NULL) para indicar si el permiso fue retirado.

Una regla importante en este tipo de relaciones es evitar duplicados. Para eso, se usa una
restricción UNIQUE sobre las columnas (User, Permission), lo que impide que un mismo
usuario tenga asignado el mismo permiso más de una vez.

Finalmente, el proceso se completa con comandos INSERT, que permiten agregar nuevos
registros. Por ejemplo, se puede insertar el usuario de María junto con su permiso de
administrador, o registrar accesos simulados en la tabla Records con diferentes usuarios y
documentos.

CREATE TABLE - Crear nuevas tablas
Sintaxis básica:
CREATE TABLE nombre_tabla (
    columna1 tipo_dato restricciones,
    columna2 tipo_dato restricciones,
    CONSTRAINT nombre_restriccion FOREIGN KEY (columna) REFERENCES tabla(columna)
);

PRIMARY KEY (Id) - Clave primaria
Identifica de forma única cada registro en una tabla.
Ejemplo: Id SERIAL PRIMARY KEY

FOREIGN KEY - Clave foránea
Crea relaciones entre tablas; asegura que los valores correspondan a registros existentes.
Sintaxis: CONSTRAINT fk_nombre FOREIGN KEY (columna_local) REFERENCES tabla_externa(columna)

Ejemplo:
CONSTRAINT fk_usr_visitor FOREIGN KEY (Visitor) REFERENCES Users(Id)

REFERENCES - Definir relación
Define a qué tabla y campo apunta una clave foránea.
Ejemplo: REFERENCES Users(Id), REFERENCES Documents(Id)

DEFAULT - Valores por defecto
Asigna un valor automático cuando no se especifica otro.
Ejemplo: TimesRequested INTEGER NOT NULL DEFAULT 1

DATETIME - Fecha y hora
Tipo de dato para guardar fecha y hora completa.
Usado en: LastAccess, AssignedAt, RevokedAt

VARCHAR(45) - Texto variable
Tipo de texto de longitud variable.
Ejemplo: IpAddress VARCHAR(45) -- Suficiente para IPv6

NULL vs NOT NULL - Campos opcionales u obligatorios
NULL: Permite que un campo esté vacío (RevokedAt puede ser NULL)
NOT NULL: Campo obligatorio (Visitor INTEGER NOT NULL)

UNIQUE - Evitar duplicados
Restringe los valores para evitar duplicados.
Ejemplo: CONSTRAINT unique_user_permission UNIQUE (User, Permission)

CONSTRAINT - Definir reglas
Define reglas dentro de la tabla como claves foráneas o restricciones únicas.
Sintaxis: CONSTRAINT nombre_constraint TIPO_CONSTRAINT

EJEMPLOS PRÁCTICOS:

Tabla Records (Registros de acceso):
CREATE TABLE Records (
    Id SERIAL PRIMARY KEY,
    Visitor INTEGER NOT NULL,
    Requested INTEGER NOT NULL,
    TimesRequested INTEGER NOT NULL DEFAULT 1,
    LastAccess DATETIME NOT NULL,
    IpAddress VARCHAR(45) NOT NULL,
    CONSTRAINT fk_usr_visitor FOREIGN KEY (Visitor) REFERENCES Users(Id),
    CONSTRAINT fk_doc_requested FOREIGN KEY (Requested) REFERENCES Documents(Id)
);

Tabla UserPermission (Permisos de usuario):
CREATE TABLE UserPermission (
    Id SERIAL PRIMARY KEY,
    User INTEGER NOT NULL,
    Permission INTEGER NOT NULL,
    AssignedAt DATETIME NOT NULL,
    RevokedAt DATETIME NULL,
    CONSTRAINT fk_user FOREIGN KEY (User) REFERENCES Users(Id),
    CONSTRAINT fk_permission FOREIGN KEY (Permission) REFERENCES Permissions(Id),
    CONSTRAINT unique_user_permission UNIQUE (User, Permission)
);

INSERT INTO - Agregar registros relacionados
Sintaxis: INSERT INTO tabla (columnas) VALUES (valores);

Ejemplos:
INSERT INTO UserPermission (User, Permission, AssignedAt) 
VALUES (1, 2, '2024-12-10 14:30:00');

INSERT INTO Records (Visitor, Requested, LastAccess, IpAddress) 
VALUES (1, 3, '2024-12-10 15:45:00', '192.168.1.100');

BUENAS PRÁCTICAS RELACIONALES:
- Siempre definir claves primarias (PRIMARY KEY)
- Usar claves foráneas para mantener integridad referencial
- Aplicar restricciones UNIQUE donde sea necesario
- Definir valores DEFAULT para campos opcionales
- Nombrar las restricciones (CONSTRAINT) de forma descriptiva
- Validar que las tablas referenciadas existan antes de crear FKs";

    [SerializeField][TextArea(10, 20)] private string unit4Content = @"

==================================================
UNIDAD 4 - JOINS Y CONSULTAS AVANZADAS

Consultas SQL con JOINs

INNER JOIN — Se usa para combinar registros que tienen coincidencias en ambas tablas.
○ Ejemplo: INNER JOIN Users U ON R.Visitor = U.Id
○ Permite mostrar solo los usuarios que sí tienen registros en Records.

LEFT JOIN — Muestra todos los registros de la tabla izquierda, aunque no tengan
coincidencias.
○ Ejemplo: LEFT JOIN Records R ON U.Id = R.Visitor
○ Se usa para listar todos los usuarios incluso si no solicitaron documentos.

RIGHT JOIN — Incluye todos los registros de la tabla derecha, incluso si no tienen
correspondencia en la izquierda.
○ Ejemplo: RIGHT JOIN Records R ON U.Id = R.Visitor
○ Útil cuando se quiere mostrar todos los accesos aunque el usuario haya
sido eliminado.

WHERE — Filtra resultados según condiciones.
○ Ejemplo: WHERE U.PriorityLevel <= 2 AND D.HasPassword = true
○ Permite seleccionar usuarios con prioridad alta y documentos protegidos.

GROUP BY y COUNT() — Agrupa registros y cuenta ocurrencias.

Ejemplo:
SELECT U.Name, COUNT(R.Id) AS totalSolicitudes
FROM Records R
INNER JOIN Users U ON R.Visitor = U.Id
GROUP BY U.Name;
○ Sirve para saber cuántas solicitudes hizo cada usuario.

COALESCE() — Reemplaza valores nulos con un valor por defecto.
○ Ejemplo: COALESCE(SUM(R.TimesRequested), 0)
○ Permite mostrar 0 accesos si el usuario no tiene registros.

Comandos de gestión de tablas

DROP TABLE — Elimina una tabla completa de la base de datos.
○ Ejemplo: DROP TABLE BathroomCurtain;

CREATE TABLE — Crea una nueva tabla SQL.

Ejemplo:
CREATE TABLE OLD_DATA (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
Name VARCHAR(50),
Status BOOL,
CreatedAt DATETIME NOT NULL
);
○ Define tipos de datos, restricciones y claves primarias.

INSERT INTO — Inserta nuevos registros en una tabla.

Ejemplo:
INSERT INTO OLD_DATA (Name, Status, CreatedAt)
VALUES ('Gatitos', true, '2025-10-20 10:00:00');

Conceptos avanzados de MongoDB

Sintaxis de creación de colección (tabla equivalente en MongoDB):
○ MongoDB no usa CREATE TABLE, las colecciones se crean
automáticamente al insertar datos.

Insertar múltiples documentos:

Comando:
db.old_data.insertMany([
{ IsAlive: true, OrganizationName: ""Arasaka"", ... },
{ IsAlive: false, OrganizationName: ""Gamefuna"", ... }
])
○ Permite agregar varios registros en una sola operación.

Consultar todos los registros:
○ Comando: db.old_data.find()

Filtrar registros por condición:
○ Comando: db.old_data.find({ IsAlive: true })
○ Funciona similar al WHERE de SQL.

EJEMPLOS PRÁCTICOS DE JOINS:

1. INNER JOIN básico - Usuarios con documentos solicitados:
SELECT U.Name, U.LastName, D.Name, R.LastAccess 
FROM Records R 
INNER JOIN Users U ON R.Visitor = U.Id 
INNER JOIN Documents D ON R.Requested = D.Id;

2. LEFT JOIN - Todos los usuarios (con o sin actividad):
SELECT U.Name, R.LastAccess 
FROM Users U 
LEFT JOIN Records R ON U.Id = R.Visitor;

3. RIGHT JOIN - Todos los registros (aunque usuario eliminado):
SELECT R.Id, U.Name, D.Name, R.LastAccess 
FROM Users U 
RIGHT JOIN Records R ON U.Id = R.Visitor 
INNER JOIN Documents D ON R.Requested = D.Id;

4. JOIN con agregación - Conteo de solicitudes:
SELECT U.Name, COUNT(R.Id) AS totalSolicitudes 
FROM Records R 
INNER JOIN Users U ON R.Visitor = U.Id 
GROUP BY U.Name;

5. COALESCE para manejo de NULLs:
SELECT U.Name, COALESCE(SUM(R.TimesRequested), 0) AS totalAccesos 
FROM Users U 
LEFT JOIN Records R ON U.Id = R.Visitor 
GROUP BY U.Name;

DIFERENCIAS SQL vs MongoDB:
- SQL usa CREATE TABLE / MongoDB crea colecciones automáticamente
- SQL usa SELECT / MongoDB usa db.collection.find()
- SQL usa WHERE / MongoDB usa filtros JSON { campo: valor }
- SQL usa INSERT INTO / MongoDB usa insertOne() o insertMany()

BUENAS PRÁCTICAS AVANZADAS:
- Usar alias (U, R, D) para tablas en JOINs complejos
- Aplicar COALESCE para manejar valores NULL en agregaciones
- Combinar LEFT/RIGHT JOIN según qué datos preservar
- Usar GROUP BY con funciones agregadas para análisis
- Nombrar columnas calculadas con AS (totalSolicitudes, totalAccesos)";
    
    private string journalContent = "";
    private int currentPage = 1;
    private int maxPages = 1; // Se calculará dinámicamente

    public SoundManagerRoom soundRoom;

    void Start()
    {
        SetContentBasedOnScene();
        SetupButtons();
        StartCoroutine(InitializePages());
    }
    
    void SetContentBasedOnScene()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        
        switch(currentScene)
        {
            case 2: // Solo Unidad 1
                journalContent = unit1Content;
                Debug.Log("Cargando: Unidad 1");
                break;
                
            case 6: 
                journalContent = unit1Content + unit2Content;
                Debug.Log("Cargando: Unidades 1-2");
                break;
                
            case 7: 
                journalContent = unit1Content + unit2Content + unit3Content;
                Debug.Log("Cargando: Unidades 1-3");
                break;
                
            case 8: 
                journalContent = unit1Content + unit2Content + unit3Content + unit4Content;
                Debug.Log("Cargando: Unidades 1-4");
                break;
                
            default: 
                journalContent = unit1Content;
                Debug.Log("Cargando por defecto: Unidad 1");
                break;
        }
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
            soundRoom.PlayChangePage();
            currentPage++;
            UpdatePagination();
        }
    }
    
    public void PreviousPage()
    {
        if (currentPage > 1)
        {
            soundRoom.PlayChangePage();
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
