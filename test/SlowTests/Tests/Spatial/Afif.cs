using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FastTests;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Queries.Facets;
using Raven.Client.Documents.Session;
using Raven.Client.Extensions;
using Raven.Server.Utils;
using SlowTests.Utils.Attributes;
using Xunit;

namespace SlowTests.Tests.Spatial
{
    public class Afif
    {
        private class ByVehicle : AbstractIndexCreationTask<Vehicle>
        {
            public ByVehicle()
            {
                Map = vehicles => from vehicle in vehicles
                                  select new
                                  {
                                      vehicle.Model,
                                      vehicle.Make,
                                      Coordinates = CreateSpatialField(vehicle.Latitude, vehicle.Longitude)
                                  };
            }
        }

        private class Vehicle
        {
            public string Id { get; set; }
            public string Model { get; set; }
            public string Make { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }

        private class HawthornEast : Location
        {
            public HawthornEast()
                : base(longitude: 145.052097, latitude: -37.834855)
            { }
        }

        private class Darwin : Location
        {
            public Darwin()
                : base(longitude: 130.841904, latitude: 12.461334)
            { }
        }

        private class Location
        {
            public Location(double longitude, double latitude)
            {
                Longitude = longitude;
                Latitude = latitude;
            }

            public double Latitude { get; private set; }
            public double Longitude { get; private set; }
        }

        public class CanGetFacetsOnVehicleSpatialSearch : RavenTestBase
        {
            private List<Vehicle> Vehicles { get; set; }

            private readonly IDocumentStore _store;

            public CanGetFacetsOnVehicleSpatialSearch()
            {
                _store = GetDocumentStore();

                Vehicles = new List<Vehicle>();
                for (int i = 0; i < 3; i++)
                {
                    Vehicles.Add(new Vehicle
                    {
                        Make = "Mazda",
                        Model = "Rx8",
                        Latitude = new Darwin().Latitude,
                        Longitude = new Darwin().Longitude
                    });
                }

                for (int i = 0; i < 3; i++)
                {
                    Vehicles.Add(new Vehicle
                    {
                        Make = "Mercedes",
                        Model = "AMG",
                        Latitude = new Darwin().Latitude,
                        Longitude = new Darwin().Longitude
                    });
                }

                for (int i = 0; i < 4; i++)
                {
                    Vehicles.Add(new Vehicle
                    {
                        Make = "Toyota",
                        Model = "Camry",
                        Latitude = new HawthornEast().Latitude,
                        Longitude = new HawthornEast().Longitude
                    });
                }

                using (var session = _store.OpenSession())
                {
                    session.Store(new FacetSetup
                    {
                        Id = "facets/Vehicle",
                        Facets = new List<Facet> { new Facet { Name = "Make" }, new Facet { Name = "Model" } }
                    });
                    new ByVehicle().Execute(session.Advanced.DocumentStore);
                    session.SaveChanges();

                    foreach (var vehicle in Vehicles)
                        session.Store(vehicle);
                    session.SaveChanges();

                    QueryStatistics stats;
                    session
                        .Query<Vehicle, ByVehicle>()
                        .Statistics(out stats)
                        .Customize(x => x.WaitForNonStaleResults())
                        .Where(x => x.Make == "Mazda")
                        .ToList();
                }
            }

            [Theory]
            [CriticalCultures]
            public void ShouldMatchMakeFacetsOnLocation(CultureInfo criticalCulture)
            {
                using (CultureHelper.EnsureCulture(criticalCulture))
                using (_store.OpenSession())
                {
                    var index = typeof(ByVehicle).Name;

                    var value = _store.Operations.Send(new GetFacetsOperation(new FacetQuery
                    {
                        Query = $"FROM INDEX '{index}' WHERE within(Coordinates, circle(5, {new Darwin().Latitude.ToInvariantString()}, {new Darwin().Longitude.ToInvariantString()}))",
                        FacetSetupDoc = "facets/Vehicle"
                    }));

                    Assert.NotNull(value);
                    Assert.Equal(2, value.Results["Make"].Values.Count());
                }
            }
        }
    }
}