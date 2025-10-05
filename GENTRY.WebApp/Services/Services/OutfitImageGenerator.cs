using GENTRY.WebApp.Services.DataTransferObjects.OutfitAIDTOs;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;

namespace GENTRY.WebApp.Services.Services
{
    public class OutfitImageGenerator
    {
        private readonly ILogger<OutfitImageGenerator> _logger;
        private readonly HttpClient _httpClient;

        public OutfitImageGenerator(ILogger<OutfitImageGenerator> logger)
        {
            _logger = logger;
            _httpClient = new HttpClient();
        }

        public async Task<byte[]?> CreateOutfitCollageAsync(List<OutfitItemDto> outfitItems)
        {
            try
            {
                // Lọc các items có hình ảnh
                var itemsWithImages = outfitItems.Where(item => !string.IsNullOrEmpty(item.ItemImageUrl)).ToList();
                
                if (!itemsWithImages.Any())
                {
                    _logger.LogWarning("No items with images found for outfit collage");
                    return null;
                }

                // Tải và xử lý hình ảnh
                var images = new List<Image>();
                foreach (var item in itemsWithImages)
                {
                    try
                    {
                        var imageBytes = await _httpClient.GetByteArrayAsync(item.ItemImageUrl);
                        var image = Image.Load(imageBytes);
                        
                        // Resize image to standard size
                        image.Mutate(x => x.Resize(200, 200));
                        images.Add(image);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to load image for item {ItemId}", item.ItemId);
                    }
                }

                if (!images.Any())
                {
                    _logger.LogWarning("No valid images loaded for outfit collage");
                    return null;
                }

                // Tạo collage
                var collageWidth = Math.Min(images.Count, 3) * 220; // 3 items per row max
                var collageHeight = ((images.Count + 2) / 3) * 220; // Calculate rows needed
                
                using var collage = new Image<Rgba32>(collageWidth, collageHeight);
                
                // Thêm background trắng
                collage.Mutate(x => x.BackgroundColor(Color.White));
                
                // Đặt các hình ảnh vào collage
                for (int i = 0; i < images.Count; i++)
                {
                    var row = i / 3;
                    var col = i % 3;
                    var x = col * 220 + 10;
                    var y = row * 220 + 10;
                    
                    collage.Mutate(ctx => ctx.DrawImage(images[i], new Point(x, y), 1f));
                }

                // Convert to bytes
                using var ms = new MemoryStream();
                await collage.SaveAsJpegAsync(ms, new JpegEncoder { Quality = 85 });
                
                // Cleanup
                foreach (var img in images)
                {
                    img.Dispose();
                }

                return ms.ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating outfit collage");
                return null;
            }
        }

        public async Task<byte[]?> CreateOutfitLayoutAsync(List<OutfitItemDto> outfitItems)
        {
            try
            {
                // Tạo layout theo category (áo ở trên, quần ở dưới, giày ở cuối, v.v.)
                var categorizedItems = CategorizeItems(outfitItems);
                
                var layoutWidth = 400;
                var layoutHeight = 600;
                
                using var layout = new Image<Rgba32>(layoutWidth, layoutHeight);
                layout.Mutate(x => x.BackgroundColor(Color.WhiteSmoke));
                
                // Thêm background và title placeholder
                // Note: Text drawing requires additional ImageSharp.Drawing package

                var currentY = 80;
                
                // Hiển thị từng category
                foreach (var category in categorizedItems)
                {
                    if (category.Value.Any())
                    {
                        // Category spacing
                        currentY += 30;
                        
                        // Vẽ items trong category
                        foreach (var item in category.Value)
                        {
                            if (!string.IsNullOrEmpty(item.ItemImageUrl))
                            {
                                try
                                {
                                    var imageBytes = await _httpClient.GetByteArrayAsync(item.ItemImageUrl);
                                    using var itemImage = Image.Load(imageBytes);
                                    itemImage.Mutate(x => x.Resize(120, 120));
                                    
                                    layout.Mutate(x => x.DrawImage(itemImage, new Point(20, currentY), 1f));
                                    
                                    currentY += 140;
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogWarning(ex, "Failed to add item {ItemName} to layout", item.ItemName);
                                }
                            }
                        }
                    }
                }

                // Convert to bytes
                using var ms = new MemoryStream();
                await layout.SaveAsJpegAsync(ms, new JpegEncoder { Quality = 90 });
                return ms.ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating outfit layout");
                return null;
            }
        }

        private Dictionary<string, List<OutfitItemDto>> CategorizeItems(List<OutfitItemDto> items)
        {
            var categories = new Dictionary<string, List<OutfitItemDto>>
            {
                { "TOPS", new List<OutfitItemDto>() },
                { "BOTTOMS", new List<OutfitItemDto>() },
                { "SHOES", new List<OutfitItemDto>() },
                { "ACCESSORIES", new List<OutfitItemDto>() }
            };

            foreach (var item in items)
            {
                var categoryName = item.CategoryName.ToUpper();
                
                if (categoryName.Contains("SHIRT") || categoryName.Contains("TOP") || 
                    categoryName.Contains("BLOUSE") || categoryName.Contains("JACKET") || 
                    categoryName.Contains("COAT") || categoryName.Contains("SWEATER"))
                {
                    categories["TOPS"].Add(item);
                }
                else if (categoryName.Contains("PANT") || categoryName.Contains("JEAN") || 
                         categoryName.Contains("SHORT") || categoryName.Contains("SKIRT") || 
                         categoryName.Contains("DRESS"))
                {
                    categories["BOTTOMS"].Add(item);
                }
                else if (categoryName.Contains("SHOE") || categoryName.Contains("BOOT") || 
                         categoryName.Contains("SNEAKER") || categoryName.Contains("SANDAL"))
                {
                    categories["SHOES"].Add(item);
                }
                else
                {
                    categories["ACCESSORIES"].Add(item);
                }
            }

            // Remove empty categories
            return categories.Where(c => c.Value.Any()).ToDictionary(c => c.Key, c => c.Value);
        }

        private string TruncateText(string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text)) return "";
            return text.Length <= maxLength ? text : text.Substring(0, maxLength - 3) + "...";
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
} 