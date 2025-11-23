using UnityEngine;
using TMPro;

public class levels_menu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro1;
    [SerializeField] private TextMeshProUGUI textMeshPro2;
    [SerializeField] private TextMeshProUGUI goal1,goal2,goal3,goal4;
    
    private string level1Title;
    private string level1Description;

    void Start()
    {
        PlayerPrefs.SetString("Level_1", "done");
        PlayerPrefs.SetString("Level_2", "available");
        PlayerPrefs.SetString("Level_3", "available");
        PlayerPrefs.SetString("Level_4", "available");
        PlayerPrefs.Save();
        
        UpdateLevelContent();
    }
    
    void UpdateLevelContent()
    {
        string selectedLevel = PlayerPrefs.GetString("selectedlevel", "Level_1");
        
        string title = "";
        string description = "";
        string goal1desc = "";     
        string goal2desc = "";
        string goal3desc = "";
        string goal4desc = "";

        switch (selectedLevel)
        {
            case "Level_1":
                title = "ACTO 1";
                description = "Eres un técnico eléctrico que queda atrapado por accidente en una vieja central donde solo tienes que arreglar unos fusibles.\n Maria es activada, una IA amable pero insistente que insiste en ayudar.\n Hambriento y estresado, quieres irte cuanto antes, pero encuentras que la sala tiene algunas cosas fuera de lugar.\n Ante esta situación desesperante, Maria inicia su protocolo: “Chistes de emergencia”.\n Por el momento, las cosas van acorde al plan.";
                goal1desc = "Tipos numéricos (INTEGER, BIGINT)";
                goal2desc = "Tipos de texto (VARCHAR vs CHAR)";
                goal3desc = "Tipos de fecha y tiempo (DATE, DATETIME)";
                goal4desc = "Restricciones (NULL, NOT NULL, UNIQUE, BOOLEAN)";
                break;
            case "Level_2":
                title = "ACTO 2";
                description = "A pesar de la desconfianza, alguno de sus chistes de Maria tuvo cierta gracia, y eso abre una pequeña conexión de amistad. Donde juntos deben buscar y comprender las instalaciones sin conocer el propósito real de este lugar.\n Maria nota algo extraño: todo el sitio ya tiene energía, así que no deberían de seguir atrapados. Una puerta se abre mal y se presenta una zona oculta donde parece que los estuvieron observando.\n Una discusión termina amedrentando la delgada y sensible amistad que se empezaba a entablar entre ellos 2. La salida está cerca y los resultados, próximos a mostrarse. Hay que meter un poco más de presión.";
                goal1desc = "Comandos básicos (SELECT, UPDATE, DELETE, INSERT)";
                goal2desc = "SELECT y sus herramientas (WHERE, LIKE, ORDER BY, GROUP BY)";
                goal3desc = "Integridad referencial (PRIMARY KEY, FOREIGN KEY, CONSTRAINT)";
                goal4desc = "Tablas de relación y restricciones adicionales (UNIQUE, DEFAULT, NULL/NOT NULL)";
                break;
            case "Level_3":
                title = "ACTO 3";
                description = "Ya no hay vuelta atrás. Las cosas están cayendo por cuenta propia a su lugar. \n Maria está haciendo las cosas como se le ordenó. \n Pero, han habido picos de energía inusuales. \n Comportamientos extraños que no fueron contemplados. \n Consecuencias imprevistas. \n ¿Dónde estás ahora?";
                goal1desc = "JOINs (INNER JOIN, LEFT JOIN, RIGHT JOIN)";
                goal2desc = "Agrupación y funciones (GROUP BY, COUNT, COALESCE, WHERE)";
                goal3desc = "Gestión de tablas (CREATE, DROP, INSERT)";
                goal4desc = "MongoDB Manejo de documentos (insertMany, find, filtros)";
                break;
            case "Level_4":
                title = "EPILOGO";
                description = "OLD_DATA";
                goal1desc = "OLD_DATA";
                goal2desc = "OLD_DATA";
                goal3desc = "OLD_DATA";
                goal4desc = "OLD_DATA";
                break;
            default:
                title = "ACTO 1";
                description = "Eres un técnico eléctrico que queda atrapado por accidente en una vieja central donde solo tienes que arreglar unos fusibles.\n Maria es activada, una IA amable pero insistente que insiste en ayudar.\n Hambriento y estresado, quieres irte cuanto antes, pero encuentras que la sala tiene algunas cosas fuera de lugar.\n Ante esta situación desesperante, Maria inicia su protocolo: “Chistes de emergencia”.\n Por el momento, las cosas van acorde al plan.";
                goal1desc = "Tipos numéricos (INTEGER, BIGINT)";
                goal2desc = "Tipos de texto (VARCHAR vs CHAR)";
                goal3desc = "Tipos de fecha y tiempo (DATE, DATETIME)";
                goal4desc = "Restricciones (NULL, NOT NULL, UNIQUE, BOOLEAN)";
                break;
        }
        
        if (textMeshPro1 != null)
            textMeshPro1.text = title;
        
        if (textMeshPro2 != null)
            textMeshPro2.text = description;

        if (goal1 != null)
            goal1.text = goal1desc;
        
        if (goal2 != null)
            goal2.text = goal2desc;
        
        if (goal3 != null)
            goal3.text = goal3desc;       

        if (goal4 != null)
            goal4.text = goal4desc;

    }


    void Update()
    {
        UpdateLevelContent();
    }
}
