using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Veille_Technologique.Data;
using Veille_Technologique.Models;

namespace Veille_Technologique.Pages
{
    [Authorize]
    public class DiscardedModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public List<Article> Discarded = new();
        public DiscardedModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public void OnGet()
        {
            if (TempData["search"] != null)
                Discarded = _db.Articles.Where(x => x.State == ArticleState.Discarded)
                                       .Where(x => x.Title.Contains(Convert.ToString(TempData["search"])))
                                       .ToList();
            else
                Discarded = _db.Articles.Where(x => x.State == ArticleState.Discarded).ToList();
        }

        public IActionResult OnPostRestore(int id)
        {
            Article? art = _db.Articles.Find(id);
            art.State = ArticleState.Saved;
            _db.Articles.Update(art);
            _db.SaveChanges();

            return RedirectToAction("Get");
        }

        public IActionResult OnPostSearch(string search)
        {
            TempData["search"] = search;
            return RedirectToAction("Get");
        }
    }
}
