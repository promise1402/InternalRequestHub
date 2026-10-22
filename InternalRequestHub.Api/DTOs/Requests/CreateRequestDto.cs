using System.ComponentModel.DataAnnotations;

namespace InternalRequestHub.Api.DTOs.Requests;

public sealed class CreateRequestDto
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(2000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    [RegularExpression("^(Low|Medium|High|Critical)$", ErrorMessage = "Priority must be Low, Medium, High, or Critical.")]
    public string Priority { get; set; } = string.Empty;
}
