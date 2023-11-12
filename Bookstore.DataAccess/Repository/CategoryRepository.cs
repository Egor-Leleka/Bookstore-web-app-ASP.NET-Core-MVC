using Bookstore.DataAccess.Data;
using Bookstore.DataAccess.Repository.IRepository;
using Bookstore.Models.Models;

namespace Bookstore.DataAccess.Repository
{
	public class CategoryRepository : Repository<Category>, ICategoryRepository
	{
		private readonly Entities _entitiy;

		public CategoryRepository(Entities entities) : base(entities)
		{
			_entitiy = entities;
		}

		public void Update(Category category)
		{
			_entitiy.Update(category);
		}
	}
}
