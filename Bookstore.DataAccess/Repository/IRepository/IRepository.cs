using System.Linq.Expressions;

namespace Bookstore.DataAccess.Repository.IRepository
{
	public interface IRepository<T> where T : class
	{
		//T - Category.
		IEnumerable<T> GetAll();
		T Get(Expression<Func<T, bool>> filter);// Method to retriv a single entity of type T.
		void Add(T entity);
		void Remove(T entity);
		void RemoveRange(IEnumerable<T> entities);
	}
}
