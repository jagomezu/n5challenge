﻿using Microsoft.EntityFrameworkCore;
using N5Challenge.Infrastructure.Interfaces;
using N5Challenge.Transverse.Entities;
using N5Challenge.Transverse.Logger;
using Serilog;

namespace N5Challenge.Infrastructure.Repository
{
    public class PermissionsQueriesRepository : IPermissionsQueriesRepository
    {
        #region Properties
        private readonly SqlServerDbContext dbContext;
        #endregion

        #region Constructor
        public PermissionsQueriesRepository(SqlServerDbContext context)
        {
            dbContext = context;
        }
        #endregion

        #region Public methods
        public async Task<List<Permissions>> GetPermissions(int index = 0, int size = 20)
        {
            Log.Information("[Get Permissions] --> Index: {@Index} and size {@Size}", index, size);
            List<Permissions> result;

            try
            {
                if (index > 0)
                {
                    int pageNumber = (index + 1);
                    result = await dbContext.Permissions.AsNoTracking().Skip(pageNumber * size).Take(size).ToListAsync();
                }
                else
                {
                    result = await dbContext.Permissions.AsNoTracking().Take(size).ToListAsync();
                }

                Log.Information("[Get Permissions] --> Index: {@Index} and size {@Size} -- Permissions found: {@PermissionsList}", index, size, result);
            }
            catch (Exception ex)
            {
                LoggerUtils.WriteLogError("Get Permissions Error] --> Index: {@Index} and size {@Size}", ex, index, size);

                throw;
            }

            return result;
        }

        public async Task<Permissions?> GetPermissionById(int id)
        {
            Log.Information("[Get Permission by id] --> Id:{@Id}", id);
            Permissions? result;

            try
            {
                result = dbContext.Permissions.AsNoTracking().FirstOrDefault(pt => pt.Id == id);

                if (result != null)
                {
                    Log.Information("[Get Permission by id] --> Id: {@Id} Permission found: {@PermissionType}", id, result);
                }
                else
                {
                    Log.Warning("[Get Permission by id] --> Id: {@Id} Permission not found", result);
                }
            }
            catch (Exception ex)
            {
                LoggerUtils.WriteLogError("[Get Permission by id Error] --> Id: {@Id}", ex, id);

                throw;
            }

            return result;
        }
        #endregion
    }
}
