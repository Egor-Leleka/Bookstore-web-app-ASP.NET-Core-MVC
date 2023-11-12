using Bookstore.DataAccess.Data;
using Bookstore.DataAccess.Repository.IRepository;
using Bookstore.Models.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Bookstore.DataAccess.Repository
{
	public class Repository<T> : IRepository<T> where T : class
	{
		private readonly Entities _entities;
		private DbSet<T> entitiesSet; // Field to hold the DbSet corresponding to the entity type T

		public Repository(Entities entities)
		{
			_entities = entities;
			this.entitiesSet = _entities.Set<T>();
		}

		public void Add(T entity)
		{
			entitiesSet.Add(entity);
		}

		public void Remove(T entity)
		{
			entitiesSet.Remove(entity);
		}

		public void RemoveRange(IEnumerable<T> entities)
		{
			entitiesSet.RemoveRange(entities);
		}

		public T Get(Expression<Func<T, bool>> filter)
		{
			IQueryable<T> query = entitiesSet;
			query = query.Where(filter);
			return query.FirstOrDefault();
		}

		public IEnumerable<T> GetAll()
		{
			IQueryable<T> query = entitiesSet;
			return query.ToList();
		}

	}
}
