using MyMusic.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyMusic.Core.Services
{
   public interface IComposerService
    {
        Task<IEnumerable<Composer>> GetAll();
        Task<Composer> GetById(string id);
        Task<Composer> Create(Composer composer);
        Task<bool> Delete(string id);
        void Update(string id, Composer composer);
    }
}
