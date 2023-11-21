using Bookstore.DataAccess.Data;
using Bookstore.DataAccess.Repository.IRepository;

namespace Bookstore.DataAccess.Repository
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly Entities _entities;
		public ICategoryRepository Category { get; private set; }
		public IProductRepository Product { get; private set; }
		public ICompanyRepository Company { get; private set; }

		public UnitOfWork(Entities entities)
        {
            _entities = entities;
			Category = new CategoryRepository(_entities);
			Product = new ProductRepository(_entities);
			Company = new CompanyRepository(_entities);
        }

		public void Save()
		{
			_entities.SaveChanges();
		}
	}
}
