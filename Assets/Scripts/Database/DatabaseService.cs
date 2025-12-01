// DatabaseService.cs
using SQLite;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DatabaseService : MonoBehaviour
{
    public static DatabaseService Instance { get; private set; }
    public SQLiteConnection Connection { get; private set; }

    // Si quieres un nombre distinto cambia aquí
    [SerializeField] private string databaseFileName = "maria_database.db";

    private void Awake()
    {
        // Singleton simple
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitDatabase();
    }

    private void InitDatabase()
    {
        string dbPath = Path.Combine(Application.persistentDataPath, databaseFileName);
        Debug.Log("DB path: " + dbPath);

        Connection = new SQLiteConnection(dbPath);

        Connection.CreateTable<Player>();
        Connection.CreateTable<Level>();
        Connection.CreateTable<Dialogue>();

        SeedDatabaseIfEmpty();
    }

    private void SeedDatabaseIfEmpty()
    {
        if (Connection.Table<Player>().Count() == 0)
        {
            Connection.Insert(new Player { Name = "Maria" });
            Connection.Insert(new Player { Name = "Narrador" });
            Connection.Insert(new Player { Name = "Jugador" });
            Connection.Insert(new Player { Name = "Desconocido" });
            Connection.Insert(new Player { Name = "Desconocido2" });

            Debug.Log("Seed de Players completado");
        }

        if (Connection.Table<Level>().Count() == 0)
        {
            Connection.Insert(new Level { Name = "Bosque Inicial" });
            Connection.Insert(new Level { Name = "Cueva Oscura" });
            Connection.Insert(new Level { Name = "Pueblo Fantasma" });
            Connection.Insert(new Level { Name = "Templo Antiguo" });
            Debug.Log("Seed de Levels completado");
        }

        if (Connection.Table<Dialogue>().Count() == 0)
        {
            var dialogues = new List<Dialogue>
            {
                new Dialogue {
                    LevelId = 1,
                    PlayerId = 4, // Desconocido
                    OrderIndex = 0,
                    Content = "¡Si cree que nuestro tiempo es algo que nos sobra, se equivoca! ¡Nos vamos de aquí!",
                    Side = "right"
                },
                new Dialogue {
                    LevelId = 1,
                    PlayerId = 5, // Desconocido2
                    OrderIndex = 1,
                    Content = "¡Alto! Por favor, dennos otra oportunidad. Fue un error humano, pero el proyecto está lis… ",
                    Side = "right"
                },
                new Dialogue {
                    LevelId = 1,
                    PlayerId = 2, // Narrador
                    OrderIndex = 2,
                    Content = "Un rostro feliz aparece en la pantalla del dispositivo.",
                    Side = "right"
                },
                new Dialogue {
                    LevelId = 1,
                    PlayerId = 1, // Maria
                    OrderIndex = 3,
                    Content = "Hola mi amiguito, ¿Cómo estás? ¡Yo también, gracias!",
                    Side = "right"
                },
                new Dialogue {
                    LevelId = 1,
                    PlayerId = 2, // Narrador
                    OrderIndex = 4,
                    Content = "La confusión invade el rostro del protagonista el cual, empieza a considerar que lo mejor era haber encontrado un animal muerto que una IA.",
                    Side = "right"
                },
                new Dialogue {
                    LevelId = 1,
                    PlayerId = 3, // Jugador
                    OrderIndex = 5,
                    Content = "No debí haber abierto esa caja…",
                    Side = "left"
                },
                new Dialogue {
                    LevelId = 1,
                    PlayerId = 1, // Maria
                    OrderIndex = 6,
                    Content = "¿Caja?, ¿Qué caja? ¿Te refieres a esa vieja caja de zapatos?",
                    Side = "right"
                },
                new Dialogue {
                    LevelId = 1,
                    PlayerId = 3, // Jugador
                    OrderIndex = 7,
                    Content = "Sí, esa caja, maldita licuadora con bocina.",
                    Side = "left"
                }
            };

            Connection.InsertAll(dialogues);
            Debug.Log("Seed de Dialogues completado");
        }

    }
}
