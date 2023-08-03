using System.ComponentModel.DataAnnotations;

namespace Todo.Grpc.Models;

public class TodoModel
{
    [Key]
    [Required]
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string TodoStatus { get; set; } = "NEW";
}