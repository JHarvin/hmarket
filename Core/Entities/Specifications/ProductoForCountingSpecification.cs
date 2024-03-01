namespace Core.Entities.Specifications;

public class ProductoForCountingSpecification:BaseSpecification<Producto>
{
    public ProductoForCountingSpecification(ProductoSpecificationParam param) 
        : base(x =>
        (string.IsNullOrEmpty(param.Search) || x.Nombre.Contains(param.Search)) &&
        (!param.marca.HasValue || x.MarcaId == param.marca) &&
        (!param.categoria.HasValue || x.CategoriaId == param.categoria)
            )
    {
        
    }
}
