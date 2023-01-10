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
                    new ForcePower{Name="Animal Kinship", MinimumRankId=3, Description="An ability of the Force that allows its user to connect mentally with animals."},
                    new ForcePower{Name="Cure Disease", MinimumRankId=7, Description="A variant of Force healing that can cure diseases afflicting living things."},
                    new ForcePower{Name="Cure Poison", MinimumRankId=4, Description="A variant of Force healing that can remove or detoxify poisons afflicting living things."},
                    new ForcePower{Name="Droid Disable", MinimumRankId=3, Description="A Force power developed during the Old Sith Wars that allows a Jedi to overload and damage electronic systems, such as droids."},
                    new ForcePower{Name="Blinding", MinimumRankId=7, Description = "Produces a burst of Force energy that overwhelms a target's optic nerves, causes temporary blindness."},
                    new ForcePower{Name="Enlightenment", MinimumRankId=6, Description = "A passive/supplementary Force power that took the Force powers a Jedi was most skilled in, pushing them to the highest degree that the Jedi had previously mastered during his or her routine training."},
                    new ForcePower{Name="Healing", MinimumRankId=5, Description = "A power that used the Force to accelerate the natural healing process rapidly and can be used to heal the most fatal of wounds and injuries."},
                    new ForcePower{Name="Stun", MinimumRankId=5, Description = "A Force power that can temporarily deaden the senses and perceptions of a targeted enemy, preventing most movements."},
                    new ForcePower{Name="Persuasion", MinimumRankId=3, Description = "The use of the Force to exert influence."},
                    new ForcePower{Name="Projection", MinimumRankId=5, Description = "A mysterious Force ability (possibly a variation of Force illusion), enabling the user to create an apparition similar to themselves to distract, confuse or lure enemies."},
                    new ForcePower{Name="Push", MinimumRankId=1, Description = "A telekinetic ability using the Force to push objects away from the user."},
                    new ForcePower{Name="Pull", MinimumRankId=1, Description = "A telekinetic ability using the Force that can cause a material body to draw close to the user. The greater the user's aptitude with this, the heavier the object that could be pulled."},
                    new ForcePower{Name="Revitalize", MinimumRankId=8, Description = " A Force technique that can revitalize an exhausted, wounded or, unconscious user, or whoever the user directed it at."},
                    new ForcePower{Name="Shatterpoint", MinimumRankId=9, Description = "A Force ability that can be used to sense the significance of an event, though it also referred to key moments where actions could change events."},
                    new ForcePower{Name="Telekinesis", MinimumRankId=5, Description = "The ability to move and otherwise manipulate physical matter in a variety of ways, all while using the power of the Force. "},
					new ForcePower{Name="Deflection", MinimumRankId=6, Description = "A Force power used to deflect incoming attacks or blaster bolts, when a lightsaber was not available to perform the action."},
                    new ForcePower{Name="Psychometry", MinimumRankId=4, Description = "Allowed the user to sense the events associated with an object or location."}
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
