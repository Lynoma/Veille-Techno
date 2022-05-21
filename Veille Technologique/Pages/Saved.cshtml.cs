using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Veille_Technologique.Data;
using Veille_Technologique.Models;

namespace Veille_Technologique.Pages
{
    [Authorize]
    public class SavedModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public List<Article> Saved = new();
        public SavedModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public void OnGet()
        {
            if (TempData["search"] != null)
                Saved = _db.Articles.Where(x => x.State == ArticleState.Saved)
                                       .Where(x => x.Title.Contains(Convert.ToString(TempData["search"])))
                                       .ToList();
            else
                Saved = _db.Articles.Where(x => x.State == ArticleState.Saved).ToList();
        }

        public IActionResult OnPostDiscard(int id)
        {
            Article? art = _db.Articles.Find(id);
            art.State = ArticleState.Discarded;
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
