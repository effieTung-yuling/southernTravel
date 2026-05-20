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
                OriginPrice = x.OriginPrice,
                Num = x.Num,
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
                OriginPrice = x.OriginPrice,
                Num = x.Num,
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
                Num= result.Num,
                Description = result.Description,
                OriginPrice = result.OriginPrice,
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

            if (product == null) return false;

            if (dto.Title != null) product.Title = dto.Title;
            if (dto.Category != null) product.Category = dto.Category;
            if (dto.Tag1 != null) product.Tag1 = dto.Tag1;
            if (dto.Tag2 != null) product.Tag2 = dto.Tag2;
            if (dto.DayNum.HasValue) product.DayNum = dto.DayNum.Value;
            if (dto.Description != null) product.Description = dto.Description;
            if (dto.OriginPrice.HasValue) product.OriginPrice = dto.OriginPrice.Value;
            if (dto.Price.HasValue) product.Price = dto.Price.Value;
            if (dto.Num.HasValue) product.Num = dto.Num.Value;
            if (dto.MainImageUrl != null) product.MainImageUrl = dto.MainImageUrl;
            if (dto.IsEnabled.HasValue) product.IsEnabled = dto.IsEnabled.Value;
            if (dto.MaxTravelers.HasValue) product.MaxTravelers = dto.MaxTravelers.Value;
            product.UpdatedAt = DateTime.UtcNow;

            // 圖片累加
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

            // 行程累加
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

            // 景點關聯
            if (dto.AttractionRefs != null)
            {
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

                foreach (var ar in newRefs) { product.AttractionRefs.Add(ar); }
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
