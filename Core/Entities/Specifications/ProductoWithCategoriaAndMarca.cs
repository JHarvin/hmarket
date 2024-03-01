using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Specifications
{
    public class ProductoWithCategoriaAndMarca : BaseSpecification<Producto>
    {
        public ProductoWithCategoriaAndMarca(ProductoSpecificationParam param)
            : base(x =>
            (string.IsNullOrEmpty(param.Search) || x.Nombre.Contains(param.Search)) &&
            (!param.marca.HasValue || x.MarcaId==param.marca ) &&
            (!param.categoria.HasValue || x.CategoriaId == param.categoria)
            )

        {
            AddInclude(P =>P.Categoria);
            AddInclude(P =>P.Marca);
            ApplyPaging(param.PageSize * (param.pageIndex - 1),param.PageSize);
            //ApplyPaging()
            if(!string.IsNullOrEmpty(param.sort)){
                switch (param.sort)
                {
                    case "categoriaAsc":
                        AddOrderBy(P =>P.Categoria.Nombre);
                        break;

                    case "precioAsc":
                        AddOrderBy(P =>P.Precio);
                        break;
                    case "precioDesc":
                        AddOrderByDescending(P =>P.Precio);
                        break;
                    case "descripcionAsc":
                        AddOrderBy(P =>P.Descripcion);
                        break;
                    case "descripcionDesc":
                        AddOrderByDescending(P => P.Descripcion);
                        break;

                    default:
                        AddOrderBy(P => P.Nombre);
                        break;

                }
            }
           
            
        }
        public ProductoWithCategoriaAndMarca(int id): base(x =>x.Id ==id)
        {
            AddInclude(P => P.Categoria);
            AddInclude(P => P.Marca);
        }
    }
}
