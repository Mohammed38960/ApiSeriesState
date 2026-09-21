using System.Linq; // import de la bibliothèque pour manipulation collections, liste... concernant les données
using System.Threading.Tasks;
using APIRESTSTATE.Controllers;
using APIRESTSTATE.Models.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace APIRESTSTATE_Tests
{
    public class SerieControllerTests
    {
        private readonly SerieDbContext _context;
        private readonly SerieController _controller;

        // 1. Le constructeur (initialisation du contexte et du contrôleur)
        public SerieControllerTests()
        {
            var builder = new DbContextOptionsBuilder<SerieDbContext>()
                .UseNpgsql("Host=localhost;Database=SerieDB;Username=postgres;Password=admin");

            _context = new SerieDbContext(builder.Options);
            _controller = new SerieController(_context);
        }

        // 2. Ton premier test unitaire
        [Fact]
        public async Task GetSeries() // méthode getseries
        {
            // On appelle la méthode
            var result = await _controller.GetSeries(); // appeller getseries pour récuperer toutes les séries avec un tps d'attente qui correspond à l'interrogation du controleur. stockage dans var 

            var listSeriesRecuperees = result.Value.Where(s => s.Serieid <= 3).ToList(); // result renvoyé par l'API après requête
                                                                                         // Value extrait ce qu'l y a dans result
                                                                                         // filtrage linq qui parcourt chaque série
                                                                                         // qui regarde si chaque id  <=3
                                                                                         // si oui prend le flux de données et 
                                                                                         // le transforme en données exploitable (liste)

            // On vérifie le résultat
            Assert.NotNull(listSeriesRecuperees); // test si variable est nulle
            Assert.Equal(3, listSeriesRecuperees.Count); // test si le nombre d'éléments récupérés est bien égale à 3
        }



        [Fact]
        public async Task GetSerie_WhenSerieExists() // // méthode getserie
        {
            int serieIdTest = 1; // choisir l'id 1 qu'on stock dans int serieIdTest pour aller l'a chercher et tester notre getserie

            var result = await _controller.GetSerie(serieIdTest); // appeller getserie et l'appliquer à id 1 avec un tps d'attente qui correspond à l'interrogation du controleur. stockage dans var 

           
            Assert.NotNull(result.Value); // test si variable est nulle
            Assert.Equal(serieIdTest, result.Value.Serieid); // test si valeur id de la serie contenue dans resultat et renvoyée correspond bien à celle demandée
        }

        [Fact]
        public async Task GetSerie_WhenSerieDoesNotExist()  // méthode getserie notfound
        {
            int serieIdTest = 9999; // choisir l'id 9999 qu'on stock dans int serieIdTest pour aller l'a chercher et tester notre getserie

            var result = await _controller.GetSerie(serieIdTest); // appeller getserie et l'appliquer à id 1 avec un tps d'attente qui correspond à l'interrogation du controleur. stockage dans var 

            
            Assert.IsType<NotFoundResult>(result.Result); // test que le résultat renvoyé est bien un NotFound
        }

        [Fact]
        public async Task DeleteSerie_WhenSerieExists() // méthode deleteserie
        {
            int serieIdTest = 37; // choisir l'id 1 qu'on stock dans int serieIdTest pour aller la supprimer

            var result = await _controller.DeleteSerie(serieIdTest); // appeler deleteserie pour supprimer la série avec un tps d'attente

            Assert.IsType<NoContentResult>(result); // test que la suppression a bien réussi et renvoie un statut de succès sans contenu
        }

        [Fact]
        public async Task DeleteSerie_WhenSerieDoesNotExist() // méthode delete serie notfound
        {
            int serieIdTest = 9999; // choisir l'id 9999 qu'on stock dans int serieIdTest pour essayer de supprimer une série inexistante

            var result = await _controller.DeleteSerie(serieIdTest); // appeler deleteserie avec l'id 9999 avec un tps d'attente

            Assert.IsType<NotFoundResult>(result); // test que le résultat renvoyé est bien un NotFound car la série n'existe pas
        }


        [Fact]
        public async Task PostSerie_WhenTitleIsMissing()
        {
            
            var serieInvalide = new Serie // déclaration nouvelle série dans var serieInavlide 
            {
                Titre = null, // omission volontaire du titre
                Resume = "Résumé de test sans titre" // remplissage synopsis série
            };

            
            await Assert.ThrowsAsync<DbUpdateException>(async () => // fct qui sert à pieger et à contenir l'erreur appellée
                                                                     // type derreur attendu 
                                                                     // interception erreur
                                                                     // temps d'attente pour execution async
            {
                await _controller.PostSerie(serieInvalide); // appeler postserie sur serieInvalide avec un tps d'attente concernant le controleur
            });
        }

        [Fact]
        public async Task PutSerie_WhenTitleIsMissing()
        {
           
            int serieIdTest = 38; // choisir l'id 1 qu'on stock dans int serieIdTest pour aller l'a modifier
            var serieInvalide = new Serie // déclaration nouvelle série dans var serieInavlide
            {
                Serieid = serieIdTest, 
                Titre = null, // omission volontaire du titre
                Resume = "Résumé modifié sans titre" // remplissage synopsis série
            };

            
            await Assert.ThrowsAsync<DbUpdateException>(async () => // fct qui sert à pieger et à contenir l'erreur appellée
                                                                     // type derreur attendu 
                                                                     // interception erreur
                                                                     // temps d'attente pour execution async
            {
                await _controller.PutSerie(serieIdTest, serieInvalide); // appeler putserie sur serieIdTest et serieInvalide et avec un tps d'attente convcernant le controleur
            });
        }


    }
}