using Microsoft.EntityFrameworkCore;
using ShopProjectMVC.Core.Interfaces;

namespace ShopProjectMVC.Storage.Repositories;

public class GenericRepository : IRepository
{
    private readonly ShopProjectContext _context;

    public GenericRepository(ShopProjectContext context)
    {
        _context = context;
    }

    public async Task<T> Add<T>(T entity) where T : class
    {
        var obj = _context.Add(entity);
        await _context.SaveChangesAsync();
        return obj.Entity;
    }

    public async Task Delete<T>(int id) where T : class
    {
        var entity = await GetById<T>(id);

        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
        
    }

    public  IQueryable<T> GetAll<T>() where T : class
    {
        return  _context.Set<T>();
    }

    public async Task<T> GetById<T>(int id) where T : class
    {
       return await _context.Set<T>().FindAsync(id);

    }

    public async Task<T> Update<T>(T entity) where T : class
    {
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
}
