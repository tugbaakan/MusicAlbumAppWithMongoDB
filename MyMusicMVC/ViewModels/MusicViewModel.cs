using Microsoft.AspNetCore.Mvc.Rendering;
using MyMusicMVC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyMusicMVC.ViewModels
{
    public class MusicViewModel
    {
        public string MusicID { get; set; }
        public Music Music { get; set; }
        public SelectList ArtistList { get; set; }
        [Required(ErrorMessage = "Please enter the Artist")]
        [Display(Name = "Artist")]
        public string AristId { get; set; }
    }
}
