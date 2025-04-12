# MakeEasy.RestClient

## Project Description
  This is an easy-to-use RESTful or Rest Web API client library. It's a wrapper of HttpClient. The goal of this library is to be very MakeEasy to use. Any suggestions or improvements to the project are welcome.

## How to use

### Post
  + Post json object
  ```csharp
  using var client = new RestClient("https://api.example.com");
  var response = await client.PostAsync("/Person/Insert", new { name = "John", age = 30 });
  ```
  In none-generic version, it will throw an exception if there's an error in network/framework. You need to check the response status code and content by yourself. For example:
  ```csharp
  if (!response.IsSuccessStatusCode) {
     throw new Exception($"Status code: {response.StatusCode} Description:{response.ReasonPhrase}");
  }
  var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
  ```
  + if you want to pass query parameters, you can use Options to generate the url.
  ```csharp
  var reponse = await client.PostAsync(
	"/Person/Insert",
	new { name = "John", age = 30 },
	new SendOptions {
		QueryParams = new {some properties})
	}
	);
  ```
  + Post MultiPart
  ```csharp
  var multipart = new MultipartFormDataContent();
  multipart.Add(new StringContent("value1"), "key1");
  multipart.Add(new StringContent("value2"), "key2");
  var fs = new FileStream("path/to/file.txt", FileMode.Open);
  multipart.Add(new StreamContent(fs), "file", "file.txt");
  var reponse = await client.PostMultipartAsync("/Person/Insert", multiPart);
  ```
  + Post pure string as body
  ```csharp
  var body = """
	{
		"name"="John",
		"age"=30
	}
  """;
  var reponse = await client.PostStringAsync("/Person/Insert", body, RestContentTypes.Json);
  ```
  + Generic Post
  ```csharp
  var count = await client.PostAsync<int>("/Person/Insert", new { name = "John", age = 30 });
  Console.WriteLine($"inserted count: {count}");
  ```
  In generic version, it will throw an exception if there's an error in network/framework or status code. You don't need to check the response status code generally.
### Get
  ```csharp
  var person = await client.GetAsync<Person>("/Person/FindByName", new { name = "John" });
  ```
### Other Methods
  Other methods like Put/Delete/Patch/Options/Head are similar to Post and Get.
