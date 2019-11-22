using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MemeCo.Areas.Identity.Data;
using MemeCo.Models;
using Microsoft.EntityFrameworkCore;

namespace MemeCo.Controllers
{

    public class EditorController : Controller
    {
        private MemeCoContext _context;


        public EditorController(MemeCoContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var result = _context.Posts.Include(o => o.Likes).Include(o => o.User).ThenInclude(o => o.Followers).Include(o => o.Comments).ToList();
            return View(result);
        }
    }
}