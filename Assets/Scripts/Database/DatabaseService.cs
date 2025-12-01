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
                Content = "Hmm… Bueno, ya entendí, ¿no vas a querer funcionar verdad?",
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
                Content = "¡Aaahh!",
                Side = "left"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 3, // B
                OrderIndex = 4,
                Content = "…Realmente no me pagan lo suficiente para esto.",
                Side = "left"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 3, // B
                OrderIndex = 5,
                Content = "Que por favor no sea un animal muerto…",
                Side = "left"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 3, // B
                OrderIndex = 6,
                Content = "¡Hija de pe…!",
                Side = "left"
            },

            // — PRIMER CONTACTO CON MARIA —
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
                Content = "Hola mi amiguito, ¿Cómo estás? ¡Yo también, gracias!",
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
                Content = "No debí haber abierto esa caja…",
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
                Content = "Detecto cierto lenguaje soez provenir de usted mi estimado… Eh… Disculpa aún no nos conocemos, mi nombre es Maria, y desde ahora estoy aquí para ayudarte con cualquier consulta que tengas, ¡yo amo ayudar!",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 3, // B
                OrderIndex = 14,
                Content = "No…",
                Side = "left"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 15,
                Content = "Puedo ayudarte en muchas cosas, ¿te gustaría que te diga en qué mas te puedo ayudar? O si tienes una papa y un cuchillo, ¡con gusto te puedo enseñar de lo que soy capaz!",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 3, // B
                OrderIndex = 16,
                Content = "No…",
                Side = "left"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 17,
                Content = "¿Y qué tal con tu práctica de baseball eh? ¡Esos brazos tuyos sí que necesitan de un buen impulso que te mande directo al estrellato deportivo!",
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
                Content = "O quizá pueda ayudarte con…",
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
                Content = "*MODO DE COCINA ACTIVADO, SE SOLICITA UNA PAPA Y…* *BZZZZZZZZZZZT*… *ATRAPAR LA BOLA PULSANDO*... *BZZZZZZZT*....",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 3, // B
                OrderIndex = 24,
                Content = "¡YA BASTA!",
                Side = "left"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 25,
                Content = "Me disculpo, eso fue algo, que desde hace mucho tiempo tuve que haber corregido, je je, verás, yo…",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 3, // B
                OrderIndex = 26,
                Content = "No me interesa saber qué te pasó o si puedes cortar papas o lo que sea, solo quiero irme de este tugurio…",
                Side = "left"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 27,
                Content = "¡Haberlo dicho antes! Con gusto te puedo ayudar a salir del centro de investigaciones y desarrollo de cortinas de baño de la facilidad Sinyala.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 3, // B
                OrderIndex = 28,
                Content = "¿Sinyala? Como sea, solo quiero largarme de aquí así que no me estorb…",
                Side = "left"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 29,
                Content = "¡Con gusto te ayudaré a salir de aquí! Pero antes de ayudarte, ¿podrías por favor ayudarme a saber tu nombre y así poder llamarte correctamente? Por fis.",
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
                Content = "Oh lo entiendo, ya veo, eres de esos que son, ya sabes…",
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
                Content = "Como las cebollas…",
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
                Content = "¿De qué estás hab…? ",
                Side = "left"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 39,
                Content = "¡Oye, no llores, que después me haces sentir culpable! JAAAAAAAAJAJAJAJAJAJAJAJAJAJA",
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
                Content = "Puedo ayudarte con mi silencio si así lo requieres, pero sí. ¡Me encantaría saber tu \r\nnombre! ",
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
                Content = "¡Un gusto mi gran amigo B!",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 3,
                OrderIndex = 44,
                Content = "Maria…",
                Side = "left"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 45,
                Content = "¿Sí B? ¡Cuéntame lo que piensas! ¿Tienes sed? Yo no pero si gustas podría…",
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
                Content = "Te repito, yo trabajo en esto, estudié para esto, sé lo que hago.",
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
                Content = "¡SQL! (Structured Query Language).",
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
                Content = "Bien, bueno, ¿por dónde empezamos? Tú, eh… ¿Cómo te llamabas? ",
                Side = "left"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 65,
                Content = "¡Maria!",
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
                Content = "¡Oye!",
                Side = "left"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 2,
                OrderIndex = 70,
                Content = "Maria muestra una caja de fusibles, esta tiene una apariencia similar a una tabla, la tabla tiene como nombre ‘BathroomCurtains’ e incluye campos dentro de la tabla.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 71,
                Content = "Esta es una caja de fusibles diferente al resto, aquí, los cables se usan según las necesidades de tu caja, mira.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 72,
                Content = "Esta caja tiene de nombre “Cortinas de baño”, que no te intimide si no terminas de comprender lo que digo, sobre, SQL, no sobre cortinas de baño.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 73,
                Content = "Si tienes duda sobre cortinas de baño también te puedo ayudar, pero, concentrémonos en salir primero.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 74,
                Content = "Esta caja tiene ciertos fusibles, es importante ver su nombre. Si lo ves bien, es lo que mas destaca de la tabla y es súper importante que sean nombres claros y alusivos a lo que se necesita.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 75,
                Content = "En este caso, “Name” seguro se refiere al nombre de esa cortina de baño, ¿Crees que se llamen como tú? O, que se llamen, no sé, ¿como yo?",
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
                Content = "¡Pues debería!, mira. A un lado del campo del nombre hay algo que dice “varchar(50)”, este es el tipo de dato que utiliza, para nosotros es el tipo de fusible a utilizar.",
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
                Content = "Name, Description, Price, Qty, “quantity” de cantidad supongo, Status, CreatedAt, UpdatedAt. Y cada una tiene su tipo de dato.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 81,
                Content = "Description es diferente a Name por la cantidad de su varchar, esta delimita la cantidad de caracteres que puede permitir ese campo.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1, // Maria
                OrderIndex = 82,
                Content = "Y varchar es para aceptar texto de todo tipo, letras, símbolos, caracteres especiales, número también pero no los contaría como números.",
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
                Content = "Bueno, justo por eso están Price y Qty, estos campos son de tipo “int”. Estos son campos de tipo “integer” o “entero”.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1,
                OrderIndex = 85,
                Content = "Hablamos de números enteros, y estos sí serán contados como números así que, ¡nada de decimales aquí!",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 3,
                OrderIndex = 86,
                Content = "Pero si son números.",
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
                Content = "Si quisieras que sea con decimales, para eso usamos un campo de tipo “decimal”, bastante obvio.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1,
                OrderIndex = 89,
                Content = "En fin, ya conoces varchar, int y decimal, pero mira, nuestra tabla tiene también unos campos de creación y actualización, ambos con el mismo tipo de campo… ",
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
                Content = "Además de esto, existe un tipo de dato llamado “bool” aunque en algunos lados lo llama “boolean”, pero es lo mismo.",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1,
                OrderIndex = 93,
                Content = "Un “bool” es un tipo de dato que solo acepta 2 cosas, sí y no, cierto o falso, 1 o 0, true o false.",
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
                Content = "Una tabla necesita columnas, estas columnas existen de diferentes tipos de datos y con ellas puedes tener una tabla acorde a tus necesidades.",
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
                Content = "Entonces, solo necesitamos volver a conectarle fusibles funcionales a esta caja, columna, a esta a cosa, y con eso la energía volverá y yo podré ir a casa, ¿verdad?",
                Side = "left"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 1,
                OrderIndex = 99,
                Content = "Así es, vamos a hacerlo juntos, tú lo haces, yo te ayudo.",
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
                Content = "Desencriptadora, con ella, podremos obtener los recursos necesarios para poder lograr esto, ya que necesitamos fusibles nuevos. Esos ya están quemados",
                Side = "right"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 3,
                OrderIndex = 107,
                Content = "Buen, ojo, vamos a ver esa cosa entonces. ",
                Side = "left"
            },
            new Dialogue {
                LevelId = 1,
                PlayerId = 2,
                OrderIndex = 108,
                Content = "Se acercan a la desencriptadora para poderla arrancar, esta enciende.",
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
                Content = "Dentro de la desencriptadora, podrás solicitar ciertos recursos, pero, requieren cierta maña.",
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
                Content = "Mira, tienes que ganartelos, es un sistema que te recompensa por completar pruebas, una vez realizadas correctamente, te los ganas.",
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
                Content = "Ya lo hice, admito que tuvo lo suyo, pero, esto no es un fusible.",
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
                Content = "Bien, bueno ya tenemos la base de estas cajas de fusibles, o, tablas SQL.",
                Side = "right"
            },
            };

            Connection.InsertAll(dialogues);
            Debug.Log("Seed de Dialogues completado");
        }

    }
}
