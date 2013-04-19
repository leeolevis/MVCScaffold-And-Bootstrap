using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp4.Specification;
using WebApp4.Entities;
using WebApp4.Infrastructure;

namespace WebApp4.Models
{ 
    public class UserRepository : IUserRepository
    {
        WebApp4Context context = new WebApp4Context();

        public IQueryable<User> All
        {
            get { return context.User; }
        }

        public IQueryable<User> AllIncluding(params Expression<Func<User, object>>[] includeProperties)
        {
            IQueryable<User> query = context.User;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public User Find(System.Guid id)
        {
            return context.User.Find(id);
        }

        public void InsertOrUpdate(User user, string[] updateAttr = null)
        {
            if (user.Id == default(System.Guid)) {
                // New entity
				user.GenerateNewIdentity();
                //user.Id = Guid.NewGuid();
                context.User.Add(user);
            } else {
                // Existing entity
				context.User.Attach(user);
                var stateEntry = ((IObjectContextAdapter)context).ObjectContext.ObjectStateManager.GetObjectStateEntry(user);
                stateEntry.SetModified();

                foreach (string item in updateAttr)
                    stateEntry.SetModifiedProperty(item);
                //context.Entry(user).State = EntityState.Modified;
            }
        }

        public void Delete(System.Guid id)
        {
            var user = context.User.Find(id);
            context.User.Remove(user);
        }

        public void Save()
        {
            context.SaveChanges();
        }

		public virtual IQueryable<User> AllMatching(ISpecification<User> specification)
        {
            return context.User.Where(specification.SatisfiedBy()).Where(c => c.IsDeleted == false)
                           .AsQueryable();
        }

        public virtual IQueryable<User> AllMatching(ISpecification<User> specification, int pageIndex, int pageCount, string orderByExpression, bool ascending, out int totalCount)
        {
            var set = context.User.Where(specification.SatisfiedBy()).Where(c => c.IsDeleted == false);
            totalCount = set.Count();

            if (ascending)
            {
                return set.OrderBy(orderByExpression)
                          .Skip(pageCount * pageIndex)
                          .Take(pageCount)
                          .AsQueryable();
            }
            else
            {
                return set.OrderByDescending(orderByExpression)
                          .Skip(pageCount * pageIndex)
                          .Take(pageCount)
                          .AsQueryable();
            }
        }

        public virtual IQueryable<User> GetPaged<KProperty>(int pageIndex, int pageCount, System.Linq.Expressions.Expression<Func<User, KProperty>> orderByExpression, bool ascending)
        {
            var set = context.User.Where(c => c.IsDeleted == false);

            if (ascending)
            {
                return set.OrderBy(orderByExpression)
                          .Skip(pageCount * pageIndex)
                          .Take(pageCount)
                          .AsQueryable();
            }
            else
            {
                return set.OrderByDescending(orderByExpression)
                          .Skip(pageCount * pageIndex)
                          .Take(pageCount)
                          .AsQueryable();
            }
        }

        public virtual IQueryable<User> GetPaged(int pageIndex, int pageCount, string orderByExpression, bool ascending)
        {
            var set = context.User.Where(c => c.IsDeleted == false);

            if (ascending)
            {
                return set.OrderBy(orderByExpression)
                          .Skip(pageCount * pageIndex)
                          .Take(pageCount)
                          .AsQueryable();
            }
            else
            {
                return set.OrderByDescending(orderByExpression)
                          .Skip(pageCount * pageIndex)
                          .Take(pageCount)
                          .AsQueryable();
            }
        }

        public virtual IQueryable<User> GetFiltered(System.Linq.Expressions.Expression<Func<User, bool>> filter)
        {
            return context.User.Where(filter)
                           .AsQueryable();
        }

        public virtual int GetCount()
        {
            return context.User.Where(c => c.IsDeleted == false).Count();
        }

        public virtual int GetCount(ISpecification<User> specification)
        {
            var set = context.User.Where(specification.SatisfiedBy()).Where(c => c.IsDeleted == false);

            return set.Count();
        }

        public void Dispose() 
        {
            context.Dispose();
        }
    }

    public interface IUserRepository : IDisposable
    {
        IQueryable<User> All { get; }

        IQueryable<User> AllIncluding(params Expression<Func<User, object>>[] includeProperties);

        User Find(System.Guid id);

        void InsertOrUpdate(User user,string[] updateAttr = null);

        void Delete(System.Guid id);

        void Save();

        IQueryable<User> AllMatching(ISpecification<User> specification);

        IQueryable<User> AllMatching(ISpecification<User> specification, int pageIndex, int pageCount, string orderByExpression, bool ascending, out int totalCount);

        IQueryable<User> GetPaged<KProperty>(int pageIndex, int pageCount, Expression<Func<User, KProperty>> orderByExpression, bool ascending);

        IQueryable<User> GetPaged(int pageIndex, int pageCount, string orderByExpression, bool ascending);

        IQueryable<User> GetFiltered(Expression<Func<User, bool>> filter);

        int GetCount();

        int GetCount(ISpecification<User> specification);
    }
}