using Bookstore.DataAccess.Data;
using Bookstore.DataAccess.Repository.IRepository;
using Bookstore.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.DataAccess.Repository
{
	internal class ProductRepository : Repository<Product>, IProductRepository
	{
		private readonly Entities _entities;

		public ProductRepository(Entities entities) : base(entities)
		{
			_entities = entities;
		}

		public void Update(Product product)
		{
			_entities.Update(product);
		}
	}
}
