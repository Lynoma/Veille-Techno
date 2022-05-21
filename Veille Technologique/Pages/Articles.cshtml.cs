using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Veille_Technologique.Data;
using Veille_Technologique.Models;
using Veille_Technologique.Services;

namespace Veille_Technologique.Pages
{
    [Authorize]
    public class ArticlesModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public List<Article> Articles = new();

        public string? Message { get; set; }

        public ArticlesModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public void OnGet()
        {
            if(TempData["success"] != null)
                Message = Convert.ToString(TempData["success"]);
            if(TempData["search"] != null)
                Articles = _db.Articles.Where(x => x.State == ArticleState.ToRead)
                                       .Where(x => x.Title.Contains(Convert.ToString(TempData["search"])))
                                       .ToList();
            else
                Articles = _db.Articles.Where(x => x.State == ArticleState.ToRead).ToList();
        }

        public IActionResult OnPostSave(int id)
        {
            Article? art = _db.Articles.Find(id);
            art.State = ArticleState.Saved;
            _db.Articles.Update(art);
            _db.SaveChanges();

            TempData["success"] = "Lien sauvegardé";

            return RedirectToAction("Get");
        }

        public IActionResult OnPostDiscard(int id)
        {
            Article? art = _db.Articles.Find(id);
            art.State = ArticleState.Discarded;
            _db.Articles.Update(art);
            _db.SaveChanges();

            TempData["success"] = "Lien supprimé";

            return RedirectToAction("Get");
        }

        public async Task<IActionResult> OnPostFetch()
        {
            await FetchService.Initiate(_db);
            TempData["success"] = "Données récupérées avec succès";
            return RedirectToAction("Get");
        }

        public IActionResult OnPostSearch(string search)
        {
            TempData["search"] = search;
            return RedirectToAction("Get");
        }
    }
}
