using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Xml.Linq;
using Holonet.Jedi.Academy.Entities.App;
using Holonet.Jedi.Academy.Entities.Charting;
using Holonet.Jedi.Academy.Entities.Configuration;
using Microsoft.AspNetCore.Hosting.Server;

namespace Holonet.Jedi.Academy.BL.Dashboards
{
	public class QuestDashboard
	{
		public SiteConfiguration Config { get; set; }
		public string UserTZOffset { get; set; }

		public QuestDashboard(SiteConfiguration config, string userTZOffset)
		{
			Config = config;
			UserTZOffset = userTZOffset;
		}

		public ChartResponse<decimal> GetQuestParticipation(List<QuestXP> questAcceptances, string chartTitle)
		{
			ChartResponse<decimal> response = new ChartResponse<decimal>("pie");
			response.title = chartTitle;
			List<string> colors = new List<string>();
			List<string> hoverColors = new List<string>();
			List<string> borderColors = new List<string>();
			Random rnd = new Random();
			Color baseColor = Color.Empty;
			var list = from quest in questAcceptances
					   group quest by quest.Status into grp
					   select new { key = grp.Key, total = grp.Count() }
					   ;
			PieGraph<decimal> dataset = new PieGraph<decimal>();
			foreach (var item in list)
			{
				if (!response.labels.Contains(item.key))
				{
					response.labels.Add(item.key);
					baseColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
					colors.Add(String.Format("rgba({0},{1},{2},{3})", baseColor.R, baseColor.G, baseColor.B, 0.5));
					hoverColors.Add(ColorTranslator.ToHtml(baseColor));
					borderColors.Add(ColorTranslator.ToHtml(Color.White));
				}
				dataset.data.Add(item.total);
			}

			dataset.SetBackgroundColors(colors);
			dataset.SetBorderColors(borderColors);
			dataset.SetHoverBackgroundColors(hoverColors);
			dataset.SetHoverBorderColors(borderColors);

			response.datasets.Add(dataset);
			return response;
		}

		public double GetQuestAvgCompletion(List<QuestXP> questAcceptances)
		{
			double avgRotations = 0;
			if (questAcceptances.Count() > 0)
			{
				avgRotations = questAcceptances.Where(x=>x.IsComplete).Average(x => (x.CompletedOn!.Value.Date - x.StartedOn.Date).TotalDays);
			}
			return avgRotations;
		}

		public string GetMostPopularPlanet(List<ObjectiveDestination> destinations)
		{
			string mostPopularPlanetName = string.Empty;
			var items = from destination in destinations
						where destination.Planet != null
					   group destination by destination.Planet!.Name into grp
					   select new { key = grp.Key, total = grp.Count() };
			if(items.Count() > 0)
			{
				mostPopularPlanetName = items.OrderByDescending(x => x.total).FirstOrDefault().key;
			}
			return mostPopularPlanetName;
		}
	}
}
