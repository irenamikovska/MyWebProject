using Microsoft.EntityFrameworkCore;
using System;
using WalksInNature.Data;

namespace WalksInNature.Test.Data
{
    public class DataBaseMock
    {
        public static WalksDbContext Instance
        {
            get
            {
                var dbContextOptions = new DbContextOptionsBuilder<WalksDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

                return new WalksDbContext(dbContextOptions);
            }
        }
    }
}
