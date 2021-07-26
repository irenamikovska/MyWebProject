using System;
using System.Collections.Generic;
using System.Linq;
using WalksInNature.Data;

namespace WalksInNature.Services.Home
{
    public class HomeService : IHomeService
    {
        private readonly WalksDbContext data;
        public HomeService(WalksDbContext data)
            => this.data = data;


    }
}
