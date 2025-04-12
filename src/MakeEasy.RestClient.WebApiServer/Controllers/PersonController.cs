namespace MakeEasy.RestClient.WebApiServer.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]/[action]")]
public class PersonController : ControllerBase
{
    [HttpGet]
    public Person? FindByName(string name)
    {
        if (name == "Mary") {
            return new Person(name, 25);
        }
        else if (name == "John") {
            return new Person(name, 35);
        }
        else if (name == "Jane") {
            return new Person(name, 28);
        }
        return null;
    }

    [HttpGet]
    public Person? FindByNameAndAge(string name, int age)
    {
        if (name == "Mary" && age == 25) {
            return new Person(name, 25);
        }
        else if (name == "John" && age == 35) {
            return new Person(name, 35);
        }
        else if (name == "Jane" && age == 28) {
            return new Person(name, 28);
        }
        return null;
    }

    [HttpGet]
    public IEnumerable<Person> FindAll()
    {
        return new List<Person> {
            new Person("Mary", 25),
            new Person("John", 35),
            new Person("Jane", 28)
        };
    }

    [HttpPost]
    public string Create([FromBody] Person person)
    {
        return $"Created {person.Name} with age {person.Age}";
    }

    [HttpPost]
    public void TestError([FromBody] Person person)
    {
        throw new Exception("Test error");
    }
}
