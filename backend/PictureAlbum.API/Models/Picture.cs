using System.ComponentModel.DataAnnotations;

public class Picture
{
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string Name { get; set; }

    public DateTime? Date { get; set; }

    [MaxLength(250)]
    public string Description { get; set; }

    [Required]
    public string FileName { get; set; }

    [Required]
    public byte[] Content { get; set; } // Store image as binary
}
