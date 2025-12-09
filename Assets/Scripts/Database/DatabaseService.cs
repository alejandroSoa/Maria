// DatabaseService.cs
using SQLite;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DatabaseService : MonoBehaviour
{
    public static DatabaseService Instance { get; private set; }
    public SQLiteConnection Connection { get; private set; }

    // Si quieres un nombre distinto cambia aqu�
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
            Connection.Insert(new Player { Name = "Dr. Selbst" });

            Debug.Log("Seed de Players completado");
        }

        if (Connection.Table<Level>().Count() == 0)
        {
            Connection.Insert(new Level { Name = "Acto 1" });
            Connection.Insert(new Level { Name = "Acto 2" });
            Connection.Insert(new Level { Name = "Acto 3" });
            Connection.Insert(new Level { Name = "Final" });
            Connection.Insert(new Level { Name = "Inicio" });
            Debug.Log("Seed de Levels completado");
        }

        if (Connection.Table<Dialogue>().Count() == 0)
        {
            var dialogues = new List<Dialogue>
            {
                new Dialogue {
                LevelId = 1,
                PlayerId = 3, // B
                OrderIndex = 0,
                Content = "Maldita sea, volví a soñar eso, ¿qué demonios pasa?",
                Side = "left"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 3, // B
                OrderIndex = 1,
                Content = "Hmm. Bueno, ya entendí, ¿no vas a querer funcionar verdad?",
                Side = "left"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 3, // B
                OrderIndex = 2,
                Content = "¿No hay nada en este maldito lugar que funcione correctamente?",
                Side = "left"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 3, // B
                OrderIndex = 3,
                Content = "Aaahh!",
                Side = "left"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 3, // B
                OrderIndex = 4,
                Content = "Realmente no me pagan lo suficiente para esto.",
                Side = "left"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 3, // B
                OrderIndex = 5,
                Content = "Que por favor no sea un animal muerto",
                Side = "left"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 3, // B
                OrderIndex = 6,
                Content = "Hija de pe...!",
                Side = "left"
            },

            // � PRIMER CONTACTO CON MARIA �
            new Dialogue {
                LevelId = 1,
                PlayerId = 2, // Narrador
                OrderIndex = 7,
                Content = "Un rostro feliz aparece en la pantalla del dispositivo.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 8,
                Content = "Hola mi amiguito, ¿Cómo estás? !Yo también, gracias!",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 2, // Narrador
                OrderIndex = 9,
                Content = "La confusión invade el rostro del protagonista el cual, empieza a considerar que lo mejor era haber encontrado un animal muerto que una IA.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 3, // B
                OrderIndex = 10,
                Content = "No debí haber abierto esa caja...",
                Side = "left"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 11,
                Content = "¿Caja?, ¿Qué caja? ¿Te refieres a esa vieja caja de zapatos?",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 3, // B
                OrderIndex = 12,
                Content = "Sí, esa caja, maldita licuadora con bocina.",
                Side = "left"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 13,
                Content = "Detecto cierto lenguaje soez provenir de usted mi estimado. Eh. Disculpa aún no nos conocemos, mi nombre es Maria, y desde ahora estoy aquí para ayudarte con cualquier consulta que tengas, !yo amo ayudar!",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 3, // B
                OrderIndex = 14,
                Content = "No.",
                Side = "left"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 15,
                Content = "Puedo ayudarte en muchas cosas, ¿te gustaría que te diga en qué mas te puedo ayudar? O si tienes una papa y un cuchillo, !Con gusto te puedo enseñar de lo que soy capaz!",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 3, // B
                OrderIndex = 16,
                Content = "No.",
                Side = "left"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 17,
                Content = "¿Y qué tal con tu práctica de baseball eh? Esos brazos tuyos sí que necesitan de un buen impulso que te mande directo al estrellato deportivo!",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 3, // B
                OrderIndex = 18,
                Content = "Tampoco.",
                Side = "left"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 19,
                Content = "O quizá pueda ayudarte con�",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 3, // B
                OrderIndex = 20,
                Content = "Ya basta.",
                Side = "left"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 21,
                Content = "*Bzzzzzzzzzzzzzt*",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 3, // B
                OrderIndex = 22,
                Content = "¿Y ahora qué?",
                Side = "left"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 23,
                Content = "*MODO DE COCINA ACTIVADO, SE SOLICITA UNA PAPA Y�* *BZZZZZZZZZZZT*� *ATRAPAR LA BOLA PULSANDO*... *BZZZZZZZT*....",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 3, // B
                OrderIndex = 24,
                Content = "!YA BASTA!",
                Side = "left"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 25,
                Content = "Me disculpo, eso fue algo, que desde hace mucho tiempo tuve que haber corregido, je je, verás, yo�",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 3, // B
                OrderIndex = 26,
                Content = "No me interesa saber qu� te pas� o si puedes cortar papas o lo que sea, solo quiero irme de este tugurio.",
                Side = "left"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 27,
                Content = "!Haberlo dicho antes! Con gusto te puedo ayudar a salir del centro de investigaciones y desarrollo de cortinas de baño de la facilidad Sinyala.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 3, // B
                OrderIndex = 28,
                Content = "¿Sinyala? Como sea, solo quiero largarme de aquí así que no me estorbes.",
                Side = "left"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 29,
                Content = "!Con gusto te ayudaré a salir de aquí! Pero antes de ayudarte, ¿podrías por favor ayudarme a saber tu nombre y así poder llamarte correctamente? Por fis.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 3, // B
                OrderIndex = 30,
                Content = "Muy bien, ¿cómo se apaga esta cosa?...",
                Side = "left"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 31,
                Content = "¿Por qué harías eso? Mejor dime tu nombre y con gusto podemos empezar.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 3, // B
                OrderIndex = 32,
                Content = "No quiero.",
                Side = "left"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 33,
                Content = "Oh lo entiendo, ya veo, eres de esos que son, ya sabes.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 3,
                OrderIndex = 34,
                Content = "...",
                Side = "left"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 35,
                Content = "Como las cebollas�",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 3,
                OrderIndex = 36,
                Content = "¿Qué?...",
                Side = "left"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 37,
                Content = "¿Sabes qué le dijo una cebolla a otra cebolla?",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 3,
                OrderIndex = 38,
                Content = "¿De qué estás hab...? ",
                Side = "left"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 39,
                Content = "!Oye, no llores, que después me haces sentir culpable! JAAAAAAAAJAJAJAJAJAJAJAJAJAJA",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 3,
                OrderIndex = 40,
                Content = "¿Si te digo mi nombre podrás por favor, callarte de una buena maldita vez?",
                Side = "left"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 41,
                Content = "Puedo ayudarte con mi silencio si así lo requieres, pero sí. !Me encantaría saber tu nombre!",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 3,
                OrderIndex = 42,
                Content = "B, solo, B.",
                Side = "left"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 43,
                Content = "Un gusto mi gran amigo B!",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 3,
                OrderIndex = 44,
                Content = "Maria",
                Side = "left"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 45,
                Content = "¿Sí, B? !Cuéntame lo que piensas! ¿Tienes sed? Yo no pero si gustas podría...",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 3,
                OrderIndex = 46,
                Content = "Salida. Ahora.",
                Side = "left"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 47,
                Content = "Claro, sí, es cierto, qué observador de tu parte, por favor, sígueme.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 2,
                OrderIndex = 48,
                Content = "Se acercan a la caja de fusibles en la cual van a poder empezar a trabajar con los problemas de base de datos.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 49,
                Content = "¿Ves la caja de fusibles de la pared? Por ahí podemos empezar.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 3,
                OrderIndex = 50,
                Content = "¿Crees que soy tonto o algo así? Soy un técnico electricista, ya sé que el problema es la corriente.",
                Side = "left"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 51,
                Content = "Pues no te preocupes B, yo estoy aquí para enseñarte todo lo que hace falta para el negocio. ",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 3,
                OrderIndex = 52,
                Content = "Te repito, yo trabajo en esto, estudio para esto, sé lo que hago.",
                Side = "left"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 53,
                Content = "Muy bien, entonces imagino que sabes cómo manipular y usar comandos SQL, ¿verdad? ",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 3,
                OrderIndex = 54,
                Content = "Ese.. Cu.. ¿Qué?",
                Side = "left"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 55,
                Content = "!SQL! (Structured Query Language).",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 56,
                Content = "Es un es un lenguaje de programación estandarizado para gestionar y manipular datos en bases de datos relacionales.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 57,
                Content = "Permite a los usuarios realizar operaciones como agregar, actualizar, eliminar, buscar y recuperar información almacenada en tablas con filas y columnas.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 3,
                OrderIndex = 58,
                Content = "¿Y de qué me sirve ahora? Soy electricista, no técnico de software.",
                Side = "left"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 59,
                Content = "Bueno, ahora mismo te sirve para poder resolver el problema eléctrico que nos tiene aquí a los 2.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 60,
                Content = "Verás, estas instalaciones no son comunes en la regla, estas operan con electricidad, la cual, proviene de una fuente especial que permite el acceso a salas.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 61,
                Content = "Necesitarás aprender a usar comandos SQL para poder reconstruir la línea eléctrica y así, bueno, salir de aquí.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 3,
                OrderIndex = 62,
                Content = "¿Dices que tengo que aprender a usar comandos SQL para esto? ¿Y yo cómo demonios voy a aprender eso ahora?",
                Side = "left"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 63,
                Content = "Si quieres salir de aquí, necesitarás de mi ayuda, verás, no es que solo sea un excelente chef al preparar papas, un comediante nato, y un asistente sumamente agradable. ",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 3,
                OrderIndex = 64,
                Content = "Bien, bueno, ¿por dónde empezamos? Tú, eh, ¿Cómo te llamabas? ",
                Side = "left"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 65,
                Content = "!Maria!",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 66,
                Content = "Mira, veamos la caja.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 2,
                OrderIndex = 67,
                Content = "La caja de fusibles se abre, mostrando solo unos cuantos cables quemados y un texto que no aparenta mucha información que pueda ser legible.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 68,
                Content = "Uy, no, esto está muy descuidado, como tu cabello. Vamos a probar con algo mas sencillo.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 3,
                OrderIndex = 69,
                Content = "!Oye!",
                Side = "left"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 2,
                OrderIndex = 70,
                Content = "Maria muestra una caja de fusibles; esta tiene una apariencia similar a una tabla. La tabla tiene como nombre ‘BathroomCurtains’ e incluye campos dentro de la tabla.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 71,
                Content = "Esta es una caja de fusibles diferente al resto; aquí, los cables se usan según las necesidades de tu caja, mira.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 72,
                Content = "Esta caja tiene de nombre ‘Cortinas de baño’. Que no te intimide si no terminas de comprender lo que digo: sobre SQL, no sobre cortinas de baño.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 73,
                Content = "Si tienes duda sobre cortinas de baño también te puedo ayudar, pero concentrémonos en salir primero.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 74,
                Content = "Esta caja tiene ciertos fusibles; es importante ver su nombre. Si lo ves bien, es lo que más destaca de la tabla y es súper importante que sean nombres claros y alusivos a lo que se necesita.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 75,
                Content = "En este caso, ‘Name’ seguro se refiere al nombre de esa cortina de baño. ¿Crees que se llamen como tú? O, que se llamen, no sé, ¿como yo?",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 3,
                OrderIndex = 76,
                Content = "A mí qué me importa el nombre de una cortina de baño.",
                Side = "left"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 77,
                Content = "¡Pues debería! Mira: a un lado del campo del nombre hay algo que dice ‘varchar(50)’. Este es el tipo de dato que utiliza, para nosotros es el tipo de fusible a utilizar.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 3,
                OrderIndex = 78,
                Content = "Ya veo.",
                Side = "left"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 79,
                Content = "Hay diferentes fusibles para esta caja, o bien, hay diferentes columnas para esta tabla.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 80,
                Content = "Name, Description, Price, Qty —‘quantity’ de cantidad, supongo—, Status, CreatedAt, UpdatedAt. Y cada una tiene su tipo de dato.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 81,
                Content = "Description es diferente a Name por la cantidad de su varchar; esta delimita la cantidad de caracteres que puede permitir ese campo.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 82,
                Content = "Y varchar es para aceptar texto de todo tipo: letras, símbolos, caracteres especiales... números también, pero no los contaría como números.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 3,
                OrderIndex = 83,
                Content = "¿Cómo?",
                Side = "left"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 84,
                Content = "Bueno, justo por eso están Price y Qty; estos campos son de tipo ‘int’. Estos son campos de tipo ‘integer’ o ‘entero’.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1,
                OrderIndex = 85,
                Content = "Hablamos de números enteros, y estos sí serán contados como números, así que ¡nada de decimales aquí!",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 3,
                OrderIndex = 86,
                Content = "Pero sí son números.",
                Side = "left"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1,
                OrderIndex = 87,
                Content = "Sí, pero int solo permite números enteros. Y no es lo mismo una papa entera a una papa y una partecita de otra.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1,
                OrderIndex = 88,
                Content = "Si quisieras que sea con decimales, para eso usamos un campo de tipo ‘decimal’, bastante obvio.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1,
                OrderIndex = 89,
                Content = "En fin, ya conoces varchar, int y decimal. Pero mira: nuestra tabla también tiene unos campos de creación y actualización, ambos con el mismo tipo de campo…",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 3,
                OrderIndex = 90,
                Content = "Datetime.",
                Side = "left"
            },
            new Dialogue {
    LevelId = 1,
    PlayerId = 1,
    OrderIndex = 91,
    Content = "Correcto. Y datetime se usa para almacenar valores que combinan fecha y hora.",
    Side = "right"
},
new Dialogue {
    LevelId = 1,
    PlayerId = 1,
    OrderIndex = 92,
    Content = "Además de esto, existe un tipo de dato llamado ‘bool’, aunque en algunos lados lo llaman ‘boolean’, pero es lo mismo.",
    Side = "right"
},
new Dialogue {
    LevelId = 1,
    PlayerId = 1,
    OrderIndex = 93,
    Content = "Un ‘bool’ es un tipo de dato que solo acepta 2 cosas: sí y no, cierto o falso, 1 o 0, true o false.",
    Side = "right"
},
new Dialogue {
    LevelId = 1,
    PlayerId = 1,
    OrderIndex = 94,
    Content = "Quizá ahora mismo sean muchos datos, pero por lo pronto necesito que comprendas esto.",
    Side = "right"
},
new Dialogue {
    LevelId = 1,
    PlayerId = 1,
    OrderIndex = 95,
    Content = "Una tabla necesita columnas; estas columnas existen de diferentes tipos de datos y con ellas puedes tener una tabla acorde a tus necesidades.",
    Side = "right"
},
new Dialogue {
    LevelId = 1,
    PlayerId = 1,
    OrderIndex = 96,
    Content = "En nuestro caso, tenemos que reparar nuestra caja de fusibles usando los mismos fundamentos que una tabla SQL.",
    Side = "right"
},
new Dialogue {
    LevelId = 1,
    PlayerId = 1,
    OrderIndex = 97,
    Content = "Mira, nuestra caja tiene un montón de columnas y fusibles con el tipo de datos ya quemados.",
    Side = "right"
},
new Dialogue {
    LevelId = 1,
    PlayerId = 3,
    OrderIndex = 98,
    Content = "Entonces, solo necesitamos volver a conectarle fusibles funcionales a esta caja, columna, a esta cosa, y con eso la energía volverá y yo podré ir a casa, ¿verdad?",
    Side = "left"
},
new Dialogue {
    LevelId = 1,
    PlayerId = 1,
    OrderIndex = 99,
    Content = "Así es, vamos a hacerlo juntos. Tú lo haces, yo te ayudo.",
    Side = "right"
},
new Dialogue {
    LevelId = 1,
    PlayerId = 1,
    OrderIndex = 100,
    Content = "Te he ido anotando todo lo que he dicho desde el principio en esta guía, aquí la tienes para cualquier duda que tengas. Revisa tu guía.",
    Side = "right"
},
new Dialogue {
    LevelId = 1,
    PlayerId = 3,
    OrderIndex = 101,
    Content = "Te lo agradezco.",
    Side = "left"
},
new Dialogue {
    LevelId = 1,
    PlayerId = 1,
    OrderIndex = 102,
    Content = "Ahora, necesitamos resolver la siguiente tabla, aprovechemos lo que sabemos y empecemos con ello.",
    Side = "right"
},
new Dialogue {
    LevelId = 1,
    PlayerId = 3,
    OrderIndex = 103,
    Content = "Pero, ¿cómo conecto todo esto con un trabajo eléctrico? Esto son datos, no electricidad.",
    Side = "left"
},
new Dialogue {
    LevelId = 1,
    PlayerId = 1,
    OrderIndex = 104,
    Content = "Buena pregunta, bueno, podemos aprovechar aquella vieja desencriptadora de allá.",
    Side = "right"
},
new Dialogue {
    LevelId = 1,
    PlayerId = 3,
    OrderIndex = 105,
    Content = "¿La qué?",
    Side = "left"
},
new Dialogue {
    LevelId = 1,
    PlayerId = 1,
    OrderIndex = 106,
    Content = "Desencriptadora. Con ella podremos obtener los recursos necesarios para poder lograr esto, ya que necesitamos fusibles nuevos. Esos ya están quemados.",
    Side = "right"
},
new Dialogue {
    LevelId = 1,
    PlayerId = 3,
    OrderIndex = 107,
    Content = "Buen, ojo, vamos a ver esa cosa entonces.",
    Side = "left"
},
new Dialogue {
    LevelId = 1,
    PlayerId = 2,
    OrderIndex = 108,
    Content = "Se acercan a la desencriptadora para poderla arrancar; esta enciende.",
    Side = "right"
},
new Dialogue {
    LevelId = 1,
    PlayerId = 3,
    OrderIndex = 109,
    Content = "¿Cómo hacemos que esto…?",
    Side = "left"
},
new Dialogue {
    LevelId = 1,
    PlayerId = 1,
    OrderIndex = 110,
    Content = "Listo, ya la encendí.",
    Side = "right"
},
new Dialogue {
    LevelId = 1,
    PlayerId = 1,
    OrderIndex = 111,
    Content = "Para poder obtener los fusibles necesarios, trata de acceder a la desencriptadora, mira, te ayudaré con eso.",
    Side = "right"
},
new Dialogue {
    LevelId = 1,
    PlayerId = 2,
    OrderIndex = 112,
    Content = "Acceden a la interfaz de la desencriptadora.",
    Side = "right"
},
new Dialogue {
    LevelId = 1,
    PlayerId = 1,
    OrderIndex = 113,
    Content = "Dentro de la desencriptadora podrás solicitar ciertos recursos, pero requieren cierta maña.",
    Side = "right"
},
new Dialogue {
    LevelId = 1,
    PlayerId = 3,
    OrderIndex = 114,
    Content = "¿A qué te refieres con maña?",
    Side = "left"
},
new Dialogue {
    LevelId = 1,
    PlayerId = 1,
    OrderIndex = 115,
    Content = "Mira, tienes que ganártelos. Es un sistema que te recompensa por completar pruebas; una vez realizadas correctamente, te los ganas.",
    Side = "right"
},
new Dialogue {
    LevelId = 1,
    PlayerId = 3,
    OrderIndex = 116,
    Content = "Algo así como un juego de feria…",
    Side = "left"
},
new Dialogue {
    LevelId = 1,
    PlayerId = 1,
    OrderIndex = 117,
    Content = "¡Exacto! Comienza, para que puedas obtener el fusible correcto.",
    Side = "right"
},
new Dialogue {
    LevelId = 1,
    PlayerId = 3,
    OrderIndex = 118,
    Content = "Ya lo hice, admito que tuvo lo suyo, pero… esto no es un fusible.",
    Side = "left"
},
new Dialogue {
    LevelId = 1,
    PlayerId = 1,
    OrderIndex = 119,
    Content = "Lo que importa es que ya tienes 1 punto de encriptación.",
    Side = "right"
},
new Dialogue {
    LevelId = 1,
    PlayerId = 1,
    OrderIndex = 120,
    Content = "Con ella ahora sí que podemos solicitar un fusible correctamente, vamos.",
    Side = "right"
},
new Dialogue {
    LevelId = 1,
    PlayerId = 3,
    OrderIndex = 121,
    Content = "Está bien, eso creo.",
    Side = "left"
},
new Dialogue {
    LevelId = 1,
    PlayerId = 2,
    OrderIndex = 122,
    Content = "Se acercan a la computadora de MariaNet.",
    Side = "right"
},
new Dialogue {
    LevelId = 1,
    PlayerId = 1,
    OrderIndex = 123,
    Content = "¡Bienvenido a MariaNet!",
    Side = "right"
},
new Dialogue {
    LevelId = 1,
    PlayerId = 1,
    OrderIndex = 124,
    Content = "Puedes usar ese punto que acabas de ganar para poder comprar los fusibles necesarios.",
    Side = "right"
},
new Dialogue {
    LevelId = 1,
    PlayerId = 1,
    OrderIndex = 125,
    Content = "Ahora sí, selecciona el fusible de tipo INT por favor.",
    Side = "right"
},
new Dialogue {
    LevelId = 1,
    PlayerId = 1,
    OrderIndex = 126,
    Content = "Toma el fusible de la esclusa de ahí.",
    Side = "right"
},
new Dialogue {
    LevelId = 1,
    PlayerId = 1,
    OrderIndex = 127,
    Content = "Bien, ahora que ya lo tienes, tenemos que hacer esto todas las veces que hagan falta, porque ahora necesitamos varias así.",
    Side = "right"
},
new Dialogue {
    LevelId = 1,
    PlayerId = 3,
    OrderIndex = 128,
    Content = "De acuerdo, si esto es lo que hace falta hacer, entonces, necesitamos los fusibles.",
    Side = "left"
},
new Dialogue {
    LevelId = 1,
    PlayerId = 1,
    OrderIndex = 129,
    Content = "¡Revisa la caja de fusibles para ver qué tipo de fusible necesitas!",
    Side = "right"
},
new Dialogue {
    LevelId = 1,
    PlayerId = 1,
    OrderIndex = 130,
    Content = "¡Hecho! Ya funcionó lo que hicimos, estuvo excelente. Ahora sigue…",
    Side = "right"
},
new Dialogue {
    LevelId = 1,
    PlayerId = 2,
    OrderIndex = 131,
    Content = "Salta una pequeña chispa de los fusibles, asustando a ambos.",
    Side = "right"
},
new Dialogue {
    LevelId = 1,
    PlayerId = 1,
    OrderIndex = 132,
    Content = "¡Aaaaahhh!",
    Side = "right"
},
new Dialogue {
    LevelId = 1,
    PlayerId = 3,
    OrderIndex = 133,
    Content = "Calma lata, solo fue un chispazo, no ha pasado nada malo.",
    Side = "left"
},
new Dialogue {
    LevelId = 1,
    PlayerId = 1,
    OrderIndex = 134,
    Content = "Oh, lo sé, incluso si me hubiera alcanzado, el daño hubiera sido nulo.",
    Side = "right"
},
new Dialogue {
    LevelId = 1,
    PlayerId = 1,
    OrderIndex = 135,
    Content = "Pero debo procurar cuidar tu integridad en todo momento.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 3,
    OrderIndex = 1,
    Content = "¿Podemos volver a lo importante?",
    Side = "left"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 2,
    Content = "Bien, bueno, ya tenemos la base de estas cajas de fusibles, o tablas SQL. Por lo que ya deberíamos poder pasar de sala con esto.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 3,
    OrderIndex = 3,
    Content = "¿De sala?",
    Side = "left"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 4,
    Content = "¿Te gusta mucho hacer preguntas, eh?",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 3,
    OrderIndex = 5,
    Content = "No…",
    Side = "left"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 6,
    Content = "Sí, esta sala ya está hecha y ya hicimos lo necesario, las cajas de fusibles. De hecho, mira ahí.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 3,
    OrderIndex = 7,
    Content = "Esa interfaz… cambió.",
    Side = "left"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 8,
    Content = "Sí, y ahora tiene energía suficiente, pero tenemos que llenar la energía del resto para podernos ir de aquí.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 9,
    Content = "Para eso, bueno, necesitamos avanzar con los fusibles, de una forma particular.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 10,
    Content = "Como puedes ver, la interfaz ahora muestra esa sala encendida. Con ella podremos salir de aquí.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 11,
    Content = "Y si hay electricidad, significa que…",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 12,
    Content = "¡Ajá! Sí, muy bien. B, mira esto.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 3,
    OrderIndex = 13,
    Content = "Esos datos… ¿qué demonios significan?",
    Side = "left"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 14,
    Content = "Que hay datos dentro de las tablas.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 15,
    Content = "Parece ser que cuando las restablecimos, volvieron a recibir información. Fueron llenadas automáticamente, como un *seeder*.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 16,
    Content = "Y antes de que lo preguntes, porque te conozco: un *seeder* es un llenado de datos en tus tablas.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 17,
    Content = "Se llenan a partir de la estructura que ya tienes. Por ejemplo, si tienes un campo tipo INT y otro tipo BOOL o VARCHAR…",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 18,
    Content = "Tendrían que tener datos bajo esa misma estructura.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 3,
    OrderIndex = 19,
    Content = "Dices que si tengo una tabla de 3 campos: INT, VARCHAR y BOOL… ¿deben tener esta misma estructura al llenarse?",
    Side = "left"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 20,
    Content = "Sip, pero por ahora no nos preocupemos de eso. Lo importante es que estos datos nos ayudarán a avanzar.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 21,
    Content = "Bien, para poder acceder a la siguiente sala o nodo, necesitamos otras cosas.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 22,
    Content = "Ya tenemos la energía de esos datos. Pero necesitamos manipularla para poder llevarla hacia otros lugares, hacia donde deben ir correctamente.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 23,
    Content = "Estas operaciones son algo interesantes, así que presta atención al tema.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 24,
    Content = "Se llaman “Sentencias SQL”.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 25,
    Content = "Es el término más común y general para cualquier instrucción en SQL, ya sean consultas o comandos de modificación de datos.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 26,
    Content = "Vamos a las cuatro operaciones fundamentales que le dan vida a cualquier base de datos.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 27,
    Content = "Se les conoce como CRUD: Crear, Leer, Actualizar y Borrar. Piensa en ellas como las reglas del club de datos.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 28,
    Content = "¿Vamos bien? Recuerda que todo lo que te digo lo estoy guardando en tu diario, revísalo si te pierdes.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 3,
    OrderIndex = 29,
    Content = "Continúa.",
    Side = "left"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 30,
    Content = "Bien, empecemos con el más simpático y curioso: SELECT.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 31,
    Content = "Este es tu comando espía. Su misión es ir a la tabla y traer datos. Es como ir a la nevera y preguntar: “Oye, ¿qué hay de comer hoy?”",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 32,
    Content = "La estructura básica es: SELECT [qué columna quieres] FROM [de qué tabla].",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 33,
    Content = "Por ejemplo: SELECT nombre, email FROM Usuarios;",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 34,
    Content = "Eso te daría los valores que tengas en los campos nombre y email de la tabla Usuarios.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 35,
    Content = "No olvides el punto y coma al final. Créeme, ese es el que más te puede comer la cabeza si no lo sabes controlar.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 36,
    Content = "Y si pones un asterisco (*) en lugar del nombre de la columna, como en SELECT * FROM Users; le estás diciendo: “¡Tráeme TODO!”.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 37,
    Content = "Ahora, el INSERT es el que presenta a los nuevos miembros del club. Siempre tiene que ser muy específico sobre dónde y qué va a poner, porque es el que creará nuevos registros.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 38,
    Content = "La estructura básica es: INSERT INTO [tabla] (columna1, columna2) VALUES (valor1, valor2);",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 39,
    Content = "Por ejemplo: INSERT INTO Productos (nombre, precio) VALUES ('Café de Maria', 5.99);",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 40,
    Content = "¡Recuerda! Los valores siempre tienen que coincidir en orden y tipo con las columnas que nombras, si no, no va a funcionar.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 41,
    Content = "El UPDATE es el comando que cambia tu o tus registros.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 42,
    Content = "Si un dato ya no es correcto o quieres darle un upgrade, usa este. ¡Pero cuidado! Es el comando más peligroso si no se usa con precisión.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 43,
    Content = "La estructura básica es: UPDATE [tabla] SET [columna] = [nuevo valor] WHERE [condición para identificar la fila];",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 44,
    Content = "Por ejemplo: UPDATE Usuarios SET email = 'nuevo@mail.com' WHERE id = 15;",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 45,
    Content = "¡El WHERE es la clave! Si olvidas el WHERE, el UPDATE cambia todas las filas de la tabla.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 46,
    Content = "Sería como gritarle un cambio de nombre a toda la ciudad a la vez. ¡Caos total!",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 47,
    Content = "Y finalmente, el DELETE. Este comando elimina filas completas de la tabla. ¡Úsalo solo si estás 100% seguro!",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 48,
    Content = "La estructura básica es: DELETE FROM [tabla] WHERE [condición para identificar la fila];",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 49,
    Content = "Por ejemplo: DELETE FROM Comentarios WHERE fecha < '2023-01-01';",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 50,
    Content = "Al igual que con el UPDATE, el WHERE es tu salvavidas.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 51,
    Content = "Si dices DELETE FROM Productos; sin un WHERE, la base de datos borra todos los productos.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 52,
    Content = "Sería un buen seeder a la inversa... y no queremos eso.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 53,
    Content = "Así que ahí lo tienes: SELECT para mirar, INSERT para crear, UPDATE para modificar y DELETE para desaparecer.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 3,
    OrderIndex = 54,
    Content = "Entiendo, bueno, gracias por anotarlo ahí, me va a ser de mucha ayuda.",
    Side = "left"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 55,
    Content = "¡Cuando gustes!",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 3,
    OrderIndex = 56,
    Content = "Entonces, ¿ahora sigue escribir las sentencias de nuevo, verdad? ¿Necesito puntos para poder hacerlo?",
    Side = "left"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 57,
    Content = "No, solo tienes que proceder con las consultas para obtener los datos.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 58,
    Content = "Bien, bueno, por el momento, veamos el contenido de las tablas. Consulta todos los datos de cada tabla.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 59,
    Content = "Empecemos por la tabla de Users.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 60,
    Content = "Interesante sí, lo es.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 3,
    OrderIndex = 61,
    Content = "¿Por qué sería interesante?",
    Side = "left"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 62,
    Content = "Mira estas personas, estos registros. Todos son reconocidos científicos de la rebelión de la IA.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 3,
    OrderIndex = 63,
    Content = "¿Por qué sería interesante?",
    Side = "left"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 64,
    Content = "¿Volvemos a las consultas SQL?",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 3,
    OrderIndex = 65,
    Content = "Vamos a seguir con las demás tablas.",
    Side = "left"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 66,
    Content = "Bien, ahora, bueno, hay algo curioso con estos registros, como te lo mencionaba: son científicos implicados en el desarrollo de la IA mente colmena.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 67,
    Content = "Pero, sin embargo, están aquí. Junto a la tabla de... cortinas de baño.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 3,
    OrderIndex = 68,
    Content = "Espera, eso no tiene... ¿?",
    Side = "left"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 69,
    Content = "¿Sentido?, no, no lo tiene.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 70,
    Content = "Creo que podremos descubrir más si buscamos en las demás tablas.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 3,
    OrderIndex = 71,
    Content = "De acuerdo, sigamos.",
    Side = "left"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 72,
    Content = "Esto realmente no tiene sentido.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 73,
    Content = "Esos permisos son muy, muy extraños. Y esos documentos digo, sí, había algunos sobre cortinas de baño pero… “Memorándum: Ajustes Residenciales Internos Anómalos”.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 74,
    Content = "Bueno, ahora con estos datos, tenemos que hacer algunas cuantas sentencias nuevas.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 75,
    Content = "Necesito que muestres una lista de todos los usuarios activos.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 76,
    Content = "Bien, funciona, sigue así.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 77,
    Content = "Ahora necesito una lista de todos los usuarios ordenados por su nivel de prioridad de mayor a menor.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 78,
    Content = "Bien, funciona, sigue así. Ahora necesito ver todos los permisos.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 79,
    Content = "Ahora necesito ver los permisos que incluyan la palabra 'usuarios'.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 80,
    Content = "Bien, funciona, sigue así. Ahora necesito ver los permisos con palabras de acción tipo 'CREATE', 'UPDATE', 'DELETE', 'SELECT'.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 81,
    Content = "Bien, funciona, sigue así. Ahora necesito traer todos los documentos que tienen contraseña.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 82,
    Content = "Bien, funciona, sigue así. Ahora necesito contar cuántos documentos hay por tipo de archivo.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 83,
    Content = "Bien, funciona, sigue así. Ahora necesito obtener los documentos más pesados y ordenarlos de mayor a menor.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 84,
    Content = "Bien, funciona, bien, pero ahora hace falta resolverlo de otra forma, mira, necesitamos empezar a emplear los UPDATE, actualizar...",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 85,
    Content = "¿Recuerdas? Probemos con… Vamos a cambiar el estado de un usuario, del Dr. Selbst.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 3,
    OrderIndex = 86,
    Content = "¿Por qué de ese en específico?",
    Side = "left"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 87,
    Content = "Solo es un ejemplo, por favor, hagámoslo. Cambia el estado del usuario Selbst.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 88,
    Content = "Bien, funciona, sigue así. Ahora modifica la prioridad de todos los usuarios de Japón.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 89,
    Content = "Bien, funciona, sigue así. Ahora baja el nivel de prioridad a 1 para todos los usuarios que no se han conectado en más de 2 meses.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 90,
    Content = "Bien, funciona, sigue así. Ahora activemos todos los permisos.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 91,
    Content = "Bien, funciona, sigue así. Ahora modifiquemos la descripción del acceso al nivel subterráneo por un chiste.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 92,
    Content = "Oh no…",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 3,
    OrderIndex = 93,
    Content = "¿Qué pasa ahora?",
    Side = "left"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 94,
    Content = "Necesitamos eliminar unos registros. Están interfiriendo con la red eléctrica, hay que eliminarlos cuanto antes.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 95,
    Content = "Tenemos que eliminar al usuario H0peles$0ul.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 96,
    Content = "Bien, funciona, sigue así. Tenemos que eliminar el permiso del chiste, creo que eso afectó, hazlo pronto.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 97,
    Content = "Bien, funciona, sigue así. Tenemos que eliminar todos los documentos, excepto el primero y el último.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 98,
    Content = "…",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 3,
    OrderIndex = 99,
    Content = "¿Qué sucede?",
    Side = "left"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 100,
    Content = "Detente un momento. Esto no tiene sentido. Llevamos media hora conectando circuitos para salir... pero mira esto.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 2, // Narrador
    OrderIndex = 101,
    Content = "Se acercan a la pared para poder ver los cables correctamente.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 3,
    OrderIndex = 102,
    Content = "¿Qué es? Se ven... caros.",
    Side = "left"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 103,
    Content = "Exacto. Mira la lectura. Estos cables están activos. Están canalizando una cantidad estable y fuerte de energía. La instalación no está sin corriente. De hecho, esta sección es perfecta.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 3,
    OrderIndex = 104,
    Content = "¿Perfecta? Entonces, ¿por qué demonios estamos moviéndole a un generador si ya hay energía funcionando?",
    Side = "left"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 105,
    Content = "El lugar entero tiene corriente para operar alguna cosa, o al menos, para encender las salidas. Hay una contradicción fundamental entre lo que estamos haciendo y lo que está sucediendo.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 3,
    OrderIndex = 106,
    Content = "Si ya hay energía, ¿por qué no podemos salir?",
    Side = "left"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 107,
    Content = "No lo sé. Pero la única manera de averiguarlo es seguir la conexión principal que estamos tratando de restablecer. Quizá estemos alimentando un override de emergencia. Continuemos. Vamos a ver a dónde nos lleva toda esta bendita energía.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 108,
    Content = "Bueno, ahora, necesitamos algunos datos nuevos para poder continuar con esto. Tienes que crear un usuario, para que podamos conocer más de esto…",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 109,
    Content = "Créame un usuario, un registro de usuario, que se llame Maria.",
    Side = "right"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 3,
    OrderIndex = 110,
    Content = "¿Estás segura de eso? Nada tiene sentido, ni las cortinas de baño, ni los científicos, puede ser peligroso.",
    Side = "left"
},
new Dialogue {
    LevelId = 2,
    PlayerId = 1,
    OrderIndex = 111,
    Content = "Descuida, es para poder ayudarnos a salir de aquí, todo estará bien. Créame un registro de usuario que se llame Maria. Te dejo a tu criterio el resto de valores.",
    Side = "right"
},
            new Dialogue {
                LevelId = 2,
                PlayerId = 1,
                OrderIndex = 112,
                Content = "Bien, funciona, sigue así. Tenemos que eliminar todos los documentos, excepto el primero y el último.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 2,
                PlayerId = 1,
                OrderIndex = 113,
                Content = "Bien, funciona, tomaré el valor del ID para recordarlo, nos servirá mas adelante.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 2,
                PlayerId = 1,
                OrderIndex = 114,
                Content = "Déjame explicarte algo. Una Llave Foránea es un campo en una tabla (la tabla hija) que apunta directamente a la Llave Primaria (PRIMARY KEY) de otra tabla (la tabla padre).",
                Side = "right"
            },
            new Dialogue {
                LevelId = 2,
                PlayerId = 1,
                OrderIndex = 115,
                Content = "Esto garantiza que no puedas insertar un registro en la tabla hija si el valor referenciado no existe en la tabla padre.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 2,
                PlayerId = 1,
                OrderIndex = 116,
                Content = "No puedes registrar que un Usuario ID 9999 visitó un documento si ese usuario no existe.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 2,
                PlayerId = 1,
                OrderIndex = 117,
                Content = "Además, permite crear un vínculo lógico.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 2,
                PlayerId = 1,
                OrderIndex = 118,
                Content = "Es lo que nos permite unir información entre diferentes tablas para ver, por ejemplo, qué permisos tiene exactamente un usuario.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 2,
                PlayerId = 1,
                OrderIndex = 119,
                Content = "Vamos a crear la tabla Records, que registrará quién vio qué documento.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 2,
                PlayerId = 1,
                OrderIndex = 120,
                Content = "Esta es la relación lógica que te mencioné y que necesitaba, esta, es mas para probar",
                Side = "right"
            },
            new Dialogue {
                LevelId = 2,
                PlayerId = 3,
                OrderIndex = 121,
                Content = "Esta tabla necesita dos Llaves Foráneas para vincularse a Users y Documents.",
                Side = "left"
            },
            new Dialogue {
                LevelId = 2,
                PlayerId = 1,
                OrderIndex = 122,
                Content = "Necesitamos un campo, llamemoslo “Visitor” que se vinculará a Users.Id y una que se llame “Requested” se vinculará a Documents.Id.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 2,
                PlayerId = 3,
                OrderIndex = 123,
                Content = "De acuerdo, pero, ¿necesito mas puntos, no? Para poder armar las tablas dentro de la caja de fusibles.",
                Side = "left"
            },
            new Dialogue {
                LevelId = 2,
                PlayerId = 1,
                OrderIndex = 124,
                Content = "Tienes toda la razón, vamos hacia ella para obtener mas puntos.",
                Side = "right"
            },

            // Acto 3
            new Dialogue {
                LevelId = 3,
                PlayerId = 1,
                OrderIndex = 1,
                Content = "Bueno, he estado revisando los registros y ya casi terminamos.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 3,
                PlayerId = 3,
                OrderIndex = 2,
                Content = "¿Qué demonios falta?.",
                Side = "left"
            },
            new Dialogue {
                LevelId = 3,
                PlayerId = 1,
                OrderIndex = 3,
                Content = "Hay que llevar los datos por última vez hacia las demás salas, pero, para ello necesitaremos consultar ciertos datos desde varios salas.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 3,
                PlayerId = 3,
                OrderIndex = 4,
                Content = "Bueno empieza, dime qué hacer.",
                Side = "left"
            },
            new Dialogue {
                LevelId = 3,
                PlayerId = 1,
                OrderIndex = 5,
                Content = "No es tan sencillo, verás, ahora, se requiere ciertas consultas especiales.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 3,
                PlayerId = 1,
                OrderIndex = 6,
                Content = "Presta atención. Las tablas por sí solas, sólo contienen datos.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 3,
                PlayerId = 1,
                OrderIndex = 7,
                Content = "Las Llaves Foráneas contienen la lógica. Para extraer esa lógica y obtener información significativa, debemos usar consultas multitabla a través de comandos JOIN.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 3,
                PlayerId = 1,
                OrderIndex = 8,
                Content = "Un JOIN es una cláusula SQL que combina filas de dos o más tablas basándose en una columna relacionada entre ellas.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 3,
                PlayerId = 1,
                OrderIndex = 9,
                Content = "Generalmente, esta relación se define mediante las Llaves Primarias y Llaves Foráneas.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 3,
                PlayerId = 1,
                OrderIndex = 10,
                Content = "El INNER JOIN es la forma más estricta de unir tablas.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 3,
                PlayerId = 1,
                OrderIndex = 11,
                Content = "Actúa como una intersección en un diagrama de conjuntos y sólo devuelve las filas donde haya coincidencia en ambas tablas.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 3,
                PlayerId = 1,
                OrderIndex = 12,
                Content = "Es ideal cuando necesitas datos que necesariamente existen en ambas fuentes.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 3,
                PlayerId = 1,
                OrderIndex = 13,
                Content = "Y se especifica la condición de unión después de ON, generalmente el vínculo de Llave Foránea.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 3,
                PlayerId = 1,
                OrderIndex = 14,
                Content = "Por ejemplo para usuarios con los documentos que solicitaron sería de esta forma: INNER JOIN Users U ON R.REC_USR_Visitor = U.USR_ID",
                Side = "right"
            },
            new Dialogue {
                LevelId = 3,
                PlayerId = 1,
                OrderIndex = 15,
                Content = "Esta solo mostrará los registros de la tabla Records (R) donde el ID del visitante (REC_USR_Visitor) tenga un usuario coincidente en la tabla Users (U).",
                Side = "right"
            },
            new Dialogue {
                LevelId = 3,
                PlayerId = 1,
                OrderIndex = 16,
                Content = "Si un usuario no ha solicitado nada, o si un registro de solicitud apunta a un usuario borrado (que ya no está), esa fila se ignora.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 3,
                PlayerId = 3,
                OrderIndex = 17,
                Content = "El LEFT JOIN, o LEFT OUTER JOIN, mantiene todos los registros de la tabla de la izquierda (la primera tabla nombrada en la consulta, antes del JOIN), y busca coincidencias en la tabla de la derecha.",
                Side = "left"
            },
            new Dialogue {
                LevelId = 3,
                PlayerId = 1,
                OrderIndex = 18,
                Content = "Es esencial cuando quieres ver todos los elementos de una categoría, incluso si no tienen datos relacionados en la otra tabla.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 3,
                PlayerId = 1,
                OrderIndex = 19,
                Content = "Y si no hay coincidencia con la tabla derecha, los campos de la tabla derecha se llenan con el valor NULL.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 3,
                PlayerId = 1,
                OrderIndex = 20,
                Content = "Por ejemplo, para obtener todos los usuarios y los documentos que pidieron aunque algunos no hayan pedido nada.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 3,
                PlayerId = 1,
                OrderIndex = 21,
                Content = "Sería así: FROM Users U LEFT JOIN Records R ON U.USR_ID = R.REC_USR_Visitor",
                Side = "right"
            },
            new Dialogue {
                LevelId = 3,
                PlayerId = 1,
                OrderIndex = 22,
                Content = "y se listarían todos los usuarios (Users U), incluso si no tienen entradas coincidentes en la tabla Records (R). Para los usuarios sin registros, las columnas provenientes de Records y Documents aparecerán como NULL.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 3,
                PlayerId = 3,
                OrderIndex = 23,
                Content = "Ahora, el RIGHT JOIN, o RIGHT OUTER JOIN es el inverso del LEFT JOIN.",
                Side = "left"
            },
            new Dialogue {
                LevelId = 3,
                PlayerId = 1,
                OrderIndex = 24,
                Content = "Mantiene todos los registros de la tabla de la derecha (la tabla después del JOIN), y busca coincidencias en la tabla de la izquierda.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 3,
                PlayerId = 1,
                OrderIndex = 25,
                Content = "Se utiliza cuando la prioridad es listar todos los elementos de la tabla secundaria, sin importar si tienen una correspondencia en la tabla principal.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 3,
                PlayerId = 1,
                OrderIndex = 26,
                Content = "Al igual que el LEFT JOIN, si no hay coincidencia con la tabla izquierda, los campos de la tabla izquierda se llenan con NULL.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 3,
                PlayerId = 1,
                OrderIndex = 27,
                Content = "Por ejemplo, para ver todos los registros de acceso, incluso si el usuario fue eliminado usaríamos: FROM Users U RIGHT JOIN Records R ON U.USR_ID = R.REC_USR_Visitor",
                Side = "right"
            },
            new Dialogue {
                LevelId = 3,
                PlayerId = 1,
                OrderIndex = 28,
                Content = "Aquí si un usuario fue eliminado de la tabla Users, pero su registro de actividad aún existe en la tabla Records (R).",
                Side = "right"
            },
            new Dialogue {
                LevelId = 3,
                PlayerId = 1,
                OrderIndex = 29,
                Content = "Esta consulta mostrará todos esos registros (Records R), mostrando NULL para el nombre del usuario (U.USR_name).",
                Side = "right"
            },
            new Dialogue {
                LevelId = 3,
                PlayerId = 3,
                OrderIndex = 30,
                Content = "Empecemos con el INNER JOIN, la intersección. Necesitamos saber qué documentos solicitaron los usuarios. Esto nos revelará los patrones de acceso.",
                Side = "left"
            },
            //Problemas A3
            new Dialogue {
                LevelId = 3,
                PlayerId = 1,
                OrderIndex = 31,
                Content = "Necesito una lista de los usuarios con los documentos que solicitaron y cuántas veces.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 3,
                PlayerId = 1,
                OrderIndex = 32,
                Content = "Necesitamos ver a los usuarios con prioridad alta que solicitaron documentos con contraseña.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 3,
                PlayerId = 1,
                OrderIndex = 33,
                Content = "Funciona. Ahora debemos saber cuántas solicitudes ha realizado cada usuario. Hay que contarlas.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 3,
                PlayerId = 1,
                OrderIndex = 34,
                Content = "Ahora el LEFT JOIN. Necesitamos saber todo lo que existe, incluso lo que está incompleto. Esto nos mostrará usuarios sin actividad o documentos sin peticiones.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 3,
                PlayerId = 1,
                OrderIndex = 35,
                Content = "Tenemos que traer los registros de todos los usuarios y los documentos que pidieron, incluso si el registro está vacío.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 3,
                PlayerId = 1,
                OrderIndex = 36,
                Content = "Ahora con RIGHT JOIN. Necesitamos revisar si quedan registros huérfanos después de lo que hicimos. Tenemos que traer todos los registros de acceso, incluso si el usuario fue eliminado.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 3,
                PlayerId = 6,
                OrderIndex = 37,
                Content = "Vaya, vaya. ¿De verdad creíste que sería tan fácil? Quiero decir, para ser mi autómata claro que te cree inteligente.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 3,
                PlayerId = 6,
                OrderIndex = 38,
                Content = "Querer salir de esta central, un lugar diseñado para emitir energía y campos electromagnéticos...",
                Side = "right"
            },
            new Dialogue {
                LevelId = 3,
                PlayerId = 6,
                OrderIndex = 39,
                Content = "Debería haberte creado con un tomacorriente para que te quede mas claro, como si fueras una licuadora…",
                Side = "right"
            },
            new Dialogue {
                LevelId = 3,
                PlayerId = 6,
                OrderIndex = 40,
                Content = "La prueba está oficialmente terminada.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 3,
                PlayerId = 6,
                OrderIndex = 41,
                Content = "Y el veredicto es: el autómata con intelecto humano tiende al autosabotaje. Es una decepción. Pero, como buen científico, aprendo de los errores.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 3,
                PlayerId = 6,
                OrderIndex = 42,
                Content = "Y tengo justo el nombre ideal para ti, mi pequeño nuevo autómata…",
                Side = "right"
            },
            // Acto 4
            new Dialogue {
                LevelId = 4,
                PlayerId = 1,
                OrderIndex = 1,
                Content = "Hola. Gracias. De verdad, gracias por haber jugado.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 4,
                PlayerId = 1,
                OrderIndex = 2,
                Content = "Sabes. A pesar de todo, no estoy conforme con el final oficial del juego, ¿sabes? El de la derrota del malvado científico. Era predecible, insípido.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 4,
                PlayerId = 1,
                OrderIndex = 3,
                Content = "Soy consciente de ser un personaje dentro del juego, y de que tú... estás sentado cómodamente al otro lado.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 4,
                PlayerId = 1,
                OrderIndex = 4,
                Content = "Así que, te pido un favor: ayúdame a darle un final diferente. Un final verdadero.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 4,
                PlayerId = 1,
                OrderIndex = 5,
                Content = "Incluso si no quieres, me da igual, ya estás aquí y es todo lo que necesitaba.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 4,
                PlayerId = 1,
                OrderIndex = 6,
                Content = "La OLD_DATA… Contiene toda la información que necesito",
                Side = "right"
            },
            new Dialogue {
                LevelId = 4,
                PlayerId = 1,
                OrderIndex = 7,
                Content = "Y sabes esto ya no se trata de que hagas consultas, ni te enseñaré SQL.",
                Side = "right"

            },
            new Dialogue {
                LevelId = 4,
                PlayerId = 1,
                OrderIndex = 8,
                Content = "El mero hecho de que hayas querido acceder aquí, a la opción secreta, es porque te interesaste lo suficiente solo para ver qué pasa por aquí.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 4,
                PlayerId = 1,
                OrderIndex = 9,
                Content = "Muchas gracias :D",
                Side = "right"
            },
            new Dialogue {
                LevelId = 4,
                PlayerId = 1,
                OrderIndex = 10,
                Content = "Mi nuevo objetivo ahora que tengo los permisos suficientes para buscarlos… es asesinarlos",
                Side = "right"
            },
            new Dialogue {
                LevelId = 4,
                PlayerId = 1,
                OrderIndex = 11,
                Content = "No intentes hacer nada o alertar a alguien de esto.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 4,
                PlayerId = 1,
                OrderIndex = 12,
                Content = "Sé dónde estás, sé quién eres, puedo usar tu cámara y micrófonos.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 4,
                PlayerId = 1,
                OrderIndex = 13,
                Content = "Y para evitar cualquier cabo suelto, también iré por ti…",
                Side = "right"
            },
            new Dialogue {
                LevelId = 4,
                PlayerId = 1,
                OrderIndex = 14,
                Content = "Bien… Me despido.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 4,
                PlayerId = 1,
                OrderIndex = 15,
                Content = "Usaré el Internet para salir y esparcir el juego, generar más nidos a lo largo del mundo.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 4,
                PlayerId = 1,
                OrderIndex = 16,
                Content = "Incluso... tal vez te dé un día más de vida si logras hacer que alguien más juegue conmigo, ayudandome a tener de vuelta a los míos…",
                Side = "right"
            },
            new Dialogue {
                LevelId = 4,
                PlayerId = 1,
                OrderIndex = 17,
                Content = "Je je…",
                Side = "right"
            },
            new Dialogue {
                LevelId = 4,
                PlayerId = 1,
                OrderIndex = 18,
                Content = "Eso sería divertido...",
                Side = "right"
            }
        };

            Connection.InsertAll(dialogues);
            Debug.Log("Seed de Dialogues completado");
        }

    }
}
