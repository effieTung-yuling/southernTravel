using System.ComponentModel.DataAnnotations;

public class CreateAttractionDto
{
    [Required(ErrorMessage = "景點名稱不能為空。")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "景點分類不能為空。")]
    public string Category { get; set; } = string.Empty;

    [Required(ErrorMessage = "景點描述不能為空。")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "景點主圖片 URL 不能為空。")]
    public string? MainImageUrl { get; set; }

    public string? Location { get; set; }
}