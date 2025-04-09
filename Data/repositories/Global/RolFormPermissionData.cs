using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.interfaces;
using Entity;
using Entity.DTOs;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.repositories.Global
{
    public class RolFormPermissionData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RolFormPermissionData> _logger;
        public RolFormPermissionData(ApplicationDbContext context, ILogger<RolFormPermissionData> logger)
        {
            _context = context;
            _logger = logger;
        }

        //======================================______________  LINQ  ______________====================================== 

        // SELECT ALL
        public async Task<IEnumerable<RolFormPermission>> GetAllAsyncLinq()
        {
            try
            {
                return await _context.Set<RolFormPermission>().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "No se pudo obetner a las RolFormPermissionas");
                throw;
            }
        }

        // SELECT BY ID
        public async Task<RolFormPermission?> GetByIdAsyncLinq(int id)
        {
            try
            {
                return await _context.Set<RolFormPermission>().FindAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al traer una RolFormPermissiona por id {id}");
                throw;
            }
        }

        // INSERT 
        public async Task<RolFormPermission> CreateAsyncLinq(RolFormPermission RolFormPermission)
        {
            try
            {
                await _context.Set<RolFormPermission>().AddAsync(RolFormPermission);
                await _context.SaveChangesAsync();
                return RolFormPermission;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"no se pudo agregar RolFormPermissiona {RolFormPermission}");
                throw;
            }
        }

        // UPDATE
        public async Task<bool> UpdateAsyncLinq(RolFormPermission RolFormPermission)
        {
            try
            {
                _context.Set<RolFormPermission>().Update(RolFormPermission);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"No se pudo actualizar {RolFormPermission}");
                throw;

            }
        }

        // DELETE PERSISTENT
        public async Task<bool> DeletePersistentAsyncLinq(int id)
        {
            try
            {
                var Delete = await _context.Set<RolFormPermission>().FindAsync(id);

                if (Delete == null) return false; // usuario inexistente

                _context.Set<RolFormPermission>().Remove(Delete);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($" error al eliminar {ex.Message}");
                return false;
            }
        }


    }
}
