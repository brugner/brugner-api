using System;
namespace Brugner.API.Core.Contracts.Services
{
    public interface IDbManagerService
    {
        /// <summary>
        /// Applies any pending migrations for the context to the database.
        /// It will create the database if it does not already exist.
        /// </summary>
        void Migrate();

        /// <summary>
        /// Adds some default values to the database.
        /// </summary>
        void Seed();
    }
}

