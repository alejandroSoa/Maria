using SQLite;

[Table("players")]
public class Player
{
    [PrimaryKey, AutoIncrement]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; }
}

[Table("levels")]
public class Level
{
    [PrimaryKey, AutoIncrement]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; }
}

[Table("dialogues")]
public class Dialogue
{
    [PrimaryKey, AutoIncrement]
    [Column("id")]
    public int Id { get; set; }

    [Indexed]
    [Column("player_id")]
    public int PlayerId { get; set; }

    [Indexed]
    [Column("level_id")]
    public int LevelId { get; set; }

    [Column("content")]
    public string Content { get; set; }

    [Column("order_index")]
    public int OrderIndex { get; set; }

    // "left" o "right"
    [Column("side")]
    public string Side { get; set; }
}
