using MyApp.DataAccessLayer.Infrastructure.IRepository;
using MyApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.DataAccessLayer.Infrastructure.Repository
{

    internal class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db):base(db)
        {
            _db=db;
        }
        public void Update(Product product)
        {
            Product productdb=_db.products.FirstOrDefault(u=>u.Id==product.Id);
            if (productdb!=null) 
            {
                productdb.Name= product.Name;
                productdb.Description= product.Description;
                productdb.Price= product.Price;
                productdb.CategoryId= product.CategoryId;
            }
            if(productdb.ImageUrl!=null) 
            {
                productdb.ImageUrl= product.ImageUrl;
            }
        }
    }
}
