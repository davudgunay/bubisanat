using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using bubisanat.Data;
using bubisanat.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.Extensions.Hosting;

namespace bubisanat.Controllers
{
   
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PostsController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        // GET: Posts
        public async Task<IActionResult> Index(short? id = null)
        {
            IQueryable<Post> applicationDbContext = _context.Posts.Include(p => p.Category).Include(p => p.NextPost).Include(p => p.PreviousPost);
            if (id != null)
            {
                applicationDbContext = applicationDbContext.Where(p => p.CategoryId == id.Value);
            }
            return View(await applicationDbContext.ToListAsync());
        }
        
        // GET: Posts/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Category)
                .Include(p => p.NextPost)
                .Include(p => p.PreviousPost)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }
            post.DisplayCount = post.DisplayCount + 1;
            _context.Update(post);
            _context.SaveChanges();
            return View(post);
        }

        [Authorize]
        // GET: Posts/Create
        public IActionResult Create()
        {
            string authorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<Post> posts = _context.Posts.Where(p => p.AuthorId == authorId).OrderBy(p => p.CreatedAt).ToList();

            ViewData["CategoryId"] = new SelectList(_context.Categories, "id", "Name");
            ViewData["TopCategoryId"] = new SelectList(_context.TopCategories, "id", "Name");
            ViewData["NextPostId"] = new SelectList(posts, "Id", "Title");
            ViewData["PreviousPostId"] = new SelectList(posts, "Id", "Title");
            ViewData["AuthorId"] = authorId;
            return View();
        }


        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,CategoryId,PreviousPostId,NextPostId,Tags,Content,AuthorId,FormImage")] Post post)
        {
            Post otherPost;
            MemoryStream memoryStream;

            post.CreatedAt = DateTime.Now;
            if (ModelState.IsValid)
            {
                if (post.FormImage != null)
                {
                    memoryStream = new MemoryStream();
                    post.FormImage.CopyTo(memoryStream);
                    post.Image = memoryStream.ToArray();

                }
                _context.Add(post);
                await _context.SaveChangesAsync();
                if (post.PreviousPostId != null)
                {
                    otherPost = _context.Posts.Find(post.PreviousPostId);
                    otherPost.NextPostId = post.Id;
                    _context.Update(otherPost);
                    _context.SaveChanges();
                } 
                if (post.NextPostId != null)
                {
                    otherPost = _context.Posts.Find(post.NextPostId);
                    otherPost.PreviousPostId = post.Id;
                    _context.Update(otherPost);
                    _context.SaveChanges();
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "id", "Name", post.CategoryId);
            ViewData["NextPostId"] = new SelectList(_context.Posts, "Id", "Content", post.NextPostId);
            ViewData["PreviousPostId"] = new SelectList(_context.Posts, "Id", "Content", post.PreviousPostId);
            return View(post);
        }

        // GET: Posts/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            if (post.AuthorId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return Unauthorized();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "id", "Name", post.CategoryId);
            ViewData["NextPostId"] = new SelectList(_context.Posts, "Id", "Content", post.NextPostId);
            ViewData["PreviousPostId"] = new SelectList(_context.Posts, "Id", "Content", post.PreviousPostId);
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Title,CreatedAt,CategoryId,PreviousPostId,NextPostId,Likes,DisplayCount,Tags,Content,Deleted")] Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            if (post.AuthorId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return Unauthorized();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "id", "Name", post.CategoryId);
            ViewData["NextPostId"] = new SelectList(_context.Posts, "Id", "Content", post.NextPostId);
            ViewData["PreviousPostId"] = new SelectList(_context.Posts, "Id", "Content", post.PreviousPostId);
            return View(post);
        }

        // GET: Posts/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Category)
                .Include(p => p.NextPost)
                .Include(p => p.PreviousPost)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }
            if (post.AuthorId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return Unauthorized();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.Posts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Posts'  is null.");
            }
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                post.Deleted = true;
                _context.Update(post);
                _context.SaveChanges();
                //_context.Posts.Remove(post);
            }
            if (post.AuthorId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return Unauthorized();
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(long id)
        {
          return _context.Posts.Any(e => e.Id == id);
        }

        [Authorize]
        public long Likes(long? id)
        {
            Post post = _context.Posts.Where(p => p.Id == id).FirstOrDefault();
               
            post.Likes = post.Likes + 1;
            _context.Update(post);
            _context.SaveChanges();
            return post.Likes;
            
        }

    }
}
