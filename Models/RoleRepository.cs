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
    public class RoleRepository : IRoleRepository
    {
        WebApp4Context context = new WebApp4Context();

        public IQueryable<Role> All
        {
            get { return context.Role; }
        }

        public IQueryable<Role> AllIncluding(params Expression<Func<Role, object>>[] includeProperties)
        {
            IQueryable<Role> query = context.Role;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public Role Find(System.Guid id)
        {
            return context.Role.Find(id);
        }

        public void InsertOrUpdate(Role role, string[] updateAttr = null)
        {
            if (role.Id == default(System.Guid)) {
                // New entity
				role.GenerateNewIdentity();
                //role.Id = Guid.NewGuid();
                context.Role.Add(role);
            } else {
                // Existing entity
				context.Role.Attach(role);
                var stateEntry = ((IObjectContextAdapter)context).ObjectContext.ObjectStateManager.GetObjectStateEntry(role);
                stateEntry.SetModified();

                foreach (string item in updateAttr)
                    stateEntry.SetModifiedProperty(item);
                //context.Entry(role).State = EntityState.Modified;
            }
        }

        public void Delete(System.Guid id)
        {
            var role = context.Role.Find(id);
            context.Role.Remove(role);
        }

        public void Save()
        {
            context.SaveChanges();
        }

		public virtual IQueryable<Role> AllMatching(ISpecification<Role> specification)
        {
            return context.Role.Where(specification.SatisfiedBy()).Where(c => c.IsDeleted == false)
                           .AsQueryable();
        }

        public virtual IQueryable<Role> AllMatching(ISpecification<Role> specification, int pageIndex, int pageCount, string orderByExpression, bool ascending, out int totalCount)
        {
            var set = context.Role.Where(specification.SatisfiedBy()).Where(c => c.IsDeleted == false);
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

        public virtual IQueryable<Role> GetPaged<KProperty>(int pageIndex, int pageCount, System.Linq.Expressions.Expression<Func<Role, KProperty>> orderByExpression, bool ascending)
        {
            var set = context.Role.Where(c => c.IsDeleted == false);

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

        public virtual IQueryable<Role> GetPaged(int pageIndex, int pageCount, string orderByExpression, bool ascending)
        {
            var set = context.Role.Where(c => c.IsDeleted == false);

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

        public virtual IQueryable<Role> GetFiltered(System.Linq.Expressions.Expression<Func<Role, bool>> filter)
        {
            return context.Role.Where(filter)
                           .AsQueryable();
        }

        public virtual int GetCount()
        {
            return context.Role.Where(c => c.IsDeleted == false).Count();
        }

        public virtual int GetCount(ISpecification<Role> specification)
        {
            var set = context.Role.Where(specification.SatisfiedBy()).Where(c => c.IsDeleted == false);

            return set.Count();
        }

        public void Dispose() 
        {
            context.Dispose();
        }
    }

    public interface IRoleRepository : IDisposable
    {
        IQueryable<Role> All { get; }

        IQueryable<Role> AllIncluding(params Expression<Func<Role, object>>[] includeProperties);

        Role Find(System.Guid id);

        void InsertOrUpdate(Role role,string[] updateAttr = null);

        void Delete(System.Guid id);

        void Save();

        IQueryable<Role> AllMatching(ISpecification<Role> specification);

        IQueryable<Role> AllMatching(ISpecification<Role> specification, int pageIndex, int pageCount, string orderByExpression, bool ascending, out int totalCount);

        IQueryable<Role> GetPaged<KProperty>(int pageIndex, int pageCount, Expression<Func<Role, KProperty>> orderByExpression, bool ascending);

        IQueryable<Role> GetPaged(int pageIndex, int pageCount, string orderByExpression, bool ascending);

        IQueryable<Role> GetFiltered(Expression<Func<Role, bool>> filter);

        int GetCount();

        int GetCount(ISpecification<Role> specification);
    }
}