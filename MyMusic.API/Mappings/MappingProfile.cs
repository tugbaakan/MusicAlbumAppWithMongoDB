using AutoMapper;
using MyMusic.API.Resources;
using MyMusic.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMusic.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Domain to Resource
            CreateMap<Music, MusicResource>();
            CreateMap<Music, SaveMusicResource>(); 
            
            CreateMap<Artist, ArtistResource>();
            CreateMap<Artist, SaveArtistResource>();

            CreateMap<Composer, ComposerResource>()
                    .ForMember(c => c.Id, opt => opt.MapFrom(c => c.Id.ToString()));
            CreateMap<Composer, SaveComposerResource>();
            
            CreateMap<User, UserResource>();


            //Resource to Domain
            CreateMap<MusicResource, Music>();
            CreateMap<SaveMusicResource, Music>(); 
            
            CreateMap<ArtistResource, Artist>();
            CreateMap<SaveArtistResource, Artist>();

            CreateMap<ComposerResource, Composer>()
                     .ForMember(m => m.Id, opt => opt.Ignore());
            CreateMap<SaveComposerResource, Composer>();

            CreateMap<UserResource, User>();

        }
    }
}
