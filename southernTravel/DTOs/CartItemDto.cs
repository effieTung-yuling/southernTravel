using System.ComponentModel.DataAnnotations;

namespace southernTravel.DTOs
{
    public class CreateCartItemDto
    {
        public int ProductId { get; set; }
        public int Qty { get; set; }
        //public int Price { get; set; }
    }
    public class UpdateCartItemDto
    {
        [Required(ErrorMessage = "ProductId 為必填欄位。")]
        [Range(1, int.MaxValue, ErrorMessage = "ProductId 必須為正整數。")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "數量為必填欄位。")]
        [Range(1, int.MaxValue, ErrorMessage = "數量必須至少為 1。")]
        public int Qty { get; set; }
    }

    public class CartDto
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public decimal FinalTotal { get; set; }
        public required List<CartItemDto> Items { get; set; }
    }

    public class CartItemDto
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; } // ⭐ 建議加
        public int Qty { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
    }
}
