using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Specifications
{
    public class ProductoWithCategoriaAndMarca : BaseSpecification<Producto>
    {
        public ProductoWithCategoriaAndMarca(string sort)
        {
            AddInclude(P =>P.Categoria);
            AddInclude(P =>P.Marca);
            if(!string.IsNullOrEmpty(sort)){
                switch (sort)
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
