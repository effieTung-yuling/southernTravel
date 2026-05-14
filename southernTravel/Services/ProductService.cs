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
                Price = x.Price,
                MainImageUrl = x.MainImageUrl,
                DayNum = x.DayNum,
                Tag1= x.Tag1,
                Tag2 = x.Tag2,
                Images = x.Images
                    .OrderBy(i => i.SortOrder)
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
                        LocationTitle = i.LocationTitle,
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
                Price = x.Price,
                // 將圖片映射到 DTO，並依 SortOrder 排序
                Images = x.Images
                    .OrderBy(i => i.SortOrder)
                    .Select(i => new ProductImageDto
                    {
                        ImageId = i.ImageId,
                        ImageUrl = i.ImageUrl,
                        SortOrder = i.SortOrder
                    }).ToList(),
                Itineraries = x.Itineraries.Select(i => new ItineraryDto
                {
                    DayNumber = i.DayNumber,
                    TimePeriod = i.TimePeriod,
                    LocationTitle = i.LocationTitle,
                    Content = i.Content
                }).ToList(),
                // 回傳時：Entity -> Response DTO
                AttractionRefs = x.AttractionRefs.Select(x => new ProductAttractionRefDto
                {
                    RefId = x.RefId,
                    ProductId = x.ProductId,
                    AttractionId = x.AttractionId,
                    IsPreview = x.IsPreview,
                    SortOrder = x.SortOrder
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
                OriginPrice = dto.OriginPrice,
                Price = dto.Price,
                Num = dto.Num,
                MainImageUrl = dto.MainImageUrl,
                MaxTravelers = dto.MaxTravelers,
                CreatedAt = DateTime.UtcNow,
                Images = dto.Images.Select((img, index) => new ProductImage
                {
                    ImageUrl = img.ImageUrl,
                    SortOrder = index + 1
                }).ToList()
            };

            product.Itineraries = dto.Itineraries.Select(i => new Itinerary
            {
                DayNumber = i.DayNumber,
                TimePeriod = i.TimePeriod,
                LocationTitle = i.LocationTitle,
                Content = i.Content,
                Product = product
            }).ToList();

            product.AttractionRefs = dto.AttractionRefs.Select((x, index) => new ProductAttractionRef
            {
                AttractionId = x.AttractionId,
                IsPreview = x.IsPreview,
                SortOrder = x.SortOrder > 0 ? x.SortOrder : index + 1,
                Product = product,
                Attraction = null!
            }).ToList();

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
                Price = result.Price,
                MainImageUrl = result.MainImageUrl,
                IsEnabled = result.IsEnabled,
                Images = result.Images.Select(img => new ProductImageDto
                {
                    ImageId = img.ImageId,
                    ImageUrl = img.ImageUrl,
                    SortOrder = img.SortOrder
                }).ToList(),
                Itineraries = result.Itineraries.Select(i => new ItineraryDto
                {
                    DayNumber = i.DayNumber,
                    TimePeriod = i.TimePeriod,
                    LocationTitle = i.LocationTitle,
                    Content = i.Content
                }).ToList(),
                AttractionRefs = result.AttractionRefs.Select(x => new ProductAttractionRefDto
                {
                    RefId = x.RefId,
                    ProductId = x.ProductId,
                    AttractionId = x.AttractionId,
                    IsPreview = x.IsPreview,
                    SortOrder = x.SortOrder
                }).ToList()
            };
        }

        public async Task<bool> UpdateProductAsync(int id, UpdateProductDto dto)
        {
            var product = await _productRepository.GetProductByIdAsync(id);

            if (product == null) throw new Exception($"Product with ID {id} not found");

            product.Title = dto.Title;
            product.Category = dto.Category;
            product.Tag1 = dto.Tag1;
            product.Tag2 = dto.Tag2;
            product.DayNum = dto.DayNum;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.MainImageUrl = dto.MainImageUrl;
            product.IsEnabled = dto.IsEnabled;
            product.UpdatedAt = DateTime.UtcNow;

            // ✅ ⭐重點：圖片「累加」不是覆蓋
            if (dto.Images != null && dto.Images.Any())
            {
                var maxOrder = product.Images.Any() ? product.Images.Max(i => i.SortOrder) : 0;
                var newImages = dto.Images.Select((img, index) => new ProductImage
                {
                    ImageUrl = img.ImageUrl,
                    SortOrder = maxOrder + index + 1,
                    ProductId = product.ProductId
                }).ToList();

                foreach (var img in newImages) { product.Images.Add(img); }
            }

            // 2. 行程處理 (Itineraries) - 同樣邏輯
            if (dto.Itineraries != null && dto.Itineraries.Any())
            {
                var newItineraries = dto.Itineraries.Select(i => new Itinerary
                {
                    DayNumber = i.DayNumber,
                    TimePeriod = i.TimePeriod,
                    LocationTitle = i.LocationTitle,
                    Content = i.Content,
                    ProductId = product.ProductId,
                    Product = product
                }).ToList();

                foreach (var it in newItineraries) { product.Itineraries.Add(it); }
            }

            // 3. 景點關聯處理 (AttractionRefs) - 修正後的邏輯
            if (dto.AttractionRefs != null)
            {
                // 建議做法：如果是更新，通常會先清空舊的關聯再重新建立，以達成「更新排序」或「刪除」的效果
                product.AttractionRefs.Clear();

                var newRefs = dto.AttractionRefs.Select((refItem, index) => new ProductAttractionRef
                {
                    ProductId = product.ProductId,
                    AttractionId = refItem.AttractionId,
                    IsPreview = refItem.IsPreview,
                    SortOrder = refItem.SortOrder > 0 ? refItem.SortOrder : index + 1,
                    Product = product,
                    Attraction = null!
                }).ToList();

                foreach (var ar in newRefs)
                {
                    product.AttractionRefs.Add(ar);
                }
            }

            await _productRepository.UpdateProductAsync(product);
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
