namespace E_Commerce.Shared.DTOs.ProductDTOs
{
    public record ProductDTO(
        int Id,
        string Name,
        string Description,
        string PictureUrl,
        decimal Price,
        string ProductType,
        string ProductBrand);
}
