namespace MakeEasy.RestClient.Test;

using Microsoft.AspNetCore.Hosting.Server;
using System.Reflection;
using System.Runtime.Loader;

[TestClass]
public sealed class TestRestUtils
{
    [TestMethod]
    public void TestQueryParams()
    {
        var absoluteUrl = RestUtils.BuildQueryUrl($"/Person/Find", new { Name = "Mary" });
        Assert.AreEqual(absoluteUrl, "/Person/Find?Name=Mary");

        absoluteUrl = RestUtils.BuildQueryUrl($"/Person/Find", new { Name = "Mary", Age = 25 });
        Assert.AreEqual(absoluteUrl, "/Person/Find?Name=Mary&Age=25");

        absoluteUrl = RestUtils.BuildQueryUrl($"/Person/Find", new { Name = new string[] { "Mary", "Jone" }, Age = 25 });
        Assert.AreEqual(RestUtils.DecodeUrl(absoluteUrl), "/Person/Find?Name=Mary,Jone&Age=25");

        absoluteUrl = RestUtils.BuildQueryUrl($"/Person/Find", "Name", "Mary");
        Assert.AreEqual(RestUtils.DecodeUrl(absoluteUrl), "/Person/Find?Name=Mary");

        absoluteUrl = RestUtils.BuildQueryUrl($"/Person/Find?Name=Mary", "Age", "25");
        Assert.AreEqual(RestUtils.DecodeUrl(absoluteUrl), "/Person/Find?Name=Mary&Age=25");
    }

    [TestMethod]
    public void TestSegmentParams()
    {
        var absoluteUrl = RestUtils.BuildSegmentUrl("/Person/Name/{Name}/Age/{Age}", new { Name = "Mary", Age=25 });
        Assert.AreEqual(absoluteUrl, "/Person/Name/Mary/Age/25");
    }
}
