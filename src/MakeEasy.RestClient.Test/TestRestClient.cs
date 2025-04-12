namespace MakeEasy.RestClient.Test;

using MakeEasy.RestClient.WebApiServer;
using Microsoft.AspNetCore.Hosting.Server;
using System.Reflection;
using System.Runtime.Loader;

[TestClass]
public sealed class TestRestClient
{
    private static readonly string url = "http://localhost:12321";
    //private Server? server;

    //[TestInitialize]
    //public void Init()
    //{
    //    server = new Server();
    //    server.StartAsync().Wait();
    //}

    //[TestCleanup]
    //public void Cleanup()
    //{
    //    server?.StopAsync().Wait();
    //}

    [TestMethod]
    public async Task TestGetNonGeneric()
    {
        using var clientx = new RestClient($"http://qwerqwerdxx:12321");
        try {
            var responsex = await clientx.GetAsync("/Person/FindByName", new { name = "Mary" }).ConfigureAwait(false);
            Assert.Fail("Should not reach here");
        }
        catch (Exception ex) {
            Assert.IsNotNull(ex.InnerException);
        }

        using var client = new RestClient(url);
        var response = await client.GetAsync("/xx", new { name = "Mary" }).ConfigureAwait(false);
        Assert.AreEqual((int)response.StatusCode, 404);

        response = await client.GetAsync("/Person/FindByName", new { name = "xxx" }).ConfigureAwait(false);
        Assert.AreEqual((int)response.StatusCode, 204);

        response = await client.GetAsync("/Person/FindByName", new { name = "Mary" }).ConfigureAwait(false);
        Assert.IsTrue(response.IsSuccessStatusCode);
        var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        Assert.IsNotNull(content);
        var content2 = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        Assert.AreEqual(content, content2);

        response = await client.GetAsync("/Person/FindAll").ConfigureAwait(false);
        Assert.IsTrue(response.IsSuccessStatusCode);
    }

    [TestMethod]
    public async Task TestGetGeneric()
    {
        using var client = new RestClient(url);
        var person = await client.GetAsync<Person>("/Person/FindByName", new { name = "Mary" }).ConfigureAwait(false);
        Assert.AreEqual(person?.Name, "Mary");

        var personList = await client.GetAsync<List<Person>>("/Person/FindAll").ConfigureAwait(false);
        Assert.IsNotNull(personList);
        Assert.IsTrue(personList.Count > 1);
        Assert.IsTrue(personList.Any(p => p.Name == "Mary"));
    }

    [TestMethod]
    public void TestPost()
    {
        var client = new RestClient(url);
        var response = client.PostAsync("/Person/Create", new { name = "Mary", age = 25 }).Result;
        Assert.IsNotNull(response);
    }
}
