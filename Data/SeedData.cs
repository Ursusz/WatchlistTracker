namespace Watchlist_Tracker.Data;

using Watchlist_Tracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<AppDbContext>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        context.Database.Migrate();

        string[] roleNames = ["Admin", "User"];
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
                await roleManager.CreateAsync(new IdentityRole(roleName));
        }

        var adminEmail = "admin@watchlist.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            var admin = new ApplicationUser
            {
                UserName = "admin",
                Email = adminEmail,
                FullName = "Administrator",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(admin, "ciscopa55");
            if (result.Succeeded)
                await userManager.AddToRoleAsync(admin, "Admin");
        }

        if (!context.Categories.Any())
        {
            var drama = new Category { Name = "Drama" };
            var comedy = new Category { Name = "Comedy" };
            var action = new Category { Name = "Action" };
            var sciFi = new Category { Name = "Sci-Fi" };
            var romance = new Category { Name = "Romance" };
            var fantasy = new Category { Name = "Fantasy" };
            var thriller = new Category { Name = "Thriller" };
            var horror = new Category { Name = "Horror" };

            context.Categories.AddRange(drama, comedy, action, sciFi, romance, fantasy, thriller, horror);
            await context.SaveChangesAsync();

            var admin = await userManager.FindByEmailAsync(adminEmail);
            var authorId = admin?.Id;

            context.Movies.AddRange(
                new Movie { Title = "The Godfather", Description = "O epopee despre crima organizată și familia Corleone.", PublishedAt = new DateTime(1972, 3, 24), CategoryId = drama.Id, ImagePath = "https://image.tmdb.org/t/p/w500/3bhkrj08vU3pSAsS969O9oqys.jpg" },
                new Movie { Title = "Forrest Gump", Description = "Viața unui om simplu care participă fără să vrea la momente istorice.", PublishedAt = new DateTime(1994, 7, 6), CategoryId = drama.Id, ImagePath = "https://image.tmdb.org/t/p/w500/ar9ruisS7SAsS969O9oqysOEJu.jpg" },
                new Movie { Title = "Schindler's List", Description = "Povestea adevărată a unui industriaș care salvează mii de evrei.", PublishedAt = new DateTime(1993, 11, 30), CategoryId = drama.Id, ImagePath = "https://image.tmdb.org/t/p/w500/sF1U49YjYALAFznT0m9Sfs0oNm9.jpg" },
                new Movie { Title = "The Green Mile", Description = "Viața gardienilor de pe culoarul morții se schimbă când întâlnesc un condamnat special.", PublishedAt = new DateTime(1999, 12, 10), CategoryId = drama.Id, ImagePath = "https://image.tmdb.org/t/p/w500/velWPhVMvTQKcxgl6YvYm7O11jH.jpg" },
                new Movie { Title = "Pulp Fiction", Description = "Povești interconectate despre violență și mântuire în Los Angeles.", PublishedAt = new DateTime(1994, 9, 10), CategoryId = drama.Id, ImagePath = "https://image.tmdb.org/t/p/w500/fIE33mllmB7SniS46TKVZ91rXqy.jpg" },
                new Movie { Title = "Goodfellas", Description = "Ascensiunea și prăbușirea unui gangster în rândurile mafiei.", PublishedAt = new DateTime(1990, 9, 12), CategoryId = drama.Id, ImagePath = "https://image.tmdb.org/t/p/w500/aKuFiU8tSAtSgpt6fS0EE1Zp4Pk.jpg" },
                new Movie { Title = "The Prestige", Description = "Doi magicieni se întrec într-o rivalitate obsesivă și fatală.", PublishedAt = new DateTime(2006, 10, 19), CategoryId = drama.Id, ImagePath = "https://image.tmdb.org/t/p/w500/8Y7SAsS969O9oqysOEJuO6nk.jpg" },
                new Movie { Title = "Fight Club", Description = "Un insomniac și un fabricant de săpun formează un club de luptă subteran.", PublishedAt = new DateTime(1999, 10, 15), CategoryId = drama.Id, ImagePath = "https://image.tmdb.org/t/p/w500/pB8SOf6m3W1j7H3u7O8XqIek89S.jpg" },

                new Movie { Title = "Die Hard", Description = "Un polițist trebuie să salveze o clădire de teroriști în Ajunul Crăciunului.", PublishedAt = new DateTime(1988, 7, 15), CategoryId = action.Id, ImagePath = "https://image.tmdb.org/t/p/w500/a9S7SAsS969O9oqysOEJuO6n.jpg" },
                new Movie { Title = "John Wick", Description = "Un fost asasin revine în activitate după ce îi este furată mașina și ucis cățelul.", PublishedAt = new DateTime(2014, 10, 24), CategoryId = action.Id, ImagePath = "https://image.tmdb.org/t/p/w500/h6Y9S7SAsS969O9oqysOEJuO6.jpg" },
                new Movie { Title = "Top Gun: Maverick", Description = "Maverick se întoarce să antreneze o nouă generație de piloți.", PublishedAt = new DateTime(2022, 5, 24), CategoryId = action.Id, ImagePath = "https://image.tmdb.org/t/p/w500/628vKyR1uSAtSgpt6fS0EE1Zp4Pk.jpg" },
                new Movie { Title = "Gladiator II", Description = "Continuarea legendarei lupte pentru libertate în arena Romei.", PublishedAt = new DateTime(2024, 11, 22), CategoryId = action.Id, ImagePath = "https://image.tmdb.org/t/p/w500/ty8TGRS7SAsS969O9oqysOEJuO.jpg" },
                new Movie { Title = "Heat", Description = "Un joc de-a șoarecele și pisica între un detectiv și un hoț profesionist.", PublishedAt = new DateTime(1995, 12, 15), CategoryId = action.Id, ImagePath = "https://image.tmdb.org/t/p/w500/uY7SAsS969O9oqysOEJuO6n.jpg" },
                new Movie { Title = "Terminator 2", Description = "Un cyborg este trimis înapoi în timp pentru a proteja un copil.", PublishedAt = new DateTime(1991, 7, 1), CategoryId = action.Id, ImagePath = "https://image.tmdb.org/t/p/w500/we9S7SAsS969O9oqysOEJuO6nk.jpg" },
                new Movie { Title = "Mission: Impossible - Fallout", Description = "Ethan Hunt trebuie să oprească un atac nuclear după o misiune eșuată.", PublishedAt = new DateTime(2018, 7, 27), CategoryId = action.Id, ImagePath = "https://image.tmdb.org/t/p/w500/Ak9S7SAsS969O9oqysOEJuO6.jpg" },
                new Movie { Title = "Braveheart", Description = "William Wallace conduce rebelii scoțieni împotriva tiraniei engleze.", PublishedAt = new DateTime(1995, 5, 24), CategoryId = action.Id, ImagePath = "https://image.tmdb.org/t/p/w500/v9S7SAsS969O9oqysOEJuO6n.jpg" },

                new Movie { Title = "Blade Runner 2049", Description = "Un vânător de replici descoperă un secret care poate schimba totul.", PublishedAt = new DateTime(2017, 10, 6), CategoryId = sciFi.Id, ImagePath = "https://image.tmdb.org/t/p/w500/gEU2S7SAsS969O9oqysOEJuO6n.jpg" },
                new Movie { Title = "Dune: Part Two", Description = "Paul Atreides pornește într-un război sfânt pe planeta deșertului.", PublishedAt = new DateTime(2024, 3, 1), CategoryId = sciFi.Id, ImagePath = "https://image.tmdb.org/t/p/w500/8Y7SAsS969O9oqysOEJuO6nk.jpg" },
                new Movie { Title = "Arrival", Description = "O lingvistă încearcă să comunice cu vizitatori extratereștri.", PublishedAt = new DateTime(2016, 11, 11), CategoryId = sciFi.Id, ImagePath = "https://image.tmdb.org/t/p/w500/k9S7SAsS969O9oqysOEJuO6nk.jpg" },
                new Movie { Title = "Aliens", Description = "Ellen Ripley se întoarce pe planeta unde a început coșmarul.", PublishedAt = new DateTime(1986, 7, 18), CategoryId = sciFi.Id, ImagePath = "https://image.tmdb.org/t/p/w500/r9S7SAsS969O9oqysOEJuO6n.jpg" },
                new Movie { Title = "The Martian", Description = "Un astronaut rămâne singur pe Marte și trebuie să supraviețuiască.", PublishedAt = new DateTime(2015, 10, 2), CategoryId = sciFi.Id, ImagePath = "https://image.tmdb.org/t/p/w500/m9S7SAsS969O9oqysOEJuO6n.jpg" },
                new Movie { Title = "Back to the Future", Description = "Un adolescent călătorește accidental în 1955 cu un DeLorean.", PublishedAt = new DateTime(1985, 7, 3), CategoryId = sciFi.Id, ImagePath = "https://image.tmdb.org/t/p/w500/f9S7SAsS969O9oqysOEJuO6nk.jpg" },
                new Movie { Title = "Avatar", Description = "Un soldat paralizat pătrunde în cultura extratereștrilor Na'vi.", PublishedAt = new DateTime(2009, 12, 18), CategoryId = sciFi.Id, ImagePath = "https://image.tmdb.org/t/p/w500/628vKyR1uSAtSgpt6fS0EE1Zp4Pk.jpg" },
                new Movie { Title = "Ex Machina", Description = "Un programator participă la un test de inteligență artificială revoluționar.", PublishedAt = new DateTime(2014, 12, 15), CategoryId = sciFi.Id, ImagePath = "https://image.tmdb.org/t/p/w500/b9S7SAsS969O9oqysOEJuO6nk.jpg" },

                new Movie { Title = "Deadpool", Description = "Un mercenar impertinent caută răzbunare într-un mod sângeros și amuzant.", PublishedAt = new DateTime(2016, 2, 12), CategoryId = comedy.Id, ImagePath = "https://image.tmdb.org/t/p/w500/y9S7SAsS969O9oqysOEJuO6n.jpg" },
                new Movie { Title = "Hot Fuzz", Description = "Un polițist de elită este trimis într-un orășel aparent liniștit.", PublishedAt = new DateTime(2007, 2, 14), CategoryId = comedy.Id, ImagePath = "https://image.tmdb.org/t/p/w500/z9S7SAsS969O9oqysOEJuO6n.jpg" },
                new Movie { Title = "Mean Girls", Description = "O fată se infiltrează în grupul celor mai populare fete din liceu.", PublishedAt = new DateTime(2004, 4, 30), CategoryId = comedy.Id, ImagePath = "https://image.tmdb.org/t/p/w500/w9S7SAsS969O9oqysOEJuO6n.jpg" },
                new Movie { Title = "Borat", Description = "Un jurnalist din Kazahstan călătorește prin America.", PublishedAt = new DateTime(2006, 11, 3), CategoryId = comedy.Id, ImagePath = "https://image.tmdb.org/t/p/w500/x9S7SAsS969O9oqysOEJuO6n.jpg" },
                new Movie { Title = "Zoolander", Description = "Un fotomodel este manipulat pentru a comite o crimă politică.", PublishedAt = new DateTime(2001, 9, 28), CategoryId = comedy.Id, ImagePath = "https://image.tmdb.org/t/p/w500/v9S7SAsS969O9oqysOEJuO6n.jpg" },
                new Movie { Title = "Anchorman", Description = "Un prezentator TV se confruntă cu schimbările din anii '70.", PublishedAt = new DateTime(2004, 7, 9), CategoryId = comedy.Id, ImagePath = "https://image.tmdb.org/t/p/w500/t9S7SAsS969O9oqysOEJuO6n.jpg" },
                new Movie { Title = "Dumb and Dumber", Description = "Doi prieteni naivi pornesc într-o călătorie nebună prin țară.", PublishedAt = new DateTime(1994, 12, 16), CategoryId = comedy.Id, ImagePath = "https://image.tmdb.org/t/p/w500/u9S7SAsS969O9oqysOEJuO6n.jpg" },


                new Movie { Title = "Get Out", Description = "O vizită la părinții iubitei devine un coșmar rasial sinistru.", PublishedAt = new DateTime(2017, 2, 24), CategoryId = horror.Id, ImagePath = "https://image.tmdb.org/t/p/w500/q9S7SAsS969O9oqysOEJuO6n.jpg" },
                new Movie { Title = "It", Description = "Un grup de copii se confruntă cu un clovn demonic.", PublishedAt = new DateTime(2017, 9, 8), CategoryId = horror.Id, ImagePath = "https://image.tmdb.org/t/p/w500/p9S7SAsS969O9oqysOEJuO6n.jpg" },
                new Movie { Title = "A Quiet Place", Description = "O familie trebuie să trăiască în tăcere pentru a evita creaturile care vânează după sunet.", PublishedAt = new DateTime(2018, 4, 6), CategoryId = horror.Id, ImagePath = "https://image.tmdb.org/t/p/w500/o9S7SAsS969O9oqysOEJuO6n.jpg" },
                new Movie { Title = "The Exorcist", Description = "O mamă cere ajutorul a doi preoți pentru a-și salva fiica posedată.", PublishedAt = new DateTime(1973, 12, 26), CategoryId = horror.Id, ImagePath = "https://image.tmdb.org/t/p/w500/n9S7SAsS969O9oqysOEJuO6n.jpg" },
                new Movie { Title = "Halloween", Description = "Un criminal mascat se întoarce în orașul natal să ucidă.", PublishedAt = new DateTime(1978, 10, 25), CategoryId = horror.Id, ImagePath = "https://image.tmdb.org/t/p/w500/m9S7SAsS969O9oqysOEJuO6n.jpg" },
                new Movie { Title = "The Thing", Description = "Cercetători din Antarctica descoperă un extraterestru care ia forma umană.", PublishedAt = new DateTime(1982, 6, 25), CategoryId = horror.Id, ImagePath = "https://image.tmdb.org/t/p/w500/l9S7SAsS969O9oqysOEJuO6n.jpg" },
                new Movie { Title = "Scream", Description = "Un criminal pasionat de filme horror terorizează un grup de liceeni.", PublishedAt = new DateTime(1996, 12, 20), CategoryId = horror.Id, ImagePath = "https://image.tmdb.org/t/p/w500/k9S7SAsS969O9oqysOEJuO6n.jpg" },

                new Movie { Title = "La La Land", Description = "Un pianist și o aspirantă la actorie se îndrăgostesc în Los Angeles.", PublishedAt = new DateTime(2016, 12, 9), CategoryId = romance.Id, ImagePath = "https://image.tmdb.org/t/p/w500/j9S7SAsS969O9oqysOEJuO6n.jpg" },
                new Movie { Title = "About Time", Description = "Un tânăr descoperă că poate călători în timp și își caută dragostea.", PublishedAt = new DateTime(2013, 9, 4), CategoryId = romance.Id, ImagePath = "https://image.tmdb.org/t/p/w500/i9S7SAsS969O9oqysOEJuO6n.jpg" },
                new Movie { Title = "Pride & Prejudice", Description = "O poveste despre dragoste și prejudecăți în Anglia secolului 18.", PublishedAt = new DateTime(2005, 9, 16), CategoryId = romance.Id, ImagePath = "https://image.tmdb.org/t/p/w500/h9S7SAsS969O9oqysOEJuO6n.jpg" },
                new Movie { Title = "A Star is Born", Description = "Un muzician experimentat ajută o tânără cântăreață să ajungă celebră.", PublishedAt = new DateTime(2018, 10, 5), CategoryId = romance.Id, ImagePath = "https://image.tmdb.org/t/p/w500/g9S7SAsS969O9oqysOEJuO6n.jpg" }
            );

            await context.SaveChangesAsync();
        }
    }
}
