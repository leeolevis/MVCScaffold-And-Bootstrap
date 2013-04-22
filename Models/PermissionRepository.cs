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
    public class PermissionRepository : IPermissionRepository
    {
        WebApp4Context context = new WebApp4Context();

        public IQueryable<Permission> All
        {
            get { return context.Permission; }
        }

        public IQueryable<Permission> AllIncluding(params Expression<Func<Permission, object>>[] includeProperties)
        {
            IQueryable<Permission> query = context.Permission;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public Permission Find(System.Guid id)
        {
            return context.Permission.Find(id);
        }

        public void InsertOrUpdate(Permission permission, string[] updateAttr = null)
        {
            if (permission.Id == default(System.Guid)) {
                // New entity
				permission.GenerateNewIdentity();
                //permission.Id = Guid.NewGuid();
                context.Permission.Add(permission);
            } else {
                // Existing entity
				context.Permission.Attach(permission);
                var stateEntry = ((IObjectContextAdapter)context).ObjectContext.ObjectStateManager.GetObjectStateEntry(permission);
                stateEntry.SetModified();

                foreach (string item in updateAttr)
                    stateEntry.SetModifiedProperty(item);
                //context.Entry(permission).State = EntityState.Modified;
            }
        }

        public void Delete(System.Guid id)
        {
            var permission = context.Permission.Find(id);
            context.Permission.Remove(permission);
        }

        public void Save()
        {
            context.SaveChanges();
        }

		public virtual IQueryable<Permission> AllMatching(ISpecification<Permission> specification)
        {
            return context.Permission.Where(specification.SatisfiedBy()).Where(c => c.IsDeleted == false)
                           .AsQueryable();
        }

        public virtual IQueryable<Permission> AllMatching(ISpecification<Permission> specification, int pageIndex, int pageCount, string orderByExpression, bool ascending, out int totalCount)
        {
            var set = context.Permission.Where(specification.SatisfiedBy()).Where(c => c.IsDeleted == false);
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

        public virtual IQueryable<Permission> GetPaged<KProperty>(int pageIndex, int pageCount, System.Linq.Expressions.Expression<Func<Permission, KProperty>> orderByExpression, bool ascending)
        {
            var set = context.Permission.Where(c => c.IsDeleted == false);

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

        public virtual IQueryable<Permission> GetPaged(int pageIndex, int pageCount, string orderByExpression, bool ascending)
        {
            var set = context.Permission.Where(c => c.IsDeleted == false);

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

        public virtual IQueryable<Permission> GetFiltered(System.Linq.Expressions.Expression<Func<Permission, bool>> filter)
        {
            return context.Permission.Where(filter)
                           .AsQueryable();
        }

        public virtual int GetCount()
        {
            return context.Permission.Where(c => c.IsDeleted == false).Count();
        }

        public virtual int GetCount(ISpecification<Permission> specification)
        {
            var set = context.Permission.Where(specification.SatisfiedBy()).Where(c => c.IsDeleted == false);

            return set.Count();
        }

        public void Dispose() 
        {
            context.Dispose();
        }
    }

    public interface IPermissionRepository : IDisposable
    {
        IQueryable<Permission> All { get; }

        IQueryable<Permission> AllIncluding(params Expression<Func<Permission, object>>[] includeProperties);

        Permission Find(System.Guid id);

        void InsertOrUpdate(Permission permission,string[] updateAttr = null);

        void Delete(System.Guid id);

        void Save();

        IQueryable<Permission> AllMatching(ISpecification<Permission> specification);

        IQueryable<Permission> AllMatching(ISpecification<Permission> specification, int pageIndex, int pageCount, string orderByExpression, bool ascending, out int totalCount);

        IQueryable<Permission> GetPaged<KProperty>(int pageIndex, int pageCount, Expression<Func<Permission, KProperty>> orderByExpression, bool ascending);

        IQueryable<Permission> GetPaged(int pageIndex, int pageCount, string orderByExpression, bool ascending);

        IQueryable<Permission> GetFiltered(Expression<Func<Permission, bool>> filter);

        int GetCount();

        int GetCount(ISpecification<Permission> specification);
    }
}