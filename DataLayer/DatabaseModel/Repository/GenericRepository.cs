using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using CommonLayer;
using DataLayer.DatabaseModel;


namespace DataLayer
{
    public class GenericRepository<T> where T : class
    {
        /// <summary>
        /// Function to get entites
        /// </summary>
        /// <returns>DbSet result of entities</returns>
        public DbSet<T> GetEntities()
        {
            StockEntities context = BaseContext.GetDbContext();
            return context.Set<T>();
        }

        /// <summary>
        /// Function to create entity instance
        /// </summary>
        /// <returns>New instance of entity</returns>
        public T CreateEntity()
        {
            using (StockEntities context = BaseContext.GetDbContext())
            {
                DbSet<T> table = context.Set<T>();
                return table.Create();
            }
        }

        /// <summary>
        /// Function to select entity by primary key
        /// </summary>
        /// <param name="pk">Primary key of entity to fetch</param>
        /// <returns></returns>
        public T SelectByID(object pk)
        {
            using (StockEntities context = BaseContext.GetDbContext())
            {
                DbSet<T> table = context.Set<T>();
                return table.Find(pk);
            }
        }

        /// <summary>
        /// Function to insert entity.
        /// </summary>
        /// <param name="entity">entity to be inserted</param>
        public string Insert(T entity)
        {
            try
            {
                using (StockEntities context = BaseContext.GetDbContext())
                {
                    DbSet<T> table = context.Set<T>();
                    table.Add(entity);
                    context.SaveChanges();
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                return CommonHelper.GetErrorMessage(ex);
            }
        }

        /// <summary>
        /// Function to update entity.
        /// </summary>
        /// <param name="entity">entity to be updated</param>
        public string Update(T entity)
        {
            try
            {
                using (StockEntities context = BaseContext.GetDbContext())
                {
                    DbSet<T> table = context.Set<T>();
                    table.Attach(entity);
                    context.Entry(entity).State = EntityState.Modified;
                    context.SaveChanges();
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                return CommonHelper.GetErrorMessage(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pk">Primary key of entity to be deleted</param>
        public string Delete(object pk)
        {
            try
            {
                using (StockEntities context = BaseContext.GetDbContext())
                {
                    DbSet<T> table = context.Set<T>();
                    T existing = table.Find(pk);
                    context.Entry(existing).State = EntityState.Deleted;
                    context.SaveChanges();
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                return CommonHelper.GetDeleteException(ex);
            }
        }

        /// <summary>
        /// Function to insert entity.
        /// </summary>
        /// <param name="entity">entity to be inserted</param>
        /// <param name="context">context to maintain transaction</param>
        public void Insert(T entity, StockEntities context)
        {
            DbSet<T> table = context.Set<T>();
            table.Add(entity);
        }

        /// <summary>
        /// Function to update entity.
        /// </summary>
        /// <param name="entity">entity to be updated</param>
        /// <param name="context">context to maintain transaction</param>
        public void Update(T entity, StockEntities context)
        {
            DbSet<T> table = context.Set<T>();
            table.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// Function to delete entity whose primary key is passes.
        /// </summary>
        /// <param name="pk">Primary key of entity to be deleted</param>
        /// <param name="context">context to maintain transaction</param>
        public void Delete(object pk, StockEntities context)
        {
            DbSet<T> table = context.Set<T>();
            T existing = table.Find(pk);
            context.Entry(existing).State = EntityState.Deleted;
        }

        //Func<T, bool> isActiveEmployee = e => e.IsActive == true;
        //Func<T, bool> isNewAndSurvived = e => e.IsActive == true && e.DateHired >= DateTime.Today.AddDays(-90);
        /// <summary>
        /// Function to search with/without any condition
        /// </summary>
        /// <param name="filter">Filter condition to apply</param>
        /// <returns>List of entity that matched search condition(if specified)</returns>
        public List<T> Search(Expression<Func<T, bool>> filter = null)
        {
            using (StockEntities context = BaseContext.GetDbContext())
            {
                if (filter == null)
                    return context.Set<T>().ToList<T>();
                return context.Set<T>().Where<T>(filter).ToList<T>();
            }

        }

        //Func<T, bool> isActiveEmployee = e => e.IsActive == true;
        //Func<T, bool> isNewAndSurvived = e => e.IsActive == true && e.DateHired >= DateTime.Today.AddDays(-90);
        /// <summary>
        /// Function to search entity with filter and sort expression. It will also return total count of records that satisfy filter condition
        /// </summary>
        /// <param name="filter">Filter condition to apply</param>
        /// <param name="sortExpression">Expression for sorting</param>
        /// <param name="total">Return total count of records that satisfy filter condition</param>
        /// <param name="index">page index</param>
        /// <param name="size">page sixe</param>
        /// <returns></returns>
        public List<T> Search(Expression<Func<T, bool>> filter, Expression<Func<T, object>> sortExpression, out int total, int index, int size)
        {
            using (StockEntities context = BaseContext.GetDbContext())
            {
                int skipCount = index * size;
                var result = ((filter != null)) ? context.Set<T>().Where<T>(filter).OrderBy<T, object>(sortExpression).AsQueryable() : context.Set<T>().OrderBy<T, object>(sortExpression).AsQueryable();
                total = result.Count();
                result = (skipCount == 0) ? result.Take(size) : result.Skip(skipCount).Take(size);
                return result.ToList();
            }
        }

        //Func<T, bool> isActiveEmployee = e => e.IsActive == true;
        //Func<T, bool> isNewAndSurvived = e => e.IsActive == true && e.DateHired >= DateTime.Today.AddDays(-90);
        /// <summary>
        /// Function to search entity with filter and sort expression. It will not return total count of records that satisfy filter condition.
        /// </summary>
        /// <param name="filter">Filter condition to apply</param>
        /// <param name="sortExpression">Expression for sorting</param>
        /// <param name="index">page index</param>
        /// <param name="size">page sixe</param>
        /// <returns></returns>
        public List<T> Search(Expression<Func<T, bool>> filter, Expression<Func<T, object>> sortExpression, int index, int size)
        {
            using (StockEntities context = BaseContext.GetDbContext())
            {
                int skipCount = index * size;
                var result = ((filter != null)) ? context.Set<T>().Where<T>(filter).OrderBy<T, object>(sortExpression).AsQueryable() : context.Set<T>().OrderBy<T, object>(sortExpression).AsQueryable();
                result = (skipCount == 0) ? result.Take(size) : result.Skip(skipCount).Take(size);
                return result.ToList();
            }
        }

        public IEnumerable<T> ExecWithStoreProcedure(string query, params object[] parameters)
        {
            using (StockEntities context = BaseContext.GetDbContext())
            {
                return context.Database.SqlQuery<T>(query, parameters);
            }
        }


        //How to use store procedure directly
        //IEnumerable<MD_Users> products =
        //    context.MD_Users.ExecWithStoreProcedure(
        //     "spGetProducts @bigCategoryId",
        //     new SqlParameter("bigCategoryId", SqlDbType.BigInt) { Value = categoryId });





    }
}
