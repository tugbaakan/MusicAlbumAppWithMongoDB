using MyMusic.Core.Repositories;
using System;
using System.Threading.Tasks;

namespace MyMusic.Core
{
    public interface IUnitOfWork: IDisposable
    {
        IMusicRepository MusicRepository { get; }
        IArtistRepository ArtistRepository { get; }
        IUserRepository UserRepository { get; }
        Task<int> CommitAsync();
    }
}
