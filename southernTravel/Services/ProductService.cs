using southernTravel.Model;
using southernTravel.Repositories;
using southernTravel.DTOs;

namespace southernTravel.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // 取得所有商品
        public async Task<List<ProductDto>> GetAllProductsAsync()
        {
            var data = await _productRepository.GetAllProductsAsync();

            return data.Select(x => new ProductDto
            {
                ProductId = x.ProductId,
                Title = x.Title,
                Category = x.Category,
                Description = x.Description,
                // 將圖片映射到 DTO，並依 SortOrder 排序
                Images = x.Images
                    .OrderBy(i => i.SortOrder) // 如果資料庫已有 sortOrder 就用這個
                    .Select(i => new ProductImageDto
                    {
                        ImageId = i.ImageId,
                        ImageUrl = i.ImageUrl,
                        SortOrder = i.SortOrder
                    })
                    .ToList(),
                    Itineraries = x.Itineraries.Select(i => new ItineraryDto
                    {
                        DayNumber = i.DayNumber,
                        TimePeriod = i.TimePeriod,
                        Title = i.Title,
                        Content = i.Content
                    }).ToList()
            }).ToList();
        }
        // 依據ID取得單一商品
        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var x = await _productRepository.GetProductByIdAsync(id);

            if (x == null) return null;

            return new ProductDto
            {
                ProductId = x.ProductId,
                Title = x.Title,
                Category = x.Category,
                Description = x.Description,
                // 將圖片映射到 DTO，並依 SortOrder 排序
                Images = x.Images
                    .OrderBy(i => i.SortOrder) // 如果資料庫已有 sortOrder 就用這個
                    .Select(i => new ProductImageDto
                    {
                        ImageId = i.ImageId,
                        ImageUrl = i.ImageUrl,
                        SortOrder = i.SortOrder
                    })
                    .ToList(),
                Itineraries = x.Itineraries.Select(i => new ItineraryDto
                {
                    DayNumber = i.DayNumber,
                    TimePeriod = i.TimePeriod,
                    Title = i.Title,
                    Content = i.Content
                }).ToList()
            };
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto dto)
        {
            var product = new Product
            {
                Title = dto.Title,
                Category = dto.Category,
                Tag1 = dto.Tag1,
                Tag2 = dto.Tag2,
                DayNum = dto.DayNum,
                Description = dto.Description,
                Content = dto.Content,
                OriginPrice = dto.OriginPrice,
                Price = dto.Price,
                Num = dto.Num,
                ImageUrl1 = dto.ImageUrl1,
                MaxTravelers = dto.MaxTravelers,
                CreatedAt = DateTime.UtcNow,
                Images = dto.Images.Select((img, index) => new ProductImage
                {
                    ImageUrl = img.ImageUrl,
                    SortOrder = index + 1 // ⭐ 每個 product 從 1 開始排序
                }).ToList(),
                Itineraries = dto.Itineraries.Select(i => new Itinerary
                {
                    DayNumber = i.DayNumber,
                    TimePeriod = i.TimePeriod,
                    Title = i.Title,
                    Content = i.Content
                    }).ToList()
                };

            var result = await _productRepository.CreateProductAsync(product);

            return new ProductDto
            {
                ProductId = result.ProductId,
                Title = result.Title,
                Category = result.Category,
                Tag1 = result.Tag1,
                Tag2 = result.Tag2,
                DayNum = result.DayNum,
                Description = result.Description,
                Content = result.Content,
                Price = result.Price,
                ImageUrl1 = result.ImageUrl1,
                IsEnabled = result.IsEnabled,

                Images = result.Images.Select(img => new ProductImageDto
                {
                    ImageId = img.ImageId,
                    ImageUrl = img.ImageUrl,
                    SortOrder = img.SortOrder
                }).ToList(),

                // 🔥 改這裡
                Itineraries = result.Itineraries.Select(i => new ItineraryDto
                {
                    DayNumber = i.DayNumber,
                    TimePeriod = i.TimePeriod,
                    Title = i.Title,
                    Content = i.Content
                }).ToList()
            };
        }

        public async Task<bool> UpdateProductAsync(int id, UpdateProductDto dto)
        {
            var productList = await _productRepository.GetProductByIdAsync(id);

            if (productList == null) throw new Exception("Product not found");


            productList.Title = dto.Title;
            productList.Category = dto.Category;
            productList.Tag1 = dto.Tag1;
            productList.Tag2 = dto.Tag2;
            productList.DayNum = dto.DayNum;
            productList.Description = dto.Description;
            productList.Content = dto.Content;
            productList.Price = dto.Price;
            productList.ImageUrl1 = dto.ImageUrl1;
            productList.IsEnabled = dto.IsEnabled;
            productList.UpdatedAt = DateTime.UtcNow;
            // ✅ ⭐重點：圖片「累加」不是覆蓋
            if (dto.Images != null && dto.Images.Any())
            {
                var maxOrder = productList.Images.Any()
                    ? productList.Images.Max(i => i.SortOrder)
                    : 0;

                var newImages = dto.Images.Select((img, index) => new ProductImage
                {
                    ImageUrl = img.ImageUrl,
                    SortOrder = maxOrder + index + 1,
                    ProductId = productList.ProductId
                }).ToList();

                foreach (var img in newImages)
                {
                    productList.Images.Add(img);
                }
            }
            if(dto.Itineraries != null && dto.Itineraries.Any())
            {
                var maxDayNum = productList.Itineraries.Any()
                    ? productList.Itineraries.Max(i => i.DayNumber)
                    : 0;
                var newItineraries = dto.Itineraries.Select(i => new Itinerary
                {
                    DayNumber = i.DayNumber,
                    TimePeriod = i.TimePeriod,
                    Title = i.Title,
                    Content = i.Content,
                    ProductId = productList.ProductId
                }).ToList();
                foreach (var it in newItineraries)
                {
                    productList.Itineraries.Add(it);
                }
            }

            await _productRepository.UpdateProductAsync(productList);

            return true;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var productList = await _productRepository.GetProductByIdAsync(id);

            if (productList == null) return false;

            await _productRepository.DeleteProductAsync(productList);

            return true;
        }

    }
}
