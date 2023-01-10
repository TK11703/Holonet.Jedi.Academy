using Holonet.Jedi.Academy.Entities.App;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Xml.Linq;

namespace Holonet.Jedi.Academy.BL.Data
{
    public static class DbInitializer
    {
        public async static Task InitializeAsync(AcademyContext context)
        {
            if (!context.Planets.Any())
            {

                var planets = new Planet[]
                {
                    new Planet{Name="Coruscant"},
                    new Planet{Name="Tatooine"},
                    new Planet{Name="Dantooine"},
                    new Planet{Name="Dagobah"},
                    new Planet{Name="Naboo"},
                    new Planet{Name="Kashyyyk"},
                    new Planet{Name="Alderaan"},
                    new Planet{Name="Geonosis"},
                    new Planet{Name="Kamino"},
                    new Planet{Name="Jedha"},
                    new Planet{Name="Hoth"},
                    new Planet{Name="Bespin"},
                    new Planet{Name="Mustafar"},
                    new Planet{Name="Jakku"},
                    new Planet{Name="Scarif"},
                    new Planet{Name="Yavin 4"},
                    new Planet{Name="Batuu"},
                    new Planet{Name="Bogano"},
					new Planet{Name="Ithor"},
					new Planet{Name="Onderon"},
					new Planet{Name="Tython"},
					new Planet{Name="Trandosha"}
				};

                context.Planets.AddRange(planets);
                await context.SaveChangesAsync();
            }

            if (!context.AlienRaces.Any())
            {

                var species = new Species[]
                {
                    new Species{Name="Jawa"},
                    new Species{Name="Ewok"},
                    new Species{Name="Wookie"},
                    new Species{Name="Gungan"},
                    new Species{Name="Twi'lek"},
                    new Species{Name="Rodian"},
                    new Species{Name="Neimoidian"},
                    new Species{Name="Iktochi"},
                    new Species{Name="Keshiri"},
                    new Species{Name="Trandoshan"},
                    new Species{Name="Geonosian"},
                    new Species{Name="Kaminoan"},
                    new Species{Name="Bothan"},
                    new Species{Name="Mon Calamari"},
                    new Species{Name="Zabraks"},
                    new Species{Name="Chiss"},
                    new Species{Name="Hutt"},
                    new Species{Name="Human"},
					new Species{Name="Ithorian"},
					new Species{Name="Trandoshan"}
				};

                context.AlienRaces.AddRange(species);
                await context.SaveChangesAsync();
            }

            if (!context.Ranks.Any())
            {

                var ranks = new Rank[]
                {
                    new Rank{Name="Initiate", RankLevel=1, Minimum=0, Maximum=1000},
                    new Rank{Name="Youngling", RankLevel=2, Minimum=1001, Maximum=2000},
                    new Rank{Name="Padawan", RankLevel=3, Minimum=2001, Maximum=3000},
                    new Rank{Name="Jedi Apprentice", RankLevel=4, Minimum=3001, Maximum=4000},
                    new Rank{Name="Jedi", RankLevel=5, Minimum=4001, Maximum=5000},
                    new Rank{Name="Jedi Knight", RankLevel=6, Minimum=5001, Maximum=6000},
                    new Rank{Name="Jedi General", RankLevel=7, Minimum=6001, Maximum=7000},
                    new Rank{Name="Jedi Master", RankLevel=8, Minimum=7001, Maximum=10000},
                    new Rank{Name="Jedi Council Member", RankLevel=9, Minimum=10001, Maximum=20000},
                    new Rank{Name="Jedi Grand Master", RankLevel=10, Minimum=20001, Maximum=30000}
                };

                context.Ranks.AddRange(ranks);
                await context.SaveChangesAsync();
            }

            if (!context.ForcePowers.Any())
            {

                var forcePowers = new ForcePower[]
                {
                    new ForcePower{Name="Animal Friendship"},
                    new ForcePower{Name="Cure Disease"},
                    new ForcePower{Name="Cure Poison"},
                    new ForcePower{Name="Droid Disable"},
                    new ForcePower{Name="Blinding"},
                    new ForcePower{Name="Enlightenment"},
                    new ForcePower{Name="Healing"},
                    new ForcePower{Name="Stun"},
                    new ForcePower{Name="Persuasion"},
                    new ForcePower{Name="Projection"},
                    new ForcePower{Name="Push"},
                    new ForcePower{Name="Pull"},
                    new ForcePower{Name="Revitalize"},
                    new ForcePower{Name="Shatterpoint"},
                    new ForcePower{Name="Telekinesis"}

                };

                context.ForcePowers.AddRange(forcePowers);
                await context.SaveChangesAsync();
            }

            if (!context.TerminationReasons.Any())
            {

                var terminationReasons = new TerminationReason[]
                {
                    new TerminationReason{Name="Death"},
                    new TerminationReason{Name="Dark Side User"},
                    new TerminationReason{Name="Attachment"},
                    new TerminationReason{Name="Possession"},
                    new TerminationReason{Name="Crime"},
                    new TerminationReason{Name="Health"},
                    new TerminationReason{Name="Free Will"}

                };

                context.TerminationReasons.AddRange(terminationReasons);
                await context.SaveChangesAsync();
            }
        }
    }
}
