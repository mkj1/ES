using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using EmbeddedStock2.Models;
using Microsoft.EntityFrameworkCore;
using EmbeddedStock2.Data;

namespace EmbeddedStock2.Repositories
{

    public class GeneriskRepository<TEntity> : IGeneriskRepository<TEntity> where TEntity : class
    {
        internal ApplicationDbContext context;
        internal DbSet<TEntity> dbSet;

        public GeneriskRepository(ApplicationDbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;
            query.AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        } 

        public virtual TEntity GetByID(object id)
        {
            return dbSet.Find(id);
        }

        public IQueryable<TEntity> GetAll()
        {
            return context.Set<TEntity>().AsNoTracking();
        }


        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
            context.SaveChanges();
        }

        public virtual void Delete(object id)
        {
            TEntity deleteEntity = dbSet.Find(id);
            Delete(deleteEntity);
            context.SaveChanges();
        }


        public virtual void Delete(TEntity id)
        {
            if (context.Entry(id).State == EntityState.Detached)
            {
                dbSet.Attach(id);
            }
            dbSet.Remove(id);
            context.SaveChanges();
            //TEntity deleteEntity = dbSet.Find(id);
            //Delete(deleteEntity);
            //context.SaveChanges();
        }


        public virtual void Update(TEntity updateEntity)
        {
            // Definerer forhold mellem to attachede objekter i en objekt contekst. 
            // Denne funktion gør ikke entiteten "Dirty", altså EntityFrameworket genererer ikke et update statement når SaveChanges() bliver kørt
            // medmindre man specifikt opdaterer en Property til entiteten.
            dbSet.Attach(updateEntity); 
            // Dette sætter entiteten til en "modified state". Det gør HELE entiteten "dirty".
            // Alle fields i entiteten bliver således opdateret når SaveChanges() køres.
            // Det er ikke altid ønsket at alle fields opdateres, men er passende nok i denne opgave.
            context.Entry(updateEntity).State = EntityState.Modified;
            context.SaveChanges(); 
        }

        public virtual void SaveChanges()
        {
            context.SaveChanges();
        }

    }
}