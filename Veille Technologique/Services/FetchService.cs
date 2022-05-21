using HtmlAgilityPack;
using System.Net;
using System.Net.Http.Headers;
using Veille_Technologique.Data;
using Veille_Technologique.Models;

namespace Veille_Technologique.Services
{
	public static class FetchService
	{
		public static async Task<bool> Initiate(ApplicationDbContext db)
		{
			string url = "https://medium.com/tag/programming/latest";
			var response = FetchData(url).Result;
			List<Article> links = ParseData(response);
			

			return await AddDataToDb(db, links);
		}
		private static async Task<string> FetchData(string fullUrl)
		{
			HttpClient client = new HttpClient();
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
			client.DefaultRequestHeaders.Accept.Clear();
			client.DefaultRequestHeaders.Add("connection", "Keep-Alive");
			client.DefaultRequestHeaders.Add("User-Agent", "PostmanRuntime/7.29.0");
			var response = client.GetStringAsync(fullUrl);
			return await response;
		}

		private static List<Article> ParseData(string response)
		{
			List<Article> data = new List<Article>();

			HtmlDocument htmlDoc = new HtmlDocument();

			htmlDoc.LoadHtml(response);

			var programmerLinks = htmlDoc.DocumentNode.Descendants("a").ToList();

			foreach (var link in programmerLinks)
			{
				string l = "https://medium.com" + link.Attributes["href"].Value;

				if (link.Attributes.Contains("aria-label"))
				{
					var div = link.ChildNodes[0].ChildNodes[0];
					string title = div.InnerText;
					if(link.Attributes["aria-label"].Value == "Post Preview Title")
						data.Add(new Article { Url = l, Title = title });
				}
			}

			return data;
		}

		private static async Task<bool> AddDataToDb(ApplicationDbContext db, List<Article> articles)
		{
			foreach (var article in articles)
			{
				if (db.Articles.Any(x => x.Url == article.Url))
					continue;
				db.Articles.Add(article);
			}
			db.SaveChanges();
			return db.SaveChangesAsync().IsCompletedSuccessfully;
		}
	}
}
