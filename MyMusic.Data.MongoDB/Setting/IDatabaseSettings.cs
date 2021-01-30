using MongoDB.Driver;
using MyMusic.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyMusic.Data.MongoDB.Setting
{
    public interface IDatabaseSettings
    {
        IMongoCollection<Composer> Composers { get; }
    }
}
